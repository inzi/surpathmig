using MySql.Data.MySqlClient;
using Serilog;
using System;
using System.Configuration;
using System.Data;
using System.IO;

namespace SurpathBackend
{
    public class ProjectConcertTester
    {
        private readonly ILogger _logger;
        private readonly string _connectionString;
        private readonly bool _isProduction;

        public ProjectConcertTester(ILogger logger)
        {
            _logger = logger;
            _connectionString = ConfigurationManager.AppSettings["ConnectionString"];
            bool.TryParse(ConfigurationManager.AppSettings["Production"]?.ToString()?.Trim(), out _isProduction);
        }

        public void RunTest(int? donorId, bool noReset, bool resetAfter)
        {
            try
            {
                _logger.Information("Project Concert Stage Test Utility");
                _logger.Information("===================================");

                if (!_isProduction)
                {
                    _logger.Warning("Running in NON-PRODUCTION mode. Test files will be generated with test data.");
                }
                else
                {
                    _logger.Warning("Running in PRODUCTION mode. Be careful!");
                }

                if (string.IsNullOrEmpty(_connectionString))
                {
                    _logger.Error("Connection string not found in configuration!");
                    return;
                }

                using (var conn = new MySqlConnection(_connectionString))
                {
                    conn.Open();

                    if (donorId.HasValue)
                    {
                        // Validate donor exists and is integrated
                        if (!ValidateDonor(conn, donorId.Value))
                        {
                            return;
                        }

                        if (!noReset)
                        {
                            _logger.Information($"Setting donor {donorId} test status to 4...");
                            if (UpdateDonorTestStatus(conn, donorId.Value, 4))
                            {
                                _logger.Information($"Successfully updated test status for donor {donorId}");
                            }
                            else
                            {
                                _logger.Warning($"No rows updated. Donor {donorId} may already have test_status = 4");
                            }
                        }
                    }
                }

                _logger.Information("Generating test files...");
                GenerateTestFiles(donorId);

                if (donorId.HasValue && resetAfter)
                {
                    using (var conn = new MySqlConnection(_connectionString))
                    {
                        conn.Open();
                        if (UpdateDonorTestStatus(conn, donorId.Value, 6))
                        {
                            _logger.Information($"Reset donor {donorId} test status back to 6");
                        }
                    }
                }

                _logger.Information("Test completed successfully!");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "An error occurred during Project Concert test");
            }
        }

        private bool ValidateDonor(MySqlConnection conn, int donorId)
        {
            // Using stored procedure if available, otherwise direct query
            string query = @"
                SELECT d.donor_id, d.first_name, d.last_name, 
                       dti.partner_id, dti.test_status, p.company_name
                FROM donor d
                LEFT JOIN donor_test_info dti ON d.donor_id = dti.donor_id
                LEFT JOIN partner p ON dti.partner_id = p.partner_id
                WHERE d.donor_id = @donorId
                ORDER BY dti.donor_test_info_id DESC
                LIMIT 1";

            using (var cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@donorId", donorId);
                using (var reader = cmd.ExecuteReader())
                {
                    if (!reader.Read())
                    {
                        _logger.Error($"Donor {donorId} not found!");
                        return false;
                    }

                    var partnerId = reader["partner_id"] != DBNull.Value ? Convert.ToInt32(reader["partner_id"]) : (int?)null;
                    var partnerName = reader["company_name"]?.ToString() ?? "Unknown";
                    var testStatus = reader["test_status"] != DBNull.Value ? Convert.ToInt32(reader["test_status"]) : (int?)null;

                    _logger.Information($"Found donor: {reader["first_name"]} {reader["last_name"]}");
                    _logger.Information($"Partner: {partnerName} (ID: {partnerId})");
                    _logger.Information($"Current test status: {testStatus}");

                    // Check if it's Project Concert
                    if (partnerId != 2445) // Project Concert partner ID
                    {
                        _logger.Warning($"Donor {donorId} is NOT integrated with Project Concert!");
                        _logger.Warning($"They are integrated with: {partnerName}");
                        
                        Console.WriteLine("\nThis donor is not linked to Project Concert. Continue anyway? (y/n): ");
                        var response = Console.ReadLine();
                        if (response?.ToLower() != "y")
                        {
                            _logger.Information("Operation cancelled by user.");
                            return false;
                        }
                    }

                    return true;
                }
            }
        }

        private bool UpdateDonorTestStatus(MySqlConnection conn, int donorId, int newStatus)
        {
            string updateQuery = @"
                UPDATE donor_test_info 
                SET test_status = @newStatus,
                    last_modified_by = @modifiedBy,
                    last_modified_on = NOW()
                WHERE donor_id = @donorId
                AND test_status != @newStatus
                ORDER BY donor_test_info_id DESC
                LIMIT 1";

            using (var cmd = new MySqlCommand(updateQuery, conn))
            {
                cmd.Parameters.AddWithValue("@donorId", donorId);
                cmd.Parameters.AddWithValue("@newStatus", newStatus);
                cmd.Parameters.AddWithValue("@modifiedBy", "ProjectConcertTest" + (newStatus == 6 ? "-Reset" : ""));
                
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        private void GenerateTestFiles(int? specificDonorId)
        {
            try
            {
                // This is a simplified version - in reality you'd call the HL7Stage class
                // For a full implementation, you would need to reference HL7ParserService
                
                var labReportPath = ConfigurationManager.AppSettings["LabReportFilePath"];
                var mroReportPath = ConfigurationManager.AppSettings["MROReportFileInboundPath"];

                if (string.IsNullOrEmpty(labReportPath) || string.IsNullOrEmpty(mroReportPath))
                {
                    _logger.Error("Lab report or MRO report paths not configured!");
                    return;
                }

                Directory.CreateDirectory(labReportPath);
                Directory.CreateDirectory(mroReportPath);

                // Generate test HL7 file
                string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
                string filename = $"TEST_{specificDonorId}_{timestamp}.hl7";
                
                string labFilePath = Path.Combine(labReportPath, filename);
                string mroFilePath = Path.Combine(mroReportPath, $"MRO_{filename}");

                // Create simple test HL7 content
                string hl7Content = GenerateHL7Content(specificDonorId);
                
                File.WriteAllText(labFilePath, hl7Content);
                _logger.Information($"Generated lab report: {labFilePath}");

                // Generate MRO report for positive results (20% chance)
                if (new Random().Next(100) < 20)
                {
                    File.WriteAllText(mroFilePath, GenerateMROContent(specificDonorId));
                    _logger.Information($"Generated MRO report: {mroFilePath}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error generating test files");
                throw;
            }
        }

        private string GenerateHL7Content(int? donorId)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            return $@"MSH|^~\&|SURPATH|LAB|PROJECTCONCERT|CLIENT|{timestamp}||ORU^R01|{timestamp}|P|2.3
PID|||{donorId}||TEST^DONOR||19800101|M|||123 Test St^^Test City^CA^12345||555-1234
OBR|1||{timestamp}|DRUG^Drug Screen Panel|||{timestamp}
OBX|1|ST|RESULT||NEGATIVE||||||F
";
        }

        private string GenerateMROContent(int? donorId)
        {
            return $@"MRO Report for Donor {donorId}
Generated: {DateTime.Now}
Result: POSITIVE
MRO Review: Required
Notes: Test positive result for review
";
        }
    }
}