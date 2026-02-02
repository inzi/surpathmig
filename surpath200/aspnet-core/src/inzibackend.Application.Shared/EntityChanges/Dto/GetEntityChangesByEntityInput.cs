using inzibackend.Dto;
using System;

namespace inzibackend.EntityChanges.Dto;

public class GetEntityChangesByEntityInput
{
    public string EntityTypeFullName { get; set; }
    public string EntityId { get; set; }
}

