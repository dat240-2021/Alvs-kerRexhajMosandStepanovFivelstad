#!/bin/bash
cd "./frontend"
npm ci
npm run build

cd ../backend
dotnet restore
dotnet run
