# build environment
FROM node:14.17-alpine as build
RUN apk add --no-cache git
WORKDIR /web
ENV PATH /web/node_modules/.bin:$PATH
COPY package.json /web/package.json
COPY yarn.lock /web/yarn.lock
RUN mkdir .yarncache
RUN yarn install --check-cache --cache-folder ./.yarncache --network-concurrency 1
RUN rm -rf .yarncache
COPY . /web
RUN yarn build

# production environment
FROM nginx:1.17.4-alpine
COPY --from=build /web/build /web
COPY nginx.conf /etc/nginx/nginx.conf
#COPY nginx.conf /etc/nginx/conf.d/default.conf
COPY entrypoint.sh /entrypoint.sh
EXPOSE 80
CMD ["sh", "/entrypoint.sh"]