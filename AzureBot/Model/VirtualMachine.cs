using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AzureBot
{
    public class VirtualMachine : Resource 
    {
        public string Address { get; set; }
        
        public bool Start()
        {
            return true;
        }

        public bool Stop()
        {
            return true;
        }
    }
}