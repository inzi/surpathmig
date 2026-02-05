# Modified
## Filename
TwilioSmsSender.cs
## Relative Path
inzibackend.Core\Net\Sms\TwilioSmsSender.cs
## Language
C#
## Summary
Both files contain a C# class implementing ISmsSender with an SendAsync method that sends messages via Twilio. The modified version includes an additional line at the end of the method which does nothing.
## Changes
The modified file adds an empty statement after creating the MessageResource in the SendAsync method.
## Purpose
The class is part of an ASP.NET Zero solution for sending SMS messages using Twilio's API, utilizing dependency injection.
