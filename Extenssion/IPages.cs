using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Forum.DomainModels
{
    public class IPages
    {
        public int Page { get; set; } = 1;
        public int Count { get; set; } = 20;
        public int MaxCount { get; set; } = 20;
        public DateTime? BeginTime { get; set; }
        public DateTime? EndTime { get; set; }
    }
}