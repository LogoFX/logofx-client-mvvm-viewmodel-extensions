using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Input;
using LogoFX.Client.Mvvm.Commanding;
using LogoFX.Client.Mvvm.Model.Contracts;
using LogoFX.Client.Mvvm.Notifications;
using LogoFX.Client.Mvvm.ViewModel.Contracts;
using LogoFX.Client.Mvvm.ViewModel.Shared;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions
{
    /// <summary>
    /// Represents screen object view model which wraps an editable model.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class EditableScreenObjectViewModel<T> : ScreenObjectViewModel<T>, IEditableViewModel, ICanBeBusy, IDataErrorInfo
        where T : IEditableModel, IHaveErrors, IDataErrorInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditableScreenObjectViewModel{T}"/> class.
        /// </summary>
        /// <param name="model"></param>
        protected EditableScreenObjectViewModel(T model)
            : base(model)
        {
            if (Model is INotifyPropertyChanged inpc)
            {
                inpc.NotifyOn("IsDirty", (o, o1) => NotifyOfPropertyChange(() => IsDirty));
                inpc.NotifyOn("CanCancelChanges", (o, o1) => NotifyOfPropertyChange(() => CanCancelChanges));
                inpc.NotifyOn("Error", (o, o1) => NotifyOfPropertyChange(() => HasErrors));
            }               
        }

        private ICommand _applyCommand;
        /// <summary>
        /// Gets the apply command.
        /// </summary>
        /// <value>
        /// The apply command.
        /// </value>
        public ICommand ApplyCommand
        {
            get
            {
                return _applyCommand ??
                       (_applyCommand = ActionCommand
                           .When(() => (ForcedDirty || Model.IsDirty) && !((IHaveErrors)Model).HasErrors)
                           .Do(async () =>
                           {
                               await SaveAsync();
                           })
                           .RequeryOnPropertyChanged(this, () => ForcedDirty)
                           .RequeryOnPropertyChanged(this, () => IsDirty)
                           .RequeryOnPropertyChanged(this, () => HasErrors));
            }
        }

        private ICommand _cancelChangesCommand;
        /// <summary>
        /// Gets the cancel changes command.
        /// </summary>
        /// <value>
        /// The cancel changes command.
        /// </value>
        public ICommand CancelChangesCommand
        {
            get
            {
                return _cancelChangesCommand ??
                       (_cancelChangesCommand = ActionCommand
                           .When(() => CanCancelChanges && IsDirty)
                           .Do(CancelChangesAsync)
                           .RequeryOnPropertyChanged(this, () => CanCancelChanges)
                           .RequeryOnPropertyChanged(this, () => IsDirty));
            }
        }

        private ICommand _closeCommand;
        /// <summary>
        /// Gets the close command.
        /// </summary>
        /// <value>
        /// The close command.
        /// </value>
        public ICommand CloseCommand
        {
            get
            {
                return _closeCommand ?? (_closeCommand = ActionCommand.When(() => IsBusy == false).Do(() => TryClose())
                    .RequeryOnPropertyChanged(this, () => IsBusy));
            }
        }

        private bool _forcedDirty;
        /// <summary>
        /// Gets or sets a value indicating whether view model's dirty state is forced.        .
        /// </summary>
        /// <value>
        ///   <c>true</c> if view model's dirty state is forced; otherwise, <c>false</c>.
        /// </value>
        public bool ForcedDirty
        {
            get => _forcedDirty;
            set
            {
                if (_forcedDirty == value)
                {
                    return;
                }

                _forcedDirty = value;
                NotifyOfPropertyChange();
            }
        }

#region Events

        /// <summary>
        /// Occurs before the model changes are applied.
        /// </summary>
        public event EventHandler Saving;

        /// <summary>
        /// Occurs after the model changes are applied.
        /// </summary>
        public event EventHandler<ResultEventArgs> Saved;

#endregion

#region Protected Members

        /// <summary>
        /// Override this method to provide custom save changes logic.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        protected abstract Task<bool> SaveMethod(T model);

        private async Task<bool> SaveAsync()
        {
            await OnSaving();
            bool result = await SaveMethod(Model);
            if (result)
            {
                Model.CommitChanges();
            }
            await OnSaved(result);
            return result;
        }

        /// <summary>
        /// Override this method to inject custom logic before the model changes are saved.
        /// </summary>
        /// <returns></returns>
        protected async virtual Task OnSaving()
        {
            var handler = Saving;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Override this method to inject custom logic after the model changes are saved.
        /// </summary>
        /// <param name="successful">The result of save changes operation.</param>
        /// <returns></returns>
        protected async virtual Task OnSaved(bool successful)
        {
            var handler = Saved;

            if (handler != null)
            {
                handler(this, new ResultEventArgs(successful));
            }
        }

        /// <summary>
        /// Displays the save changes prompt and captures the selected prompt option.
        /// </summary>
        /// <returns></returns>
        protected abstract Task<MessageResult> OnSaveChangesPrompt();

        /// <summary>
        /// Reacts to the save changes error.
        /// </summary>
        /// <returns></returns>
        protected abstract Task OnSaveChangesWithErrors();

        private async void CancelChangesAsync()
        {
            try
            {
                await OnChangesCanceling();
            }
            catch (Exception ex)
            {                
                throw ex;
            }

            try
            {
                if (Model.CanCancelChanges)
                {
                    Model.CancelChanges();
                }
                else
                {
                    Model.ClearDirty();
                }
            }
            catch (Exception)
            {
                //TODO: add proper rollback
                throw;
            }
            
            await OnChangesCanceled();
        }

        /// <summary>
        /// Override this method to inject custom logic before the model changes are canceled.
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnChangesCanceling()
        {
            return Task.Run(() => { });
        }

        /// <summary>
        /// Override this method to inject custom logic after the model changes are canceled.
        /// </summary>
        /// <returns></returns>
        protected virtual Task OnChangesCanceled()
        {
            return Task.Run(() => { });
        }

        /// <summary>
        /// Called to check whether or not this instance can close.
        /// </summary>
        /// <param name="callback">The implementor calls this action with the result of the close check.</param>
        public override async void CanClose(Action<bool> callback)
        {
            if (IsDirty)
            {
                MessageResult retVal = await OnSaveChangesPrompt();

                switch (retVal)
                {
                    case MessageResult.Cancel:
                        callback(false);
                        return;
                    case MessageResult.Yes:
                        if (HasErrors)
                        {
                            await OnSaveChangesWithErrors();
                            callback(false);
                            return;
                        }

                        var successful = await SaveAsync();
                        if (successful == false)
                        {
                            callback(false);
                            return;
                        }
                        break;
                    case MessageResult.No:
                        CancelChangesCommand.Execute(null);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            base.CanClose(callback);
        }

#endregion

#region IEditableViewModel

        /// <summary>
        /// Gets a value indicating whether the view model is dirty.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the view model is dirty; otherwise, <c>false</c>.
        /// </value>
        public bool IsDirty => Model.IsDirty;

        /// <summary>
        /// Gets or sets a value indicating whether the changes can be cancelled.
        /// </summary>
        /// <value>
        /// <c>true</c> if the changes can be cancelled; otherwise, <c>false</c>.
        /// </value>
        public bool CanCancelChanges
        {
            get => Model.CanCancelChanges;
            set => Model.CanCancelChanges = value;
        }

        bool IHaveErrors.HasErrors => Model.HasErrors;

        void IEditableViewModel.CancelChanges()
        {
            CancelChangesAsync();
        }

        Task<bool> IEditableViewModel.SaveAsync()
        {
            return SaveAsync();
        }

        #endregion

#region IDataErrorInfo

        /// <summary>
        /// Gets the error message for the property with the given name.
        /// </summary>
        /// <returns>
        /// The error message for the property. The default is an empty string ("").
        /// </returns>
        /// <param name="columnName">The name of the property whose error message to get. </param>
        public virtual string this[string columnName] => Model[columnName];

        /// <summary>
        /// Gets an error message indicating what is wrong with this object.
        /// </summary>
        /// <returns>
        /// An error message indicating what is wrong with this object. The default is an empty string ("").
        /// </returns>
        public virtual string Error => Model.Error;

        #endregion
    }
}