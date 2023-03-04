using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows;
using Monitor.List.Common;

namespace Monitor.List
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        private static Mutex _appMutex;

        public App()
        {
            var directoryInfo = new DirectoryInfo(Assembly.GetExecutingAssembly().Location);
            var configFile = Path.Combine(directoryInfo.Parent?.FullName ?? string.Empty, "NLog.config");
            LogHelper.Startup(configFile);
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            _appMutex = new Mutex(true, "Monitor.List", out var createdNew);

            if (!createdNew)
            {
                var current = Process.GetCurrentProcess();

                foreach (var process in Process.GetProcessesByName(current.ProcessName))
                {
                    if (process.Id != current.Id)
                    {
                        Win32Helper.SetForegroundWindow(process.MainWindowHandle);
                        break;
                    }
                }
                Shutdown();
            }
        }

        protected override void OnExit(ExitEventArgs e)
        {
            LogHelper.Shutdown();

            base.OnExit(e);
        }
    }
}
