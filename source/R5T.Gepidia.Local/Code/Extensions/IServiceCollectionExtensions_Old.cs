using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Dacia;
using R5T.Lombardy;


namespace R5T.Gepidia.Local
{
    public static partial class IServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="LocalFileSystemOperator"/> implementation of <see cref="ILocalFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddLocalFileSystemOperatorOnly_Old(this IServiceCollection services,
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
        public static IServiceAction<ILocalFileSystemOperator> AddLocalFileSystemOperatorOnlyAction_Old(this IServiceCollection services,
            IServiceAction<IStringlyTypedPathOperator> stringlyTypedPathOperatorAction)
        {
            var serviceAction = ServiceAction.New<ILocalFileSystemOperator>(() => services.AddLocalFileSystemOperatorOnly_Old(
                stringlyTypedPathOperatorAction));

            return serviceAction;
        }

        /// <summary>
        /// Adds the <see cref="ILocalFileSystemOperator"/> implementation of <see cref="IFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddFileSystemOperator_Old(this IServiceCollection services,
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
        public static IServiceAction<IFileSystemOperator> AddFileSystemOperatorAction_Old(this IServiceCollection services,
            IServiceAction<ILocalFileSystemOperator> localFileSystemOperatorAction)
        {
            var serviceAction = ServiceAction.New<IFileSystemOperator>(() => services.AddFileSystemOperator_Old(
                localFileSystemOperatorAction));

            return serviceAction;
        }


        public static (
            IServiceAction<ILocalFileSystemOperator> localFileSystemOperatorAction,
            IServiceAction<IFileSystemOperator> fileSystemOperatorAction
            )
            AddLocalFileSystemOperatorAction_Old(this IServiceCollection services,
            IServiceAction<IStringlyTypedPathOperator> stringlyTypedPathOperatorAction)
        {
            var localFileSystemOperatorAction = services.AddLocalFileSystemOperatorOnlyAction_Old(stringlyTypedPathOperatorAction);
            var fileSystemOperatorAction = services.AddFileSystemOperatorAction_Old(localFileSystemOperatorAction);

            return (
                localFileSystemOperatorAction,
                fileSystemOperatorAction);
        }
    }
}
