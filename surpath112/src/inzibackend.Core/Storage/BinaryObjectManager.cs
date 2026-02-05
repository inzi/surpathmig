using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Text;
using Abp.Extensions;
using System.Threading;
using System.Linq;
using Abp.Domain.Uow;
using System.Data.Entity;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using inzibackend.Surpath;

//BinaryObjectManager

namespace inzibackend.Storage
{
    public class BinaryObjectManager : IBinaryObjectManager, ITransientDependency
    {
        private readonly IRepository<BinaryObject, Guid> _binaryObjectRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ICurrentUnitOfWorkProvider _unitOfWorkProvider;


        public BinaryObjectManager(IRepository<BinaryObject, Guid> binaryObjectRepository,
            ICurrentUnitOfWorkProvider unitOfWorkProvider,
            IUnitOfWorkManager unitOfWorkManager
            )
        {
            _binaryObjectRepository = binaryObjectRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }
        public Task<BinaryObject> GetOrNullAsync(Guid id, int? TenantId = null, bool tracking = true)
        {


            if (TenantId == null || TenantId == 0) _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);

            // return _binaryObjectRepository.FirstOrDefaultAsync(id);
            return Task.Run(() =>
            {
                try
                {
                    var binaryObject = _binaryObjectRepository.Get(id);

                    if (binaryObject != null && binaryObject.IsFile)
                    {
                        var _fileBinaryObject = new BinaryObject()
                        {
                            Id = binaryObject.Id,
                            IsFile = binaryObject.IsFile,
                            TenantId = binaryObject?.TenantId,
                            Description = binaryObject?.Description,
                            Bytes = binaryObject?.Bytes,
                            FileName = binaryObject?.FileName,
                            Metadata = binaryObject?.Metadata,
                            OriginalFileName = binaryObject?.OriginalFileName,
                        };

                        // Check if DummyDocuments is enabled - this overrides everything
                        if (SurpathSettings.DummyDocuments && !string.IsNullOrEmpty(SurpathSettings.DummyDocumentFileName))
                        {
                            if (File.Exists(SurpathSettings.DummyDocumentFileName))
                            {
                                _fileBinaryObject.Bytes = File.ReadAllBytes(SurpathSettings.DummyDocumentFileName);
                                _fileBinaryObject.OriginalFileName = Path.GetFileName(SurpathSettings.DummyDocumentFileName);
                                return _fileBinaryObject;
                            }
                        }

#if DEBUG
                        // replace path in prod path
                        if (_fileBinaryObject.FileName.ToLower().Contains("f:"))
                        {
                            // f:\Surpath\surscandocs\6\804\00655830-87c1-1471-949b-3a08e2486c1e
                            _fileBinaryObject.FileName = _fileBinaryObject.FileName.Replace("F:\\Surpath\\Surscandocs\\", "c:\\devfolders\\surscandocs\\",StringComparison.InvariantCultureIgnoreCase);
                            var _path = Path.GetDirectoryName(_fileBinaryObject.FileName);
                            if (!Directory.Exists(_path))
                                Directory.CreateDirectory(_path);
                            var _ext = Path.GetExtension(_fileBinaryObject.OriginalFileName);
                            File.Copy($"c:\\devfolders\\holder{_ext}", _fileBinaryObject.FileName);
                            binaryObject.FileName = _fileBinaryObject.FileName;
                        }
#endif
                        _fileBinaryObject.Bytes = File.ReadAllBytes(binaryObject.FileName);
                        return _fileBinaryObject;
                    }
                    
                    // Check if DummyDocuments is enabled even for non-file objects
                    if (SurpathSettings.DummyDocuments && !string.IsNullOrEmpty(SurpathSettings.DummyDocumentFileName))
                    {
                        if (File.Exists(SurpathSettings.DummyDocumentFileName) && binaryObject != null)
                        {
                            binaryObject.Bytes = File.ReadAllBytes(SurpathSettings.DummyDocumentFileName);
                            return binaryObject;
                        }
                    }
                    
                    return binaryObject;
                }
                catch (Exception ex)
                {

                    return null;
                }

                //binaryObject = _binaryObjectRepository.GetAll().AsNoTracking().Where(b=>b.Id==id).FirstOrDefault();
                // var newb = new BinaryObject();


            });
        }
        public Task<String?> GetDescription(Guid id, int? TenantId = null)
        {
            if (TenantId == null || TenantId == 0) _unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant);
            return Task.Run(() =>
            {
                string? desc = null;
                if (id == null) return desc;
                if (id == Guid.Empty) return desc;
                desc = _binaryObjectRepository.Query<string>(q => q.Where(r => r.Id == id).Select(r => r.Description).FirstOrDefault());

                if (string.IsNullOrEmpty(desc)) desc = null;
                return desc;
            });
        }
        public Task SaveAsync(BinaryObject file, int? TenantId = null)
        {
            //  return _binaryObjectRepository.InsertAsync(file);

            return Task.Run(async () =>
            {
                if (file.IsFile && !file.FileName.IsNullOrEmpty())
                {
                    FileInfo _file = new FileInfo(file.FileName);
                    while (IsFileLocked(_file))
                        Thread.Sleep(50);
                    if (File.Exists(file.FileName)) _file.Delete();
                    File.WriteAllBytes(file.FileName, file.Bytes);
                    // await File.WriteAllBytesAsync(file.FileName, file.Bytes);
                    file.Bytes = Encoding.UTF8.GetBytes("");
                }
                await _binaryObjectRepository.InsertAsync(file);
                return Task.CompletedTask;
            });
        }

        public Task DeleteAsync(Guid id, int? TenantId = null)
        {
            //return _binaryObjectRepository.DeleteAsync(id);
            return Task.Run(async () =>
            {

                var binaryObject = _binaryObjectRepository.Get(id);

                if (binaryObject != null && binaryObject.IsFile)
                {
                    FileInfo file = new FileInfo(binaryObject.FileName);
                    while (IsFileLocked(file))
                        Thread.Sleep(50);
                    file.Delete();
                }
                await _binaryObjectRepository.DeleteAsync(id);
                return Task.CompletedTask;
            });
        }

        protected virtual bool IsFileLocked(FileInfo file, int? TenantId = null)
        {
            FileStream stream = null;

            if (!File.Exists(file.FullName)) return false;
            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally
            {
                if (stream != null)
                    stream.Close();
            }

            //file is not locked
            return false;
        }
    }
}