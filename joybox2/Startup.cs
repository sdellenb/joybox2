using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Funq;
using ServiceStack;
using joybox2.ServiceInterface;
using System;
using ServiceStack.Text;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace joybox2 {
    public class Startup : ModularStartup {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public new void ConfigureServices(IServiceCollection services) {
            services.AddSingleton<IDbConnectionFactory>(
                // InMemory Sqlite DB, replace with environment-dependent path to use a static file.
                new OrmLiteConnectionFactory(":memory:", SqliteDialect.Provider)
            );
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseServiceStack(new AppHost {
                AppSettings = new NetCoreAppSettings(Configuration)
            });

            // Override GUID serializer to keep the dashes.
            JsConfig<Guid>.SerializeFn = guid => guid.ToString();
        }
    }

    public class AppHost : AppHostBase {
        public AppHost() : base("joybox2", typeof(MyServices).Assembly) { }

        // Configure your AppHost with the necessary configuration and dependencies your App needs
        public override void Configure(Container container) {
            Plugins.Add(new SharpPagesFeature()); // enable server-side rendering, see: https://sharpscript.net/docs/sharp-pages

            SetConfig(new HostConfig {
                UseSameSiteCookies = true,
                AddRedirectParamsToQueryString = true,
                DebugMode = AppSettings.Get(nameof(HostConfig.DebugMode), HostingEnvironment.IsDevelopment()),
            });
        }
    }
}
