using Microsoft.EntityFrameworkCore;
using TechFixSolution.OrderServices.Data;
using TechFixSolution.OrderServices.Services;

var builder = WebApplication.CreateBuilder(args);

// Database Configuration
builder.Services.AddDbContext<OrderContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register HttpClient with BaseAddress
builder.Services.AddHttpClient("QuotationClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:QuotationService"]);
});
builder.Services.AddHttpClient("InventoryClient", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["Services:InventoryService"]);
});

// Service Registration
builder.Services.AddScoped<OrderService>();

// CORS Configuration
builder.Services.AddCors(options => options.AddPolicy("AllowAll", policy =>
    policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

// API Configuration
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Middleware Pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");
app.MapControllers();

// Database Migration
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<OrderContext>();
    db.Database.Migrate();
}

app.Run();