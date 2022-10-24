using System.ComponentModel;
using System.Threading;
using System.Windows;
using System;
using DalFacade.DO;
using DalFacade;

namespace PL.Windows.Tracking
{
    public partial class DroneTrackingWindow
    {
        #region Properties
        private BackgroundWorker? QueryWorker1 { get; set; }
        private BackgroundWorker? QueryWorker2 { get; set; }
        private bool _query1Running;
        private bool _query1ShouldStop;
        private bool _query2Running;
        private bool _query2ShouldStop;
        private double lat;
        private double lon;
        #endregion

       
        private void GetQuery(Location location)
        {
            lat = location.Latitude;
            lon = location.Longitude;
            if (_query1Running) { _query1ShouldStop = true; /*and should*/ return; }

            QueryWorker1 = new BackgroundWorker();
            QueryWorker2 = new BackgroundWorker();

            QueryWorker1.WorkerSupportsCancellation = true;
            QueryWorker2.WorkerSupportsCancellation = true;

            _query1ShouldStop = false;
            _query2ShouldStop = false;

            _query1Running = true;
            _query2Running = true;
           
            QueryWorker1.DoWork += Query1_DoWork!;
            QueryWorker1.RunWorkerCompleted += Query1_Completed;

            QueryWorker2.DoWork += Query2_DoWork!;
            QueryWorker2.RunWorkerCompleted += Query2_Completed;

            QueryWorker1.RunWorkerAsync();
            QueryWorker2.RunWorkerAsync();
        }

        private void Query1_DoWork(object sender, DoWorkEventArgs e)
        {
            MapUrl = new Uri($"https://www.openstreetmap.org/?mlat={lat}&amp;mlon={lon}#map=12/{lat}/{lon}");

            Query1_Completed(sender, new RunWorkerCompletedEventArgs(sender, null, true));
        }

        private void Query1_Completed(object? o, RunWorkerCompletedEventArgs e)
        {
            QueryWorker1?.CancelAsync();
        }
        private void Query2_DoWork(object sender, DoWorkEventArgs e)
        {
            Details = FileReader.LoadNominatim(new Location(lat, lon));

            Query2_Completed(sender, new RunWorkerCompletedEventArgs(sender, null, true));
        }

        private void Query2_Completed(object? o, RunWorkerCompletedEventArgs e)
        {
            QueryWorker1?.CancelAsync();
        }

    }
}
