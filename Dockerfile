FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["BotMassiveDel/BotMassiveDel.csproj", "BotMassiveDel/"]
RUN dotnet restore "BotMassiveDel/BotMassiveDel.csproj"
COPY . .
WORKDIR "/src/BotMassiveDel"
RUN dotnet build "BotMassiveDel.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "BotMassiveDel.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BotMassiveDel.dll"]
