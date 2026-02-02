using System;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using inzibackend.Authorization.Users;
using inzibackend.Surpath;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace inzibackend.Auditing
{
    /// <summary>
    /// Service for resolving entity property values to user-friendly descriptions
    /// </summary>
    public class EntityPropertyValueResolver : IEntityPropertyValueResolver, ITransientDependency
    {
        private readonly IRepository<Cohort, Guid> _cohortRepository;
        private readonly IRepository<User, long> _userRepository;

        public EntityPropertyValueResolver(
            IRepository<Cohort, Guid> cohortRepository,
            IRepository<User, long> userRepository)
        {
            _cohortRepository = cohortRepository;
            _userRepository = userRepository;
        }

        public async Task<string> ResolvePropertyValueAsync(string entityTypeFullName, string propertyName, string value)
        {
            if (string.IsNullOrWhiteSpace(value) || value == "null")
            {
                return null;
            }

            try
            {
                // Remove quotes if the value is JSON-serialized
                var cleanValue = value.Trim('"');

                // Handle CohortId properties
                if (propertyName.EndsWith("CohortId", StringComparison.OrdinalIgnoreCase))
                {
                    return await ResolveCohortNameAsync(cleanValue);
                }

                // Handle UserId properties  
                if (propertyName.EndsWith("UserId", StringComparison.OrdinalIgnoreCase))
                {
                    return await ResolveUserNameAsync(cleanValue);
                }

                // Add more entity type resolvers here as needed
                // Example: OrganizationId, DepartmentId, etc.

                return null; // Unable to resolve
            }
            catch (Exception)
            {
                // Log the exception if needed, but don't fail the audit display
                return null;
            }
        }

        private async Task<string> ResolveCohortNameAsync(string cohortIdValue)
        {
            if (!Guid.TryParse(cohortIdValue, out var cohortId))
            {
                return null;
            }

            var cohort = await _cohortRepository
                .GetAll()
                .Where(c => c.Id == cohortId)
                .Select(c => new { c.Name, c.Description })
                .FirstOrDefaultAsync();

            if (cohort == null)
            {
                return "[Deleted Cohort]";
            }

            // Return name with description if available
            return string.IsNullOrWhiteSpace(cohort.Description) 
                ? cohort.Name 
                : $"{cohort.Name} ({cohort.Description})";
        }

        private async Task<string> ResolveUserNameAsync(string userIdValue)
        {
            if (!long.TryParse(userIdValue, out var userId))
            {
                return null;
            }

            var user = await _userRepository
                .GetAll()
                .Where(u => u.Id == userId)
                .Select(u => new { u.Name, u.Surname, u.UserName })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return "[Deleted User]";
            }

            // Return full name or username if name not available
            var fullName = $"{user.Name} {user.Surname}".Trim();
            return string.IsNullOrWhiteSpace(fullName) ? user.UserName : fullName;
        }
    }
} 