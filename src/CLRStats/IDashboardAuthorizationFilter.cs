#if (NETSTANDARD2_0 || NETSTANDARD2_1)
using Microsoft.AspNetCore.Http;
#endif
#if (NET45 || NET46)
using Microsoft.Owin;
#endif


namespace CLRStats
{
    public interface IDashboardAuthorizationFilter
    {
#if (NETSTANDARD2_0 || NETSTANDARD2_1)
        bool Authorize(HttpRequest request);
#endif
#if (NET45 || NET46)
        bool Authorize(IOwinRequest request);
#endif
    }
}
