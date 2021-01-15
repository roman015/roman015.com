# HomePage
Contains The frontend code for www.roman015.com  

This code is a blazor webassembly project that runs on the client's web browser. This can be run directly from Visual Studio and is hosted in production as a static website using [Digital Ocean's App Platform](https://www.digitalocean.com/products/app-platform/)

## Quirks
- When debugging the project, IIS express knows that directly going to a non-root url means you have to open the static website first. When hosting the website in Digital Ocean's App Platform as a Static Website, you'll have to make a [workaround](https://github.com/roman015/roman015.com/blame/HomePage/App.razor#L48) for that by first setting redirect.html in Digital Ocean as the page to redirect to for all unknown links.