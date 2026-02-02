# Modified
## Filename
DelegateNewUserInput.cs
## Relative Path
inzibackend.Application.Shared\Authorization\Users\Delegation\Dto\DelegateNewUserInput.cs
## Language
C#
## Summary
The modified CreateUserDelegationDto class includes additional validation checks to ensure that the EndTime is not earlier than the StartTime. This helps prevent invalid time ranges for user delegation operations.
## Changes
Added a validation check in the Validate method to ensure that StartTime is not greater than EndTime.
## Purpose
The file validates user delegation operation times, ensuring that the start time occurs before the end time.
