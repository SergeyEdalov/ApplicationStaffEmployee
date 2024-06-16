FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
#USER app
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
#COPY Identity.DAL/. ./Identity.DAL
WORKDIR "/src/AppStaffEmployee"
RUN dotnet build "./AppStaffEmployee.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./AppStaffEmployee.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY certificates/aspnetapp.pfx .
COPY --from=publish /app/publish .
#RUN rm -rf /app/Identity.DAL
#COPY --from=build /app/Identity.DAL /app/Identity.DAL
ENTRYPOINT ["dotnet", "AppStaffEmployee.dll"]