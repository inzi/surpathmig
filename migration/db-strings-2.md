# Surpath Migration DB Strings Search - 2026-02-04

## Directory
~/projects/surpathmig

## Search Command
grep -r 'surpath112_db|surpath200_db|ConnectionStrings' **/appsettings*.json
- Note: ** glob didn't work as expected; used recursive grep with --include instead.

## Results Summary
- No matches for `surpath112_db` or `surpath200_db`.
- Many `ConnectionStrings` sections found in `inzibackend` project appsettings*.json files across:
  - `./aspnetzeromvc11.4/inzibackend/...`
  - `./surpath112/...`
  - `./surpath200/aspnet-core/...`

## Key Files Examined
### surpath112/src/inzibackend.Migrator/appsettings.json
```
{
  \"ConnectionStrings\": {
    \"Default\": \"server=127.0.0.1;Username=surpath;Password=z24XrByS1;database=surpathv2;\",
    \"surpathlive\": \"server=127.0.0.1;port=3310;Username=surpath;Password=z24XrByS1;database=surpathlive; convert zero datetime=True\"
  }
}
```
- MySQL connections (surpathv2, surpathlive). No EF DbContext connection named `surpath112_db`.

### surpath200/aspnet-core/src/inzibackend.Migrator/appsettings.json
```
{
  \"ConnectionStrings\": {
    \"Default\": \"Server=localhost; Database=inzibackendDb; Trusted_Connection=True; TrustServerCertificate=True;\"
  }
}
```
- SQL Server `inzibackendDb`.

### oldsurrpath/Surpath.Backend/SurpathBackend/appsettings.json
No ConnectionStrings section. Contains logging/service config only.

## EF Scaffold Prep
No exact `surpath112_db` or `surpath200_db` keys found. Possible scaffold commands (adjust context/provider):

**For surpath112 MySQL (surpathv2):**
```
dotnet ef dbcontext scaffold "server=127.0.0.1;Username=surpath;Password=z24XrByS1;database=surpathv2;" MySql.EntityFrameworkCore --context SurpathV2Context --output-dir Models --project ./surpath112/src/inzibackend.EntityFrameworkCore
```

**For surpath200 SQL Server (inzibackendDb):**
```
dotnet ef dbcontext scaffold "Server=localhost;Database=inzibackendDb;Trusted_Connection=True;TrustServerCertificate=True;" Microsoft.EntityFrameworkCore.SqlServer --context InziBackendDbContext --output-dir Models --project ./surpath200/aspnet-core/src/inzibackend.EntityFrameworkCore
```

**Next Steps:**
- Confirm exact DB names/servers from production configs (e.g., appsettings.Production.json).
- Identify EF projects (likely inzibackend.EntityFrameworkCore).
- Install providers: `MySql.EntityFrameworkCore` for MySQL, `Microsoft.EntityFrameworkCore.SqlServer` for SQL Server.
- Use `--data-annotations` or `--use-database-names` as needed.

Run from project root with correct NuGet packages installed.
