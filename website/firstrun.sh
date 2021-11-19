#!/bin/bash
cd "./frontend"
npm ci
npm run build

cd ../backend
mkdir -p ./Infrastructure/Data
dotnet ef migrations add init
dotnet ef database update
dotnet restore
dotnet run
