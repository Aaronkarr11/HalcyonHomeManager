using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalcyonHomeManager.Models
{
    public class ErrorLogModel
    {
            public string MethodName { get; set; }
            public string ClassName { get; set; }
            public string Message { get; set; }
            public DateTime? ErrorDate { get; set; }
            public string DeviceName { get; set; }    
    }
}
