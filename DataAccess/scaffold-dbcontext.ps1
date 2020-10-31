# Generate EfObjects on DataOnjects project, and DbContect in DataAccesProject
# There is no option to control generated namespace. Use DataObjects as current project and replace generated namespace for DbContect
dotnet ef dbcontext scaffold "Server=.;Database=SmartGarden;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer --schema Data  --project "..\Core.DataObjects" --output-dir ".\EFObjects" --context-dir "..\DataAccess\DbContexts" --context SGContext --force --no-build

#replace namespace + include EfObjects namespace
$path = '.\DbContexts\SGContext.cs'
@("using Core.DataObjects.EFObjects;", (Get-Content $path).replace('Core.DataObjects', 'DataAccess.DbContexts').replace('DataAccess.DbContexts.EFObjects', 'DataAccess.DbContexts')) | Set-Content $path
