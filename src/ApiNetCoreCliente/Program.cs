using ApiNetCoreCliente.Configurations;
using ApiNetCoreCliente.Repository;
using ApiNetCoreCliente.Setup;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;

//-----------ConfigureServices------------
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.ConfigureApiVersioning();
builder.Services.ConfigureSwagger();
builder.Services.AddHealthChecks();
builder.Services.AddDbContext<ClienteContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnectionString"), x => x.MigrationsAssembly(typeof(ClienteContext).Assembly.FullName)));
builder.Services.RegisterServices();
builder.Services.AddMediatRApi();

//-----------Configure--------------------
var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.ConfigureSwagger(app.Services.GetService<IApiVersionDescriptionProvider>());
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.UseHealthChecks("/clientes-webapi/Health");
app.MapControllers();
app.Run();
