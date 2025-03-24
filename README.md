# CrudCore 🧱

A minimal, reusable base layer for building CRUD APIs in ASP.NET Core.

This project contains foundational building blocks like:
- BaseRepository<T> and IRepository<T> — for clean, reusable data access logic
- BaseService<T> and IBaseService<T> — for organizing business rules and service-layer behavior
- BaseDtoController<TDto, TEntity, ...> and BaseEntityController<TEntity> — generic RESTful endpoints, with or without DTOs
- Interfaces like IEntity, IValidatable, and IPatchable — to support ID-based access, validation, and partial updates

---

### Why this exists

I had a project with a very short deadline, and I couldn’t afford to waste time rewriting CRUD logic for every entity.
CrudCore was born out of necessity, to help me move fast, keep things clean.

---

## ⚙️ How to Use

See [USAGE.md](./USAGE.md) for a full example with code snippets.
