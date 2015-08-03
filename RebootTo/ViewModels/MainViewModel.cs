using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Caliburn.Micro;

namespace RebootTo.ViewModels
{
    public class MainViewModel : ScreenViewModel
    {
        public MainViewModel()
        {
            DisplayName = "Reboot To";
            _bcd = new BcdService();
            LoadBootEntriesAsync();
        }

        private readonly BcdService _bcd;
        private async void LoadBootEntriesAsync()
        {
            var entries = await _bcd.GetBootEntriesAsync();
            BootEntries.Clear();
            foreach (var entry in entries.OrderBy(e => e.Description))
            {
                BootEntries.Add(entry);
            }
        }

        public async void RebootTo(BootEntry entry)
        {
            try
            {
                await _bcd.RebootAsync(entry);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.ToString(), "Error");
                throw;
            }
        }

        private BindableCollection<BootEntry> _bootEntries = new BindableCollection<BootEntry>();

        public BindableCollection<BootEntry> BootEntries
        {
            get { return _bootEntries; }
            set
            {
                if (value == _bootEntries)
                    return;
                _bootEntries = value;
                NotifyOfPropertyChange(() => BootEntries);
            }
        }
    }
}
