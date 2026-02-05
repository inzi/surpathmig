using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Uow;
using inzibackend.Storage;
using inzibackend.Surpath;
using inzibackend.Surpath.Dtos.LegalDocuments;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace inzibackend.Tests.Surpath.LegalDocuments
{
    public class LegalDocumentsAppService_Tests : AppTestBase
    {
        private readonly ILegalDocumentsAppService _legalDocumentsAppService;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly Guid _testDocumentId = Guid.NewGuid();

        public LegalDocumentsAppService_Tests()
        {
            _legalDocumentsAppService = Resolve<ILegalDocumentsAppService>();
            _binaryObjectManager = Resolve<IBinaryObjectManager>();
            _unitOfWorkManager = Resolve<IUnitOfWorkManager>();
            
            SeedTestData();
        }

        private void SeedTestData()
        {
            var currentTenant = GetCurrentTenant();

            var legalDocument = new LegalDocument
            {
                Id = _testDocumentId,
                Type = DocumentType.PrivacyPolicy,
                FileName = "TestPrivacyPolicy.html",
                ViewUrl = "/LegalDocuments/View/0",
                TenantId = currentTenant.Id,
                ExternalUrl = "https://example.com/privacy-policy"
            };

            UsingDbContext(context =>
            {
                context.Set<LegalDocument>().Add(legalDocument);
                context.SaveChanges();
            });
        }

        [Fact]
        public async Task Should_Get_All_LegalDocuments()
        {
            // Act
            var result = await _legalDocumentsAppService.GetAll(new GetAllLegalDocumentsInput());

            // Assert
            result.TotalCount.ShouldBe(1);
            result.Items.Count.ShouldBe(1);
            result.Items.Any(x => x.LegalDocument.Id == _testDocumentId).ShouldBe(true);
        }

        [Fact]
        public async Task Should_Get_LegalDocument_For_View()
        {
            // Act
            var result = await _legalDocumentsAppService.GetLegalDocumentForView(new GetLegalDocumentForViewInput { Id = _testDocumentId });

            // Assert
            result.ShouldNotBeNull();
            result.LegalDocument.Id.ShouldBe(_testDocumentId);
            result.LegalDocument.Type.ShouldBe(DocumentType.PrivacyPolicy);
        }

        [Fact]
        public async Task Should_Get_LegalDocument_For_Edit()
        {
            // Act
            var result = await _legalDocumentsAppService.GetLegalDocumentForEdit(new GetLegalDocumentForEditInput { Id = _testDocumentId });

            // Assert
            result.ShouldNotBeNull();
            result.LegalDocument.Id.ShouldBe(_testDocumentId);
            result.LegalDocument.Type.ShouldBe(DocumentType.PrivacyPolicy);
        }

        [Fact]
        public async Task Should_Create_LegalDocument()
        {
            // Arrange
            var newDocumentId = Guid.NewGuid();
            var newDocument = new CreateOrUpdateLegalDocumentDto
            {
                LegalDocument = new LegalDocumentDto
                {
                    Id = newDocumentId,
                    Type = DocumentType.TermsOfService,
                    FileName = "TestTermsOfService.html",
                    ExternalUrl = "https://example.com/terms-of-service"
                }
            };

            // Act
            await _legalDocumentsAppService.CreateOrEdit(newDocument);

            // Assert
            await UsingDbContextAsync(async context =>
            {
                var entity = await context.Set<LegalDocument>().FirstOrDefaultAsync(e => e.Id == newDocumentId);
                entity.ShouldNotBeNull();
                entity.Type.ShouldBe(DocumentType.TermsOfService);
                entity.FileName.ShouldBe("TestTermsOfService.html");
                entity.ExternalUrl.ShouldBe("https://example.com/terms-of-service");
                entity.ViewUrl.ShouldBe("/LegalDocuments/View/1"); // 1 is the enum value for TermsOfService
            });
        }

        [Fact]
        public async Task Should_Update_LegalDocument()
        {
            // Arrange
            var updatedExternalUrl = "https://example.com/updated-privacy-policy";
            var updateDocument = new CreateOrUpdateLegalDocumentDto
            {
                LegalDocument = new LegalDocumentDto
                {
                    Id = _testDocumentId,
                    Type = DocumentType.PrivacyPolicy,
                    FileName = "UpdatedPrivacyPolicy.html",
                    ExternalUrl = updatedExternalUrl
                }
            };

            // Act
            await _legalDocumentsAppService.CreateOrEdit(updateDocument);

            // Assert
            await UsingDbContextAsync(async context =>
            {
                var entity = await context.Set<LegalDocument>().FirstOrDefaultAsync(e => e.Id == _testDocumentId);
                entity.ShouldNotBeNull();
                entity.FileName.ShouldBe("UpdatedPrivacyPolicy.html");
                entity.ExternalUrl.ShouldBe(updatedExternalUrl);
            });
        }

        [Fact]
        public async Task Should_Delete_LegalDocument()
        {
            // Act
            await _legalDocumentsAppService.Delete(_testDocumentId);

            // Assert
            await _unitOfWorkManager.WithUnitOfWorkAsync(async () =>
            {
                await UsingDbContextAsync(async context =>
                {
                    var entity = await context.Set<LegalDocument>().FirstOrDefaultAsync(e => e.Id == _testDocumentId);
                    entity.ShouldBeNull();
                });
            });
        }

        [Fact]
        public async Task Should_Get_Latest_Document_By_Type()
        {
            // Act
            var result = await _legalDocumentsAppService.GetLatestDocumentByType(DocumentType.PrivacyPolicy);

            // Assert
            result.ShouldNotBeNull();
            result.LegalDocument.Type.ShouldBe(DocumentType.PrivacyPolicy);
        }
    }
}
