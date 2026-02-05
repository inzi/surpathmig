using System;

namespace inzibackend
{
    /// <summary>
    /// This interface is used to make an entity archivable.
    /// When an entity is archived, it means it's no longer current/active but should be retained for historical purposes.
    /// Archive audit fields (ArchivedTime and ArchivedByUserId) are automatically set when IsArchived changes to true.
    /// </summary>
    public interface IHasArchiving
    {
        /// <summary>
        /// Indicates if this entity is archived (not current/active).
        /// </summary>
        bool IsArchived { get; set; }

        /// <summary>
        /// The date and time when this entity was archived.
        /// Automatically set when IsArchived changes from false to true.
        /// </summary>
        DateTime? ArchivedTime { get; set; }

        /// <summary>
        /// The user who archived this entity.
        /// Automatically set when IsArchived changes from false to true.
        /// </summary>
        long? ArchivedByUserId { get; set; }
    }
}