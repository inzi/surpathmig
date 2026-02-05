# Modified
## Filename
CurrentUserProfileEditDto.cs
## Relative Path
inzibackend.Application.Shared\Authorization\Users\Profile\Dto\CurrentUserProfileEditDto.cs
## Language
C#
## Summary
The modified CurrentUserProfileEditDto includes additional properties such as Address, Zip, and DateOfBirth compared to the unmodified version. It also adds constraints on string lengths.
## Changes
Added Address { get; set; } with [Required] [StringLength(UserConsts.MaxAddressLength)]
Added Zip { get; set; }
## Purpose
The file defines a data transfer object for user profile information, ensuring proper structure and validation.
