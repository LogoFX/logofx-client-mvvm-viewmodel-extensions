cd contents
rmdir /Q /S lib
mkdir lib
cd lib
mkdir net461\
robocopy ../../../../../src/Bin/netframework/Release net461 LogoFX.Client.Mvvm.ViewModel.Extensions.Platform.* /E
robocopy ../../../../../src/lib/Caliburn.Micro/net45 net461 Caliburn.Micro.Platform.* /E
robocopy ../../../../../src/lib/Caliburn.Micro/net45 net461 System.Windows.Interactivity.* /E
mkdir net5.0-windows
robocopy ../../../../../src/Bin/net/Release net5.0-windows LogoFX.Client.Mvvm.ViewModel.Extensions.Platform.* /E
robocopy ../../../../../src/lib/Caliburn.Micro/net45 net5.0-windows Caliburn.Micro.Platform.* /E
robocopy ../../../../../src/lib/Caliburn.Micro/net45 net5.0-windows System.Windows.Interactivity.* /E
cd net5.0-windows
rmdir /Q /S ref
cd ..
mkdir netcoreapp3.1
robocopy ../../../../../src/Bin/netcore/Release netcoreapp3.1 LogoFX.Client.Mvvm.ViewModel.Extensions.Platform.* /E
robocopy ../../../../../src/lib/Caliburn.Micro/net45 netcoreapp3.1 Caliburn.Micro.Platform.* /E
robocopy ../../../../../src/lib/Caliburn.Micro/net45 netcoreapp3.1 System.Windows.Interactivity.* /E
mkdir uap10.0
robocopy ../../../../../src/Bin/uwp/Release uap10.0 LogoFX.Client.Mvvm.ViewModel.Extensions.Platform.* /E
robocopy ../../../../../src/lib/Caliburn.Micro/uap10.0 uap10.0 Caliburn.Micro.Platform.* /E
cd ../../
nuget pack contents/LogoFX.Client.Mvvm.ViewModel.Extensions.nuspec -OutputDirectory ../../../output