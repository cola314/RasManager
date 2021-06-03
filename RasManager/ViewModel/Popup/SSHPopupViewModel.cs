using RasManager.Model;
using RasManager.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasManager.ViewModel.Popup
{
    public class SSHPopupViewModel : BaseViewModel
    {
        public SSHPopupViewModel(string ip)
        {
            this.Ip = ip;
        }

        public string id_ { get; set; }
        public string Id
        {
            get => id_;
            set
            {
                id_ = value;
                OnPropertyChange(nameof(Id));
            }
        }

        public string Ip { get; private set; }

        public long port_ { get; set; } = 22;
        public long Port
        {
            get => port_;
            set
            {
                port_ = value;
                OnPropertyChange(nameof(Port));
            }
        }

        public DelegateCommand StartSSH
            => new DelegateCommand((o) =>
            {
                Process.Start("ssh", $"{Id}@{Ip} -p {Port}");
            });
    }
}
