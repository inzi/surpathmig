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
    public class MedicalUnitsAppService_Tests : AppTestBase
    {
        private readonly IMedicalUnitsAppService _medicalUnitsAppService;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly int _medicalUnitTestId;

        public MedicalUnitsAppService_Tests()
        {
            _medicalUnitsAppService = Resolve<IMedicalUnitsAppService>();
            _unitOfWorkManager = Resolve<IUnitOfWorkManager>();
            _medicalUnitTestId = 1;
            SeedTestData();
        }

        private void SeedTestData()
        {
            var currentTenant = GetCurrentTenant();

            var medicalUnit = new MedicalUnit
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
                Id = _medicalUnitTestId,
                TenantId = currentTenant.Id
            };

            UsingDbContext(context =>
            {
                context.MedicalUnits.Add(medicalUnit);
            });
        }

        [Fact]
        public async Task Should_Get_All_MedicalUnits()
        {
            var medicalUnits = await _medicalUnitsAppService.GetAll(new GetAllMedicalUnitsInput());

            medicalUnits.TotalCount.ShouldBe(1);
            medicalUnits.Items.Count.ShouldBe(1);

            medicalUnits.Items.Any(x => x.MedicalUnit.Id == _medicalUnitTestId).ShouldBe(true);
        }

        [Fact]
        public async Task Should_Get_MedicalUnit_For_View()
        {
            var medicalUnit = await _medicalUnitsAppService.GetMedicalUnitForView(_medicalUnitTestId);

            medicalUnit.ShouldNotBe(null);
        }

        [Fact]
        public async Task Should_Get_MedicalUnit_For_Edit()
        {
            var medicalUnit = await _medicalUnitsAppService.GetMedicalUnitForEdit(new EntityDto { Id = _medicalUnitTestId });

            medicalUnit.ShouldNotBe(null);
        }

        [Fact]
        protected virtual async Task Should_Create_MedicalUnit()
        {
            var medicalUnit = new CreateOrEditMedicalUnitDto
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
                Id = _medicalUnitTestId
            };

            await _medicalUnitsAppService.CreateOrEdit(medicalUnit);

            await UsingDbContextAsync(async context =>
            {
                var entity = await context.MedicalUnits.FirstOrDefaultAsync(e => e.Id == _medicalUnitTestId);
                entity.ShouldNotBe(null);
            });
        }

        [Fact]
        protected virtual async Task Should_Update_MedicalUnit()
        {
            var name = GetRandomString(0);
            var primaryContact = GetRandomString(0);
            var primaryContactPhone = GetRandomString(0);
            var primaryContactEmail = GetRandomString(0);
            var address1 = GetRandomString(0);
            var address2 = GetRandomString(0);
            var city = GetRandomString(0);
            var zipCode = GetRandomString(0);

            var medicalUnit = new CreateOrEditMedicalUnitDto
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
                Id = _medicalUnitTestId
            };

            await _medicalUnitsAppService.CreateOrEdit(medicalUnit);

            await UsingDbContextAsync(async context =>
            {
                var entity = await context.MedicalUnits.FirstOrDefaultAsync(e => e.Id == medicalUnit.Id);
                entity.ShouldNotBeNull();

                entity.Name.ShouldBe(name);
                entity.PrimaryContact.ShouldBe(primaryContact);
                entity.PrimaryContactPhone.ShouldBe(primaryContactPhone);
                entity.PrimaryContactEmail.ShouldBe(primaryContactEmail);
                entity.Address1.ShouldBe(address1);
                entity.Address2.ShouldBe(address2);
                entity.City.ShouldBe(city);
                entity.State.ShouldBe((enumUSStates)0);
                entity.ZipCode.ShouldBe(zipCode);
            });
        }

        [Fact]
        public async Task Should_Delete_MedicalUnit()
        {
            await _medicalUnitsAppService.Delete(new EntityDto { Id = _medicalUnitTestId });

            await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                await UsingDbContextAsync(async context =>
                {
                    var entity = await context.MedicalUnits.FirstOrDefaultAsync(e => e.Id == _medicalUnitTestId);
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