using GeminiAI;
using GenAIWithGemini.Client;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
string apiKey = "AIzaSyAkXfp4jCNO1OGRShvkwS7oBcVr5Mm8bPI";
builder.Services.AddSingleton(new GeminiApiClient(apiKey));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.OperationFilter<FileUploadOperationFilter>();
});
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
#region
var port = Environment.GetEnvironmentVariable("PORT") ?? "8081";
builder.WebHost.UseUrls($"http://*:{port}");

//var app = builder.Build();

//Get swagger.json following root directory 
app.UseSwagger(options => { options.RouteTemplate = "{documentName}/swagger.json"; });
//Load swagger.json following root directory 
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/v1/swagger.json", "GeminiAI V1"); c.RoutePrefix = string.Empty; });
#endregion

app.UseCors("AllowAll");

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
