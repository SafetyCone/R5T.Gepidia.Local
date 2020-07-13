using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Dacia;
using R5T.Lombardy;


namespace R5T.Gepidia.Local
{
    public static class IServiceCollectionExtensions
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
        /// Adds the <see cref="LocalFileSystemOperator"/> implementation of <see cref="ILocalFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<ILocalFileSystemOperator> AddLocalFileSystemOperatorOnlyAction(this IServiceCollection services,
            IServiceAction<IStringlyTypedPathOperator> stringlyTypedPathOperatorAction)
        {
            var serviceAction = ServiceAction.New<ILocalFileSystemOperator>(() => services.AddLocalFileSystemOperatorOnly(
                stringlyTypedPathOperatorAction));

            return serviceAction;
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

        /// <summary>
        /// Forwards the <see cref="ILocalFileSystemOperator"/> implementation of <see cref="IFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<IFileSystemOperator> AddFileSystemOperatorAction(this IServiceCollection services,
            IServiceAction<ILocalFileSystemOperator> localFileSystemOperatorAction)
        {
            var serviceAction = ServiceAction.New<IFileSystemOperator>(() => services.AddFileSystemOperator(
                localFileSystemOperatorAction));

            return serviceAction;
        }


        public static (
            IServiceAction<ILocalFileSystemOperator> localFileSystemOperatorAction,
            IServiceAction<IFileSystemOperator> fileSystemOperatorAction
            )
            AddLocalFileSystemOperatorAction(this IServiceCollection services,
            IServiceAction<IStringlyTypedPathOperator> stringlyTypedPathOperatorAction)
        {
            var localFileSystemOperatorAction = services.AddLocalFileSystemOperatorOnlyAction(stringlyTypedPathOperatorAction);
            var fileSystemOperatorAction = services.AddFileSystemOperatorAction(localFileSystemOperatorAction);

            return (
                localFileSystemOperatorAction,
                fileSystemOperatorAction);
        }
    }
}
