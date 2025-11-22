using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NutriAI_Core.DTOs.Options;
using NutriAI_Core.Interfaces;
using NutriAI_Services.Services.Auth;
using NutriAI_Services.Services.Common;
using NutriAI_Services.Services.PDF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace NutriAI_Services
{
    public static class ServiceRegistration
    {
        public static void AddProjectServices(this IServiceCollection services, IConfiguration? configuration = null)
        {
            // Register specific services manually since interfaces are in different assembly
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IMasterDataService, MasterDataService>();
            services.AddScoped<IIngredienteService, IngredienteService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IPerfilService, PerfilService>();
            services.AddScoped<IPdfDocumentService, PdfDocumentService>();

            // Auto-register any other services that follow the pattern I[Nombre]Service -> [Nombre]Service
            // within the same assembly
            var assembly = Assembly.GetExecutingAssembly();
            var types = assembly.GetTypes()
                .Where(t => t.IsClass && !t.IsAbstract && t.Name.EndsWith("Service"))
                .Where(t => t != typeof(AuthService) && 
                           t != typeof(MasterDataService) && 
                           t != typeof(IngredienteService) && 
                           t != typeof(UsuarioService) && 
                           t != typeof(PerfilService) &&
                           t != typeof(PdfDocumentService)) // Exclude manually registered services
                .Select(t => new
                {
                    Service = t.GetInterface($"I{t.Name}"),
                    Implementation = t
                })
                .Where(t => t.Service != null);

            foreach (var t in types)
            {
                services.AddScoped(t.Service!, t.Implementation);
            }
        }
    }
}
