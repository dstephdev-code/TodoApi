# Basic Task Tracker
Here is simple ToDo API project which I try to lead through different stages.

I'm using C# 14, Microsoft SQL server, EF Core. Github for tracking. Swagger for API testing.

I designed models and relationships in code first. Used **Fluent API** inheriting *IEntityTypeConfiguration* interface for every table I need.

Use FluentValidation for client/controller validation, xUnit and NSubstitute for making tests.

### For task I use these fields
> Name <br>
 Description <br>
 Date of creation <br>
 Expected completion date <br>
 Date of last task update <br>
 Status of task (created, in progress, completed and canceled for now) <br>
 Priority of task (low, mid, high)

 ### For user model
 > First name <br>
  Last name <br>

Id is generated on .NET side, type of Guid. Enums are stored on DB side as strings. For linking task and user I created table Task Assignment which for now just stores id's of pair *task - assigned user*.

---
*> 05.06.2026* >>> Implemented Create, GetAll and GetById methods.<br>
*> 06.06.2026* >>> Implemented Remove method and extracted logic to separate services module.<br>
*> 08.06.2026* >>> Implemented Patch method. Now we have all CRUD-ops but without validation and logic.<br>
*> 26.06.2026* >>> Added fluent validation for create and patch methods and tests with xUnit. Reorganized project

*License MIT &copy;2026*