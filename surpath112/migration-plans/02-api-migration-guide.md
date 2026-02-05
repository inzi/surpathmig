# API Migration Guide: Preparing Backend for Angular

## Overview

This guide details the steps required to prepare the backend API for Angular consumption, including setting up the Web.Host project, optimizing API endpoints, and ensuring proper CORS and security configurations.

## Current MVC API Structure

The current MVC application has some API endpoints but they're primarily designed for AJAX calls from jQuery:

```csharp
// Current MVC Controller Pattern
[Area("App")]
public class CohortsController : inzibackendControllerBase
{
    [HttpPost]
    public async Task<JsonResult> GetAll(GetAllCohortsInput input)
    {
        var cohorts = await _cohortsAppService.GetAll(input);
        return Json(new { data = cohorts.Items, recordsTotal = cohorts.TotalCount });
    }
}
```

## Target Web.Host API Structure

The Angular edition requires a dedicated Web.Host project with proper API controllers:

```csharp
// Target API Controller Pattern
[Route("api/[controller]")]
[ApiController]
public class CohortsController : AbpControllerBase
{
    private readonly ICohortsAppService _cohortsAppService;

    [HttpGet]
    public async Task<PagedResultDto<GetCohortForViewDto>> GetAll([FromQuery] GetAllCohortsInput input)
    {
        return await _cohortsAppService.GetAll(input);
    }

    [HttpPost]
    public async Task CreateOrEdit([FromBody] CreateOrEditCohortDto input)
    {
        await _cohortsAppService.CreateOrEdit(input);
    }

    [HttpDelete("{id}")]
    public async Task Delete(Guid id)
    {
        await _cohortsAppService.Delete(new EntityDto<Guid> { Id = id });
    }
}
```

## Step-by-Step Migration Process

### Step 1: Create Web.Host Project Structure

#### 1.1 Project Setup
```bash
# Create new Web.Host project (if not exists)
dotnet new webapi -n inzibackend.Web.Host
```

#### 1.2 Required NuGet Packages
```xml
<!-- inzibackend.Web.Host.csproj -->
<PackageReference Include="Abp.AspNetCore" Version="7.3.0" />
<PackageReference Include="Abp.ZeroCore.EntityFrameworkCore" Version="7.3.0" />
<PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="6.0.0" />
<PackageReference Include="Swashbuckle.AspNetCore" Version="6.2.3" />
<PackageReference Include="Microsoft.AspNetCore.Cors" Version="2.2.0" />
```

#### 1.3 Startup Configuration
```csharp
// Startup.cs
public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        // ABP Configuration
        services.AddAbp<inzibackendWebHostModule>();

        // CORS Configuration
        services.AddCors(options =>
        {
            options.AddPolicy("AllowAngularApp", builder =>
            {
                builder
                    .WithOrigins("http://localhost:4200", "https://yourdomain.com")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        // JWT Authentication
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

        // API Controllers
        services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = 
                    new AbpMvcContractResolver(IocManager.Instance);
            });

        // Swagger
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Surpath API", Version = "v1" });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseAbp();
        
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Surpath API v1"));
        }

        app.UseRouting();
        app.UseCors("AllowAngularApp");
        app.UseAuthentication();
        app.UseAuthorization();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}
```

### Step 2: Convert MVC Controllers to API Controllers

#### 2.1 Cohorts API Controller Example

```csharp
// Controllers/CohortsController.cs
[Route("api/services/app/[controller]")]
[ApiController]
[AbpAuthorize(AppPermissions.Pages_Cohorts)]
public class CohortsController : inzibackendControllerBase
{
    private readonly ICohortsAppService _cohortsAppService;

    public CohortsController(ICohortsAppService cohortsAppService)
    {
        _cohortsAppService = cohortsAppService;
    }

    [HttpGet]
    public async Task<PagedResultDto<GetCohortForViewDto>> GetAll([FromQuery] GetAllCohortsInput input)
    {
        return await _cohortsAppService.GetAll(input);
    }

    [HttpGet("{id}")]
    public async Task<GetCohortForViewDto> Get(Guid id)
    {
        return await _cohortsAppService.GetCohortForView(id);
    }

    [HttpGet("GetCohortForEdit")]
    public async Task<GetCohortForEditOutput> GetCohortForEdit([FromQuery] Guid id)
    {
        return await _cohortsAppService.GetCohortForEdit(new EntityDto<Guid> { Id = id });
    }

    [HttpPost]
    [AbpAuthorize(AppPermissions.Pages_Cohorts_Create)]
    public async Task CreateOrEdit([FromBody] CreateOrEditCohortDto input)
    {
        await _cohortsAppService.CreateOrEdit(input);
    }

    [HttpDelete("{id}")]
    [AbpAuthorize(AppPermissions.Pages_Cohorts_Delete)]
    public async Task Delete(Guid id)
    {
        await _cohortsAppService.Delete(new EntityDto<Guid> { Id = id });
    }

    [HttpGet("GetCohortsToExcel")]
    public async Task<FileResult> GetCohortsToExcel([FromQuery] GetAllCohortsForExcelInput input)
    {
        var file = await _cohortsAppService.GetCohortsToExcel(input);
        return File(file.FileArray, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", file.FileName);
    }

    [HttpGet("GetCompliance")]
    public async Task<PagedResultDto<GetCohortForComplianceViewDto>> GetCompliance([FromQuery] GetAllCohortsInput input)
    {
        return await _cohortsAppService.GetCompliance(input);
    }
}
```

#### 2.2 Records API Controller (Complex Example with File Upload)

```csharp
// Controllers/RecordsController.cs
[Route("api/services/app/[controller]")]
[ApiController]
[AbpAuthorize(AppPermissions.Pages_Records)]
public class RecordsController : inzibackendControllerBase
{
    private readonly IRecordsAppService _recordsAppService;
    private readonly IBinaryObjectManager _binaryObjectManager;

    public RecordsController(
        IRecordsAppService recordsAppService,
        IBinaryObjectManager binaryObjectManager)
    {
        _recordsAppService = recordsAppService;
        _binaryObjectManager = binaryObjectManager;
    }

    [HttpGet]
    public async Task<PagedResultDto<GetRecordForViewDto>> GetAll([FromQuery] GetAllRecordsInput input)
    {
        return await _recordsAppService.GetAll(input);
    }

    [HttpPost]
    [AbpAuthorize(AppPermissions.Pages_Records_Create)]
    public async Task CreateOrEdit([FromBody] CreateOrEditRecordDto input)
    {
        await _recordsAppService.CreateOrEdit(input);
    }

    [HttpPost("UploadFile")]
    [AbpAuthorize(AppPermissions.Pages_Records_Create)]
    public async Task<UploadFileOutput> UploadFile(IFormFile file)
    {
        if (file == null || file.Length == 0)
            throw new UserFriendlyException("No file uploaded");

        // Validate file type and size
        var allowedExtensions = new[] { ".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        
        if (!allowedExtensions.Contains(extension))
            throw new UserFriendlyException("File type not allowed");

        if (file.Length > 10 * 1024 * 1024) // 10MB limit
            throw new UserFriendlyException("File size exceeds limit");

        // Save to binary object storage
        using var stream = file.OpenReadStream();
        var binaryObject = new BinaryObject(AbpSession.TenantId, stream.ToByteArray());
        await _binaryObjectManager.SaveAsync(binaryObject);

        return new UploadFileOutput
        {
            Id = binaryObject.Id,
            FileName = file.FileName,
            FileSize = file.Length,
            ContentType = file.ContentType
        };
    }

    [HttpGet("DownloadFile/{id}")]
    public async Task<IActionResult> DownloadFile(Guid id)
    {
        var binaryObject = await _binaryObjectManager.GetOrNullAsync(id);
        if (binaryObject == null)
            return NotFound();

        var record = await _recordsAppService.GetRecordByFileId(id);
        return File(binaryObject.Bytes, record.ContentType, record.FileName);
    }

    [HttpDelete("DeleteFile/{id}")]
    [AbpAuthorize(AppPermissions.Pages_Records_Delete)]
    public async Task DeleteFile(Guid id)
    {
        await _binaryObjectManager.DeleteAsync(id);
    }
}
```

### Step 3: Enhance DTOs for Angular Consumption

#### 3.1 Add Validation Attributes
```csharp
// Dtos/CreateOrEditCohortDto.cs
public class CreateOrEditCohortDto : EntityDto<Guid?>
{
    [Required]
    [StringLength(CohortConsts.MaxNameLength)]
    public string Name { get; set; }

    [StringLength(CohortConsts.MaxDescriptionLength)]
    public string Description { get; set; }

    public bool DefaultCohort { get; set; }

    public Guid? TenantDepartmentId { get; set; }

    // Add display names for Angular forms
    [Display(Name = "Cohort Name")]
    public string DisplayName => Name;

    // Add validation summary for client-side validation
    public Dictionary<string, string> ValidationErrors { get; set; } = new();
}
```

#### 3.2 Add Lookup DTOs
```csharp
// Dtos/CohortLookupDto.cs
public class CohortLookupDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string TenantDepartmentName { get; set; }
}

// In CohortsAppService
public async Task<List<CohortLookupDto>> GetCohortsForLookup()
{
    return await _cohortRepository.GetAll()
        .Include(c => c.TenantDepartmentFk)
        .Select(c => new CohortLookupDto
        {
            Id = c.Id,
            Name = c.Name,
            Description = c.Description,
            TenantDepartmentName = c.TenantDepartmentFk.Name
        })
        .ToListAsync();
}
```

### Step 4: File Upload API Enhancement

#### 4.1 Chunked Upload Support
```csharp
// Controllers/FileUploadController.cs
[Route("api/services/app/[controller]")]
[ApiController]
public class FileUploadController : inzibackendControllerBase
{
    [HttpPost("UploadChunk")]
    public async Task<IActionResult> UploadChunk([FromForm] ChunkUploadDto input)
    {
        var tempPath = Path.Combine(Path.GetTempPath(), "uploads", input.UploadId);
        Directory.CreateDirectory(tempPath);

        var chunkPath = Path.Combine(tempPath, $"chunk_{input.ChunkNumber}");
        using (var stream = new FileStream(chunkPath, FileMode.Create))
        {
            await input.File.CopyToAsync(stream);
        }

        // Check if all chunks are uploaded
        if (input.ChunkNumber == input.TotalChunks - 1)
        {
            await CombineChunks(input.UploadId, input.FileName, input.TotalChunks);
        }

        return Ok(new { ChunkNumber = input.ChunkNumber, Status = "Uploaded" });
    }

    [HttpPost("CompleteUpload")]
    public async Task<UploadFileOutput> CompleteUpload([FromBody] CompleteUploadDto input)
    {
        var tempFilePath = Path.Combine(Path.GetTempPath(), "uploads", input.UploadId, "complete");
        
        if (!System.IO.File.Exists(tempFilePath))
            throw new UserFriendlyException("Upload not found or incomplete");

        var fileBytes = await System.IO.File.ReadAllBytesAsync(tempFilePath);
        var binaryObject = new BinaryObject(AbpSession.TenantId, fileBytes);
        await _binaryObjectManager.SaveAsync(binaryObject);

        // Cleanup temp files
        Directory.Delete(Path.Combine(Path.GetTempPath(), "uploads", input.UploadId), true);

        return new UploadFileOutput
        {
            Id = binaryObject.Id,
            FileName = input.FileName,
            FileSize = fileBytes.Length
        };
    }
}

public class ChunkUploadDto
{
    public string UploadId { get; set; }
    public int ChunkNumber { get; set; }
    public int TotalChunks { get; set; }
    public string FileName { get; set; }
    public IFormFile File { get; set; }
}
```

### Step 5: Authentication API

#### 5.1 Token Authentication Controller
```csharp
// Controllers/TokenAuthController.cs
[Route("api/[controller]")]
[ApiController]
public class TokenAuthController : inzibackendControllerBase
{
    private readonly LogInManager _logInManager;
    private readonly IConfiguration _configuration;

    [HttpPost("Authenticate")]
    public async Task<AuthenticateResultModel> Authenticate([FromBody] AuthenticateModel model)
    {
        var loginResult = await GetLoginResultAsync(
            model.UserNameOrEmailAddress,
            model.Password,
            model.TenancyName
        );

        var accessToken = CreateAccessToken(CreateJwtClaims(loginResult.Identity));

        return new AuthenticateResultModel
        {
            AccessToken = accessToken,
            EncryptedAccessToken = GetEncryptedAccessToken(accessToken),
            ExpireInSeconds = (int)_configuration.GetValue<TimeSpan>("Authentication:JwtBearer:Expiration").TotalSeconds,
            UserId = loginResult.User.Id
        };
    }

    [HttpGet("GetCurrentLoginInformations")]
    [AbpAuthorize]
    public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
    {
        var output = new GetCurrentLoginInformationsOutput
        {
            Application = new ApplicationInfoDto
            {
                Version = AppVersionHelper.Version,
                ReleaseDate = AppVersionHelper.ReleaseDate,
                Features = new Dictionary<string, bool>()
            }
        };

        if (AbpSession.TenantId.HasValue)
        {
            output.Tenant = ObjectMapper.Map<TenantLoginInfoDto>(await GetCurrentTenantAsync());
        }

        if (AbpSession.UserId.HasValue)
        {
            output.User = ObjectMapper.Map<UserLoginInfoDto>(await GetCurrentUserAsync());
        }

        return output;
    }

    private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
    {
        var now = DateTime.UtcNow;
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _configuration["Authentication:JwtBearer:Issuer"],
            audience: _configuration["Authentication:JwtBearer:Audience"],
            claims: claims,
            notBefore: now,
            expires: now.Add(expiration ?? _configuration.GetValue<TimeSpan>("Authentication:JwtBearer:Expiration")),
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["Authentication:JwtBearer:SecurityKey"])),
                SecurityAlgorithms.HmacSha256)
        );

        return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
    }
}
```

### Step 6: Error Handling and Validation

#### 6.1 Global Exception Handler
```csharp
// Middleware/GlobalExceptionMiddleware.cs
public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<GlobalExceptionMiddleware> _logger;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var response = new ApiErrorResponse();

        switch (exception)
        {
            case AbpValidationException validationEx:
                response.Message = "Validation failed";
                response.Details = validationEx.ValidationErrors.Select(e => e.ErrorMessage).ToArray();
                context.Response.StatusCode = 400;
                break;

            case AbpAuthorizationException:
                response.Message = "Access denied";
                context.Response.StatusCode = 401;
                break;

            case UserFriendlyException userFriendlyEx:
                response.Message = userFriendlyEx.Message;
                context.Response.StatusCode = 400;
                break;

            default:
                response.Message = "An error occurred while processing your request";
                context.Response.StatusCode = 500;
                break;
        }

        var jsonResponse = JsonConvert.SerializeObject(response);
        await context.Response.WriteAsync(jsonResponse);
    }
}

public class ApiErrorResponse
{
    public string Message { get; set; }
    public string[] Details { get; set; }
    public int StatusCode { get; set; }
}
```

### Step 7: API Documentation with Swagger

#### 7.1 Enhanced Swagger Configuration
```csharp
// In Startup.cs ConfigureServices
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo 
    { 
        Title = "Surpath API", 
        Version = "v1",
        Description = "Medical Compliance Management System API"
    });

    // Include XML comments
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // JWT Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

    // Group by areas
    c.TagActionsBy(api => new[] { api.GroupName ?? api.ActionDescriptor.RouteValues["controller"] });
    c.DocInclusionPredicate((name, api) => true);
});
```

## Testing the API

### Step 8: API Testing Strategy

#### 8.1 Postman Collection Setup
Create a Postman collection with the following requests:

1. **Authentication**
   - POST `/api/TokenAuth/Authenticate`
   - GET `/api/TokenAuth/GetCurrentLoginInformations`

2. **Cohorts**
   - GET `/api/services/app/Cohorts`
   - POST `/api/services/app/Cohorts`
   - GET `/api/services/app/Cohorts/{id}`
   - DELETE `/api/services/app/Cohorts/{id}`

3. **File Upload**
   - POST `/api/services/app/FileUpload/UploadChunk`
   - POST `/api/services/app/FileUpload/CompleteUpload`

#### 8.2 Integration Tests
```csharp
// Tests/CohortsController_Tests.cs
public class CohortsController_Tests : inzibackendWebTestBase
{
    [Fact]
    public async Task Should_Get_All_Cohorts()
    {
        // Arrange
        await LoginAsDefaultTenantAdmin();

        // Act
        var response = await GetResponseAsObjectAsync<PagedResultDto<GetCohortForViewDto>>(
            GetUrl<CohortsController>(nameof(CohortsController.GetAll))
        );

        // Assert
        response.Items.ShouldNotBeNull();
        response.TotalCount.ShouldBeGreaterThan(0);
    }

    [Fact]
    public async Task Should_Create_Cohort()
    {
        // Arrange
        await LoginAsDefaultTenantAdmin();
        var input = new CreateOrEditCohortDto
        {
            Name = "Test Cohort",
            Description = "Test Description"
        };

        // Act & Assert
        await PostAsync(GetUrl<CohortsController>(nameof(CohortsController.CreateOrEdit)), input);
    }
}
```

## Performance Optimization

### Step 9: API Performance Enhancements

#### 9.1 Response Caching
```csharp
// Add to Startup.cs
services.AddResponseCaching();
services.AddMemoryCache();

// In controller
[HttpGet("GetCohortsForLookup")]
[ResponseCache(Duration = 300)] // Cache for 5 minutes
public async Task<List<CohortLookupDto>> GetCohortsForLookup()
{
    return await _cohortsAppService.GetCohortsForLookup();
}
```

#### 9.2 Compression
```csharp
// Add to Startup.cs
services.AddResponseCompression(options =>
{
    options.Providers.Add<GzipCompressionProvider>();
    options.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/json" });
});
```

## Next Steps

1. **Set up the Web.Host project** following the structure above
2. **Convert all MVC controllers** to API controllers using the patterns shown
3. **Test each API endpoint** using Postman or Swagger
4. **Implement file upload APIs** with progress tracking
5. **Set up proper error handling** and validation
6. **Configure CORS** for Angular development

---

**Next**: [Angular Application Structure](./03-angular-app-structure.md) - Learn how to structure the Angular frontend application.
