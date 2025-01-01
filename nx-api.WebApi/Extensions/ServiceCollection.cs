using nx_api.Application.Services.Users;
using nx_api.Data.Context;
using nx_api.Data.Repositories;
using nx_api.Domain.Context;
using nx_api.Domain.Repositories.Users;
using nx_api.Domain.Services.Users;

namespace nx_api.WebApi.Extensions
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddContexts(this IServiceCollection services)
        {            
            services.AddSingleton<INXDBContext, NxContext>();            
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddSingleton<IUserRepository, UserRepository>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddSingleton<IUserService, UserService>();
            return services;
        }
    }
}
