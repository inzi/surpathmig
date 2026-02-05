# Registration Process Audit

## Flow Summary
- **Entry point** – `AccountController.Register` builds the registration wizard with tenant service data, donor-pay flag, and zeroes out donor-pay hidden fields when the tenant is not charging upfront (`src/inzibackend.Web.Mvc/Controllers/AccountController.cs:487`, `src/inzibackend.Web.Mvc/wwwroot/view-resources/Views/Account/Register.js:480`).
- **Client wizard** – `Register.js` now performs asynchronous email uniqueness checks before allowing Step 1 to advance and still opens the payment modal when donor-pay is enabled (`src/inzibackend.Web.Mvc/wwwroot/view-resources/Views/Account/Register.js:56`, `src/inzibackend.Web.Mvc/wwwroot/view-resources/Views/Account/Register.js:325`).
- **Payment modal** – `_PurchaseModal.js` gathers card data, runs Accept.js, and calls `PurchaseAppService.PreAuth` before the main registration POST; zero-balance paths short-circuit the pre-auth but still require the modal (`src/inzibackend.Web.Mvc/wwwroot/view-resources/Views/Payment/_PurchaseModal.js:64`, `src/inzibackend.Application/Surpath/Purchase/PurchaseAppService.cs:304`).
- **Server-side registration** – the POST action creates the user, enforces email uniqueness, assigns departments/cohorts, conditionally captures the prior authorization, and attempts immediate login (`src/inzibackend.Web.Mvc/Controllers/AccountController.cs:626`, `src/inzibackend.Web.Mvc/Controllers/AccountController.cs:686`).

## Major Issues

### P0 – Payment capture still lacks rollback/compensation
- The controller now delays capture until after department/cohort assignments, but `CapturePreAuthCreditCardRequest` is still invoked before ledger creation, user activation, and the final view (`src/inzibackend.Web.Mvc/Controllers/AccountController.cs:700`).
- Any failure after capture (ledger persistence, downstream services, unexpected exceptions) ends up in the catch block while leaving the authorization in a captured state (`src/inzibackend.Web.Mvc/Controllers/AccountController.cs:734`, `src/inzibackend.Web.Mvc/Controllers/AccountController.cs:804`).
- There is no call to void/refund the transaction or to flag the partial state for recovery, so users can still be billed without a complete account.
- **Recommendation:** introduce a coordinator that captures only after all post-registration work succeeds, or adds compensation (void) logic whenever an exception occurs after capture.

### P1 – Zero/invoice registrations remain tied to the payment modal
- Donor-pay configuration continues to replace the submit button with “Checkout,” forcing every registrant through `_PurchaseModal` even when the backend detects a zero balance (`src/inzibackend.Web.Mvc/wwwroot/view-resources/Views/Account/Register.js:325`, `src/inzibackend.Web.Mvc/Controllers/AccountController.cs:718`).
- Zero-balance flows skip the actual capture (`hasPaymentToken` guard) but users must still open the modal and click a faux “Register” button inside the modal to proceed, leaving room for confusion and abandonment.
- **Recommendation:** surface the computed amount due to the wizard and bypass the modal entirely when the amount is ≤ 0, so the primary submit button can be re-enabled.

### P1 – Pre-authorization still precedes final server validation
- Accept.js pre-authorization is triggered from the modal before the POST action validates the full registration payload (username uniqueness, department assignments, etc.) (`src/inzibackend.Web.Mvc/wwwroot/view-resources/Views/Payment/_PurchaseModal.js:110`, `src/inzibackend.Application/Surpath/Purchase/PurchaseAppService.cs:304`).
- Recent changes expose anonymous email/username checks and the controller now re-validates email uniqueness (`src/inzibackend.Application/Surpath/ComplianceManager/SurpathComplianceAppService.cs:663`, `src/inzibackend.Web.Mvc/Controllers/AccountController.cs:640`), but other server-side failures (e.g., cohort assignment, ledger creation) still occur after the pre-auth.
- When the POST fails, users must reopen the modal and repeat the Accept.js flow, creating the double-authorization experience that prompted the original report.
- **Recommendation:** run comprehensive validation prior to calling `PreAuth`, and void previously pre-authorized transactions if the registration flow aborts afterwards.

### P1 – Pay button text never flips to “Register” on the main wizard
- Step 4 still hides `#register-submit-btn` in donor-pay scenarios, even when the amount due is zero (`src/inzibackend.Web.Mvc/wwwroot/view-resources/Views/Account/Register.js:325`).
- The user-facing expectation (“Register” vs “Checkout”) is only updated inside the modal, so the primary CTA remains mislabeled on the wizard itself.
- **Recommendation:** reflect the payment state directly on the wizard controls so the next action is obvious without opening the modal.

## Additional Observations
- **Truncated ledger amounts** – `SurpathPayManager.CreateLedgerEntry` still casts the paid amount to `long`, dropping cents and breaking reconciliation (`src/inzibackend.Core/Surpath/SurpathPayManager/SurpathPayManager.cs:145`).
- **PII/payment data in logs** – `AuthNetManager` continues to log serialized request/response payloads that contain cardholder data and opaque tokens (`src/inzibackend.Core/Surpath/AuthNet/AuthNetManager.cs:407`, `src/inzibackend.Core/Surpath/AuthNet/AuthNetManager.cs:552`). This remains a PCI concern.
- **Anonymous pre-auth endpoint** – `PurchaseAppService.PreAuth` is still `[AbpAllowAnonymous]`, allowing scripted payment-token attempts without anti-forgery or rate limiting (`src/inzibackend.Application/Surpath/Purchase/PurchaseAppService.cs:304`).
- **Server-side coercion risks** – the controller still casts `TenantDepartmentId`/`CohortId` to `Guid` without null checks, producing a 500 after capture if a malicious client tampers with the payload (`src/inzibackend.Web.Mvc/Controllers/AccountController.cs:671`).

## Suggested Next Steps
- Introduce a registration orchestration service that validates all input, persists prerequisites, and only captures payment once commits succeed (with void/refund handling on failure).
- Keep the anonymous validation endpoints but also void pre-auths when the POST fails; add tenant-aware throttling and CSRF protection to `PreAuth`.
- Detect zero-due scenarios before the modal and re-enable the “Register” CTA so users are not forced into a no-op payment dialog.
- Scrub payment logging, review PCI obligations, and harden anonymous payment endpoints (rate limits, tenant verification, antiforgery).

## Remediation Status
- **Phase 5** – Decimal-safe ledger surfaces are deployed, the `LedgerDecimalPrecision` migration is ready, and regression coverage locks in paid/amount-due math.
- **Phase 7** – Registration validation/coordination tests now exercise failure, zero-balance, and capture/void paths (`test/inzibackend.Tests/Surpath/RegistrationWorkflow_Tests.cs`).
- **Manual verification** – End-to-end scenarios for zero-balance, donor-pay capture, and compensation are documented in `docs/registration-process-e2e-tests.md` and should be run before hardening (phase 6).
*** End Patch
