using System;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Castle.Logging.Log4Net;
using Abp.Runtime.Session;
using Abp.Authorization;
using Castle.Facilities.Logging;
using inzibackend.Authorization;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos;
using Microsoft.AspNetCore.Identity;
using static inzibackend.Configuration.AppSettings.UserManagement;
using inzibackend.MultiTenancy;
using inzibackend.Surpath.ParserClasses;
using Abp.Domain.Repositories;
using System.Data.Entity;
using Abp.Domain.Uow;

namespace ConsoleAppDemo.ConsoleApplication
{


    class Program
    {
        static string pathofimportfiles = "D:\\Surpath\\Import\\August DS Uploads";

        static CRLLabReportParser crlLabReportParser = new CRLLabReportParser();

        static IUnitOfWorkManager unitOfWorkManager;

        static async Task Main(string[] args)
        {
            using (var bootstrapper = AbpBootstrapper.Create<ConsoleAppApplicationAppModule>())
            {
                bootstrapper.IocManager.IocContainer
                    .AddFacility<LoggingFacility>(f => f.UseAbpLog4Net()
                        .WithConfig("log4net.config")
                    );

                bootstrapper.Initialize();

                var _cohortUsersAppService = bootstrapper.IocManager.Resolve<ICohortUsersAppService>();
                var _abpSession = bootstrapper.IocManager.Resolve<IAbpSession>();
                var _logInManager = bootstrapper.IocManager.Resolve<LogInManager>();
                var _userPIDAppService = bootstrapper.IocManager.Resolve<IUserPidsAppService>();
                var _cohortUserRepo = bootstrapper.IocManager.Resolve<IRepository<CohortUser, Guid>>();
                var _userPidRepo = bootstrapper.IocManager.Resolve<IRepository<UserPid, Guid>>();
                var _tenantManager = bootstrapper.IocManager.Resolve<TenantManager>();
                var _unitOfWorkManager = bootstrapper.IocManager.Resolve<IUnitOfWorkManager>(); // Resolve UnitOfWorkManager
                //var _activeUnitOfWork = bootstrapper.IocManager.Resolve<IActiveUnitOfWork>(); // Resolve UnitOfWorkManager

                var loginResult = await _logInManager.LoginAsync("chris@inzi.com", "123qwe", "");
                if (loginResult.Result == AbpLoginResultType.Success)
                {
                    var tenantId = loginResult.Tenant?.Id;
                    var userId = loginResult.User.Id;

                    using (_abpSession.Use(tenantId, userId))
                    {
                        var allPdfFiles = ListPdfFiles(pathofimportfiles);

                        foreach (var pdfFile in allPdfFiles)
                        {
                            using (var unitOfWork = _unitOfWorkManager.Begin()) // Begin a unit of work
                            {
                                _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);

                                Console.WriteLine($"PDF File: {pdfFile}");
                                var pdfText = PDFToTextConverter.ConvertPdfToText(pdfFile);
                                var result = crlLabReportParser.ParseLabReport(pdfText);
                                var ssn = FormatSsn(result.PatientId); // Format the SSN

                                var _t = _userPidRepo.GetAll()
                                    .Include(e => e.UserFk)
                                    .Include(e => e.PidTypeFk)
                                    .Where(u => u.Pid == ssn)
                                    .FirstOrDefault();


                                var _pid = _userPidRepo.GetAll()
                                    .Include(e => e.UserFk)
                                    .Include(e => e.PidTypeFk)
                                    .Where(e => e.UserFk.IsDeleted == false && e.UserId != null && e.PidTypeFk.Name == "SSN" && e.Pid == ssn);

                                var pid = _pid.FirstOrDefault();

                                if (pid != null)
                                {
                                    var _cohortuser = _cohortUserRepo.GetAll()
                                        .AsNoTracking()
                                        .Include(cu => cu.UserFk) // Ensure UserFk is included
                                        .Where(cu => cu.UserId == pid.UserId)
                                        .FirstOrDefault();

                                    if (_cohortuser != null)
                                    {
                                        var coc = result.SlipId;
                                        Console.WriteLine($"COC: {coc}, fname: {_cohortuser.UserFk.Name}, lname: {_cohortuser.UserFk.Surname}, SSN: {ssn}");
                                    }
                                }

                                await unitOfWork.CompleteAsync(); // Complete the unit of work
                            }
                        }
                    }
                }

                Console.WriteLine("Press ENTER to exit...");
                Console.ReadLine();
            }
        }
        
        private static string FormatSsn(string ssn)
        {
            if (ssn.Length == 9)
            {
                return $"{ssn.Substring(0, 3)}-{ssn.Substring(3, 2)}-{ssn.Substring(5, 4)}";
            }
            return ssn; // Return the original value if it's not 9 characters long
        }

        private static List<CRLLabReport> parseLabResults(List<string> pdfFiles)
        {
            var result = new List<CRLLabReport>();
            try
            {
                // load the pdf content
                foreach (var pdfFile in pdfFiles)
                {
                    var pdfContent = File.ReadAllText(pdfFile);
                    var report = crlLabReportParser.ParseLabReport(pdfContent);
                    if (report != null)
                        result.Add(report);
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error listing PDF files: {ex.Message}");
                return new List<CRLLabReport>();
            }
        }

        private static List<string> ListPdfFiles(string directoryPath)
        {
            try
            {
                return Directory.GetFiles(directoryPath, "*.pdf", SearchOption.AllDirectories).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error listing PDF files: {ex.Message}");
                return new List<string>();
            }
        }
    }
}