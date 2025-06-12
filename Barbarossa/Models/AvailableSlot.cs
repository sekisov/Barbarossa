using CommunityToolkit.Mvvm.ComponentModel;

namespace Barbarossa.Models
{
    public partial class AvailableSlot : ObservableObject
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string time;

        [ObservableProperty]
        private string _date;
    }
}
