# Technical Implementation Plan - BookStore Backend Infrastructure Layer

Mục tiêu tài liệu này là tổng hợp, đánh giá và cập nhật toàn bộ kế hoạch triển khai tầng **Infrastructure & Persistence** cho hệ thống `BookStore Backend` dựa trên các nguyên tắc Production-ready Clean Architecture, DDD và phản hồi nâng cấp kiến trúc (Codex Review).

---

## 📊 Đánh giá & Phân tích Phản hồi (Codex Review Evaluation)

| Hạng mục | Đánh giá | Quyết định & Định hướng giải quyết |
| :--- | :--- | :--- |
| **1. Value Object Mapping** | **Rất chính xác (Giữ nguyên & Áp dụng)** | Tất cả Value Objects 1 giá trị (`Email`, `FullName`, `PhoneNumber`, `PasswordHash`, `UserId`, `RoleId`) dùng **`ValueConverter`**. Tạo thư mục `Persistence/Converters` để tái sử dụng. Không dùng `Owned Types` cho 1-property VO. |
| **2. Backing Field & Navigation** | **Chính xác (Bổ sung)** | Cấu hình tường minh `.UsePropertyAccessMode(PropertyAccessMode.Field)` cho navigation `Roles` (`_roles`). |
| **3. UserRole Primary Key** | **Đã làm rõ (Giữ nguyên Domain & Áp dụng DB Mapping)** | Domain giữ `UserRole : Entity<RoleId>`. Dưới Database map Composite Key `(UserId, RoleId)` thông qua Shadow FK `UserId` + `RoleId`. Vừa bảo tồn Domain model vừa đúng chuẩn Relational DB. |
| **4. Cascade Delete** | **Chính xác (Cải thiện)** | Cấu hình `Cascade Delete` từ `User` xuống `UserRoles`. Khi User bị xoá, `UserRoles` tự động xoá theo. |
| **5. Outbox & DbContext** | **Chính xác (Cải thiện SOLID)** | `DbContext` không trực tiếp serialize JSON hay dùng `AssemblyQualifiedName`. Tách `IOutboxMessageFactory` tạo `OutboxMessage`. `EventTypeName` dùng `Namespace.TypeName`. `ClearDomainEvents()` gọi **SAU** khi `base.SaveChangesAsync()` thành công. |
| **6. DesignTimeDbContextFactory** | **Chính xác (Cải thiện)** | Tách `ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>` thành class độc lập. |
| **7. Repository Interface** | **Chính xác (Cải thiện)** | Đặt `IUserRepository` ở `BookStore.Domain/Users` (hoặc `Application`). Interface nhận strongly-typed VOs: `GetByEmailAsync(Email email)`, `ExistsByEmailAsync(Email email)`. |
| **8. Migration** | **Chính xác (Giữ nguyên)** | Loại bỏ hoàn toàn `EnsureCreated()`, chỉ dùng EF Core Migrations (`Add-Migration` / `Update-Database`). |

---

## 🏗️ Kiến trúc Chi tiết & Các bước Triển khai

### PHASE 1: EF Core Value Converters & Entity Configurations

#### 1.1 Converters (`BookStore.Infrastructure/Persistence/Converters`)
Tạo các `ValueConverter` kế thừa `ValueConverter<TModel, TProvider>` để map 2 chiều giữa StronglyTypedId/ValueObjects và Primitive Types trong DB:
- `UserIdConverter`: `UserId` <-> `Guid`
- `RoleIdConverter`: `RoleId` <-> `Guid`
- `EmailConverter`: `Email` <-> `string`
- `FullNameConverter`: `FullName` <-> `string`
- `PhoneNumberConverter`: `PhoneNumber` <-> `string`
- `PasswordHashConverter`: `PasswordHash` <-> `string`

#### 1.2 UserRoleConfiguration (`BookStore.Infrastructure/Persistence/Configurations/UserRoleConfiguration.cs`)
- Map vào bảng `UserRoles`.
- PK Composite: `(UserId, RoleId)`.
- HasConversion cho `RoleId` bằng `RoleIdConverter`.
- Map property `AssignedAt` (`DateTime`).

#### 1.3 UserConfiguration (`BookStore.Infrastructure/Persistence/Configurations/UserConfiguration.cs`)
- Map vào bảng `Users`.
- PK: `Id` với `UserIdConverter`.
- Value Objects: `Email`, `FullName`, `PhoneNumber`, `PasswordHash` dùng các `ValueConverter` tương ứng.
- Enum & Flags: `Status` (map enum string/int), `TwoFactorMethod` (nullable string/enum converter), `EmailConfirmed`, `FailedLoginAttempts`, `LockoutEndUtc`, `CreatedOnUtc`, `UpdatedOnUtc`.
- Navigation `Roles`:
  - `builder.HasMany(u => u.Roles).WithOne().HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade);`
  - `builder.Navigation(u => u.Roles).UsePropertyAccessMode(PropertyAccessMode.Field);`
- Indexes: `HasIndex(u => u.Email).IsUnique()`.

---

### PHASE 2: Outbox Pattern & ApplicationDbContext

#### 2.1 OutboxMessage Entity & Configuration
- Entity `OutboxMessage`:
  - `Guid Id`
  - `string Type` (`Namespace.TypeName`)
  - `string Content` (JSON Payload)
  - `DateTime OccurredOnUtc`
  - `DateTime? ProcessedOnUtc`
  - `string? Error`
- Configuration `OutboxMessageConfiguration`: Table `OutboxMessages`, Primary Key `Id`, Index trên `ProcessedOnUtc`.

#### 2.2 OutboxMessageFactory (`BookStore.Infrastructure/Persistence/Outbox/OutboxMessageFactory.cs`)
- Interface `IOutboxMessageFactory` & Class `OutboxMessageFactory`:
  - Nhận `IDomainEvent`.
  - Serialize sang JSON (`System.Text.Json`).
  - Trả về `OutboxMessage` với `Type = domainEvent.GetType().FullName`.

#### 2.3 ApplicationDbContext (`BookStore.Infrastructure/Persistence/ApplicationDbContext.cs`)
- Inherit `DbContext`.
- Sets: `DbSet<User> Users`, `DbSet<UserRole> UserRoles`, `DbSet<OutboxMessage> OutboxMessages`.
- Overrride `OnModelCreating`: `builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly)`.
- Override `SaveChangesAsync`:
  1. Thu thập tất cả domain events từ `ChangeTracker.Entries<IHasDomainEvents>()`.
  2. Dùng `IOutboxMessageFactory` tạo các bản ghi `OutboxMessage`.
  3. Thêm `OutboxMessages` vào ChangeTracker.
  4. Gọi `await base.SaveChangesAsync(cancellationToken)`.
  5. Khi save DB thành công, tiến hành gọi `ClearDomainEvents()` trên từng entity để tránh phát sinh duplicate event.

#### 2.4 ApplicationDbContextFactory (`BookStore.Infrastructure/Persistence/ApplicationDbContextFactory.cs`)
- Implement `IDesignTimeDbContextFactory<ApplicationDbContext>` cho lệnh `dotnet ef migrations add`.

---

### PHASE 3: Services & Repositories Implementation

#### 3.1 Domain Services Implementations
- `BcryptPasswordHasher` (`BookStore.Infrastructure/Security/BcryptPasswordHasher.cs`) implement `IPasswordHasher` (BCrypt.Net-Next, WorkFactor 12).
- `SystemClock` (`BookStore.Infrastructure/Services/SystemClock.cs`) implement `ISystemClock`.

#### 3.2 Repositories
- Interface `IUserRepository` (Đã có hoặc thêm tại `BookStore.Domain/Users/IUserRepository.cs`):
  - `Task<User?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default);`
  - `Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);`
  - `Task<bool> ExistsByEmailAsync(Email email, CancellationToken cancellationToken = default);`
  - `void Add(User user);`
  - `void Update(User user);`
- `UserRepository` (`BookStore.Infrastructure/Persistence/Repositories/UserRepository.cs`):
  - Implement `IUserRepository` bằng `ApplicationDbContext`.

---

### PHASE 4 & 5: Dependency Injection & Application Integration

#### 4.1 Dependency Injection Registration (`BookStore.Infrastructure/DependencyInjection.cs`)
- AddDbContext với SQL Server/PostgreSQL/SQLite connection string từ `appsettings.json`.
- Register `IPasswordHasher` -> `BcryptPasswordHasher`.
- Register `ISystemClock` -> `SystemClock`.
- Register `IUserRepository` -> `UserRepository`.
- Register `IOutboxMessageFactory` -> `OutboxMessageFactory`.

---

## 🧪 Verification Plan

### Automated Verification
1. **EF Core Migration Verification:**
   - Chạy `dotnet ef migrations add InitialCreate --project src/BookStore.Infrastructure --startup-project src/BookStore.WebApi`.
   - Kiểm tra file SQL Generated hoặc Migration C# file đảm bảo schema tạo đúng:
     - Foreign Key `UserId` -> `Users(Id)` với `ON DELETE CASCADE`.
     - Bảng `UserRoles` có Composite PK `(UserId, RoleId)`.
     - Bảng `OutboxMessages` với index `ProcessedOnUtc`.
     - Bảng `Users` với `Email` Unique Index.
2. **Build & Integration Test:**
   - Biên dịch thành công toàn bộ Solution (`dotnet build`).

---

## ❓ Open Questions / Clarification

> [!NOTE]
> 1. **Database Provider cho EF Core Migration:** Dự án đang ưu tiên sử dụng Database Engine nào? (Ví dụ: `SQL Server` via `Microsoft.EntityFrameworkCore.SqlServer`, `PostgreSQL` via `Npgsql.EntityFrameworkCore.PostgreSQL`, hay `SQLite` cho dev)?
> 2. **Vị trí của IUserRepository:** Bạn muốn đặt `IUserRepository` ở `BookStore.Domain/Users/IUserRepository.cs` hay ở `BookStore.Application/Common/Interfaces/Persistence/IUserRepository.cs`?
