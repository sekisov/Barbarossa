using CommunityToolkit.Mvvm.ComponentModel;
namespace Barbarossa.Models
{
    public partial class Master : ObservableObject
    {
        [ObservableProperty]
        private int _id;

        [ObservableProperty]
        private string _name;

        [ObservableProperty]
        private string _jobTitle;

        [ObservableProperty]
        private string _avatarUrl;

        [ObservableProperty]
        private List<Service> _services = new();

        [ObservableProperty]
        private Dictionary<string, List<AvailableSlot>> _availableSlots = new();
    }
}