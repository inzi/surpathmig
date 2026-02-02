# Registration Process E2E Test Plan

These flows can be executed against any tenant with self-registration enabled. Use seeded data in the default tenant when running locally.

## 1. Zero / Invoice Registration
1. Ensure the target tenant’s donor-pay flag is **disabled** and at least one service is invoiced/zero-cost.
2. Start the registration wizard and fill out Steps 1–3 with valid data.
3. Observe that Step 4 shows **“Register Now”** and the primary CTA stays enabled – no modal is required.
4. Submit the form. The server should skip payment orchestration and immediately show `RegisterResult`.
5. Verify in the database that the new user has `IsPaid = true`, `IsActive = true`, and **no** ledger entry was created.

## 2. Donor-Pay Capture (Happy Path)
1. Enable donor-pay for the tenant and ensure services have non-zero prices.
2. Walk through the wizard until Step 4; confirm the CTA reads “Checkout” and opens `_PurchaseModal`.
3. Complete Accept.js/tokenization and submit the registration.
4. After the server response, confirm the thank-you page appears with the user logged in (if email confirmed).
5. Validate back-end effects:
   - `LedgerEntries` contains an entry for the user with `AmountDue = 0` and `PaidAmount` equal to the captured amount.
   - `LedgerEntryDetails` contains line items that sum to the `TotalPrice`.
   - No extra captures were scheduled (one transaction in Authorize.Net console or fake gateway log).

## 3. Compensation / Double-Authorization Protection
1. Keep donor-pay enabled but configure the payment gateway (or the new test double) to **force a capture failure** after Accept.js succeeds.
2. Submit the wizard with valid data and card token.
3. The UI should stay on the register page and surface the friendly “Payment Failed” error without duplicating charges.
4. Validate back-end effects:
   - The pending user record exists but has `IsActive = false` and `IsPaid = false`.
   - The fake gateway (or Authorize.Net logs) shows a **void** for the failed transaction id.
   - No ledger entry is written.

Document the tenant, timestamp, and transaction ids each time these checks are executed to keep the audit trail current.
