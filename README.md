Universal Analytics ![project-status](http://stillmaintained.com/echovoice/UniversalAnalytics.png)
============

A C# library for sending server-side tracking data to Google Analytics using the new Measurement Protocol (Universal Analytics)

This library is available on Nuget as [UniversalAnalytics](https://www.nuget.org/packages/UniversalAnalytics).



Why use Google Universal Analytics?
============================
You get **REAL** user data inside Google Analytics.
Thanks to this new API from Google, we can actually **send a visitors real IP and User agent**.


Page Tracking Usage
============================

```csharp
public static UATracker tracker = new UATracker("UA-XXXXX-X");

protected void Application_BeginRequest(object sender, EventArgs e)
{
	tracker.TrackPageView();
}
```

You can call the method **ASYNC** to prevent Google from slowing down your requests ```TrackPageViewAsync()```.

```csharp
public static UATracker tracker = new UATracker("UA-XXXXX-X");

protected void Application_BeginRequest(object sender, EventArgs e)
{
	tracker.TrackPageViewAsync();
}
```

Sometimes your server is behind a proxy and UserHostAddress is not the end user.
Instead you need to use something like **Headers["X-Forwarded-For"]**.

Build a ```UAClient``` object and override the IP address, then pass that into the page view methods

```csharp
public static UATracker tracker = new UATracker("UA-XXXXX-X");

protected void Application_BeginRequest(object sender, EventArgs e)
{
	UAClient client = new UAClient(); 
    client.IPOverride("XXX.XXX.XXX.XXX");

    tracker.TrackPageViewAsync(client);
}
```

**You only have to do this if the data is being routed from a proxy, otherwise this library automatically takes care of feeding Google the ```UserHostAddress``` value**

In addition you can manually set the **page** **title**, **url** and **hostname**.

```csharp
public static UATracker tracker = new UATracker("UA-XXXXX-X");

protected void Application_BeginRequest(object sender, EventArgs e)
{
	tracker.TrackPageViewAsync("Page Title Here", "/url", "example.com");
}
```


Event Tracking Usage
============================

```csharp
public static UATracker tracker = new UATracker("UA-XXXXX-X");

protected void Application_BeginRequest(object sender, EventArgs e)
{
	tracker.TrackEvent("Category", "Action");
}
```

You can call the method **ASYNC** to prevent Google from slowing down your requests ```TrackEventAsync()```.

You can also send a **label** and **value**.

```csharp
public static UATracker tracker = new UATracker("UA-XXXXX-X");

protected void Application_BeginRequest(object sender, EventArgs e)
{
	tracker.TrackEventAsync("Category", "Action", "Label", 25);
}
```


User Session Tracking
============================

Google now allows you to **track clients between session** by providing a globally unique User ID.

Build a ```UAClient``` object and set both the current session ID and the User ID.

```csharp
public static UATracker tracker = new UATracker("UA-XXXXX-X");

protected void Application_BeginRequest(object sender, EventArgs e)
{
	  UAClient client = new UAClient("session_id_here", "user_id_here");

    tracker.TrackPageViewAsync(client);
}
```

If you supply a real session ID, then you **won't get inflatted user counts in Google Analytics**.


What is the Google Universal Analytics (Measurement Protocol)?
============================
The Google Analytics Measurement Protocol allows developers to make HTTP requests to send raw user interaction data directly to Google Analytics servers. This allows developers to measure how users interact with their business from almost any environment. Developers can then use the Measurement Protocol to:

Measure user activity in new environments.
Tie online to offline behavior.
Send data from both the web and server.

[1]: https://developers.google.com/analytics/devguides/collection/protocol/v1/


Library Restrictions?
============================
 - ASP.NET Managed Apps Only
 - **.NET 4.5.2**

We needed to use the new HostingEnvironment.QueueBackgroundWorkItem method found in .NET 4.5.2 to ensure we didn't have to worry about processes being shut down prematurely by IIS.


Todo
============================
* ~~Event Tracking~~
* Ecommerce Tracking
* Enhanced Ecommerce Tracking
* Social Interactions
* Exception Tracking
* User Timing Tracking
* App / Screen Tracking
