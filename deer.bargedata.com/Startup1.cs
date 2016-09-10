using System;
using System.Threading.Tasks;
using Microsoft.Owin;
using Owin;
using System.Web.Http;
using System.Collections.Generic;


[assembly: OwinStartup(typeof(deer.bargedata.com.Startup1))]

namespace deer.bargedata.com
{
    using AppFunc = Func<IDictionary<string, object>, Task>;
    public class Startup1
    {
        
        public void Configuration(IAppBuilder app)
        {
            // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=316888

            HttpConfiguration httpConfiguration = new HttpConfiguration();
            WebApiConfig.Register(httpConfiguration);

            //Use this middle ware piece to log requests and responses
            app.Use(new Func<AppFunc, AppFunc>(next => (async env =>
           {
               //gather request data and start the audit record
               await next.Invoke(env);
               //gather response data and finailize the audit record
           })));

            app.UseWebApi(httpConfiguration);
        }
    }
}
