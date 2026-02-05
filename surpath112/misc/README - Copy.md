------ Default Context:
from here: https://github.com/dotnet/efcore/issues/23840

$PSDefaultParameterValues.add('Add-Migration:Context','inzibackendDbContext')
$PSDefaultParameterValues.add('Remove-Migration:Context','inzibackendDbContext')
$PSDefaultParameterValues.add('Update-Database:Context','inzibackendDbContext')

$PSDefaultParameterValues=@{ 'Add-Migration:Context'='inzibackendDbContext';'Update-Database:Context'='inzibackendDbContext';'Remove-Migration:Context'='inzibackendDbContext' }
$PSDefaultParameterValues["Disabled"]=$False
$PSDefaultParameterValues


# ASP.NET ZERO

When merging into release branch, do not merge migration files.

After merge, add a migration for the branch coming in.



This repository is configured and used for AspNet Zero Team's development. 
It is not suggested for our customers to use this repository directly. It is suggested to download a project from https://aspnetzero.com/Download.


cd \dev\Surpathv2\src\inzibackend.Web.Mvc
npm run create-bundles

____________


## Most Recent Release

|  #   |     Status     |  Release Date  |                         Change Logs                          |                          Milestone                           |
| :--: | :------------: | :--------: | :----------------------------------------------------------: | :----------------------------------------------------------: |
| 11.1 | ✔️ &thinsp; **RELEASED** | 2022-02-28 | [Release Notes](https://docs.aspnetzero.com/en/common/latest/Change-Logs) | [Closed-RC](https://github.com/aspnetzero/aspnet-zero-core/milestone/93?closed=1) [Closed-Final](https://github.com/aspnetzero/aspnet-zero-core/milestone/95?closed=1) |

## Current Milestone
|  #   |    Status     |  Due Date  |                          Milestone                           |
| :--: | :-----------: | :--------: | :----------------------------------------------------------: |
| 11.2  | 🚧 &thinsp; In Progress | 2022-04-28 | [Open](https://github.com/aspnetzero/aspnet-zero-core/milestone/94)<br>[Closed](https://github.com/aspnetzero/aspnet-zero-core/milestone/94?closed=1) |

____________
added datatables group row.
Did a yarn add datatables.net-rowgroup
then added that to common scripts bundle (bundles.json)
Then redid rebuild bundles
added dt objects to app - probably could have just added it - test that.
____________
# Github workflow to IIS
Found this -> [Open]https://devopsjournal.io/blog/2020/11/24/github-actions-with-private-runner-iis

Details how to setup workflows to push to your private IIS server.


Serilizing a form to json - for checkboxes:
https://github.com/marioizquierdo/jquery.serializeJSON

To examine:
$('form#checkboxes').serializeJSON({checkboxUncheckedValue: 'NOPE'});
<input type="checkbox" name="checked[b]:boolean"   value="true" data-unchecked-value="false" checked/>
 
However, this ensures progressive enhancement 
<!-- Only one booleanAttr will be serialized, being "true" or "false" depending if the checkbox is selected or not -->
<input type="hidden"   name="booleanAttr" value="false" />
<input type="checkbox" name="booleanAttr" value="true" />

- Tickets of interest
-- https://support.aspnetzero.com/QA/Questions/10469/Security-weakness-to-IMustHaveTenant--IMayHaveTenant


--- Tasks

enum for state


class task
-- Name
-- Type (need enum)
-- Tenant Id
-- user id
-- document id

--- Should I have a type / id entity for this?
---- could hold enum and id to tie back




then milestones

-- Name
-- Step
-- next step?
-- Day start
-- day due
-- day completed


---

On testing Onsite / Offsite

----
dallas college 
christy carter

dallas college - radiology most complete
nursing is most problamatic


--- Elsa Integration:
https://docs.aspnetzero.com/en/aspnet-core-mvc/latest/Core-Mvc-Elsa-Integration#create-project


--- Microsoft's Rules Engine w/ Entity Framework
https://github.com/microsoft/RulesEngine

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




-------------  Good anz support articles
To keep upgradable, this approach to seed engine - interesting approach

https://support.aspnetzero.com/QA/Questions/10153/Documentation-for-initializing-new-tenants

replace the blob storage - 
- this example used azure, but file system ought to be possible
https://support.aspnetzero.com/QA/Questions/8497/Azure-blob-storage-manager

Using filesystem:
https://support.aspnetzero.com/QA/Questions/1940/File-Upload-Service

Angular thread:
https://support.aspnetzero.com/QA/Questions/10519/File-upload-and-storage-Eg-PDFs-JPGs-etc


A per tenant setting?
https://support.aspnetzero.com/QA/Questions/6812/Configuration-for-saving-images-to-a-database-or-at-a-specific-path-at-the-Tenant-level

User doing multi file uploads:
https://support.aspnetzero.com/QA/Questions/1940/File-Upload-Service

Automated permission generation
https://support.aspnetzero.com/QA/Questions/996/Automate-the-creation-process-of-AppPermissions


Good idea to kick off migrator:
https://support.aspnetzero.com/QA/Questions/11103/CI-CD-Migrator


--------- Data Tables - style the rows

https://datatables.net/forums/discussion/28998/add-class-to-cell-when-using-server-side-processing


---------- Metronic

FontAwesome Fonts
https://preview.keenthemes.com/metronic/demo1/features/icons/fontawesome5.html


---------- The stat bar inspiration
http://jsfiddle.net/j08691/p6RZQ/






$PSDefaultParameterValues




------------ Maybe a way to, if needed, grab only certain data attributes.
https://stackoverflow.com/questions/16230878/get-all-key-value-pairs-from-data-attr-that-have-the-same-prefix

Basically, data() gives back an object

or store a whole object?
https://stackoverflow.com/questions/8542746/store-json-object-in-data-attribute-in-html-jquery#:~:text=Using%20the%20documented%20jquery%20.data%20%28obj%29%20syntax%20allows,entire%20object%20can%20be%20returned%20with%20.data%20%28%29.


horizontal bar chart:
https://codepen.io/jamiecalder/pen/NrROeB


This is a good post about a anonymous api access using a token:
https://support.aspnetzero.com/QA/Questions/10883/Add-Custom-Header-and-handle-it-Custom-AppServiceBase


Generate ids and names for objects:
https://stackoverflow.com/questions/17665426/selectlistitem-with-data-attributes
Effectively - name="@Html.NameFor(Function(model) model.CityId)"
        id="@Html.IdFor(Function(model) model.CityId)"

Actual implementation:
                                    var _id = Html.IdFor(r => Model.RecordStateRecordStatusList[_idx].Id);
                                    var _name = Html.NameFor(r => Model.RecordStateRecordStatusList[_idx].Id);


Grant a permission to all users
INSERT INTO AbpPermissions
SELECT GETDATE() AS CreationTime, NULL AS CreatorUserId, 'UserPermissionSetting' AS Discriminator, 1 AS IsGranted, 'PermisisonName' AS Name, TenantId, NULL AS RoleId, Id AS UserId FROM AbpUsers



Reload a modal:
https://support.aspnetzero.com/QA/Questions/8726/Refresh-Modal-ViewModel-while-inside-Modal


Serialize form to json name formatting:
https://github.com/marioizquierdo/jquery.serializeJSON


Soft Delete - Good post
https://www.ryansouthgate.com/2019/01/07/entity-framework-core-soft-delete/


Replace a service:

https://support.aspnetzero.com/QA/Questions/8746/Replacement-of-services-by-injection



--- jquery-timepicker
https://www.jonthornton.com/jquery-timepicker/