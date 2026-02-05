using System.Threading.Tasks;
using Abp.Application.Services;
using inzibackend.Friendships.Dto;

namespace inzibackend.Friendships;

public interface IFriendshipAppService : IApplicationService
{
    Task<FriendDto> CreateFriendshipRequest(CreateFriendshipRequestInput input);

    Task<FriendDto> CreateFriendshipWithDifferentTenant(CreateFriendshipWithDifferentTenantInput input);

    Task<FriendDto> CreateFriendshipForCurrentTenant(CreateFriendshipForCurrentTenantInput input);

    Task BlockUser(BlockUserInput input);

    Task UnblockUser(UnblockUserInput input);

    Task AcceptFriendshipRequest(AcceptFriendshipRequestInput input);

    Task RemoveFriend(RemoveFriendInput input);
}

