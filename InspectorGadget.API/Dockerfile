FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["InspectorGadget/InspectorGadget.csproj", "InspectorGadget/"]
RUN dotnet restore "InspectorGadget/InspectorGadget.csproj"
COPY . .
WORKDIR "/src/InspectorGadget"
RUN dotnet build "InspectorGadget.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "InspectorGadget.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InspectorGadget.dll"]
