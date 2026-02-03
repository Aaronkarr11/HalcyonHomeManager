using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalcyonHomeManager.Entities
{
    public class WMSchedule
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string StartingDate { get; set; }

    }
}
