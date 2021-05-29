using Attest.Testing.Bootstrapping;
using Attest.Testing.Contracts;
using Attest.Testing.Core;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Infra;
using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Bootstrapping
{
    internal sealed class Startup : StartupBase<Bootstrapper>
    {
        public Startup(IIocContainer iocContainer)
            : base(iocContainer, c => new Bootstrapper(c))
        {

        }

        protected override void InitializeOverride(Bootstrapper bootstrapper)
        {
            base.InitializeOverride(bootstrapper);
            //TODO: Replace with Middleware
            bootstrapper.Registrator
                .AddSingleton<IStartApplicationService, StartApplicationService>()
                .AddSingleton<ScenarioHelper>()
                .AddSingleton<RootObjectScenarioDataStore>()
                .UseLocalApplicationForIntegration();
        }
    }
}