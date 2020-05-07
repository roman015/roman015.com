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
```

