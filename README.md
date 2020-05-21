# Logging into droplet

The scripts in this project will provision a remote droplet on Digital Ocean. 
With the private key used to create the droplet, you can SSH into the box with
the following command,

```
$ ssh -i id_rsa $DROPLET_HOST_IP
```

Where `$DROPLET_HOST_IP` is the IP of the remote droplet. 


# Verifying different services

We use systemd to run different services on the remote droplet. You can use the 
following commands to check status of each of them,

```
$ sudo systemctl status nginx
$ sudo systemctl status webapp
$ sudo systemctl status worker
```

# Reading systemd logs

The worker writes logs to the systemd-journal service. You can stream them with 
the following command,

```
$ journalctl -u worker.service -f
$ journalctl -u webapp -f

```

# Updating DNS records

To configure what DNS records to use, environment variables need to be updated 
on CircleCI. Specifically these,

```
CLOUDFLARE_ACCOUNT_EMAIL  <- should be the email used to login to cloudflare
CLOUDFLARE_API_TOKEN <- should be the Cloudflare account Global Key
```

You can get the global key by logging into Cloudflare and navigating to,

My Profile > API Tokens (the tab) > select view "Global API Key" near the 
bottom. 
NOTE: We are *not* using Cloudflare API Tokens at this time. 


Also 2 variables need to be updated in this repository at,
./provision/ansible_config/group_vars/all.yml ;

```
dns_zone:  <- should point to root domain, eg: example.com
dns_www_record:  <- the subdomain for the www webapp, eg: ccc
```

Editing the file will configure nginx to respond to requests,
for eg: ccc.example.com

After doing these changes, deploying the app via CircleCI should update 
everything.
