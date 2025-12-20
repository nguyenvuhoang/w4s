using O24OpenAPI.Kit.OCR.Abstractions;
using O24OpenAPI.Kit.OCR.Options;
using O24OpenAPI.Kit.OCR.Services;
using Tesseract;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
