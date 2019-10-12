using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSB_SEM5_Kalkulator.Helper
{
    public class TokenResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public string userName { get; set; }
        public string GetAsJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
