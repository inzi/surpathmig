using inzibackend;

using System;
using System.Linq;
using System.Threading.Tasks;
using inzibackend.Surpath.Dtos;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using inzibackend.Surpath;
using Shouldly;
using Xunit;
using Abp.Timing;
using Abp.Domain.Uow;

using Abp;
using System.Text.RegularExpressions;

namespace inzibackend.Tests.Surpath
{
    public class HospitalsAppService_Tests : AppTestBase
    {
        private readonly IHospitalsAppService _hospitalsAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly int _hospitalTestId;

        public HospitalsAppService_Tests()
        {
            _hospitalsAppService = Resolve<IHospitalsAppService>();
            _unitOfWorkManager = Resolve<IUnitOfWorkManager>();
            _hospitalTestId = 1;
            SeedTestData();
        }

        private void SeedTestData()
        {
            var currentTenant = GetCurrentTenant();

            var hospital = new Hospital
            {
                Name = GetRandomString(0),
                PrimaryContact = GetRandomString(0),
                PrimaryContactPhone = GetRandomString(0),
                PrimaryContactEmail = GetRandomString(0),
                Address1 = GetRandomString(0),
                Address2 = GetRandomString(0),
                City = GetRandomString(0),
                State = (enumUSStates)Enum.ToObject(typeof(enumUSStates), 0),
                ZipCode = GetRandomString(0),
                Id = _hospitalTestId,
                TenantId = currentTenant.Id
            };

            UsingDbContext(context =>
            {
                context.Hospitals.Add(hospital);
            });
        }

        [Fact]
        public async Task Should_Get_All_Hospitals()
        {
            var hospitals = await _hospitalsAppService.GetAll(new GetAllHospitalsInput());

            hospitals.TotalCount.ShouldBe(1);
            hospitals.Items.Count.ShouldBe(1);

            hospitals.Items.Any(x => x.Hospital.Id == _hospitalTestId).ShouldBe(true);
        }

        [Fact]
        public async Task Should_Get_Hospital_For_View()
        {
            var hospital = await _hospitalsAppService.GetHospitalForView(_hospitalTestId);

            hospital.ShouldNotBe(null);
        }

        [Fact]
        public async Task Should_Get_Hospital_For_Edit()
        {
            var hospital = await _hospitalsAppService.GetHospitalForEdit(new EntityDto { Id = _hospitalTestId });

            hospital.ShouldNotBe(null);
        }

        [Fact]
        protected virtual async Task Should_Create_Hospital()
        {
            var hospital = new CreateOrEditHospitalDto
            {
                Name = GetRandomString(0),
                PrimaryContact = GetRandomString(0),
                PrimaryContactPhone = GetRandomString(0),
                PrimaryContactEmail = GetRandomString(0),
                Address1 = GetRandomString(0),
                Address2 = GetRandomString(0),
                City = GetRandomString(0),
                State = 0,
                ZipCode = GetRandomString(0),
                Id = _hospitalTestId
            };

            await _hospitalsAppService.CreateOrEdit(hospital);

            await UsingDbContextAsync(async context =>
            {
                var entity = await context.Hospitals.FirstOrDefaultAsync(e => e.Id == _hospitalTestId);
                entity.ShouldNotBe(null);
            });
        }

        [Fact]
        protected virtual async Task Should_Update_Hospital()
        {
            var name = GetRandomString(0);
            var primaryContact = GetRandomString(0);
            var primaryContactPhone = GetRandomString(0);
            var primaryContactEmail = GetRandomString(0);
            var address1 = GetRandomString(0);
            var address2 = GetRandomString(0);
            var city = GetRandomString(0);
            var zipCode = GetRandomString(0);

            var hospital = new CreateOrEditHospitalDto
            {
                Name = name,
                PrimaryContact = primaryContact,
                PrimaryContactPhone = primaryContactPhone,
                PrimaryContactEmail = primaryContactEmail,
                Address1 = address1,
                Address2 = address2,
                City = city,
                State = 0,
                ZipCode = zipCode,
                Id = _hospitalTestId
            };

            await _hospitalsAppService.CreateOrEdit(hospital);

            await UsingDbContextAsync(async context =>
            {
                var entity = await context.Hospitals.FirstOrDefaultAsync(e => e.Id == hospital.Id);
                entity.ShouldNotBeNull();

                entity.Name.ShouldBe(name);
                entity.PrimaryContact.ShouldBe(primaryContact);
                entity.PrimaryContactPhone.ShouldBe(primaryContactPhone);
                entity.PrimaryContactEmail.ShouldBe(primaryContactEmail);
                entity.Address1.ShouldBe(address1);
                entity.Address2.ShouldBe(address2);
                entity.City.ShouldBe(city);
               // entity.State.ShouldBe((enumUSStates)0);
                entity.ZipCode.ShouldBe(zipCode);
            });
        }

        [Fact]
        public async Task Should_Delete_Hospital()
        {
            await _hospitalsAppService.Delete(new EntityDto { Id = _hospitalTestId });

            await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                await UsingDbContextAsync(async context =>
                {
                    var entity = await context.Hospitals.FirstOrDefaultAsync(e => e.Id == _hospitalTestId);
                    entity.ShouldBeNull();
                });
            });
        }

        private string GetRandomString(int minLength = 0, int maxLength = 13, string regexPattern = "[^A-Za-z0-9]")
        {
            const string DefaultStringChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            if (minLength < 0 || maxLength < 0 || minLength > maxLength)
            {
                throw new AbpException("Invalid minLength or maxLength parameters");
            }

            var random = new Random();
            var regex = new Regex(regexPattern);

            var length = random.Next(minLength, maxLength + 1);

            var filteredChars = new string(DefaultStringChars.Where(c => !regex.IsMatch(c.ToString())).ToArray());

            return new string(Enumerable.Repeat(filteredChars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}