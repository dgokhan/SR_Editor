using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SR_Editor.Api
{
    public class LoginResultOk
    {
        public string access_token { get; set; }
        public string refresh_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
    public class LoginResultFail
    {
        public string error { get; set; }
        public string error_description { get; set; }
        public string error_uri { get; set; }
    }
}
