using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Abp;
using Abp.Domain.Entities;

namespace inzibackend.Storage
{
    [Table("AppBinaryObjects")]
    public class BinaryObject : Entity<Guid>, IMayHaveTenant
    {
        public virtual int? TenantId { get; set; }

        public virtual string Description { get; set; }

        [Required]
        [MaxLength(BinaryObjectConsts.BytesMaxSize)]
        public virtual byte[] Bytes { get; set; }

        public BinaryObject()
        {
            Id = SequentialGuidGenerator.Instance.Create();
        }

        public BinaryObject(int? tenantId, byte[] bytes, string description = null)
            : this()
        {
            TenantId = tenantId;
            Bytes = bytes;
            Description = description;
            IsFile = false;
            FileName = String.Empty;
            Metadata = "{}";

        }


        public virtual bool IsFile { get; set; } = false;
        public virtual string FileName { get; set; }
        public virtual string Metadata { get; set; }

        public virtual string OriginalFileName { get; set; }
        //public BinaryObject(int? tenantId, byte[] bytes, string description = null, bool isfile = false, string filename = "", string metadata = "")
        //   : this()
        //{
        //    IsFile = isfile;
        //    FileName = $"{filename}{Id}";
        //    Metadata = metadata;
        //    TenantId = tenantId;
        //    Bytes = bytes;
        //    Description = description;
        //}

        public BinaryObject(int? tenantId, byte[] bytes, string description = null, bool isfile = false, string folder = "", string filename = "", string metadata = "")
            : this()
        {
            IsFile = isfile;
            FileName = $"{folder}{Id}";
            Metadata = metadata;
            TenantId = tenantId;
            Bytes = bytes;
            Description = description;
            OriginalFileName = filename;
        }
    }
}
