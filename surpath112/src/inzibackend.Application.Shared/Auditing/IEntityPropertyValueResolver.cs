using System.Threading.Tasks;

namespace inzibackend.Auditing
{
    /// <summary>
    /// Service for resolving entity property values to user-friendly descriptions
    /// </summary>
    public interface IEntityPropertyValueResolver
    {
        /// <summary>
        /// Resolves a property value to a user-friendly description
        /// </summary>
        /// <param name="entityTypeFullName">Full name of the entity type</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="value">The property value (typically an ID)</param>
        /// <returns>User-friendly description or null if not resolvable</returns>
        Task<string> ResolvePropertyValueAsync(string entityTypeFullName, string propertyName, string value);
    }
} 