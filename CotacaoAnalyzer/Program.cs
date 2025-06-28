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

    builder.Services.AddScoped<IProdutoRepository, ProdutoRepository>();
    builder.Services.AddScoped<ICotacaoRepository, CotacaoRepository>();

    builder.Services.AddScoped<IEntidadeLeituraRepository, EntidadeLeituraRepository>();

    builder.Services.AddAutoMapper(typeof(ProdutoProfile).Assembly);
    builder.Services.AddAutoMapper(typeof(ProdutoProfile), typeof(CotacaoItemProfile));
    builder.Services.AddAutoMapper(typeof(CotacaoItemProfile), typeof(CotacaoProfile));

    builder.Services.AddControllersWithViews();
    builder.Services.AddEndpointsApiExplorer();


    var app = builder.Build();

    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthorization();

    app.MapControllerRoute
    (
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
    );

    app.Run();

}
catch (Exception ex)
{
    Console.WriteLine($"Erro não tratado: {ex}");
    Console.ReadLine(); // Para ver o erro antes de fechar
}