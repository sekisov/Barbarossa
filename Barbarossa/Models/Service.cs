using CommunityToolkit.Mvvm.ComponentModel;

namespace Barbarossa.Models
{
    public partial class Service : ObservableObject
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string? _name;

        [ObservableProperty]
        private string? _duration;

        [ObservableProperty]
        private decimal _price;

        [ObservableProperty]
        private bool _isSelected;
    }
}