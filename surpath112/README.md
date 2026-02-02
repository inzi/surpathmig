This solution is a multi-tenant asp.net MVC application that uses AspNetZero framework to provide a saas offering to schools to track student compliance with school policies such as drug testing, documentation, background checks, and more.

inzibackend.Core.Shared folder contains consts, enums and helper classes used both in mobile & web projects.
inzibackend.Core folder contains domain layer classes (like entities and domain services).
inzibackend.Application.Shared folder contains application service interfaces and DTOs.
inzibackend.Application folder contains application logic (like application services).
inzibackend.EntityFrameworkCore folder contains your DbContext, repository implementations, database migrations and other Entity Framework Core specific concepts.
inzibackend.Web.Mvc folder contains the presentation/API layer (Controllers, Views, JavaScript files, styles, images and so on) for backend and frontend applications.
inzibackend.Web.Host folder does not contain any web related files like HTML, CSS or JS. Instead, it just serves the application as remote API. So, any device can consume your application as API. For more information see Web.Host Project
inzibackend.Web.Core folder contains common classes used by MVC and Host projects.
