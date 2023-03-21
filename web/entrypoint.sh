#!/bin/sh

#set -e

#cp /web/index.html /web/index.html.tpl
#envsubst '$STAND' < /web/index.html.tpl > /web/index.html
#rm /web/index.html.tpl

nginx -g 'daemon off;'
