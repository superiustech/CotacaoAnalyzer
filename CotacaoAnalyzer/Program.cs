using Bussiness.Services;
using Domain.Interfaces;
using Infra.Contexts;
using Infra.Mapping;
using Infra.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddCors(options =>
    {
        options.AddPolicy("AllowReactApp",
            policy =>
            {
                policy.WithOrigins(
                        "https://localhost:5102",
                        "http://localhost:5103",
                        "http://localhost:3000",
                        "https://localhost:3000"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
    });

    builder.Services.AddDbContext<AnalyzerDbContext>(options =>
        options.UseNpgsql(
            builder.Configuration.GetConnectionString("DefaultConnection")
        )
    );

    builder.Services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "Cotação Analyzer", Version = "v1" });
    });

    builder.Services.AddScoped<IProdutoService, ProdutoService>();
    builder.Services.AddScoped<ICotacaoService, CotacaoService>();
    builder.Services.AddScoped<IScoreService, ScoreService>();

    builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
    builder.Services.AddScoped<ICotacaoRepository, CotacaoRepository>();
    builder.Services.AddScoped<IScoreRepository, ScoreRepository>();

    builder.Services.AddScoped<IEntidadeLeituraRepository, EntidadeLeituraRepository>();

    builder.Services.AddAutoMapper(typeof(ProdutoProfile).Assembly);
    builder.Services.AddAutoMapper(typeof(ScoreProfile).Assembly);
    builder.Services.AddAutoMapper(typeof(ProdutoProfile), typeof(CotacaoItemProfile));
    builder.Services.AddAutoMapper(typeof(CotacaoItemProfile), typeof(CotacaoProfile));

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();


    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseCors("AllowReactApp");
    app.UseAuthorization();
    app.MapControllers();
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine($"Erro não tratado: {ex}");
    Console.ReadLine();
}