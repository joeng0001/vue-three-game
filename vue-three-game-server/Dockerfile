FROM mcr.microsoft.com/dotnet/sdk:6.0 

COPY . .

workdir ./server

RUN dotnet restore

RUN dotnet dev-certs https

EXPOSE 7155

ENTRYPOINT ["dotnet", "run","--project","server.csproj","--urls", "https://0.0.0.0:7155;http://0.0.0.0:5136"]