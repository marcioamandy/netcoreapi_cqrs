# Imagem Docker que fara o build...
FROM mcr.microsoft.com/dotnet/sdk:3.1-alpine AS builder  

WORKDIR /source

# Copiar codigo-fonte para dentro do container que fara o build...
COPY . .

#faz o build
RUN dotnet publish Globo.PIC.sln --output /app/ --configuration Release

#roda os teste dentro do container
RUN dotnet test Globo.PIC.Tests --output /tests/ --results-directory /app/TestResults --logger "trx;LogFileName=TestResults.trx"

# Prepara a imagem final (apenas com o runtime)
FROM mcr.microsoft.com/dotnet/aspnet:3.1-alpine

#Resolvendo Unable to load DLL 'libgdiplus' ao exportar para Excel     
RUN apk add libgdiplus-dev --update-cache --repository http://dl-3.alpinelinux.org/alpine/edge/testing/ --allow-untrusted

#Resolvendo Font '?' cannot be found
RUN apk --no-cache add msttcorefonts-installer fontconfig && \
	update-ms-fonts && \ 
	fc-cache -f

#Adiciona o Pacote icu-libs (para o Locale) 
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT false
RUN apk add --no-cache icu-libs

#Seta o Locale com UTF8
ENV LC_ALL en_US.UTF-8
ENV LANG en_US.UTF-8

WORKDIR /root/

# Copia o codigo compilado do primeiro container para o segundo
COPY --from=builder /app .
# Substitui o arquivo resolv.conf para conter o endereço do barramento
ADD resolv.conf  /etc/resolv.conf

# Expoe a porta do container final/imagem
ENV ASPNETCORE_URLS="http://*:5001"
EXPOSE 5001/tcp

#seta a variavel de ambiente que foi passada como argumento
#ARG APP_ENV
#ENV ASPNETCORE_ENVIRONMENT $APP_ENV

# comando que vai ser executado quando subirmos o container (a partir da imagem)
ENTRYPOINT ["dotnet", "Globo.PIC.API.dll"]

# Como testar a imagem (manualmente)
#turn on daemon docker first and then...
#docker build -t pic-service-api .
#docker run --name pic-service-api_test -p 8000:80 pic-service-api
#http://localhost:8000/pic-service-api/swagger

#http://www.projectatomic.io/blog/2015/07/what-are-docker-none-none-images/
#$> docker system prune -f