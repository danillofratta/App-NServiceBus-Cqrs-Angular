# Estágio de build
FROM node:18 as builder

WORKDIR /app

ARG ENVIRONMENT=production

# Copiar arquivos de configuração
COPY package*.json ./

# Instalar dependências
RUN npm install

# Copiar código fonte
COPY . .

# Build da aplicação
RUN npm run build -- --configuration=${ENVIRONMENT}

# Estágio de produção
FROM nginx:alpine

# Copiar arquivos de build para o nginx
COPY --from=builder /app/dist/app-client-sale/browser /usr/share/nginx/html

# Copiar configuração personalizada do nginx (opcional)
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expor porta 80
EXPOSE 80

# Iniciar nginx
CMD ["nginx", "-g", "daemon off;"]

# docker build --build-arg ENVIRONMENT=production -t app-client-sale .
# docker build -t app-angular .
# docker run -d -p 8080:80 app-angular

## Desenvolvimento
# docker-compose up angular-dev
# # Produção
# docker-compose up angular-prod

# version: '3.8'
# services:
#   angular-app:
#     build:
#       context: .
#       dockerfile: Dockerfile
#     ports:
#       - "8080:80"
#     volumes:
#       - ./src:/app/src


# version: '3.8'
# services:
#   # Ambiente de desenvolvimento
#   angular-dev:
#     build:
#       context: .
#       dockerfile: Dockerfile
#       args:
#         - ENVIRONMENT=development
#     ports:
#       - "8080:80"
#     volumes:
#       - ./src:/app/src
#     environment:
#       - NODE_ENV=development

#   # Ambiente de produção
#   angular-prod:
#     build:
#       context: .
#       dockerfile: Dockerfile
#       args:
#         - ENVIRONMENT=production
#     ports:
#       - "8081:80"
#     environment:
#       - NODE_ENV=production
