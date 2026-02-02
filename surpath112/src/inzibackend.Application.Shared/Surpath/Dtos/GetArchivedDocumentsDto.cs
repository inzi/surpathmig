using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath.Dtos
{
    /// <summary>
    /// DTO for displaying archived documents grouped by requirement category
    /// </summary>
    public class GetArchivedDocumentsDto
    {
        public RecordRequirementDto RecordRequirement { get; set; }
        public RecordCategoryDto RecordCategory { get; set; }
        public string CategoryName { get; set; }
        public int TotalDocuments { get; set; }
        public List<ArchivedDocumentItemDto> ArchivedDocuments { get; set; }
        
        public GetArchivedDocumentsDto()
        {
            ArchivedDocuments = new List<ArchivedDocumentItemDto>();
        }
    }

    /// <summary>
    /// DTO for individual archived document items
    /// </summary>
    public class ArchivedDocumentItemDto : EntityDto<Guid>
    {
        public Guid RecordStateId { get; set; }
        public Guid? RecordId { get; set; }
        public string FileName { get; set; }
        public DateTime CreationTime { get; set; }
        public RecordStatusDto RecordStatus { get; set; }
        public EnumRecordState State { get; set; }
        public string Notes { get; set; }
        public Guid? BinaryObjId { get; set; }
        public bool IsArchived { get; set; }
        public string CreatedByUserName { get; set; }
        public long? CreatedByUserId { get; set; }
    }

    /// <summary>
    /// Input parameters for getting archived documents
    /// </summary>
    public class GetArchivedDocumentsInput : PagedAndSortedResultRequestDto
    {
        public long UserId { get; set; }
        public Guid? RecordCategoryId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    /// <summary>
    /// Input parameters for reassociating a document to a different category
    /// </summary>
    public class ReassociateDocumentInput
    {
        public Guid RecordStateId { get; set; }
        public Guid NewRecordCategoryId { get; set; }
        public string Notes { get; set; }
    }
}