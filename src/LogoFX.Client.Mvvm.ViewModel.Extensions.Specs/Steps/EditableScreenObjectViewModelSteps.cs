using FluentAssertions;
using JetBrains.Annotations;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Infra;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.ViewModels;
using LogoFX.Client.Mvvm.ViewModel.Extensions.Tests;
using LogoFX.Client.Mvvm.ViewModel.Shared;
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
            _scenarioDataStore.Model = simpleModel;
            var mockMessageService = new FakeMessageService();
            _scenarioDataStore.MockMessageService = mockMessageService;

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

        [When(@"I set all confirmation to '(.*)'")]
        public void WhenISetAllConfirmationTo(string result)
        {
            switch (result)
            {
                case "Yes":
                    _scenarioDataStore.MockMessageService.SetMessageResult(MessageResult.Yes);
                    break;
                case "No":
                    _scenarioDataStore.MockMessageService.SetMessageResult(MessageResult.No);
                    break;
                case "Cancel":
                    _scenarioDataStore.MockMessageService.SetMessageResult(MessageResult.Cancel);
                    break;
            }
        }


        [When(@"I set the name to be a valid name")]
        public void WhenISetTheNameToBeAValidName()
        {
            _scenarioDataStore.Model.Name = DataGenerator.ValidName;
        }

        [When(@"I close the editable screen object view model")]
        public void WhenICloseTheEditableScreenObjectViewModel()
        {
            _scenarioDataStore.SystemUnderTest.CloseCommand.Execute(null);
        }

        [Then(@"The editable screen object view model is dirty")]
        public void ThenTheEditableScreenObjectViewModelIsDirty()
        {
            _scenarioDataStore.SystemUnderTest.IsDirty.Should().BeTrue();
        }

        [Then(@"The editable screen object view model is not dirty")]
        public void ThenTheEditableScreenObjectViewModelIsNotDirty()
        {
            _scenarioDataStore.SystemUnderTest.IsDirty.Should().BeFalse();
        }

        [Then(@"The model is dirty")]
        public void ThenTheModelIsDirty()
        {
            _scenarioDataStore.Model.IsDirty.Should().BeTrue();
        }

        [Then(@"The model is not dirty")]
        public void ThenTheModelIsNotDirty()
        {
            _scenarioDataStore.Model.IsDirty.Should().BeFalse();
        }

        [Then(@"The changes in the model are saved")]
        public void ThenTheChangesInTheModelAreSaved()
        {
            //TODO: Put intermediate value into the data store
            _scenarioDataStore.Model.Name.Should().Be(DataGenerator.ValidName);
        }

        [Then(@"The changes in the model are not saved")]
        public void ThenTheChangesInTheModelAreNotSaved()
        {
            //TODO: Put intermediate value into the data store
            _scenarioDataStore.Model.Name.Should().NotBe(DataGenerator.ValidName);
        }

        [Then(@"A message is displayed")]
        public void ThenAMessageIsDisplayed()
        {
            _scenarioDataStore.MockMessageService.WasCalled.Should().BeTrue();
        }

        [Then(@"A message is not displayed")]
        public void ThenAMessageIsNotDisplayed()
        {
            _scenarioDataStore.MockMessageService.WasCalled.Should().BeFalse();
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
