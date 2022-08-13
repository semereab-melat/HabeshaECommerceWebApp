using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DukanHabeshaUtility
{
    public class StripeSettings
    {
        //the atrributes in this class shoud be exact same with attribute name in appsetting.json
        
        public string SecretKey { get; set; }
        public string PublishableKey { get; set; }
        //we should map these attributes with the one in appsetting.json in program.cs
    }
}
