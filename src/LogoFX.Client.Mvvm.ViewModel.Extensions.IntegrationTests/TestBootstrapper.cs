using LogoFX.Client.Bootstrapping;
using LogoFX.Client.Bootstrapping.Adapters.SimpleContainer;
using Solid.Bootstrapping;
using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.IntegrationTests
{
    public class TestBootstrapper : BootstrapperContainerBase<ExtendedSimpleContainerAdapter>
        .WithRootObject<TestConductorViewModel>, IHaveResolver
    {
        private static readonly ExtendedSimpleContainerAdapter _container = new ExtendedSimpleContainerAdapter();

        public TestBootstrapper()
            :base(_container, new BootstrapperCreationOptions
            {
                UseApplication = false                
            })
        {                   
        }

        public IDependencyResolver Resolver => _container;
    }
}
