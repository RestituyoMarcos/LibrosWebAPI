using BooksWebAPI.Services;
using MediatR;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<IBookService, BookService>();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var apiBaseUrl = builder.Configuration.GetSection("ExternalAPI:BaseUrl").Value;
var webAppURL = builder.Configuration.GetSection("ExternalAPI:WebAppURL").Value;

builder.Services.AddHttpClient("BaseURL", client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddCors(op =>
{
    op.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins(webAppURL).AllowAnyMethod().AllowAnyHeader().AllowCredentials();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseCors();

app.Run();
