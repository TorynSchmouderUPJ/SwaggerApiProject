using PersonApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Register controllers with Newtonsoft.Json as the default JSON serializer
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options =>
    {
        options.SerializerSettings.Formatting = Newtonsoft.Json.Formatting.Indented;
        options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
    });

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Person API",
        Version = "v1",
        Description = "A simple Person WebAPI with JSON file persistence."
    });
});

// Pre-warm the FileManager singleton so the data file is ready
_ = FileManager.Instance;

var app = builder.Build();

// Middleware pipeline 
app.UseSwagger(); 
app.UseSwaggerUI();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
