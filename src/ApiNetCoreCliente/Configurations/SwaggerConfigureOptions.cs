using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiNetCoreCliente.Configurations;

public class SwaggerConfigureOptions : IConfigureOptions<SwaggerGenOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public SwaggerConfigureOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

    public void Configure(SwaggerGenOptions options)
    {
        foreach (var apiVersionDesc in _provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(apiVersionDesc.GroupName, new OpenApiInfo
            {
                Version = apiVersionDesc.ApiVersion.ToString(),
                Title = "Web Api Clientes",
                Description = "Api de Cadastro de Clientes",
                Contact = new OpenApiContact() { Email = "joao-missio@hotmail.com", Name = "João Orlando Missio" }
            });
        }
    }
}
