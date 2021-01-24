# CLRStats

A plug-in that counts the usage of .NET application resources, including: CPU usage, GC, thread status, support for obtaining status information through web requests (access paths and authentication can be customized), and data will be returned in JSON format.

- Support .NET Framework >= 4.5
- Support .NET Core >= 2.0

Installation
-----------------

CLRStats can be obtained through the NuGet website, and you can install it with the following command:

```
PM> Install-Package CLRStats
```

Usage
-----------------

The samples folder of the warehouse contains samples for ASP.NET MVC and ASP.NET Core.

**ASP.NET MVC**

The project needs to create a new OWIN startup file, the configuration is as follows:

```csharp
public class Startup
{
	public void Configuration(IAppBuilder app)
	{
		app.UseCLRStatsDashboard();
	}
}
```

After the configuration is complete, start the project, and by visiting the site **/clr** path, you can obtain the following information:

> {"Server":{"MachineName":"DESKTOP-ZH5FQFC","SystemDateTime":"2021/1/24 20:05:44"},"Application":{"CPU":{"UsagePercent":1.171875},"GC":{"Gen0CollectCount":0,"Gen1CollectCount":0,"Gen2CollectCount":0,"HeapMemory":60529392,"HeapMemoryFormat":"57 M","IsServerGC":true},"Thread":{"AvailableCompletionPortThreads":1000,"AvailableWorkerThreads":8190,"UsedCompletionPortThreads":0,"UsedWorkerThreads":1,"UsedThreadCount":39,"MaxCompletionPortThreads":1000,"MaxWorkerThreads":8191}}}


**ASP.NET Core**

In this example, the access path will be customized and the identity authentication function will be added.

Identity authentication needs to inherit the IDashboardAuthorizationFilter class and implement the Authorize method in it. The code is as follows:

```csharp
public class TokenVerification : IDashboardAuthorizationFilter
{
	public bool Authorize(HttpRequest request)
	{
		if (request.Headers.ContainsKey("Token") && request.Headers["Token"].Equals("test"))
		{
			return true;
		}
		return false;
	}
}
```

To customize the access path and add identity authentication, in the Configure method of the Startup class, add the following configuration:

```csharp
app.UseCLRStatsDashboard("/custom-link", new DashboardOptions()
{
	Authorization = new IDashboardAuthorizationFilter[] { new TokenVerification() }
});
```

After the configuration is complete, enable the project, access the site **/custom-link** path, and the Token parameter needs to be carried in the request header, with the value: test, to be able to access successfully.

Use the curl tool under Windows to test, the command is as follows:
```bash
curl "http://localhost:4409/custom-link" --header "Token: test"
```

The following picture:

![CLRStats](https://attach.itsvse.com/img/clr.png "CLRStats")



