using RasManager.Model;
using RasManager.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace RasManager.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        Task task;
        CancellationTokenSource token = new CancellationTokenSource();

        public MainWindowViewModel()
        {
            task = run(token);

            ComputerInfoList = new ObservableCollection<ComputerInfo>()
            {
                new ComputerInfo()
            };
        }

        public async Task run(CancellationTokenSource token)
        {
            while (!token.IsCancellationRequested)
            {
                Refresh();
                await Task.Delay(1000);
            }
        }

        private void Refresh()
        {
            ComputerInfoService.Instance.GetComputerList((result, data) =>
            {
                if (result == HttpStatusCode.OK)
                {
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ComputerInfoList = new ObservableCollection<ComputerInfo>(data);
                    });
                }
            });
        }

        private ObservableCollection<ComputerInfo> computerInfoList_;
        public ObservableCollection<ComputerInfo> ComputerInfoList
        {
            get { return computerInfoList_; }
            set
            {
                computerInfoList_ = value;
                OnPropertyChange(nameof(ComputerInfoList));
            }
        }
    }
}
