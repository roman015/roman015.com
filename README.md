# HomePage
Contains The frontend code for www.roman015.com  

This code is a blazor webassembly project that runs on the client's web browser. This can be run directly from Visual Studio and is hosted in production as a static website using [Azure Static Web App](https://azure.microsoft.com/en-us/services/app-service/static/)

## DEV NOTES

To add GitHub Packages as a NuGet Repository (This is a system wide change):

    dotnet nuget add source --username <USERNAME_HERE> --password <TOKEN_HERE_>  --store-password-in-clear-text --name "Roman015Github" "https://nuget.pkg.github.com/roman015-com/index.json"
