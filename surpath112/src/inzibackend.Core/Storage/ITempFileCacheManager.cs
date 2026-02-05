using Abp.Dependency;

namespace inzibackend.Storage
{
    public interface ITempFileCacheManager : ITransientDependency
    {
        void SetFile(string token, byte[] content);

        byte[] GetFile(string token);

        void SetFile(string token, TempFileInfo info);

        TempFileInfo GetFileInfo(string token);

        /// <summary>
        /// Set file with user and tenant association for security
        /// </summary>
        void SetFile(string token, byte[] content, long? userId, int? tenantId);

        /// <summary>
        /// Get file only if it belongs to the specified user/tenant
        /// </summary>
        byte[] GetFile(string token, long? userId, int? tenantId);

        /// <summary>
        /// Get file info only if it belongs to the specified user/tenant
        /// </summary>
        TempFileInfo GetFileInfo(string token, long? userId, int? tenantId);
    }
}