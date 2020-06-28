#!/bin/bash
# Script to Publish this Dotnet App.

echo "Stopping Service..."

systemctl stop NginxConfigWeb.service

sleep 3

dotnet publish -o /usr/local/NginxConfigWeb/

sleep 3

echo "Restarting Service..."

systemctl start NginxConfigWeb.service