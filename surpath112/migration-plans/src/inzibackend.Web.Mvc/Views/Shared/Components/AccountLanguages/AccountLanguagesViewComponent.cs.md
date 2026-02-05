# Modified
## Filename
AccountLanguagesViewComponent.cs
## Relative Path
inzibackend.Web.Mvc\Views\Shared\Components\AccountLanguages\AccountLanguagesViewComponent.cs
## Language
C#
## Summary
The modified file defines an AccountLanguagesViewComponent class that implements an IViewComponent interface. It accepts a LanguageSelectionViewModel with current language and active languages data from an ILanguageManager, and invokes the View method to render the component.
## Changes
The parameter name in the constructor was changed from 'languageManager' to '_languageManager'.
## Purpose
This class is part of an ASP.NET Zero application's dependency injection infrastructure, providing language selection functionality across web views.
