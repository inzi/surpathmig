Data Structure


A record requirement describes the requirement.

A record state is the state of a record.

A record status is the type of statuses available and their color.

Record Notes are notes associated with a record.

A record category describes the type of record.

A record category rule defines the rules around a category, such as expiration, notification rules, etc.




A user has an account

A corhort has Cohort Users.

A cohort user has an account and records.


A record category has
	A requirement
	A rule
	
	



The record state is the key.
	It has a record.
		A record has the file
		The category of the record
			The category has a requirement
			The category has a rule
			The category has instructions
	It has a record status.
	
Getting a cohort user's compliance state needs to
		Get all record states for the cohort user
			Get the category for the record
			


Requirements are all requirements that are not specific to a dept or a cohort
plus
all requirements that are specific to a cohort or a dept that the user is in.
plus
all requirements that are specific to a user.

