FROM mcr.microsoft.com/dotnet/aspnet:6.0

ENV ASPNETCORE_ENVIRONMENT "Production"
ENV ASPNETCORE_URLS "http://+:80"

WORKDIR /app
COPY /publish .

ENTRYPOINT ["dotnet", "ItemsAdministration.WebHost.dll"]