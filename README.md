# roman015.com

This repo contains the source code used to host www.roman015.com. Currently in it's third iteration, which is implemented using .Net 5 and Blazor (Web Assembly).

# Components

The three components of the current iteration is kept in three branches:

1. HomePage
2. Blog
3. Roman015API

These are currently hosted using Digital Ocean's App Platform with HomePage and Blog as static sites and Roman015API as a docker app. 

# Previous Code

Older Versions of this website are hosted in the following repositories:

## 2.0
Unlike the other versions, everything was placed inside one single branch with the components in separate folders. The source code can be found in https://github.com/roman015/roman015.com/tree/2.0

This has 3 main components in three different folders, "www" contains the main website, "blog" contains the blog and "worker" contained code for a Discord Bot that is no longer in use.

## 1.0
* https://github.com/roman015/ServerFrontEnd
This contains the code for the frontend made in ASP.NET Core 
 
* https://github.com/roman015/ServerCode
This contains the code for the Proxy Server using .Net Core
 
* https://github.com/roman015/ServerAPIs
This contains the code for the rest APIs using .Net Core
