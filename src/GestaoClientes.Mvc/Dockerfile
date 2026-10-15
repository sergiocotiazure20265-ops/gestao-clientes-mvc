FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/GestaoClientes.Mvc/GestaoClientes.Mvc.csproj src/GestaoClientes.Mvc/
RUN dotnet restore src/GestaoClientes.Mvc/GestaoClientes.Mvc.csproj
COPY src/GestaoClientes.Mvc/ src/GestaoClientes.Mvc/
RUN dotnet publish src/GestaoClientes.Mvc/GestaoClientes.Mvc.csproj -c Release -o /app/publish --no-restore
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENTRYPOINT ["dotnet", "GestaoClientes.Mvc.dll"]
