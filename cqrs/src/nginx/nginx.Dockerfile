FROM nginx

COPY nginx/nginx.local.conf /etc/nginx/nginx.conf
COPY nginx/identity.crt /etc/ssl/certs/identity.saturn72.com.crt
COPY nginx/identity.key /etc/ssl/private/identity.saturn72.com.key