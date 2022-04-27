using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Lombardy;

using R5T.T0063;


namespace R5T.Gepidia.Local
{
    public static partial class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="LocalFileSystemOperator"/> implementation of <see cref="ILocalFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddLocalFileSystemOperatorOnly(this IServiceCollection services,
            IServiceAction<IStringlyTypedPathOperator> stringlyTypedPathOperatorAction)
        {
            services
                .AddSingleton<ILocalFileSystemOperator, LocalFileSystemOperator>()
                .Run(stringlyTypedPathOperatorAction)
                ;

            return services;
        }

        /// <summary>
        /// Adds the <see cref="ILocalFileSystemOperator"/> implementation of <see cref="IFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddFileSystemOperator(this IServiceCollection services,
            IServiceAction<ILocalFileSystemOperator> localFileSystemOperatorAction)
        {
            services
                .Run(localFileSystemOperatorAction)
                .AddSingleton<IFileSystemOperator>(serviceProvider =>
                {
                    var localFileSystemOperator = serviceProvider.GetRequiredService<ILocalFileSystemOperator>();
                    return localFileSystemOperator;
                })
                ;

            return services;
        }
    }
}
