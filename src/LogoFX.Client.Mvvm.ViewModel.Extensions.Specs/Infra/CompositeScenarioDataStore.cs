using Attest.Testing.SpecFlow;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.ViewModels;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Tests;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Infra
{
    [UsedImplicitly]
    public sealed class CompositeScenarioDataStore : ScenarioDataStoreBase
    {
        public CompositeScenarioDataStore(ScenarioContext scenarioContext) : base(scenarioContext)
        {
        }

        public TestEditableScreenCompositeObjectViewModel SystemUnderTest
        {
            get => GetValueImpl<TestEditableScreenCompositeObjectViewModel>();
            set => SetValueImpl(value);
        }

        public CompositeEditableModel Model
        {
            get => GetValueImpl<CompositeEditableModel>();
            set => SetValueImpl(value);
        }

        public FakeMessageService MockMessageService
        {
            get => GetValueImpl<FakeMessageService>();
            set => SetValueImpl(value);
        }
    }
}