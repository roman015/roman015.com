#!/bin/bash
#
# This script will bootstrap the ansible playbook, which runs on the remote
# droplet.

# Parsing arguments
# ref: https://unix.stackexchange.com/a/580258/410983
while [ $# -gt 0 ]; do
  case "$1" in
    --remote-ip*|-ip*)
      if [[ "$1" != *=* ]]; then shift; fi # Value is next arg if no `=`
      REMOTE_IP="${1#*=}"
      ;;
    --help|-h)
      echo "Usage: ./run_ansible.sh [OPTION]... "
      echo "Used to bootstrap ansible playbook on desired remote host"
      echo ""
      echo "  -ip, --remote-ip     The remote IP of the server to run ansible scripts against"
      echo ""
      echo "Example usage:"
      echo "$ ./run_ansible --remote-ip 127.0.0.1"
      exit 0
      ;;
    *)
      >&2 printf "Error: Invalid argument\n"
      exit 1
      ;;
  esac
  shift
done

if [ -z "$REMOTE_IP" ]
then
    echo "Error: remote-ip not set!"
    echo "Pass --help as command arugment for details"
    exit 1
fi

# Running our playbook
ansible-playbook basic-provision.yml \
                 -e working_host=$REMOTE_IP \
                 -e dns_cloudflare_account_email=$CLOUDFLARE_ACCOUNT_EMAIL \
                 -e dns_cloudflare_api_token=$CLOUDFLARE_API_TOKEN \
                 -e azureb2c_clientid=$AZUREB2C_CLIENTID \
                 -e azureb2c_tenant=$AZUREB2C_TENANT \
                 -e azureb2c_signupsignin_policy_id=$AZUREB2C_SIGNUPSIGNIN_POLICY_ID \
                 -e azureb2c_reset_password_policy_id=$AZUREB2C_RESET_PASSWORD_POLICY_ID \
                 -e azureb2c_edit_profile_policy_id=$AZUREB2C_EDIT_PROFILE_POLICY_ID \
                 -e azureb2c_redirect_uri=$AZUREB2C_REDIRECT_URI \
                 -e azureb2c_client_secret=$AZUREB2C_CLIENT_SECRET \
                 -e azureb2c_api_uri_https=$AZUREB2C_API_URI_HTTPS \
                 -e azureb2c_apiscopes=$AZUREB2C_APISCOPES \
                 -e urlshortener_connectionstring=$URLSHORTENER_CONNECTIONSTRING \
                 -e terraria_dotoken=$TERRARIA_DOTOKEN \
                 -e terraria_discord_bot_token=$TERRARIA_DISCORD_BOT_TOKEN \
                 -e terraria_discord_channel=$TERRARIA_DISCORD_CHANNEL_ID \
                 -e worker_discord_bot_token=$WORKER_DISCORD_BOT_TOKEN \
                 -e dropbox_access_token=$DROPBOX_ACCESS_TOKEN \
                 -e 'ansible_python_interpreter=/usr/bin/python3'
