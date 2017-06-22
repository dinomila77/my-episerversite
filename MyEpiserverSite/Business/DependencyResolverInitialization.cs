using System;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

namespace MyEpiserverSite.Business
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class DependencyResolverInitialization : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            DependencyResolver.SetResolver(new StructureMapDependencyResolver(context.Container));
        }

        public void Initialize(InitializationEngine context)
        {
            //Add initialization logic, this method is called once after CMS has been initialized
        }

        public void Uninitialize(InitializationEngine context)
        {
            //Add uninitialization logic
        }

        public void Preload(string[] parameters)
        {
        }
    }
}