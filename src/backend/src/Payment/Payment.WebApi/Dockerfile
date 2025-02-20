FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar projetos Base
COPY Base/Base.Core.Domain/*.csproj Base/Base.Core.Domain/
COPY Base/Base.Core.Domain.Application/*.csproj Base/Base.Core.Domain.Application/
COPY Base/Base.WebApi/*.csproj Base/Base.WebApi/

COPY Shared/Shared.Dto/*.csproj Shared/Shared.Dto/
COPY Shared/Shared.Infrasctructure/*.csproj Shared/Shared.Infrasctructure/

# Copiar projetos Payment
COPY Payment/Payment.Core.Domain/*.csproj Payment/Payment.Core.Domain/
COPY Payment/Payment.Core.Domain.Application/*.csproj Payment/Payment.Core.Domain.Application/
COPY Payment/Payment.Infrastructure.Bus/*.csproj Payment/Payment.Infrastructure.Bus/
COPY Payment/Payment.Infrastructure/*.csproj Payment/Payment.Infrastructure/
COPY Payment/Payment.WebApi/*.csproj Payment/Payment.WebApi/

# Restaura os pacotes
RUN dotnet restore Payment/Payment.WebApi/Payment.WebApi.csproj

# Copia o restante dos arquivos e faz o build
COPY . .
RUN dotnet publish Payment/Payment.WebApi/Payment.WebApi.csproj -c Release -o out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build /src/out .
ENTRYPOINT ["dotnet", "Payment.WebApi.dll"]