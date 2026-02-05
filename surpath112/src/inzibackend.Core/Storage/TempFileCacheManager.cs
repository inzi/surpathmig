using System;
using Abp.Runtime.Caching;

namespace inzibackend.Storage
{
    public class TempFileCacheManager : ITempFileCacheManager
    {
        private const string TempFileCacheName = "TempFileCacheName";

        private readonly ITypedCache<string, TempFileInfo> _cache;

        public TempFileCacheManager(ICacheManager cacheManager)
        {
            _cache = cacheManager.GetCache<string, TempFileInfo>(TempFileCacheName);
        }

        public void SetFile(string token, byte[] content)
        {
            _cache.Set(token, new TempFileInfo(content), TimeSpan.FromMinutes(1)); // expire time is 1 min by default
        }

        public byte[] GetFile(string token)
        {
            var cache = _cache.GetOrDefault(token);
            return cache?.File;
        }

        public void SetFile(string token, TempFileInfo info)
        {
            _cache.Set(token, info, TimeSpan.FromMinutes(1)); // expire time is 1 min by default
        }

        public TempFileInfo GetFileInfo(string token)
        {
            return _cache.GetOrDefault(token);
        }

        public void SetFile(string token, byte[] content, long? userId, int? tenantId)
        {
            var info = new TempFileInfo
            {
                File = content,
                UserId = userId,
                TenantId = tenantId
            };
            _cache.Set(token, info, TimeSpan.FromMinutes(1)); // expire time is 1 min by default
        }

        public byte[] GetFile(string token, long? userId, int? tenantId)
        {
            var fileInfo = _cache.GetOrDefault(token);
            if (fileInfo == null)
            {
                return null;
            }

            // Verify ownership - file must belong to the same user/tenant
            if (fileInfo.UserId != userId || fileInfo.TenantId != tenantId)
            {
                return null; // Security: Don't reveal file exists, just return null
            }

            return fileInfo.File;
        }

        public TempFileInfo GetFileInfo(string token, long? userId, int? tenantId)
        {
            var fileInfo = _cache.GetOrDefault(token);
            if (fileInfo == null)
            {
                return null;
            }

            // Verify ownership - file must belong to the same user/tenant
            if (fileInfo.UserId != userId || fileInfo.TenantId != tenantId)
            {
                return null; // Security: Don't reveal file exists, just return null
            }

            return fileInfo;
        }
    }
}