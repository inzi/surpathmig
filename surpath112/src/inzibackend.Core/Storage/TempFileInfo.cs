namespace inzibackend.Storage
{
    public class TempFileInfo
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public byte[] File { get; set; }

        /// <summary>
        /// User ID that created/owns this temporary file
        /// </summary>
        public long? UserId { get; set; }

        /// <summary>
        /// Tenant ID that owns this temporary file
        /// </summary>
        public int? TenantId { get; set; }

        public TempFileInfo()
        {
        }

        public TempFileInfo(byte[] file)
        {
            File = file;
        }

        public TempFileInfo(string fileName, string fileType, byte[] file)
        {
            FileName = fileName;
            FileType = fileType;
            File = file;
        }

        public TempFileInfo(string fileName, string fileType, byte[] file, long? userId, int? tenantId)
        {
            FileName = fileName;
            FileType = fileType;
            File = file;
            UserId = userId;
            TenantId = tenantId;
        }
    }
}