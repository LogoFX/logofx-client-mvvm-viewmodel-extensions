using Attest.Testing.SpecFlow;
using Caliburn.Micro;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.ViewModels;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Infra
{
    public class StartApplicationService : Attest.Testing.Integration.StartApplicationServiceBase
    {
        private readonly ScenarioDataStore _scenarioDataStore;
        private readonly ScenarioContext _scenarioContext;

        protected StartApplicationService(
            ScenarioDataStore scenarioDataStore,
            ScenarioContext scenarioContext)
        {
            _scenarioDataStore = scenarioDataStore;
            _scenarioContext = scenarioContext;
        }

        protected override void Setup()
        {
            base.Setup();
            OnArrange();
            var bootstrapperBridge = new BootstrapperBridge(_scenarioContext);
            bootstrapperBridge.InitializeRootObject();
            LogoFX.Client.Testing.Shared.TestHelper.Setup();
        }

        protected virtual void OnArrange()
        {

        }

        protected override void OnStart(object rootObject)
        {
            base.OnStart(rootObject);
            _scenarioDataStore.RootObject = (TestConductorViewModel) rootObject;
            ActivateRootObject(rootObject);
        }

        private static void ActivateRootObject(object rootObject)
        {
            ScreenExtensions.TryActivate(rootObject);
        }

        private sealed class BootstrapperBridge : IntegrationTestsBase<TestConductorViewModel, TestBootstrapper>.
            WithExplicitRootObjectCreation
        {
            public BootstrapperBridge(ScenarioContext scenarioContext)
                : base(scenarioContext)
            {

            }
        }
    }
}