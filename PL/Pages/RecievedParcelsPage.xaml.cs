using BLAPI;
using DalFacade.DO;
using PL.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

#pragma warning disable CS8601 // Possible null reference assignment.

namespace PL.Pages
{
    public partial class ReceivedParcelsPage
    {
        private const double SelectedOpacity = 0.95;
        private const double DefaultOpacity = 0.3;

        private readonly SolidColorBrush _defaultColor =
            new BrushConverter().ConvertFromString("#FFA3ACBD") as SolidColorBrush;

        private readonly SolidColorBrush _selectedColor =
            new BrushConverter().ConvertFromString("#FF051B44") as SolidColorBrush;

        public List<ParcelViewModel> ParcelViewModels { get; private set; }
        private IEnumerable<Parcel> SentParcels { get; }
        private readonly BlApi _bl;


        public ReceivedParcelsPage(BlApi ibl, User user)
        {
            _bl = ibl;
            SentParcels = ibl.GetParcels(p => p.targetId == user.customerId);
            InitializeComponent();
            DataContext = this;

            ParcelViewModels = SentParcels.Select(p => new ParcelViewModel(_bl, p)).ToList();

            DeselectAll();
            AllOrdersBtn.Foreground = _selectedColor;
            AllOrdersBtn.Opacity = SelectedOpacity;
        }

        private void DeselectAll()
        {
            AllOrdersBtn.Foreground = Button2.Foreground = Button3.Foreground = Button4.Foreground = _defaultColor;
            AllOrdersBtn.Opacity = Button2.Opacity = Button3.Opacity = Button4.Opacity = DefaultOpacity;
        }

        private void AllOrdersBtn_Click(object sender, RoutedEventArgs e)
        {
            DeselectAll();
            AllOrdersBtn.Foreground = _selectedColor;
            AllOrdersBtn.Opacity = SelectedOpacity;
            ParcelViewModels = SentParcels.Select(
                p => new ParcelViewModel(_bl, p)).ToList();
            InvalidateVisual();
        }

        private void Panel2_Click(object sender, RoutedEventArgs e)
        {
            DeselectAll();
            Button2.Foreground = _selectedColor;
            Button2.Opacity = SelectedOpacity;
            ParcelViewModels = SentParcels.Where(p => p.delivered != default).Select(
                p => new ParcelViewModel(_bl, p)).ToList();
            InvalidateVisual();
        }

        private void Panel3_Click(object sender, RoutedEventArgs e)
        {
            DeselectAll();
            Button3.Foreground = _selectedColor;
            Button3.Opacity = SelectedOpacity;
            ParcelViewModels = SentParcels.Where(p => p.delivered == default && p.collected != default).Select(
                p => new ParcelViewModel(_bl, p)).ToList();
            InvalidateVisual();
        }

        private void Panel4_Click(object sender, RoutedEventArgs e)
        {
            DeselectAll();
            Button4.Foreground = _selectedColor;
            Button4.Opacity = SelectedOpacity;
            ParcelViewModels = SentParcels.Where(p => p.scheduled == default && p.requested != default).Select(
                p => new ParcelViewModel(_bl, p)).ToList();
            InvalidateVisual();
        }
    }
}