using System.Reflection;
using Autofac;
using Autofac.Extras.DynamicProxy;
using DocumentManagement.Api.Infrastructure.Interceptors;
using DocumentManagement.Commands.Commands;
using MediatR;

namespace DocumentManagement.Api.Infrastructure.AutofacModules
{
    public class MediatorModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder
                .RegisterAssemblyTypes(typeof(UploadDocumentCommand).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("CommandHandler"))
                .AsImplementedInterfaces()
                .EnableInterfaceInterceptors()
                .InterceptedBy(typeof(ExceptionInterceptor));

            builder.Register<ServiceFactory>(ctx =>
            {
                var componentContext = ctx.Resolve<IComponentContext>();
                return t =>
                    componentContext.Resolve(t);
            });
        }
    }
}
