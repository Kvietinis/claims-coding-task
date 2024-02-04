using System.Text.Json.Serialization;
using Claims.Auditing.IoC;
using Claims.Business.IoC;
using Claims.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }
);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddOptions();
builder.Services.Configure<CosmosDbConnectionSettings>(builder.Configuration.GetSection(nameof(CosmosDbConnectionSettings)));

builder.Services.ConfigureBusinessServices(builder.Configuration);
builder.Services.ConfigureAuditingServices(builder.Configuration);

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

app.Run();

namespace Claims.RestApi
{
    public partial class Program { }
}