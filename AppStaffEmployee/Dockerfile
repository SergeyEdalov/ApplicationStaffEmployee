FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7129
EXPOSE 5062

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["AppStaffEmployee/AppStaffEmployee.csproj", "AppStaffEmployee/"]
COPY ["Identity.DAL/Identity.DAL.csproj", "Identity.DAL/"]
RUN dotnet restore "./AppStaffEmployee/AppStaffEmployee.csproj"
RUN dotnet restore "Identity.DAL/Identity.DAL.csproj"
COPY . .
WORKDIR "/src/AppStaffEmployee"
RUN dotnet build "./AppStaffEmployee.csproj" -c Release -o /app/build
RUN dotnet publish "./AppStaffEmployee.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS migration
WORKDIR /app
COPY --from=build /app/publish .

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
COPY . /src
RUN apt-get update && apt-get install -y netcat-openbsd

CMD until nc -z -v -w30 db 5432; do echo "Waiting for database connection..."; sleep 1; done && \
    dotnet ef database update --project /src/AppStaffEmployee/AppStaffEmployee.csproj --context EmployeeContext && \
    dotnet ef database update --project /src/Identity.DAL/Identity.DAL.csproj --startup-project /src/AppStaffEmployee/AppStaffEmployee.csproj --context IdentityContext

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "AppStaffEmployee.dll"]