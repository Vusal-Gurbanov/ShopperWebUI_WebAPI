using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.FileProviders;

namespace Shopper_WebUI.Middlewares
{
    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder CustomStaticFiles(this IApplicationBuilder app)
        {
            //Directory.GetCurrentDirectory() => C:\Users\altan\source\repos\YAZILIMUZMANLIGI_19-22\WEB YAZILIM\CORE6\Shopper\Shopper_WebUI
            var path = Path.Combine(Directory.GetCurrentDirectory(), "node_modules");

            var options = new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(path),
                RequestPath = "/modules"
            };

            app.UseStaticFiles(options);

            return app;
        }
    }
}
