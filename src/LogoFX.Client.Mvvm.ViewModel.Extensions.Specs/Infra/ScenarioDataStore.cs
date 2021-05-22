using Attest.Testing.SpecFlow;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.ViewModels;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Tests;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Infra
{
    [UsedImplicitly]
    public sealed class ScenarioDataStore : ScenarioDataStoreBase
    {
        public ScenarioDataStore(
            ScenarioContext scenarioContext) : 
            base(scenarioContext)
        {
        }

        public TestEditableScreenSimpleObjectViewModel SystemUnderTest
        {
            get => GetValueImpl<TestEditableScreenSimpleObjectViewModel>();
            set => SetValueImpl(value);
        }

        public SimpleEditableModel Model
        {
            get => GetValueImpl<SimpleEditableModel>();
            set => SetValueImpl(value);
        }

        public FakeMessageService MockMessageService
        {
            get => GetValueImpl<FakeMessageService>();
            set => SetValueImpl(value);
        }

        public bool WasDirtyRaised
        {
            get => GetValueImpl<bool>();
            set => SetValueImpl(value);
        }

        public bool WasCancelChangesRaised
        {
            get => GetValueImpl<bool>();
            set => SetValueImpl(value);
        }

        public TestConductorViewModel RootObject
        {
            get => GetValueImpl<TestConductorViewModel>();
            set => SetValueImpl(value);
        }
    }
}
