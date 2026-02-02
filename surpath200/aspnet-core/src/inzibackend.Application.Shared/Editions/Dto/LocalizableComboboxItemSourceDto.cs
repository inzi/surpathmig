using System.Collections.ObjectModel;

namespace inzibackend.Editions.Dto;

//Mapped in CustomDtoMapper
public class LocalizableComboboxItemSourceDto
{
    public Collection<LocalizableComboboxItemDto> Items { get; set; }
}

