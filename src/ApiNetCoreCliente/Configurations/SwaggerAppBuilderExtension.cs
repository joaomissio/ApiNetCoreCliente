using Microsoft.AspNetCore.Mvc.ApiExplorer;

namespace ApiNetCoreCliente.Configurations;

public static class SwaggerAppBuilderExtension
{
    private const string AppRouteName = "clientes-webapi";
    private const string AppRouteSwagger = "swagger";
    private const string ServiceName = "Clientes WebApi";
    public static void ConfigureSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider apiVersionDescriptionProvider)
    {
        app.UseSwagger(x => x.RouteTemplate = $"{AppRouteName}/{AppRouteSwagger}/" + "{documentName}/swagger.json");
        app.UseSwaggerUI(s =>
        {
            s.RoutePrefix = $"{AppRouteName}/{AppRouteSwagger}";
            foreach (var desc in apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                s.SwaggerEndpoint($"/{AppRouteName}/{AppRouteSwagger}/{desc.GroupName}/swagger.json", $"{ServiceName} - {desc.ApiVersion}");
            }
        });
    }
}
