Title: A New Website! 
Published: 12/20/2020
Tags: ["Website", "DigitalOcean", "3.0", "DigitalOcean Apps"]
---
So [.Net5.0](https://docs.microsoft.com/en-us/dotnet/core/dotnet-five) came out sometime back and I figured it's time to do some remodelling! And so the website's architechture is now completly changed. 

Previously it was completely built on .Net Core and was running on a single Droplet in Digital Ocean.

Now it's running on the Digital Ocean Apps Platform in three pieces - 

1. The Static Site for www.roman015.com. This one is done in .Net5 webassembly.
2. The Static Site for blog.roman015.com. This one is still using wyam.io
3. The APIs will be hosted in api.roman015.com

Now the main benefit of running it on the Apps Platform is that there's potential for scalability (not that I'll actually need it, but it's good to know there's a backup plan)

The code for these three can be found in different branches in [github](https://github.com/roman015/roman015.com). You can even find the source code for the old version in the 2.0 Branch.

Two things I'll need to do is to consider updating the static site generator for the blog and to add some extra documentation (I think I'll use the master branch for that from now on)

One extra thing I'd like to point out - QR Code generation in www.roman015.com/QRCode is now comletely client side and is no longer dependent on any server side execution, hurray!