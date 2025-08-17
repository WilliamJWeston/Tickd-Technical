using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Dtos
{
    public class FileModelDto
    {
        public string Name { get; set; }
        public IBrowserFile File { get; set; }
    }
}
