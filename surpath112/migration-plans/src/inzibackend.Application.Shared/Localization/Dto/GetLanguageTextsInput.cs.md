# Modified
## Filename
GetLanguageTextsInput.cs
## Relative Path
inzibackend.Application.Shared\Localization\Dto\GetLanguageTextsInput.cs
## Language
C#
## Summary
The modified file introduces TargetValueFilter as a new property without required attributes. It also removes the [Required] and [StringLength] attributes from BaseLanguageName.
## Changes
Added TargetValueFilter public string { get; set; }, removed [Required] and [StringLength] (MaxLength) from BaseLanguageName.
## Purpose
The class handles localization data, possibly for filtering or formatting language texts.
