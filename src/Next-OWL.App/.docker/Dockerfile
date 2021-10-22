FROM nginx

COPY .docker/nginx.conf /etc/nginx/nginx.conf
RUN mkdir -p /www
COPY ./src /www/