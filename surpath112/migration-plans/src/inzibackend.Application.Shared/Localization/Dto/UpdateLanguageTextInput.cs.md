# Modified
## Filename
UpdateLanguageTextInput.cs
## Relative Path
inzibackend.Application.Shared\Localization\Dto\UpdateLanguageTextInput.cs
## Language
C#
## Summary
The modified class UpdateLanguageTextInput includes four required properties: LanguageName, SourceName, Key, and Value. The Value property now allows empty strings due to the addition of AllowEmptyStrings = true in its Required attribute.
## Changes
Added AllowEmptyStrings = true to the Value property's Required attribute in the modified version.
## Purpose
The class is part of a localization module for validating translation properties, ensuring string length constraints and allowing empty values where necessary.
