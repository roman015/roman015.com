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
                 -e dns_cloudflare_zone_id=$CLOUDFLARE_ZONE_ID \
                 -e 'ansible_python_interpreter=/usr/bin/python3'
