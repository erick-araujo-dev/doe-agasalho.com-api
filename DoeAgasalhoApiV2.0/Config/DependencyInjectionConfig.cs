using DoeAgasalhoApiV2._0.Repository.Interface;
using DoeAgasalhoApiV2._0.Repository;
using DoeAgasalhoApiV2._0.Services;
using DoeAgasalhoApiV2._0.Services.Interface;
using DoeAgasalhoApiV2._0.Repositories;
using DoeAgasalhoApiV2._0.Repositories.Interface;

namespace DoeAgasalhoApiV2._0.Config
{
    public class DependencyInjectionConfig
    {
        public static void Configure(IServiceCollection services)
        {
            services.AddScoped<IUsuarioRepository, UsuarioRepository>();            
            services.AddScoped<IPontoColetaRepository, PontoColetaRepository>();            
            services.AddScoped<IProdutoRepository, ProdutoRepository>();            
            services.AddScoped<ITamanhoRepository, TamanhoRepository>();            
            services.AddScoped<ITipoRepository, TipoRepository>();            
            services.AddScoped<IDoacaoRepository, DoacaoRepository>();            
            services.AddScoped<IPontoColetaService, PontoColetaService>();            
            services.AddScoped<IUsuarioService, UsuarioService>();            
            services.AddScoped<ILoginService, LoginService>();            
            services.AddScoped<ITokenService, TokenService>();            
            services.AddScoped<IEnderecoService, EnderecoService>();
            services.AddScoped<IUtilsService, UtilsService>();
            services.AddScoped<IProdutoService, ProdutoService>();
            services.AddScoped<ITamanhoService, TamanhoService>();
            services.AddScoped<ITipoService, TipoService>();
            services.AddScoped<IDoacaoService, DoacaoService>();

        }
    }
}
