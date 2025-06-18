using CommunityToolkit.Mvvm.ComponentModel;

namespace Barbarossa.Models
{
    public partial class Product : ObservableObject
    {
        [ObservableProperty]
        private string _id;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(FullDisplayName))]
        private string _name;

        [ObservableProperty]
        private string _description;

        [ObservableProperty]
        private decimal _price;

        [ObservableProperty]
        private string _imageUrl;

        public string FullDisplayName => $"{Name} - {Price:C}";
    }
}