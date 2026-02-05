# Modified
## Filename
DefaultInvoiceNumberGenerator.cs
## Relative Path
inzibackend.Core\MultiTenancy\Accounting\DefaultInvoiceNumberGenerator.cs
## Language
C#
## Summary
The DefaultInvoiceNumberGenerator class generates invoice numbers by checking the last transaction's invoice number. It appends the current year and month to create a new invoice number.
## Changes
Added conditional check for year and month mismatch, setting 'invoiceNumberToIncrease' to zero if they don't match the current time.
## Purpose
Generates unique invoice numbers based on last transaction and current date.
