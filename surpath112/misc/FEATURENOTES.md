---- To test
Notifications / Email Alerts

------------- Task List by Priority:
User signup (must pick cohort, dept, and provide pids)
Public site
Tenant DNS integration



------------- Features PushList:

Library
Messaging
Compliance engin logic / Biz Rules
File Upload / Donor View
Approval / Rejection

Library needs a rework
When you create a lib cat, you can add a file - by editing the record - which takes you to create new record.
This page, though, needs to *know* we're adding a library document.
So it needs a copy of this page - and passing in the library category.  Also, the entity needs a description.

Need to create a message entity
Needs everything that chat does, but the app service should send notifications to users.

Need to modify the document upload so they pick the category before uploading.
(upload first then assign, not done.)

Make donor land on donor view page, no dashboard (at this moment).

Need the approval / rejection page for surpath




------------------------------------------------------------------------------


From Meeting on 6-28-2022


---- Auto report
-- To school

--- To the student - a weekly scheduled by the school to each student that has any records in selected status..


--- On requirements:

------ Option to edits of requirement instructions - an approval process where when school edits it -
it goes to awaiting approval, where surscan can make sure it matched the website then his "publish"
In this way, we can ensure that the users of the system are not putting conflicting information out to the students

--- 
Rquirements - if student notified requirement for login within x hours or resend the notication.


-----------
It possible to have one record for two requirements - 
Example - Heb document has initial and titus in same document.
- for surscan, modify a requiement to an existing document so if both requirements are on diffent pages of the same upload, we can note each and accept or reject


--- Nice to have - OCR upload so searchable?
examples:
pictures of blurry or clear to explain to student what's acceptable


----- Alternative names (maiden name)
------- A type of record the users would upload or we could upload that would be 
------- category of record that the user can alway upload (DL, Passport, Visa, marriage license, divorse)
---------  Surscan needs the ability to inject a reequirement on a per user basis for compliance

--- In rules - we need a "depends on subsequent" - Moves to non-compliant if dependant is rejected.


--- On Requirements, a "Submitter instructions", and a when they try to upload, we show that as a modal and the user has to check a box saying "I have read this"
----- and we log the user did it.


--- anytime status changes, a reason must be supplied
-----

---- We need a status change reason drop down on record status for SURSCAN - Where a canned reason can be applied.
Example: Met requirement. did not meet requirement
A permission reequired to see on record state change


- but for schools, they must enter a reason.

The drop downs should be an entity and editable by surscan.

A reason is required to change status.

--- Widget for tenant initiated status changes with the ability to add internal note



---------------- dallas college 
christy carter

dallas college - radiology most complete
nursing is most problamatic

--- Check Fido Alliance for sign in.

---- Notes from 6/21 call:

Filter by holistic view

-- permissions 
-- User Prefilters
-- Non Compliance Widge a must for Friday - filterable

-- cohorts

by dept
by cohort
by compliance


dept by cohort

Add a feedback place for feature requests - We Want entity


Modify rules so each rule row can be added - and each has "notify, sms, email"

Modify document types so if they change, a rule is applied

Migrator does NOT like environment variables

------------- Features to do:
Tenant Library
Create DNS record when setting up Tenant
Tenant archiving of students
Can we do customizations as an edition?
Restrict what permissions a tenant can assign to roles, etc.



-------------- Notes from 7/7/22 Review of Feedback
For nursing, for example:
 Program -> Campus -> Cohorts

Health services will be different.


Division
program
group (aka campus)
Cohorts

Divisiions
Programes
Groups (campus or a dicsicpline)
Cohorts


Last first corhort - Then view specific data - then action buttons

Error in new / edit cohort

Records are not being grouped correctly - they should be ordered by last name, then grouped by user

Requirements need to be optionally be assigned at either organization/ division / program level / group level
Type of requirement- so deepest requirement applies

Archive all other records when a document it accepted.

If rule has expires, require date of expiration to set to approved and notify dates work backwards from that date

Annual / Sdcheduled checkbox for TOS / Be professional reminders.

- On Notifications - see if we can pull user preferences for SMS / Email - and offer to select those.

Library needs to be per program or division - 

"Chair" is a role - for the PROGRAM

Permission by record category - dynamically generate?

Default cohort - PER program

Widget users not in any active cohort


ADA WDAC -> https://accessibe.com/


Features:
https://aspnetboilerplate.com/Pages/Documents/Feature-Management#checking-features



------------------------------------------------------------------------------

We have background checks, and drug screen, and compliance tracking services
-- These services have a System Requirements that cannot be viewed or edited by Tenant
--- These should be applicaable to dept and cohorts

Products to sell are services
-- Host should have pricing
--- Tenant can have own pricing or discount

Services show up as cart items
-- User can add to cart
--- checkout they pay via credit card
--- On successful payment, a leger entry is added
---- Items bought have a ledgerdetail item with expiration

-- There is a shopping cart view model
-- there is a paymentsubmission view model
-- there is a paymentmanager core service
-- there is a paymentappappservice app service
-- there is a usersubscriptionservice


-- A LedgerEntry will contain transaction info
-- A LedgerEntryItem will contain the service and expiration date of the service



1) Do the services so we have somethign for the cart
1a) Display will be feature enabled and invoice client / donorpay + active subscription
2) Services should be mapped to features enabled for the client?
2b) Do we create a dropdown of features and map the feature name to the tenant?

3) When getting session - if ClientPaymentType is donorpay, get the donor's ledger for the requested feature / service 
-- or do I do this at the app level?


if the tenant has donorpay paymenttype...

Version 2 of payment
 Do they have an active subscription for the service?
 If not


------------------------------------------------------------------------------


The "view"

Every requirement category they have to supply

This needs to look at cohortuser, and all requirements that apply and list them.
Every requirement will have a category, but they may have more than one - list them indented?

If no upload - show "No progress"
if uploaded - show recordstate

Which brings up - notes and record states.
A record state is 1:1 with a record.

When a user uploads a record for an existing record state by category, that record state needs to be updated to point to the *NEW* record, and the old record soft deleted
Add a note that 'user replaced record on x date'

------------------------------------------------------------------------------
3-9-2023
Tevis in oCCC is admin but being asked to pay but shouln't

Items Needed

	1.	Payment 
			- Admin still be billed (Resolved)
			- When opening a student's profile, a payment button
				- opens payment panel
					- Checkbox for "payment owed"
						- enter an amount next to each service enabled, checkbox
							- Forces user to payment
							- For school, everything is hidden with message "Account Unavailable due to non-payment, please have user contact surscan"
					- Manual transaction
						- Show all due broken down by service
							- Enter amount, transactionID, name on Credit Card
						- Alert if amount is more or less than all due
							*** need partial payment in ledger for payments in cash and credit cards - cash transactions would be put in memo of transaction
					- Maybe payment review
						- History
					- Should show when services paid expire
					-- On account balance that rolls forward
	2.	Compliance Report Fix
		- fix this
	3.	Ability to see SSN in donor information edit page and export spreadsheet for background check
		- Edit profile on VCH, needs to to donor
			- Under DOB, list all PIDS
			- On cohorts, when looking at members, the export to excel, include PIDs in export
			- Add 2nd export to excel that would include data and pids, not compliance report
				- fn mn ln ssn dob address suite city state zip
	4.	Department Assignment - Faculty
		- See below
	5. Expiration Date Input / Administered Date / Covid Brand Type
		- Covid Brand Type
		- Expiring soon
		

ViewCohortUsers, see previous versions
a button to show previous versions
- show dropdown below documents ordered by note


Cohort page - 
We don't default cohort
- but a drop down for tenants, then a drop down for tenant dept




Grayson EMS Excel export - 

Cohort Users - move to 25 results default at a time, not 5000

--- Cohorts, child tables to 250 results default

--- Active user dept membership should limit all search results 

---- Dpt users needs first lastname as well, Tenant / Tenant Dept filters

---- Need a page for dept visibility (to edit / set, etc) , by default can see dept a member of

---- PID page should be clear that PIDS are global and they don't need to be added for tenants
----- Remove all the old PID Types except global
----- Set all to required (SSN)


------- Dept Limiting
- A dept user would be considered a faculty member, so their visibilty for documents and users to only that dept
-- Create a Dept Faculty Role

------- Coppel High School - paying tomorrow

----- Dept Setting for auto assign cohort
Defatult cohort by dept - rearrange the cohorts table to show that to the right.
Then, let a corhort be assigned to a dept as default
Then, on edit dept, have a checkbox to "automatically assign new registrations to the corhot:" and show the name of the depts default cohort

=== Maybe a way to move all members to new cohort function?

Review Queue changes - we need FIFO check order
--- Review Queue needs the same dept filter
---- REview queue - limit host user to assigned tenant + depts all or selected
---- Review queue show the age of the document, remvoe state and user name
action tenant dept requirement name, limited in business days



------------------------------------------------------------------------------
