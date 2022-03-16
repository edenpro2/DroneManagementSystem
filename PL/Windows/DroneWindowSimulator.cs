using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace PL.Windows
{
    public partial class DroneWindow
    {
        private BackgroundWorker _worker;
        private bool _simulationRunning;
        private const int Time = 1000;
        private bool _shouldStop;

        private void SimulatorBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_simulationRunning) { _shouldStop = true; /*and should*/ return; }

            _worker = new BackgroundWorker();
            _worker.WorkerReportsProgress = true;
            _worker.WorkerSupportsCancellation = true;
            _shouldStop = false;
            _worker.DoWork += Worker_DoWork!;
            _worker.ProgressChanged += Worker_ProgressChanged;
            _worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            _simulationRunning = true;
            ProgressBox.Text = "Starting simulator...";
            _worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!_shouldStop)
            {
                Thread.Sleep(Time);
                var (drone, progress) = _bl.DroneSimulator(ViewModel.Drone);
                ViewModel.Drone = drone;
                Dispatcher.Invoke(() => { ProgressBox.Text = progress; });
                _worker.ReportProgress(1);
            }

            Worker_RunWorkerCompleted(sender, new RunWorkerCompletedEventArgs(sender, null, true));
        }

        private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e) => UpdateContent();

        private void Worker_RunWorkerCompleted(object? o, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ProgressBox.Text = "Fatal Error stopping thread";
                throw new Exception("Fix");
            }
            Dispatcher.Invoke(() => { ProgressBox.Text = "Stopping simulator..."; });
            _worker.CancelAsync();
            _simulationRunning = false;
            Dispatcher.Invoke(() => { ProgressBox.Text = ""; });
        }
    }
}
