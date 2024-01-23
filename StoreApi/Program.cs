using StoreApi.Repositories;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Service registrations
builder.Services.AddControllers();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddHttpClient();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Store API", Version = "v1" });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFilename);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Redirect root to Swagger UI
app.Use(async (context, next) =>
{
    if (string.IsNullOrEmpty(context.Request.Path.Value) || context.Request.Path.Value == "/")
    {
        // Redirect to Swagger UI
        context.Response.Redirect("/swagger");
    }
    else
    {
        await next();
    }
});

app.UseCors(builder =>
    builder.WithOrigins("http://localhost:3000") // Local React app
           .AllowAnyMethod()
           .AllowAnyHeader()
);

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
