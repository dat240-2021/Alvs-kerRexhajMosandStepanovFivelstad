FROM mcr.microsoft.com/dotnet/sdk:5.0


COPY website/ /data/

RUN curl -sL https://deb.nodesource.com/setup_16.x | bash -
RUN apt-get -y update && apt-get -y upgrade && apt-get install nodejs

WORKDIR /data/frontend
RUN npm install -g npm@latest
RUN npm install
RUN npm run build

WORKDIR /data/backend/
RUN dotnet dev-certs https
RUN dotnet tool install --global dotnet-ef
ENV PATH $PATH:/root/.dotnet/tools
RUN dotnet restore
RUN dotnet build
RUN dotnet ef migrations add init
RUN mkdir -p ./Infrastructure/Data
RUN dotnet ef database update

ENTRYPOINT ["dotnet","run","--launch-profile","deployment"]
