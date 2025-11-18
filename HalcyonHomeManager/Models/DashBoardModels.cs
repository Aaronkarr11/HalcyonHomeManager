using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HalcyonHomeManager.Models
{

        public class WorkTaskCompletedStats
        {
            public double percentCompleted { get; set; }
            public double percentUnCompleted { get; set; }
            public int completedCount { get; set; }
            public int unCompletedCount { get; set; }
        }

        public class LineGraphModel
        {
            public List<string> months { get; set; }
            public List<int> completedTasks { get; set; }
        }

        public class LineGraphModelItem
        {
            public int TotalCompleted { get; set; }
            public string Name { get; set; }
        }

        public class BarGraphModelItem
        {
            public int CompletedCountForLastMonth { get; set; }
            public int CompletedCountForCurrentMonth { get; set; }
            public string CurrentMonth { get; set; }
            public string LastMonth { get; set; }
        }


        public class DashBoard
        {
            public WorkTaskCompletedStats percentageData { get; set; }
            public BarGraphModelItem barGraphData { get; set; }
            public List<LineGraphModelItem> lineGraphModel { get; set; }
        }
    
}
