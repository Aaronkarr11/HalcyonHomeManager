using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalcyonHomeManager.Entities
{
    public class WorkItem
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string type { get; set; }
        public string name { get; set; }
        public string project { get; set; }
        public string deletedDate { get; set; }
        public string deletedBy { get; set; }
        public int code { get; set; }
        public string url { get; set; }
    }
}
