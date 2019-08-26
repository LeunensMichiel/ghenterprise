using Ghenterprise.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ghenterprise.Models
{
    public class Opening_Hours
    {
        public string Id { get; set; }
        public int Day_Of_Week { get; set; }

        [JsonConverter(typeof(JsonTimeConverter))]
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public TimeSpan Start { get; set; }

        [JsonConverter(typeof(JsonTimeConverter))]
        [JsonProperty(TypeNameHandling = TypeNameHandling.All)]
        public TimeSpan End { get; set; }
    }
}
