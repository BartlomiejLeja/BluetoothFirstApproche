FROM microsoft/aspnetcore:2.0-nanoserver-1709 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0-nanoserver-1709 AS build
WORKDIR /src
COPY NewestSignalRService/NewestSignalRService.csproj NewestSignalRService/
RUN dotnet restore NewestSignalRService/NewestSignalRService.csproj
COPY . .
WORKDIR /src/NewestSignalRService
RUN dotnet build NewestSignalRService.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish NewestSignalRService.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "NewestSignalRService.dll"]
