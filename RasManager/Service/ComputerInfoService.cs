using RasManager.Model;
using RasManager.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace RasManager.Service
{
    public class ComputerInfoService : Singleton<ComputerInfoService>
    {
        public void GetComputerList(Action<HttpStatusCode, List<ComputerInfo>> callback)
        {
            HttpApiClient.Instance.SendData<object, List<ComputerInfo>>(SendType.GET, "api/ComputerInfoInformation", null, callback);
        }
    }
}
