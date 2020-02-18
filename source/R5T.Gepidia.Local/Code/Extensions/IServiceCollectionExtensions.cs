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
        public static IServiceCollection AddLocalFileSystemOperator(this IServiceCollection services,
            ServiceAction<IStringlyTypedPathOperator> addStringlyTypedPathOperator)
        {
            services
                .AddSingleton<ILocalFileSystemOperator, LocalFileSystemOperator>()
                .RunServiceAction(addStringlyTypedPathOperator)
                ;

            return services;
        }

        /// <summary>
        /// Adds the <see cref="LocalFileSystemOperator"/> implementation of <see cref="ILocalFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static ServiceAction<ILocalFileSystemOperator> AddLocalFileSystemOperatorAction(this IServiceCollection services,
            ServiceAction<IStringlyTypedPathOperator> addStringlyTypedPathOperator)
        {
            var serviceAction = new ServiceAction<ILocalFileSystemOperator>(() => services.AddLocalFileSystemOperator(
                addStringlyTypedPathOperator));
            return serviceAction;
        }

        /// <summary>
        /// Adds the <see cref="FileSystemOperator"/> implementation of <see cref="IFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddLocalBasedFileSystemOperator(this IServiceCollection services,
            ServiceAction<ILocalFileSystemOperator> addLocalFileSystemOperator)
        {
            services
                .AddSingleton<IFileSystemOperator, FileSystemOperator>()
                .RunServiceAction(addLocalFileSystemOperator)
                ;

            return services;
        }

        /// <summary>
        /// Adds the <see cref="FileSystemOperator"/> implementation of <see cref="IFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static ServiceAction<IFileSystemOperator> AddLocalBasedFileSystemOperatorAction(this IServiceCollection services,
            ServiceAction<ILocalFileSystemOperator> addLocalFileSystemOperator)
        {
            var serviceAction = new ServiceAction<IFileSystemOperator>(() => services.AddLocalBasedFileSystemOperator(
                addLocalFileSystemOperator));
            return serviceAction;
        }

        /// <summary>
        /// Adds the <see cref="FileSystemOperator"/> implementation of <see cref="IFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceCollection AddLocalBasedFileSystemOperator(this IServiceCollection services,
            ServiceAction<IStringlyTypedPathOperator> addStringlyTypedPathOperator)
        {
            services.AddLocalBasedFileSystemOperator(
                services.AddLocalFileSystemOperatorAction(addStringlyTypedPathOperator));

            return services;
        }

        /// <summary>
        /// Adds the <see cref="FileSystemOperator"/> implementation of <see cref="IFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static ServiceAction<IFileSystemOperator> AddLocalBasedFileSystemOperatorAction(this IServiceCollection services,
            ServiceAction<IStringlyTypedPathOperator> addStringlyTypedPathOperator)
        {
            var serviceAction = new ServiceAction<IFileSystemOperator>(() => services.AddLocalBasedFileSystemOperator(
                addStringlyTypedPathOperator));
            return serviceAction;
        }
    }
}
