using Autofac;
using DocumentManagement.Api.Infrastructure.AppSettings;
using DocumentManagement.Api.Infrastructure.Interceptors;
using DocumentManagement.Api.Infrastructure.TracingStore;
using DocumentManagement.Api.Services;
using DocumentManagement.Commands.Common.Compensations;
using DocumentManagement.Commands.Common.ExecutionHistory;
using DocumentManagement.Data.DataServices;
using DocumentManagement.Data.Repositories;
using DocumentManagement.Domain;

namespace DocumentManagement.Api.Infrastructure.AutofacModules
{
    public class ApplicationModule : Module
    {
        private readonly UploadFileSettings _uploadFileSettings;
        private readonly AzBlobStorageSettings _azBlobStorageSettings;

        public ApplicationModule(UploadFileSettings uploadFileSettings, 
            AzBlobStorageSettings azBlobStorageSettings)
        {
            _uploadFileSettings = uploadFileSettings;
            _azBlobStorageSettings = azBlobStorageSettings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CompensationActionsFactory>()
                .As<ICompensationActionsFactory>()
                .InstancePerLifetimeScope();

            builder.RegisterAssemblyTypes(typeof(ICompensationAction).Assembly)
                .Where(t => typeof(ICompensationAction).IsAssignableFrom(t))
                .InstancePerDependency()
                .AsImplementedInterfaces();

            builder.RegisterType<ExceptionInterceptor>();

            builder.RegisterType<ExecutionHistoryStore>()
                .As<IExecutionHistoryStore>()
                .InstancePerLifetimeScope();

            builder.RegisterType<BlobDataService>()
                .As<IBlobDataService>()
                .WithParameter("connectionString", _azBlobStorageSettings.ConnectionString)
                .WithParameter("containerName", _azBlobStorageSettings.ContainerName)
                .InstancePerLifetimeScope();

            builder.RegisterType<FileStreamReader>()
                .WithParameter("sizeLimit", _uploadFileSettings.SizeLimit)
                .WithParameter("permittedExtensions", _uploadFileSettings.PermittedExtensions)
                .As<IFileStreamReader>()
                .InstancePerLifetimeScope();

            builder.RegisterType<DocumentRepository>()
                .As<IDocumentRepository>()
                .InstancePerLifetimeScope();
        }
    }
}
