using System;
using System.Configuration;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using Serilog;

namespace HL7ParserService
{
    /// <summary>
    /// Standalone test utility for Project Concert integration
    /// Build with: csc ProjectConcertTestProgram.cs /reference:... /out:ProjectConcertTest.exe
    /// </summary>
    class ProjectConcertTestProgram
    {
        static void Main(string[] args)
        {
            // Configure Serilog
            var logger = new LoggerConfiguration()
                .WriteTo.Console()
                .MinimumLevel.Debug()
                .CreateLogger();

            logger.Information("Project Concert Test Utility - HL7 Stage Generator");
            logger.Information("=================================================");

            if (args.Length < 1)
            {
                ShowUsage();
                return;
            }

            try
            {
                // Get connection string from App.config (same as HL7ParserService)
                string connectionString = ConfigurationManager.AppSettings["ConnectionString"];

                if (string.IsNullOrEmpty(connectionString))
                {
                    logger.Error("Connection string not found in configuration!");
                    return;
                }

                // Parse command line arguments
                bool resetStatus = !args.Contains("--no-reset");
                bool generateOnly = args.Contains("--generate-only");
                int? donorId = null;

                if (!generateOnly)
                {
                    if (!int.TryParse(args[0], out int parsedDonorId))
                    {
                        logger.Error("Invalid donor ID. Please provide a numeric donor ID.");
                        ShowUsage();
                        return;
                    }
                    donorId = parsedDonorId;
                }

                using (var conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    if (donorId.HasValue && resetStatus)
                    {
                        // Check if donor exists and is linked to Project Concert
                        logger.Information($"Checking donor {donorId} for Project Concert integration...");

                        string checkQuery = @"
                            SELECT 
                                d.donor_id,
                                d.donor_first_name,
                                d.donor_last_name,
                                dti.donor_test_info_id,
                                dti.test_status,
                                dti.client_department_id,
                                c.client_name,
                                ip.partner_name,
                                cm.backend_integration_partner_client_map_id
                            FROM donors d
                            INNER JOIN donor_test_info dti ON d.donor_id = dti.donor_id
                            INNER JOIN client_departments cd ON cd.client_department_id = dti.client_department_id
                            INNER JOIN clients c ON c.client_id = cd.client_id
                            LEFT JOIN backend_integration_partner_client_map cm 
                                ON cm.client_id = c.client_id 
                                AND cm.client_department_id = dti.client_department_id
                            LEFT JOIN backend_integration_partners ip 
                                ON cm.backend_integration_partner_id = ip.backend_integration_partner_id
                            WHERE d.donor_id = @donorId
                            ORDER BY dti.donor_test_info_id DESC
                            LIMIT 1";

                        using (var cmd = new MySqlCommand(checkQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@donorId", donorId);
                            using (var reader = cmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    string firstName = reader["donor_first_name"].ToString();
                                    string lastName = reader["donor_last_name"].ToString();
                                    int testInfoId = Convert.ToInt32(reader["donor_test_info_id"]);
                                    int currentStatus = Convert.ToInt32(reader["test_status"]);
                                    string clientName = reader["client_name"].ToString();
                                    bool isIntegrated = !reader.IsDBNull(reader.GetOrdinal("backend_integration_partner_client_map_id"));
                                    string partnerName = reader.IsDBNull(reader.GetOrdinal("partner_name")) 
                                        ? "Not Integrated" 
                                        : reader["partner_name"].ToString();

                                    logger.Information($"Found donor: {firstName} {lastName} (ID: {donorId})");
                                    logger.Information($"Client: {clientName}");
                                    logger.Information($"Integration Partner: {partnerName}");
                                    logger.Information($"Current test status: {currentStatus}");
                                    logger.Information($"Test Info ID: {testInfoId}");

                                    if (!isIntegrated)
                                    {
                                        logger.Warning("WARNING: This donor's client is NOT linked to Project Concert!");
                                        logger.Warning("The HL7Stage will not pick up this donor.");
                                        Console.Write("Continue anyway? (y/n): ");
                                        if (Console.ReadLine()?.ToLower() != "y")
                                        {
                                            logger.Information("Cancelled by user.");
                                            return;
                                        }
                                    }
                                }
                                else
                                {
                                    logger.Error($"Donor {donorId} not found or has no test info!");
                                    return;
                                }
                            }
                        }

                        // Update donor test status to 4
                        logger.Information($"Setting donor {donorId} test status to 4 (Pre-Registration)...");

                        string updateQuery = @"
                            UPDATE donor_test_info 
                            SET test_status = 4,
                                last_modified_by = 'ProjectConcertTest',
                                last_modified_on = NOW()
                            WHERE donor_id = @donorId
                            AND test_status != 4
                            ORDER BY donor_test_info_id DESC
                            LIMIT 1";

                        using (var cmd = new MySqlCommand(updateQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@donorId", donorId);
                            int rowsAffected = cmd.ExecuteNonQuery();
                            
                            if (rowsAffected > 0)
                            {
                                logger.Information($"Successfully updated test status for donor {donorId}");
                            }
                            else
                            {
                                logger.Warning($"No rows updated. Donor {donorId} may already have test_status = 4");
                            }
                        }
                    }

                    // Show what will be processed
                    if (donorId.HasValue)
                    {
                        logger.Information($"\nProcessing specific donor: {donorId}");
                    }
                    else
                    {
                        string countQuery = @"
                            SELECT COUNT(DISTINCT d.donor_id) as donor_count
                            FROM donors d
                            INNER JOIN donor_test_info dti ON d.donor_id = dti.donor_id
                            INNER JOIN client_departments cd ON cd.client_department_id = dti.client_department_id
                            INNER JOIN clients c ON c.client_id = cd.client_id
                            INNER JOIN backend_integration_partner_client_map cm 
                                ON cm.client_id = c.client_id 
                                AND cm.client_department_id = dti.client_department_id
                            INNER JOIN backend_integration_partners ip 
                                ON cm.backend_integration_partner_id = ip.backend_integration_partner_id
                            WHERE dti.test_status = 4";

                        using (var cmd = new MySqlCommand(countQuery, conn))
                        {
                            int donorCount = Convert.ToInt32(cmd.ExecuteScalar());
                            logger.Information($"\nFound {donorCount} donors with test_status = 4 linked to integration partners");
                        }
                    }
                }

                // Run HL7Stage
                logger.Information("\n--- Running HL7Stage Generator ---");
                
                HL7Stage stage = new HL7Stage(logger);
                bool success = stage.Gen(donorId);

                if (success)
                {
                    logger.Information("HL7Stage generation completed successfully!");
                    logger.Information("\nGenerated files should be in:");
                    logger.Information($"  Lab Reports: {ConfigurationManager.AppSettings["LabReportFilePath"]}");
                    logger.Information($"  MRO Reports: {ConfigurationManager.AppSettings["MROReportFileInboundPath"]}");
                }
                else
                {
                    logger.Error("HL7Stage generation failed!");
                }

                // Reset status if requested
                if (donorId.HasValue && resetStatus && args.Contains("--reset-after"))
                {
                    using (var conn = new MySqlConnection(connectionString))
                    {
                        conn.Open();
                        string resetQuery = @"
                            UPDATE donor_test_info 
                            SET test_status = 6,
                                last_modified_by = 'ProjectConcertTest-Reset',
                                last_modified_on = NOW()
                            WHERE donor_id = @donorId
                            AND test_status = 4
                            ORDER BY donor_test_info_id DESC
                            LIMIT 1";

                        using (var cmd = new MySqlCommand(resetQuery, conn))
                        {
                            cmd.Parameters.AddWithValue("@donorId", donorId);
                            cmd.ExecuteNonQuery();
                            logger.Information($"Reset donor {donorId} test status back to 6 (Processing)");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "An error occurred");
            }

            logger.Information("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void ShowUsage()
        {
            Console.WriteLine("Usage: ProjectConcertTest.exe <donor_id> [options]");
            Console.WriteLine("       ProjectConcertTest.exe --generate-only");
            Console.WriteLine();
            Console.WriteLine("Options:");
            Console.WriteLine("  --no-reset        Don't set the donor test status to 4");
            Console.WriteLine("  --reset-after     Reset donor test status to 6 after generation");
            Console.WriteLine("  --generate-only   Only run HL7Stage for existing status=4 donors");
            Console.WriteLine();
            Console.WriteLine("Examples:");
            Console.WriteLine("  ProjectConcertTest.exe 12345");
            Console.WriteLine("  ProjectConcertTest.exe 12345 --reset-after");
            Console.WriteLine("  ProjectConcertTest.exe --generate-only");
        }
    }
}