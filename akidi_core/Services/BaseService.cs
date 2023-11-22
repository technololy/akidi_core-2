using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BackEndServices.Services
{
    public abstract class BaseService
    {
        public string Message { get; set; }
        public int Code { get; set; }
    }
}