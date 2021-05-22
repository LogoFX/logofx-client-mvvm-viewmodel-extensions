using FluentAssertions;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Infra;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.ViewModels;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Tests;
using TechTalk.SpecFlow;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Steps
{
    [Binding, UsedImplicitly]
    class EditableScreenObjectViewModelSteps
    {
        private readonly ScenarioDataStore _scenarioDataStore;

        public EditableScreenObjectViewModelSteps(ScenarioDataStore scenarioDataStore)
        {
            _scenarioDataStore = scenarioDataStore;
        }

        [When(@"I use editable screen object view model")]
        public void WhenIUseEditableScreenObjectViewModel()
        {
            var simpleModel = new SimpleEditableModel();
            var mockMessageService = new FakeMessageService();

            var screenObjectViewModel = new TestEditableScreenSimpleObjectViewModel(mockMessageService, simpleModel);
            screenObjectViewModel.PropertyChanged += (sender, args) =>
            {
                switch (args.PropertyName)
                {
                    case "IsDirty":
                        _scenarioDataStore.WasDirtyRaised = true;
                        break;
                    case "CanCancelChanges":
                        _scenarioDataStore.WasCancelChangesRaised = true;
                        break;
                }
            };
            _scenarioDataStore.SystemUnderTest = screenObjectViewModel;
            _scenarioDataStore.RootObject.ActivateItem(screenObjectViewModel);
        }

        [When(@"I set the name to be a valid name")]
        public void WhenISetTheNameToBeAValidName()
        {
            _scenarioDataStore.SystemUnderTest.Model.Name = DataGenerator.ValidName;
        }

        [Then(@"A dirty notification is raised")]
        public void ThenADirtyNotificationIsRaised()
        {
            _scenarioDataStore.WasDirtyRaised.Should().BeTrue();
        }

        [Then(@"A changes cancellation notification is raised")]
        public void ThenAChangesCancellationNotificationIsRaised()
        {
            _scenarioDataStore.WasCancelChangesRaised.Should().BeTrue();
        }

    }
}
