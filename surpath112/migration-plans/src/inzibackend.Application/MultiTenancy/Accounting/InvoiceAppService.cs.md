# Modified
## Filename
InvoiceAppService.cs
## Relative Path
inzibackend.Application\MultiTenancy\Accounting\InvoiceAppService.cs
## Language
C#
## Summary
The modified code introduces additional fields for edition details in the GetInvoiceInfo method. It also includes formatting changes for host and tenant information using Replace and Split methods. In the CreateInvoice method, it corrects a typo by changing Getaways to GetInvoiceNoAsync and updates the payment's InvoiceNo after inserting the new invoice.
## Changes
1. Added EditionDisplayName in the returned InvoiceDto in GetInvoiceInfo.
2. Updated GetInvoiceInfo to use Replace and Split for host and tenant addresses.
3. In CreateInvoice, corrected method name from Getaways to GetInvoiceNoAsync.
4. Updated payment.InvoiceNo after inserting new invoice.
## Purpose
The modified code enhances data consistency by including edition details, corrects a typo, and ensures proper handling of payment records when creating invoices.
