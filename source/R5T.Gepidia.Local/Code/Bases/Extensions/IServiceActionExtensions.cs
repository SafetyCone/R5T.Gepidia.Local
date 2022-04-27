using System;

using Microsoft.Extensions.DependencyInjection;

using R5T.Lombardy;

using R5T.T0062;
using R5T.T0063;


namespace R5T.Gepidia.Local
{
    public static class IServiceActionExtensions
    {
        /// <summary>
        /// Adds the <see cref="LocalFileSystemOperator"/> implementation of <see cref="ILocalFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<ILocalFileSystemOperator> AddLocalFileSystemOperatorActionOnly(this IServiceAction _,
            IServiceAction<IStringlyTypedPathOperator> stringlyTypedPathOperatorAction)
        {
            var serviceAction = _.New<ILocalFileSystemOperator>(services => services.AddLocalFileSystemOperatorOnly(
                stringlyTypedPathOperatorAction));

            return serviceAction;
        }

        /// <summary>
        /// Forwards the <see cref="ILocalFileSystemOperator"/> implementation of <see cref="IFileSystemOperator"/> as a <see cref="ServiceLifetime.Singleton"/>.
        /// </summary>
        public static IServiceAction<IFileSystemOperator> AddFileSystemOperatorAction(this IServiceAction _,
            IServiceAction<ILocalFileSystemOperator> localFileSystemOperatorAction)
        {
            var serviceAction = _.New<IFileSystemOperator>(services => services.AddFileSystemOperator(
                localFileSystemOperatorAction));

            return serviceAction;
        }

        public static (
            IServiceAction<ILocalFileSystemOperator> localFileSystemOperatorAction,
            IServiceAction<IFileSystemOperator> fileSystemOperatorAction
            )
            AddLocalFileSystemOperatorAction(this IServiceAction _,
            IServiceAction<IStringlyTypedPathOperator> stringlyTypedPathOperatorAction)
        {
            var localFileSystemOperatorAction = _.AddLocalFileSystemOperatorActionOnly(stringlyTypedPathOperatorAction);
            var fileSystemOperatorAction = _.AddFileSystemOperatorAction(localFileSystemOperatorAction);

            return (
                localFileSystemOperatorAction,
                fileSystemOperatorAction);
        }
    }
}
