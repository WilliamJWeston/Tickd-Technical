using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class MeterReadingStatusDto
    {
        public int TotalRecords { get; set; }
        public int TotatSuccessfulRecords { get; set; }
        public int TotatFailedRecords { get; set; }
    }
}
