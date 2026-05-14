using Microsoft.EntityFrameworkCore;
using ShiftsLoggerAPI.Ledana.Data;
using ShiftsLoggerAPI.Ledana.Services;

var builder = WebApplication.CreateBuilder(args);

//1configuring the connection to the database using sql server
var connectionString = builder.Configuration.GetConnectionString("ShiftsLoggerDb")
                       ?? throw new InvalidOperationException("Connection string 'ShiftsLoggerDb' not found!");
builder.Services.AddDbContext<ShiftContext>(options =>
        options.UseSqlServer(connectionString));

//2injecting shift service
builder.Services.AddScoped<IShiftService, ShiftService>();
//5adding the worker service
builder.Services.AddScoped<IEmplyeeService, EmployeeService>();

// Add services to the container.
//3configuring json serializer to handle cycles
builder.Services.AddControllers().AddJsonOptions(opt =>
    opt.JsonSerializerOptions.ReferenceHandler =
    System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles);

//4adding the mapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    
}

app.MapControllers();

app.Run();
