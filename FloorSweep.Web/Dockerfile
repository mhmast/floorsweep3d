#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM nginx AS base
EXPOSE 80
EXPOSE 443

FROM node:current-buster AS build
COPY package*.json ./
RUN npm install
COPY . .
RUN npm run export

FROM base AS final
COPY --from=build /__sapper__/export/. /usr/share/nginx/html/

