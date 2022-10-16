using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;

namespace PL.Windows.Tracking
{
    public partial class DroneTrackingWindow : Window
    {
        private BackgroundWorker? Worker { get; set; }
        private bool _simulationRunning;
        private const int Time = 1000;
        private bool _shouldStop;

        private void SimulatorBtn_Click(object sender, RoutedEventArgs e)
        {
            if (_simulationRunning) { _shouldStop = true; /*and should*/ return; }

            Worker = new BackgroundWorker();
            Worker.WorkerReportsProgress = true;
            Worker.WorkerSupportsCancellation = true;
            _shouldStop = false;
            Worker.DoWork += Worker_DoWork!;
            Worker.ProgressChanged += Worker_ProgressChanged;
            Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            _simulationRunning = true;
            //ProgressBox.Text = "Starting simulator...";
            Worker.RunWorkerAsync();
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while (!_shouldStop)
            {
                Thread.Sleep(Time);
                var (drone, progress, currentDistance, totalDistance) = _bl.DroneSimulator(ViewModel.Drone, totalDist);
                ViewModel.Drone = drone;
                currentDist = currentDistance;
                totalDist = totalDistance;
                //Dispatcher.Invoke(() => { ProgressBox.Text = progress; });
                Worker?.ReportProgress(1);
            }

            Worker_RunWorkerCompleted(sender, new RunWorkerCompletedEventArgs(sender, null, true));
        }

        private void Worker_ProgressChanged(object? sender, ProgressChangedEventArgs e)
        {
            UpdateContent();
        }

        private void Worker_RunWorkerCompleted(object? o, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                ProgressBox.Text = "Fatal Error stopping thread";
                throw new Exception("Fix");
            }
            Dispatcher.Invoke(() => { ProgressBox.Text = "Stopping simulator..."; });
            Worker?.CancelAsync();
            _simulationRunning = false;
            Dispatcher.Invoke(() => { ProgressBox.Text = ""; });
        }
    }
}
