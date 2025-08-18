using API;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorFrontend", builder =>
    {
        builder.WithOrigins("https://localhost:5001") 
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// Ensure MeterReadingService is registered
builder.Services.AddScoped<IMeterReadingService,MeterReadingService>();
builder.Services.AddScoped<IRepositoryService,RepositoryService>();
builder.Services.AddScoped<ICsvReaderService,CsvReaderService>();

// DB Context
builder.Services.AddDbContext<TickdDbContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("TickdDbContext"));
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TickdDbContext>();

    if (!dbContext.Database.CanConnect())
    {
        throw new NotImplementedException("Can't connect to the DB.");
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorFrontend");
app.UseAuthorization();
app.MapControllers();

app.Run();
