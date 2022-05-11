using AdvertisePublish.Mapper;
using AdvertisePublish.Models;
using AdvertisePublish.Validation;
using AdvertisePudlish.Middleware;
using AdvertisePudlish.Services.Abstractions;
using AdvertisePudlish.Services.Implementation;
using Domain;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

ConfigurationManager configuration = builder.Configuration;

builder.Services.AddDbContext<AppDbContext>((DbContextOptionsBuilder options) =>

               options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IImageConverter,ImageConverter>();
builder.Services.AddControllers();
builder.Services.AddControllersWithViews().AddFluentValidation();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IValidator<CreateAdvertiseViewModel>, AdvertiseValidator>();

builder.Services.AddAutoMapper(typeof(AdvertiseProfile));

builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    options.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
});

builder.Services.AddSwaggerGen((SwaggerGenOptions o) =>{


    o.SwaggerDoc("v1", new OpenApiInfo
    {


        Description = "Test project",
        Version = "v1",
        Title = "Advertise API example"
    });   
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    o.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
}); 

builder.Services.AddCors();

var app = builder.Build();


app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseSwagger();
app.UseSwaggerUI((SwaggerUIOptions c) =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Advertise service");
});

app.UseStaticFiles();

string folderName = "images";

var dir = Path.Combine(Directory.GetCurrentDirectory(), folderName);

if (!Directory.Exists(dir))
{
    Directory.CreateDirectory(dir);
}

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(dir),
    RequestPath = "/images"
});

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.UseCustomExceptionHandler();

app.UseCors(x => x
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller}/{action=Index}/{id?}");
});

app.Run();
