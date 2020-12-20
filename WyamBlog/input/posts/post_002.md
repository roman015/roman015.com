Title: Setting up SSL
Published: 01/11/2020
Tags: ["Azure", "Cloudflare", "SSL"]
---
Got lots of plans for hosting a great website, but first things first - I need to let everyone into my site using HTTPS!

The basic gist of my problem is that I don't have a huge budget, and the minimum plan that Azure has that suports SSL certificates costs.....above 50USD, which is waaay above my budget. Fortunately I found this : https://www.troyhunt.com/how-to-get-your-ssl-for-free-on-shared/

The basic gist is - Cloudflare has a free tier with which it automatically creates and renews SSL certificates (and allows you to manage DNS records to your website). So once you create a Cloudflare account and sign up for the free plan, all you gotta do is....relax!

No kidding, Cloudflare just copies existing DNS records and SSL is just the free cherry on the top of all that! Now just to make sure that all my web apps on Azure are set to allow SSL by default.