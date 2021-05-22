using Attest.Testing.SpecFlow;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.ViewModels;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Infra
{
    [UsedImplicitly]
    public sealed class CommonScenarioDataStore : ScenarioDataStoreBase
    {
        public CommonScenarioDataStore(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        public TestConductorViewModel RootObject
        {
            get => GetValueImpl<TestConductorViewModel>();
            set => SetValueImpl(value);
        }
    }
}