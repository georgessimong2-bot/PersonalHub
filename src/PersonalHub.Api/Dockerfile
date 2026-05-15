FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .

RUN dotnet publish -c Release -o /out

FROM base
WORKDIR /app
COPY --from=build /out .

ENTRYPOINT ["dotnet", "PersonalHub.Api.dll"]