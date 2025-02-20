FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar projetos Base
COPY Base/Base.Core.Domain/*.csproj Base/Base.Core.Domain/
COPY Base/Base.Core.Domain.Application/*.csproj Base/Base.Core.Domain.Application/
COPY Base/Base.WebApi/*.csproj Base/Base.WebApi/

COPY Shared/Shared.Dto/*.csproj Shared/Shared.Dto/
COPY Shared/Shared.Infrasctructure/*.csproj Shared/Shared.Infrasctructure/

# Copiar projetos Stock
COPY Stock/Stock.Core.Domain/*.csproj Stock/Stock.Core.Domain/
COPY Stock/Stock.Core.Domain.Application/*.csproj Stock/Stock.Core.Domain.Application/
COPY Stock/Stock.Core.Domain.Contracts/*.csproj Stock/Stock.Core.Domain.Contracts/
COPY Stock/Stock.Infrastructure.Bus/*.csproj Stock/Stock.Infrastructure.Bus/
COPY Stock/Stock.Infrastructure/*.csproj Stock/Stock.Infrastructure/
COPY Stock/Stock.WebApi/*.csproj Stock/Stock.WebApi/

# Restaura os pacotes
RUN dotnet restore Stock/Stock.WebApi/Stock.WebApi.csproj

# Copia o restante dos arquivos e faz o build
COPY . .
RUN dotnet publish Stock/Stock.WebApi/Stock.WebApi.csproj -c Release -o out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build /src/out .
ENTRYPOINT ["dotnet", "Stock.WebApi.dll"]