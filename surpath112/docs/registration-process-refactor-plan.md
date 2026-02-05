# Registration Process Refactor Plan

## Phase 1 - Front-load Validation & API Accessibility
- [x] Refactor `src/inzibackend.Web.Mvc/Controllers/AccountController.cs` to centralize pre-submit validation (email, username, department, cohort) into a reusable service invoked before any payment logic.
- [x] Harden the anonymous validation APIs in `src/inzibackend.Application/Surpath/ComplianceManager/SurpathComplianceAppService.cs` with tenant scoping, rate limiting, and audit logging so the wizard can trust them without inviting abuse.
- [x] Update `src/inzibackend.Web.Mvc/wwwroot/view-resources/Views/Account/Register.js` to call the centralized validation endpoint(s) prior to launching the payment modal and to surface validation failures inline so users never initiate payment with bad data.

## Phase 2 - Payment Orchestration & Transaction Safety
- [x] Introduce an orchestration layer (for example, `RegistrationPaymentCoordinator` under `src/inzibackend.Application/Surpath`) that owns pre-auth, user creation, ledger creation, and capture, ensuring all database work commits before capture finalizes.
- [x] Modify the POST workflow in `src/inzibackend.Web.Mvc/Controllers/AccountController.cs` to delegate to the orchestrator, eliminate direct calls to `_authNetManager.CapturePreAuthCreditCardRequest`, and add compensation (void/refund) handling on failure.
- [x] Extend `src/inzibackend.Core/Surpath/AuthNet/AuthNetManager.cs` with helpers for voiding/canceling pre-auths and return rich results so the orchestrator can deterministically respond to gateway outcomes.

## Phase 3 - Zero / Invoice Payment Path
- [x] Compute the authoritative amount due on the server (via `SurpathPayManager` or the orchestrator) and pass it through `src/inzibackend.Web.Mvc/Views/Account/Register.cshtml` so the wizard knows when the balance is zero.
- [x] Adjust `Register.js` (`src/inzibackend.Web.Mvc/wwwroot/view-resources/Views/Account/Register.js`) so the primary CTA reads "Register Now" when the amount due is less than or equal to zero and skip opening `_PurchaseModal` for that path.
- [x] Update `AccountController.Register` POST to bypass payment orchestration when the amount due is zero or invoiced, persist the zero-payment outcome, and mark the user as paid/active accordingly.

## Phase 4 - Logging & PCI Hygiene
- [x] Review adjacent touchpoints (`PurchaseAppService`, `_PurchaseModal.js`, telemetry hooks) to confirm payment tokens and card details are never echoed to logs or analytics.
- [x] Audit `src/inzibackend.Core/Surpath/AuthNet/AuthNetManager.cs` logging to remove raw request/response payloads and replace them with masked summaries (card last four, token identifiers).
- [x] Introduce shared logging helpers (for example, under `src/inzibackend.Core/Surpath`) to guarantee no subsystem writes PII or opaque payment tokens to persistent logs.


## Phase 5 - Ledger Integrity & Data Types
- [x] Update `src/inzibackend.Core/Surpath/SurpathPayManager/SurpathPayManager.cs` to preserve decimal precision (replace `(long)amount` with a decimal-safe structure and update entities/migrations as needed).
- [x] Add integration tests under `test/` verifying that paid amounts, ledger totals, and due balances align exactly (no truncation, no rounding errors).

## Phase 6 - Endpoint Hardening & Anti-forgery
- [ ] Restrict `PurchaseAppService.PreAuth` (`src/inzibackend.Application/Surpath/Purchase/PurchaseAppService.cs`) to verified tenant contexts, enforce CSRF/anti-forgery tokens, and throttle repeated attempts to block scripted misuse.
- [ ] Add an anti-forgery token to the registration form (`src/inzibackend.Web.Mvc/Views/Account/Register.cshtml`) and include it in Accept.js submissions so the server can validate the origin of payment requests.
- [ ] Document the new throttling and security requirements in `docs/` so future changes maintain the hardened posture.

## Phase 7 - Regression Coverage & Documentation
- [x] Add unit and integration tests (for example, under `test/inzibackend.Application.Tests`) covering validation failures, successful capture flows, compensation paths, and zero-balance registrations.
- [x] Update `docs/registration-process-audit.md` with a status appendix summarizing completed remediation as phases ship.

## Phase 8 - E2E UI Tests
- [x] Author end-to-end UI test scripts or manual test plans verifying that zero-balance flows bypass payment, donor-pay flows capture exactly once, and double-authorization regressions are prevented.
