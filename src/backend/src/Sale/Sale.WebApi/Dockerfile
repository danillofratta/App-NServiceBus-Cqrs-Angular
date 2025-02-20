FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar projetos Base
COPY Base/Base.Core.Domain/*.csproj Base/Base.Core.Domain/
COPY Base/Base.Core.Domain.Application/*.csproj Base/Base.Core.Domain.Application/
COPY Base/Base.WebApi/*.csproj Base/Base.WebApi/

COPY Shared/Shared.Dto/*.csproj Shared/Shared.Dto/
COPY Shared/Shared.Infrasctructure/*.csproj Shared/Shared.Infrasctructure/

# Copiar projetos Sale
COPY Sale/Sale.Core.Domain/*.csproj Sale/Sale.Core.Domain/
COPY Sale/Sale.Core.Domain.Application/*.csproj Sale/Sale.Core.Domain.Application/
COPY Sale/Sale.Core.Domain.Contracts/*.csproj Sale/Sale.Core.Domain.Contracts/
COPY Sale/Sale.Core.Domain.Saga/*.csproj Sale/Sale.Core.Domain.Saga/
COPY Sale/Sale.Infrastructure.Bus/*.csproj Sale/Sale.Infrastructure.Bus/
COPY Sale/Sale.Infrastructure/*.csproj Sale/Sale.Infrastructure/
COPY Sale/Sale.SaleMessageProcessor/*.csproj Sale/Sale.SaleMessageProcessor/
COPY Sale/Sale.WebApi/*.csproj Sale/Sale.WebApi/

# Restaura os pacotes
RUN dotnet restore Sale/Sale.WebApi/Sale.WebApi.csproj

# Copia o restante dos arquivos e faz o build
COPY . .
RUN dotnet publish Sale/Sale.WebApi/Sale.WebApi.csproj -c Release -o out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build /src/out .
ENTRYPOINT ["dotnet", "Sale.WebApi.dll"]