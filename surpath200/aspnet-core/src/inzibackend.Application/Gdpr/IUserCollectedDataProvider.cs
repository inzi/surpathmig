using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using inzibackend.Dto;

namespace inzibackend.Gdpr;

public interface IUserCollectedDataProvider
{
    Task<List<FileDto>> GetFiles(UserIdentifier user);
}
