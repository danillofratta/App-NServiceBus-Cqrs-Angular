# Etapa de build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /App

# Copia apenas os arquivos de projeto primeiro (melhora o cache)
COPY Base/Base.Core.Domain/*.csproj Base/Base.Core.Domain/
COPY Base/Base.Core.Domain.Application/*.csproj Base/Base.Core.Domain.Application/
COPY Base/Base.WebApi/*.csproj Base/Base.WebApi/

COPY Shared/Shared.Dto/*.csproj Shared/Shared.Dto/
COPY Shared/Shared.Infrasctructure/*.csproj Shared/Shared.Infrasctructure/

COPY Product/Product.Core.Application/*.csproj Product/Product.Core.Application/
COPY Product/Product.Core.Domain/*.csproj Product/Product.Core.Domain/
COPY Product/Product.Infrastructure/*.csproj Product/Product.Infrastructure/
COPY Product/Product.WebApi/*.csproj Product/Product.WebApi/

# Restaura os pacotes
RUN dotnet restore Product/Product.WebApi/Product.WebApi.csproj

# Copia o restante dos arquivos e faz o build
COPY . .
RUN dotnet publish Product/Product.WebApi/Product.WebApi.csproj -c Release -o out

# Etapa de runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /App
COPY --from=build /App/out .
ENTRYPOINT ["dotnet", "Product.WebApi.dll"]


#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /src
#
## Copiar projetos Base
#COPY ["Base/Base.Core.Domain/Base.Core.Domain.csproj", "Base/Base.Core.Domain/"]
#COPY ["Base/Base.Core.Domain.Application/Base.Core.Domain.Application.csproj", "Base/Base.Core.Domain.Application/"]
#COPY ["Base/Base.WebApi/Base.WebApi.csproj", "Base/Base.WebApi/"]
#
## Copiar projetos Product
#COPY ["Product.Core.Application/Product.Core.Application.csproj", "Product/Product.Core.Application/"]
#COPY ["Product.Core.Domain/Product.Core.Domain.csproj", "Product/Product.Core.Domain/"]
#COPY ["Product.Infrastructure/Product.Infrastructure.csproj", "Product/Product.Infrastructure/"]
#COPY ["Product.WebApi/Product.WebApi.csproj", "Product/Product.WebApi/"]
#
## Restaurar pacotes
#RUN dotnet restore "Product.WebApi/Product.WebApi.csproj"
#
## Copiar o resto dos arquivos
#COPY Product.WebApi/ Product.WebApi/
#
#
## Build e publish
#WORKDIR "/src/Product/Product.WebApi"
#RUN dotnet build "Product.WebApi.csproj" -c Release -o /app/build
#RUN dotnet publish "Product.WebApi.csproj" -c Release -o /app/publish
#
## Imagem final
#FROM mcr.microsoft.com/dotnet/aspnet:8.0
#WORKDIR /app
#COPY --from=build /app/publish .
#ENTRYPOINT ["dotnet", "Product.WebApi.dll"]
#
# # Usar a imagem do .NET SDK para build
# FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
# WORKDIR /app

# # Copiar os arquivos do projeto
# COPY . ./

# # Restaurar dependências
# RUN dotnet restore

# # Construir a aplicação
# RUN dotnet publish -c Release -o /out

# # Usar a imagem do .NET Runtime para rodar a aplicação
# FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
# WORKDIR /app
# COPY --from=build /out .

# # Expor a porta da API
# EXPOSE 5000
# ENV ASPNETCORE_URLS=http://+:5000

# # Executar a API
# ENTRYPOINT ["dotnet", "Product.WebApi.dll"]



#
## Etapa de build
#FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
#WORKDIR /src
#
## Copia os arquivos do projeto individualmente para evitar conflitos
#COPY ["Product.WebApi/Product.WebApi.csproj", "Product.WebApi/"]
#COPY ["Product.Core.Application/Product.Core.Application.csproj", "Product.Core.Application/"]
#COPY ["Product.Core.Domain/Product.Core.Domain.csproj", "Product.Core.Domain/"]
#COPY ["Product.Infrastructure/Product.Infrastructure.Orm.csproj", "Product.Infrastructure/"]
#COPY ["Base/Base.WebApi/Base.WebApi.csproj", "Base/Base.WebApi/"]
#
## Restaurar dependências
#RUN dotnet restore "Product.WebApi/Product.WebApi.csproj"
#
## Copiar todo o código-fonte
#COPY . .
#
## Construir a aplicação
#WORKDIR /src/Product.WebApi
#RUN dotnet publish -c Release -o /out
#
## Etapa de runtime
#FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
#WORKDIR /app
#COPY --from=build /src/Product.WebApi/out .
#
## Expor a porta da API
#EXPOSE 5000
#ENV ASPNETCORE_URLS=http://+:5000
#
## Executar a API
#ENTRYPOINT ["dotnet", "Product.WebApi.dll"]
#