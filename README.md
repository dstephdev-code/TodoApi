# Basic Task Tracker
Here is simple ToDo API project which I try to lead through different stages.

I'm using C# 14, Microsoft SQL server, EF Core. Github for tracking. Swagger for API testing.

I designed models and relationships in code first. Used **Fluent API** inheriting *IEntityTypeConfiguration* interface for every table I need.

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

To make project as much scalable but not overengineered I divided project into 4 main parts + frontend in future. Models storing basig entities, data access with repository pattern allows service module to work with storage of models independently of way of storing data. And controllers which just make basic rediriction to services logic.

---
*> 05.06.2026* >>> Implemented Create, GetAll and GetById methods.<br>
*> 06.06.2025* >>> Implemented Remove method and extracted logic to separate services module.

*License MIT &copy;2026*