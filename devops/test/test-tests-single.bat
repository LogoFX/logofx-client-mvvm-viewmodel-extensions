rem TODO: Provide more generic and reusable way
cd ../../src/%1
dotnet test %1.csproj -c Release
cd ../../devops/test