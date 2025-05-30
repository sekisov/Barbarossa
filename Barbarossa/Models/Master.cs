using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.Generic;

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
        private Dictionary<string, List<string>> _availableSlots = new();
    }
}