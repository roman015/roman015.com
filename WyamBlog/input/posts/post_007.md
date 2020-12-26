Title: Timezone Tool 
Published: 12/26/2020
Tags: ["Timezone", "Blazor", "Webassembly"]
---
This time it's a simple tool to view the same time across multiple timezones. Add as many clocks as desired, and if you change the time on any of the clocks the others will change to show the exact same time on the time zone selected above it. I'll be adding the daylight savings feature sometime later.

I made this mainly to try out the [Parameters](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/cascading-values-and-parameters?view=aspnetcore-5.0) and [EventCallback](https://docs.microsoft.com/en-us/aspnet/core/blazor/components/event-handling?view=aspnetcore-5.0#eventcallback)features that can be tacked on to components.

The Timezone tool can be viewed over [Here](https://www.roman015.com/TimezoneTool)

One interesting thing I found out while making this was how many timezones there are. Boy there's a lot of them! For now, I just used the Canonical (I hope that's enough!) list of items from Wikipedia's entry for the [tz database](https://en.wikipedia.org/wiki/List_of_tz_database_time_zones) for the dropdown.