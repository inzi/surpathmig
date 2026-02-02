using System;
using inzibackend.Surpath.Dtos;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using inzibackend.Storage;
using Abp.Application.Services.Dto;

namespace inzibackend.Surpath
{
    public interface ISurpathToolReviewQueueAppService
    {
        Task<exdtos.PagedResultDto<GetRecordStateForQueueViewDto>> GetAll(GetAllRecordStatesInput input);

    }
}