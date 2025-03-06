using Microsoft.EntityFrameworkCore;
using TechFixSolution.AuthServices.Data;
using TechFixSolution.AuthServices.Services;

var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddDbContext<UserContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<AuthService>();

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

// Add controllers and Swagger/OpenAPI support
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure Swagger UI if in the Development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Enable CORS for the API Gateway
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
app.UseAuthorization();

// Map the controllers to their routes
app.MapControllers();

app.Run();
