using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalcyonHomeManager.Entities
{
    public class ErrorLog
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string MethodName { get; set; }
        public string ClassName { get; set; }
        public string Message { get; set; }
        public DateTime? ErrorDate { get; set; }
        public string DeviceName { get; set; }
    }
}
