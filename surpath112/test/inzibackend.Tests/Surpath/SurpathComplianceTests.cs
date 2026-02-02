using Abp.Domain.Repositories;
using Castle.MicroKernel.Registration;
using inzibackend.Authorization.Accounts;
using inzibackend.Authorization.Accounts.Dto;
using inzibackend.Authorization.Users;
using inzibackend.Storage;
using inzibackend.Surpath;
using inzibackend.Surpath.Jobs;
using inzibackend.Surpath.MetaData;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NSubstitute;
using Shouldly;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace inzibackend.Tests.Surpath
{
    public class SurpathComplianceTests : AppTestBase
    {
        private readonly IRepository<Cohort, Guid> _cohortRepository;

        private readonly ISurpathComplianceAppService _surpathComplianceAppService;

        private readonly IBinaryObjectManager _binaryObjectManager;

        public SurpathComplianceTests()
        {
            _cohortRepository = Resolve<IRepository<Cohort, Guid>>();
            //_surpathComplianceAppService = Resolve<ISurpathComplianceAppService>();
            _binaryObjectManager = Resolve<IBinaryObjectManager>();
        }

        //public TestCohortComplianceResults()
        //{

        //}
        [Fact]
        public async Task Should_Notify_Email()
        {
            //Arrange

            //UsingDbContext(context =>
            //{
            //    //Set IsEmailConfirmed to false to provide initial test case
            //    var currentUser = context.Users.Single(u => u.Id == AbpSession.UserId.Value);
            //    currentUser.IsEmailConfirmed = true;
            //});

            //var user = await GetCurrentUserAsync();
            //user.IsEmailConfirmed.ShouldBeTrue();


            //var fakeUserEmailer = Substitute.For<IUserEmailer>();
            //var localUser = user;
            //fakeUserEmailer.SendEmailForComplianceRelatedNotification(Arg.Any<User>(), Arg.Any<string>()).Returns(callInfo =>
            //{
            //    var calledUser = callInfo.Arg<User>();
            //    calledUser.EmailAddress.ShouldBe(localUser.EmailAddress);
            //    confirmationCode = calledUser.EmailConfirmationCode; //Getting the confirmation code sent to the email address
            //    return Task.CompletedTask;
            //});

            //LocalIocManager.IocContainer.Register(Component.For<IUserEmailer>().Instance(fakeUserEmailer).IsDefault());

            //var accountAppService = Resolve<IAccountAppService>();

            ////Act

            //await accountAppService.SendEmailActivationLink(
            //    new SendEmailActivationLinkInput
            //    {
            //        EmailAddress = user.EmailAddress
            //    }
            //);

            //await accountAppService.ActivateEmail(
            //    new ActivateEmailInput
            //    {
            //        UserId = user.Id,
            //        ConfirmationCode = confirmationCode
            //    }
            //);

            ////Assert

            //user = await GetCurrentUserAsync();
            //user.IsEmailConfirmed.ShouldBeTrue();
        }

        //public class MetaDataNotifications
        //{
        //    public DateTime WarnDaysBeforeFirst { get; set; }
        //    public DateTime WarnDaysBeforeSecond { get; set; }
        //    public DateTime WarnDaysBeforeFinal { get; set; }
        //    public DateTime ExpiredNotification { get; set; }
        //}


        [Fact]
        public async Task TestMetaDataNotifications()
        {
            try
            {
                var metadatastring = "";
                var metadata = new MetaDataObject();
                var metadataNotifications = new MetaDataNotifications();
                metadatastring.IsJson().ShouldBeFalse();
                metadata.MetaDataNotifications = metadataNotifications;
                var _mtd = JsonConvert.SerializeObject(metadata);
                _mtd.IsJson().ShouldBeTrue();
                JObject obj = JObject.Parse(_mtd);
                if (obj.ContainsKey("MetaDataNotifications"))
                {
                    metadataNotifications = obj["MetaDataNotifications"].ToObject<MetaDataNotifications>();
                    true.ShouldBeTrue();
                    // Do something
                }

//                var _metadataNotifications = d.MetaDataNotifications as MetaDataNotifications;
                true.ShouldBeTrue();
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        static Random rd = new Random();
        [Fact]
        public async Task SaveBinaryFile()
        {
            var _numFiles = 50;
            while (_numFiles > 0)
            {
                //surpathdocs\unittest\testfiles
                var _folder = "c:\\devfolders\\surpathdocs\\unittest\\testfiles\\";
                var _length = rd.Next(100000, 250000);
                var _file = CreateString(_length);
                var _filename = Guid.NewGuid().ToString() + ".txt";
                //var storedFile = new BinaryObject(AbpSession.TenantId, fileCache.File, $"{fileCache.FileName} uploaded by {AbpSession.UserId.ToString()}", true, _folder);
                var storedFile = new BinaryObject(
                    AbpSession.TenantId,
                    Encoding.ASCII.GetBytes(_file),
                    $"{DateTime.Now:g} - {_filename} uploaded by UnitTest",
                    true,
                    _folder,
                    _filename,
                    "");
                await _binaryObjectManager.SaveAsync(storedFile);
                _numFiles--;
            }
            true.ShouldBeTrue();
        }


        internal static string CreateString(int stringLength)
        {
            //const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@$?_-";
            const string allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            char[] chars = new char[stringLength];

            for (int i = 0; i < stringLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
    }
}
