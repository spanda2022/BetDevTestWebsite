using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RestSharp;
using System.Net;

namespace BetgDevTestWebsite.Models
{
  public  interface IBetDevTestViewFormatter
    {
        public BetDevTestViewModel FormatViewModel(IRestResponse response1, BetDevTestViewModel betViewModel, string modelName);
             
    }
}
