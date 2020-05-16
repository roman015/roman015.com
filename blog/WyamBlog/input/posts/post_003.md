Title: Moving from Azure to DigitalOcean
Published: 05/16/2020
Tags: ["DigialOcean", "Moving", "Circle CI", "Github"]
---
I've moved from Azure to DigitalOcean!

Well... I had some more interesting plans and spending 60 bucks on the next tier of hosting solutions on Azure isn't exactly my thing (or on my budget) - so from now on I'm hosting this site on DigitalOcean. 

<img title="Did anyone notice that the logo on the website is a single word? I just noticed that while typing out this blog" src="/assets/img/DO_Powered_by_Badge_blue.png" height="40px"></img>

Also, the source code is finally on GitHub too - you can see it over here at <br/>
<a href="https://github.com/roman015/roman015.com"><img src="/assets/img/GitHub-Mark-120px-plus.png" height="20px" width="20px"></img> https://github.com/roman015/roman015.com</a>

So the basic setup is like this now - Source code on Github is picked up by Circle CI which builds the code and deploys the .Net Core executables to the DigitalOcean Droplet. The Droplet consists of an nginx server which reverse proxys the http requests to the different .Net Core Applications (Currently there's only two of them, but there's more to come!)