using O24OpenAPI.Kit.OCR.Abstractions;
using O24OpenAPI.Kit.OCR.Options;
using O24OpenAPI.Kit.OCR.Services;
using Tesseract;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.AI;
using OpenAI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IChatClient>(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>()
                   .GetSection("AI:OpenAI");

//Add OCR service
var opt = new TesseractOcrOptions
{
    TessdataPath = Path.Combine(AppContext.BaseDirectory, "tessdata"),
    DefaultLanguage = "vie",
    EngineMode = EngineMode.LstmOnly,
    PageSegMode = PageSegMode.SparseText
};
opt.EngineVariables["user_defined_dpi"] = "300";
opt.EngineVariables["preserve_interword_spaces"] = "1";

builder.Services.AddSingleton<IOcrService>(_ => new TesseractOcrService(opt));

    IChatClient chatClient =
    new OpenAIClient(config["ApiKey"]!).GetChatClient(config["Model"] ?? "gpt-4o-mini").AsIChatClient();

    return chatClient;
});
builder.Services.AddLinKitCqrs();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
app.UseSwagger();
app.UseSwaggerUI();
app.MapGeneratedEndpoints();

app.Run();
