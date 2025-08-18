using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class MeterReadingUploadResultDto
    {
        public string Name { get; set; }
        public int TotalRecordsProcessed { get; set; }
        public int SuccessfulRecords { get; set; }
        public int FailedRecords { get; set; }
    }
}
