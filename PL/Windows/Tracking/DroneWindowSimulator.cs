using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace PL.Windows.Tracking
{
    public partial class DroneTrackingWindow : INotifyPropertyChanged
    {
        #region Properties
        private BackgroundWorker? Worker { get; set; }
        private bool _simulationRunning;
        private const int Time = 1000; //ms
        private bool _shouldStop;
        #endregion

        private void SimulatorBtn_Click(object sender, RoutedEventArgs routedEventArgs)
        {
            IsChecked = !IsChecked;
            Simulation();
        }

        private void Simulation()
        {
            if (_simulationRunning) { _shouldStop = true; /*and should*/ return; }

            Worker = new BackgroundWorker();
            Worker.WorkerReportsProgress = true;
            Worker.WorkerSupportsCancellation = true;
            _shouldStop = false;
            _simulationRunning = true;
            Worker.DoWork += Worker_DoWork!;
            Worker.ProgressChanged += Worker_ProgressChanged;
            Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            Worker.RunWorkerAsync();
            ProgressMessage = "Starting simulator...";
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!_shouldStop)
            {
                Thread.Sleep(Time);
                var (drone, progress) = _bl.DroneSimulator(ViewModel);
                ViewModel = drone;
                Dispatcher.Invoke(() => { ProgressMessage = progress; });
                Worker?.ReportProgress(50);
            }

            Worker_RunWorkerCompleted(sender, new RunWorkerCompletedEventArgs(sender, null, true));
        }

        private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e) => UpdateContent();

        private void Worker_RunWorkerCompleted(object? o, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                throw new Exception("Fatal Error - stopping thread");
            }

            Dispatcher.Invoke(() => { ProgressMessage = "Stopping simulator..."; });
            Worker?.CancelAsync();
            _simulationRunning = false;
            Dispatcher.Invoke(() => { ProgressMessage = ""; });
        }

    }
}
