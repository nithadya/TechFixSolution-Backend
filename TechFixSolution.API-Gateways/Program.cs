var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();  // Add HttpClient for inter-service communication
builder.Services.AddControllers(); // Register controllers

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Configure Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable CORS for the API Gateway
app.UseCors("AllowAll");

// Enable Swagger UI for testing in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Enable Authorization middleware
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Set the server URL and port to 5000
app.Urls.Add("http://localhost:5000");

app.Run();
