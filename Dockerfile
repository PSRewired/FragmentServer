FROM mcr.microsoft.com/dotnet/runtime-deps:10.0-noble AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:10.0-noble AS build
WORKDIR /srv
COPY Fragment.NetSlum.sln .
COPY src/. src/.

RUN dotnet restore src/Fragment.NetSlum.Server

FROM build as testrunner
COPY test/. test/.
RUN dotnet test -c Release --filter "Category=Unit" /p:CollectCoverage=true /p:ExcludeByFile="**/Migrations/*.cs" /p:CoverletOutput='/test/build/' /p:CoverletOutputFormat='json%2ccobertura' /p:MergeWith='/test/build/coverage.json'

FROM build AS publish
WORKDIR /srv/src/Fragment.NetSlum.Server

RUN dotnet publish Fragment.NetSlum.Server.csproj -c Release --runtime linux-x64 -o /app --self-contained

WORKDIR /srv/src/Fragment.NetSlum.Console
RUN dotnet publish Fragment.NetSlum.Console.csproj -c Release --runtime linux-x64 -o /app/console --self-contained

FROM base AS final
EXPOSE 49000

WORKDIR /app
COPY --from=publish /app .

USER $APP_UID

CMD ["./Fragment.NetSlum.Server"]
