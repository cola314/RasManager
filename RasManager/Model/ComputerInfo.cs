using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RasManager.Model
{
    public class ComputerInfo
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("osName")]
        public string OS { get; set; }

        [JsonProperty("online")]
        public bool IsOnline { get; set; }

        [JsonProperty("temperature")]
        public int Temperature { get; set; }

        [JsonProperty("cpuConsumption")]
        public int CpuUsage { get; set; }

        [JsonProperty("ramConsumption")]
        public int RamUsage { get; set; }

        [JsonIgnore]
        public string OSImagePath
        {
            get
            {
                var OSImageMap = new Dictionary<string, string>()
                {
                    ["ubuntu"] = "Image/OS/Ubuntu.png",
                    ["arch"] = "Image/OS/Arch.png",
                    ["raspbian"] = "Image/OS/Raspbian.png",
                };

                if(OS != null && OSImageMap.ContainsKey(OS.ToLower()))
                {
                    return OSImageMap[OS.ToLower()];
                }
                else
                {
                    return OSImageMap["raspbian"];
                }
            }
        }

        [JsonIgnore]
        public double CpuUsagePercentage
        {
            get
            {
                return (100 - CpuUsage) / 100.0;
            }
        }

        [JsonIgnore]
        public double RamUsagePercentage
        {
            get
            {
                return RamUsage / 100.0;
            }
        }
    }
}
