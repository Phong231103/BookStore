# BookStore Backend — Knowledge Base

> **Cập nhật lần cuối:** 2026-07-30
> **Trạng thái:** Domain Layer hoàn thành 100% · Infrastructure chưa bắt đầu · Application trống

---

## 1. Mục tiêu dự án

- **Không phải demo** — production-ready BookStore Backend.
- Có thể dùng làm **template** cho các dự án .NET sau này.
- Ưu tiên **khả năng mở rộng** và **tính nhất quán** hơn tối ưu hoá sớm (YAGNI).

---

## 2. Tech Stack

| Lớp | Công nghệ |
|---|---|
| Runtime | .NET 8 / C# |
| Web | ASP.NET Core Web API |
| Architecture | Clean Architecture + DDD + CQRS |
| Mediator | MediatR 12.x |
| ORM | EF Core 8 (SQL Server) |
| Validation | FluentValidation 12.x |
| Cache | Redis (StackExchange.Redis) |
| Messaging | MassTransit + RabbitMQ |
| Email | MailKit |
| Storage | Azure Blob Storage |
| Logging | Serilog (Console + File) |
| Auth | JWT Bearer |
| API Versioning | Asp.Versioning.Mvc 8.x |
| Mapping | AutoMapper 16.x |
| Docs | Swashbuckle (Swagger) |

---

## 3. Triết lý thiết kế (Đã chốt — KHÔNG thay đổi)

| Nguyên tắc | Chi tiết |
|---|---|
| **Pure Domain** | Domain không phụ thuộc EF Core, ASP.NET Core, MediatR, BCrypt |
| **No DateTime.UtcNow** | Domain dùng ISystemClock, không gọi trực tiếp |
| **No hash in Domain** | Domain chỉ chứa PasswordHash (value object), Infrastructure mới hash |
| **StronglyTypedId chỉ Guid** | Không hỗ trợ int hoặc string |
| **ValueObject không dùng record** | Dùng class + override Equals/GetHashCode |
| **Entity không có Domain Events** | Chỉ AggregateRoot mới raise events |
| **Aggregate Reference by Id** | Không giữ object reference của Aggregate khác |
| **Không có AggregateRootBase** | Chỉ có AggregateRoot<TId> |
| **Không leak data trong Exception** | Message chung chung, không in dữ liệu user |

---

## 4. Kiến trúc tổng thể

```
BookStore.slnx
└── src/
    ├── BookStore.Domain          ← Pure Domain, không dependency ngoài
    ├── BookStore.Application     ← CQRS, MediatR, FluentValidation
    ├── BookStore.Infrastructure  ← EF Core, Redis, RabbitMQ, BCrypt
    ├── BookStore.WebApi          ← ASP.NET Core, Controllers, Swagger
    └── BookStore.SharedKernel    ← (trống, dự phòng)
```

### Dependency Flow (chiều phụ thuộc một chiều)
```
WebApi → Application → Domain
Infrastructure → Application → Domain
```

---

## 5. Domain Kernel (BookStore.Domain.Common) — ĐÓNG BĂNG

### 5.1 Interfaces (Common/Intefaces/)

| File | Nội dung |
|---|---|
| IDomainEvent | Guid EventId, DateTime OccurredOnUtc — không phụ thuộc MediatR |
| IHasDomainEvents | IReadOnlyList<IDomainEvent> DomainEvents, void ClearDomainEvents() |

### 5.2 Primitives (Common/Primitives/)

| Class | Mô tả | Ghi chú |
|---|---|---|
| DomainEvent | Abstract base, tự sinh EventId = Guid.NewGuid(), OccurredOnUtc = DateTime.UtcNow | Implements IDomainEvent |
| DomainException | Abstract base cho Business Exception | Không chứa HTTP/ErrorCode/StatusCode |
| ValueObject | Abstract, equality theo components | Có ==, !=, Equals(), GetHashCode(), GetEqualityComponents() |
| Entity<TId> | where TId : StronglyTypedId, equality theo Id | Không có Domain Events |
| AggregateRoot<TId> | Kế thừa Entity<TId>, implement IHasDomainEvents | Có AddDomainEvent(), ClearDomainEvents() |

### 5.3 Identifiers (Common/Identifiers/)

```csharp
public abstract class StronglyTypedId
{
    protected StronglyTypedId(Guid value)   // ArgumentOutOfRangeException nếu Empty
    public Guid Value { get; }
    // Equals, GetHashCode, ToString đều sealed
}
```

### 5.4 Domain Services (Common/Services/)

| Interface | Contract |
|---|---|
| IPasswordHasher | string Hash(string password), bool Verify(string password, string passwordHash) |
| ISystemClock | DateTime UtcNow { get; } |

---

## 6. User Aggregate (BookStore.Domain.Users) — HOÀN THÀNH

### 6.1 Cấu trúc folder

```
Users/
├── User.cs              ← State, Constructor, Factory, Helpers
├── User.Account.cs      ← ConfirmEmail()
├── User.Role.cs         ← AssignRole(), RevokeRole()
├── User.Secutiry.cs     ← ChangePassword(), RecordFailedLogin(), RecordSuccessfulLogin(), EnableTwoFactor(), DisableTwoFactor()
├── User.Lifecycle.cs    ← Deactivate(), Reactivate()
├── ChildEntity/
│   └── UserRole.cs
├── Enums/
│   ├── UserStatus.cs
│   └── TwoFactorMethod.cs
├── Events/              ← 10 Domain Events
├── Exceptions/          ← 11 Domain Exceptions
├── Identifiers/
│   ├── UserId.cs
│   └── RoleId.cs
└── ValueObjects/
    ├── Email.cs
    ├── FullName.cs
    ├── PasswordHash.cs
    └── PhoneNumber.cs
```

NOTE: File User.Secutiry.cs có typo (thừa chữ 'u'), thực tế là Security behavior.

### 6.2 User Properties (State)

| Property | Type | Ghi chú |
|---|---|---|
| Id | UserId | Từ AggregateRoot<UserId> |
| Email | Email (VO) | |
| PasswordHash | PasswordHash (VO) | |
| FullName | FullName (VO) | |
| PhoneNumber | PhoneNumber (VO) | |
| Status | UserStatus | Mặc định Active khi tạo |
| EmailConfirmed | bool | Mặc định false |
| TwoFactorMethod | TwoFactorMethod? | null = 2FA tắt |
| IsTwoFactorEnabled | bool (computed) | => TwoFactorMethod is not null — KHÔNG lưu riêng |
| FailedLoginAttempts | int | Mặc định 0 |
| LockoutEndUtc | DateTime? | null khi không bị khóa |
| CreatedOnUtc | DateTime | Immutable sau khi tạo |
| UpdatedOnUtc | DateTime | Cập nhật qua Touch() |
| Roles | IReadOnlyCollection<UserRole> | Backing field _roles |

### 6.3 Factory

```csharp
public static User Register(UserId id, Email email, PasswordHash passwordHash,
    FullName fullName, PhoneNumber phoneNumber, RoleId defaultRoleId, DateTime createdAt)
```
- Là nơi duy nhất tạo User.
- Constructor là private.
- Tự thêm 1 role mặc định.
- Raise UserRegisteredDomainEvent.

### 6.4 Business Methods

| Method | File | Invariant |
|---|---|---|
| ConfirmEmail(DateTime) | Account | Idempotent — không làm gì nếu đã confirmed |
| AssignRole(RoleId, DateTime) | Role | Throw DuplicateUserRoleException nếu đã có |
| RevokeRole(RoleId, DateTime) | Role | Throw CannotRemoveLastRoleException nếu chỉ còn 1 role |
| ChangePassword(PasswordHash, DateTime) | Security | Throw InvalidPasswordException nếu same password |
| RecordFailedLogin(int, TimeSpan, DateTime) | Security | Lock khi vượt maxAttempts |
| RecordSuccessfulLogin(DateTime) | Security | Reset failed login state |
| EnableTwoFactor(TwoFactorMethod, DateTime) | Security | Idempotent |
| DisableTwoFactor(DateTime) | Security | Idempotent |
| Deactivate(DateTime) | Lifecycle | Idempotent |
| Reactivate(DateTime) | Lifecycle | Idempotent |

### 6.5 Private Helpers

| Helper | Mục đích |
|---|---|
| Touch(DateTime) | Cập nhật UpdatedOnUtc |
| HasRole(RoleId) | Kiểm tra role đã có chưa |
| FindRole(RoleId) | Tìm UserRole theo RoleId |
| AddRole(UserRole) | Thêm vào _roles |
| RemoveRole(UserRole) | Xóa khỏi _roles |
| ResetFailedLoginState() | Reset FailedLoginAttempts = 0, LockoutEndUtc = null |
| LockUntil(DateTime) | Set LockoutEndUtc |

### 6.6 Domain Event Helpers — Quy tắc

```
Update State → Touch() → Raise Event
```
- Event helper CHỈ gọi AddDomainEvent(...).
- KHÔNG được sửa state trong event helper.

### 6.7 Invariants (đã chốt)

- Email unique → Application kiểm tra (không phải Domain)
- Luôn có ít nhất 1 Role
- Không có Role trùng
- Không đổi sang Password hiện tại
- Chỉ Active + EmailConfirmed mới login được
- Sai login quá số lần → Lock
- Enable 2FA phải cung cấp Method

---

## 7. UserRole (ChildEntity/UserRole.cs)

```csharp
public sealed class UserRole : Entity<RoleId>
{
    public RoleId RoleId => Id;
    public DateTime AssignedAt { get; }
    public static UserRole Create(RoleId roleId, DateTime assignedAt)
}
```

- Association Entity (không phải Aggregate).
- Không chứa Permission.
- Không chứa Role Aggregate.
- User Aggregate chỉ tham chiếu Role qua RoleId.

---

## 8. Identifiers — Pattern chuẩn

```csharp
public sealed class UserId : StronglyTypedId
{
    private UserId(Guid value) : base(value) { }
    public static UserId Create(Guid value) => new(value);
    public static UserId New() => new(Guid.NewGuid());
    public static implicit operator Guid(UserId id) => id.Value;
}
```

| Id | Dùng ở |
|---|---|
| UserId | User Aggregate |
| RoleId | UserRole, User.Role.cs |

---

## 9. Value Objects — Pattern chuẩn

```
private ctor → static Create() → Validate → Normalize → Immutable
```

| VO | Validation | Normalization |
|---|---|---|
| Email | Regex ^[^@\s]+@[^@\s]+\.[^@\s]+$ | Trim().ToLowerInvariant() |
| FullName | MaxLength = 100 | Trim() + Regex.Replace(@"\s+", " ") |
| PhoneNumber | Regex ^\+?[0-9]{8,15}$ | Strip space, dash, parens |
| PasswordHash | NotNullOrWhiteSpace | Không normalize |

---

## 10. Domain Events (10 events)

| Event | Payload |
|---|---|
| UserRegisteredDomainEvent | UserId, Email |
| UserEmailConfirmedDomainEvent | UserId |
| UserPasswordChangedDomainEvent | UserId |
| UserLockedOutDomainEvent | UserId, LockoutEndUtc |
| RoleAssignedToUserDomainEvent | UserId, RoleId |
| RoleRevokedFromUserDomainEvent | UserId, RoleId |
| TwoFactorEnabledDomainEvent | UserId, TwoFactorMethod |
| TwoFactorDisabledDomainEvent | UserId |
| UserDeactivatedDomainEvent | UserId |
| UserReactivatedDomainEvent | UserId |

Quy tắc: Immutable, chỉ get;, không có logic, không mang dữ liệu nhạy cảm.

---

## 11. Domain Exceptions (11 exceptions)

| Exception | Ghi chú |
|---|---|
| CannotRemoveLastRoleException | Generic message |
| DuplicateUserRoleException | Generic message |
| InvalidEmailException | |
| InvalidFullNameException | |
| InvalidPasswordException | |
| InvalidPhoneNumberException | |
| InvalidUserIdException | |
| PasswordMustBeDifferentException | |
| TwoFactorMethodRequiredException | |
| UserInactiveException | |
| UserLockedException | |

---

## 12. Enums

```csharp
public enum UserStatus { PendingVerification = 0, Active = 1, Locked = 2, Deactivated = 3 }
public enum TwoFactorMethod { Email = 1, Totp = 2 }
```

---

## 13. Application Layer — Trạng thái hiện tại

- Cấu trúc folder đã tạo nhưng chưa có code.
- Package đã có: MediatR 12.4.1, FluentValidation.DependencyInjectionExtensions 12.1.1.
- Folders placeholder (trống): Features/Auth/, Security/Password/, Security/LockOut/, Security/Device/, Security/UserName/

---

## 14. Infrastructure Layer — Trạng thái hiện tại

- Chưa có code nào, chỉ có .csproj.
- Package đã cài: EF Core 8 (SqlServer + Design), MassTransit.RabbitMQ 8.2.3, StackExchange.Redis 2.8.0, MailKit, Azure.Storage.Blobs, System.Text.Json

---

## 15. WebApi Layer — Trạng thái hiện tại

- Program.cs — skeleton cơ bản (chưa tích hợp DI Application/Infrastructure).
- Extensions/ServiceExtensions.cs — có ConfigureCors() và ConfigureSerilog().
- Controllers/ — trống.
- Package đã có: JWT Bearer, Serilog, AutoMapper, API Versioning, Swagger, FluentValidation.AspNetCore.

---

## 16. Roadmap (đã chốt thứ tự)

| Phase | Nội dung | Trạng thái |
|---|---|---|
| Domain | Domain Kernel + User Aggregate | HOÀN THÀNH |
| Phase 1 | EF Core Mapping: UserConfiguration, UserRoleConfiguration, ValueObject, Backing Field, Owned Types, Value Converter | BẮT ĐẦU TIẾP THEO |
| Phase 2 | ApplicationDbContext — quét IHasDomainEvents cho Outbox | Chưa bắt đầu |
| Phase 3 | IUserRepository + UserRepository | Chưa bắt đầu |
| Phase 4 | Application: RegisterUserCommand, Handler, Validators | Chưa bắt đầu |
| Phase 5 | Outbox Pattern: SaveChanges → Collect Events → Save Outbox → ClearEvents | Chưa bắt đầu |
| Phase 6 | RabbitMQ: Publish Integration Events | Chưa bắt đầu |

---

## 17. Ghi chú kỹ thuật quan trọng

1. **Typo filename:** User.Secutiry.cs (thừa 'u') — lưu ý khi tham chiếu.
2. **Namespace typo:** BookStore.Domain.Common.Intefaces (thiếu 'r') — đã tồn tại trong code, giữ nguyên để không break build.
3. **BookStore.SharedKernel** hiện trống — dự phòng sau.
4. **DomainEvent.OccurredOnUtc** dùng DateTime.UtcNow trong constructor — ngoại lệ chấp nhận được cho event timestamp.
5. **User.Lifecycle.cs** (Deactivate/Reactivate) hiện KHÔNG raise Domain Event — cần xem lại khi implement Outbox.
