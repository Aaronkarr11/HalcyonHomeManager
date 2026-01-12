using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalcyonHomeManager.Entities
{

    public class Project
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string DeviceName { get; set; }
        public string State { get; set; }
        public string Title { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? TargetDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long ConvertedDateTimeStamp { get; set; }
        public string Description { get; set; }
        public string LocationCategory { get; set; }
        public string Severity { get; set; }
        public int Priority { get; set; }
        public int Completed { get; set; }
    }

    public class ProjectHierarchy
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }
        public string DeviceName { get; set; }
        public string State { get; set; }
        public string Title { get; set; }
        public string DisplayStartDate { get; set; }
        public string DisplayTargetDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? TargetDate { get; set; }
        public long ConvertedDateTimeStamp { get; set; }
        public string Description { get; set; }
        public string LocationCategory { get; set; }
        public string Severity { get; set; }
        public int Priority { get; set; }
        public int Completed { get; set; }
        public List<WorkTask> WorkTaskHierarchy { get; set; }
    }
}
