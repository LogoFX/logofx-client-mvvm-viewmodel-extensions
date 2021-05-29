using Attest.Testing.Core;
using Attest.Testing.SpecFlow;
using JetBrains.Annotations;
using Solid.Practices.IoC;
using Solid.Practices.Modularity;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Infra
{
    [UsedImplicitly]
    internal sealed class Module : ICompositionModule<IDependencyRegistrator>
    {
        public void RegisterModule(IDependencyRegistrator dependencyRegistrator) => dependencyRegistrator
            .AddSingleton<IKeyValueDataStore, ScenarioContextKeyValueDataStoreAdapter>();
    }
}
