using Google.Apis.Auth.OAuth2;
using Google.Cloud.BigQuery.V2;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Spotify 2023", Version = "v1" });
});

// Configure Google Cloud BigQuery
builder.Services.AddSingleton(_ =>
{
    var projectId = "cmoefenen"; // Replace with your actual Google Cloud project ID
    var credential = GoogleCredential.FromFile("cmoefenen-4eea8b75c1ee.json");
    return BigQueryClient.Create(projectId, credential);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Spotify 2023 v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
