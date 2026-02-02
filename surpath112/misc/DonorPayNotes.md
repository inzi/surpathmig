Each ledger entry will exist before payment.

If a user logs in, check if current. If not current, we add ledger entries, and details for all services, they're currently assigned.

Each ledger entry will have a total balance. SHould be zero or positive.

The details entries will also have a balance. This way, an increase if moved from one program to another can be detailed.

There will be a flag on the entry to supress requesting payment.

If donor pay is enabled, an alert will come up asking if supression of donor pay for *existing* donors should be enabled.
If yes, any user that is already registered should not be prompted to pay.  

This is not a flag, but a supress until date time

When a user is flagged to request payment, that date is set to mindate.

---

Is current should pull all active subscriptions for the user, and compare that with the subscriptions for the tenant.  If all active, they are current.

If not, they're presented with a dialog requiring payment.

----

We need a new *external* service object.

This object will require a name, a switch to notify the user, a cost for external service, a flag if price can be altered for a user

Then, on donor page, have the ability to grant external service to user. If the price altered flag can be used, allow the user to edit the price with a note.

That creates the ledger entry for the external service.

----

We'll need a bulk grant option.