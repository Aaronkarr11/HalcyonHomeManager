using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalcyonHomeManager.Entities
{
    public class WorkTask : HouseHoldMember
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string DeviceName { get; set; }
        public string Assignment { get; set; }
        public string State { get; set; }
        public Microsoft.Maui.Graphics.Color StateColor { get; set; }
        public string Title { get; set; }
        public string Risk { get; set; }
        public int Effort { get; set; }
        public int Priority { get; set; }
        public string DisplayStartDate { get; set; }
        public string DisplayTargetDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? TargetDate { get; set; }
        public string Description { get; set; }
        public bool SendSMS { get; set; }
        public int Completed { get; set; }
    }
}
