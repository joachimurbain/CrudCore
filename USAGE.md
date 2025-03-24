# 🧰 Usage Guide – CrudCore

This is a quick example of how to use **CrudCore** to wire up a fully working CRUD API with validation and optional partial updates.

---

## 🧱 Entity

```csharp
public class Product : IEntity, IValidatable, IPatchable<ProductUpdate>
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public decimal Price { get; set; }

    public void Validate()
    {
        if (Price < 0)
            throw new ValidationException("Price must be positive.");
    }

    public void ApplyPatch(ProductUpdate patch)
    {
        Name = patch.Name ?? Name;
        Price = patch.Price ?? Price;
    }
}
```

```csharp
public class ProductUpdate
{
    public string? Name { get; set; }
    public decimal? Price { get; set; }
}
```

---

## 🔧 Repository & Service

```csharp
public interface IProductRepository : IRepository<Product> { }

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(DbContext ctx) : base(ctx) { }
}

public interface IProductService : IBaseService<Product> { }

public class ProductService : BaseService<Product>, IProductService
{
    public ProductService(IProductRepository repo) : base(repo) { }
}
```

---

## 🌐 Controller

```csharp
[Route("api/[controller]")]
public class ProductController : BaseEntityController<Product>
{
    public ProductController(IProductService service) : base(service) { }
}
```

---

## ⚙️ Dependency Injection

```csharp
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
```

---

✅ That’s it — you now have a complete, fully working CRUD API for `Product`:
- With optionaL validation
- With optional patch support
- Without writing the same boilerplate twice
