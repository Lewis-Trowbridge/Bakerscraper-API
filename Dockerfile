FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source

COPY . .

RUN [ "dotnet", "restore", "/source/Bakerscraper/Bakerscraper.csproj" ]

WORKDIR /source/Bakerscraper
RUN [ "dotnet", "publish", "-c", "Release", "-o", "/app", "--no-restore" ]

FROM mcr.microsoft.com/dotnet/aspnet:5.0

WORKDIR /app
COPY --from=build /app .

ENV ASPNETCORE_URLS http://+

ENTRYPOINT [ "/app/Bakerscraper" ]
