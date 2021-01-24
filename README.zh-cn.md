# CLRStats

一个统计 .NET 应用资源使用情况的插件，包含：CPU 使用率、GC、线程情况，支持通过 Web 请求获取状态信息（可以自定义访问路径和身份验证），数据将以 JSON 格式返回。

- 支持 .NET Framework >= 4.5
- 支持 .NET Core >= 2.0

安装
-----------------

CLRStats 可以通过 NuGet 网站获取，您可以使用如下命令进行安装：

```
PM> Install-Package CLRStats
```

用法
-----------------

仓库 samples 文件夹包含 ASP.NET MVC 和 ASP.NET Core 的示例。

**ASP.NET MVC**

项目需要新建 OWIN 启动文件，配置如下：

```csharp
public class Startup
{
	public void Configuration(IAppBuilder app)
	{
		app.UseCLRStatsDashboard();
	}
}
```

配置完成后，启用项目，通过访问站点 **/clr** 路径，可以获取到如下信息：

> {"Server":{"MachineName":"DESKTOP-ZH5FQFC","SystemDateTime":"2021/1/24 20:05:44"},"Application":{"CPU":{"UsagePercent":1.171875},"GC":{"Gen0CollectCount":0,"Gen1CollectCount":0,"Gen2CollectCount":0,"HeapMemory":60529392,"HeapMemoryFormat":"57 M","IsServerGC":true},"Thread":{"AvailableCompletionPortThreads":1000,"AvailableWorkerThreads":8190,"UsedCompletionPortThreads":0,"UsedWorkerThreads":1,"UsedThreadCount":39,"MaxCompletionPortThreads":1000,"MaxWorkerThreads":8191}}}


**ASP.NET Core**

此示例中，将自定义访问路径以及增加身份认证功能。

身份认证需要继承 IDashboardAuthorizationFilter 类，实现其中的 Authorize 方法，代码如下：

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

自定义访问路径和添加身份认证，在 Startup 类 Configure 方法中，添加如下配置：

```csharp
app.UseCLRStatsDashboard("/custom-link", new DashboardOptions()
{
	Authorization = new IDashboardAuthorizationFilter[] { new TokenVerification() }
});
```

配置完成后，启用项目，通过访问站点 **/custom-link** 路径，并且请求头里面需要携带 Token 参数，值为：test，才能够访问成功。

使用 Windows 系统下面 curl 工具进行测试，命令如下：

```bash
curl "http://localhost:4409/custom-link" --header "Token: test"
```

如下图：

![CLRStats](https://attach.itsvse.com/img/clr.png "CLRStats")



