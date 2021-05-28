using Attest.Testing.Bootstrapping;
using Solid.Practices.IoC;

namespace LogoFX.Client.Mvvm.ViewModel.Extensions.Specs.Bootstrapping
{
    internal sealed class Startup : StartupBase<Bootstrapper>
    {
        public Startup(IIocContainer iocContainer)
            : base(iocContainer, c => new Bootstrapper(c))
        {

        }
    }
}