using Attest.Testing.Contracts;
using Attest.Testing.Core;
using Attest.Testing.SpecFlow;
using JetBrains.Annotations;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;
using RootObjectScenarioDataStore = Attest.Testing.Core.RootObjectScenarioDataStore;
using ScenarioHelper = Attest.Testing.Core.ScenarioHelper;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Infra
{
    [UsedImplicitly]
    internal sealed class Module : ICompositionModule<IDependencyRegistrator>
    {
        public void RegisterModule(IDependencyRegistrator dependencyRegistrator) => dependencyRegistrator
            .AddSingleton<IStartApplicationService, StartApplicationService>()
            .AddSingleton<IDataAccessor, ScenarioContextDataAccessorAdapter>()
            .AddSingleton<ScenarioHelper>()
            .AddSingleton<RootObjectScenarioDataStore>()
            .UseLocalApplicationForIntegration();
    }
}
