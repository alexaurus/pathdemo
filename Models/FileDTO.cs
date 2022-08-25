using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace pathdemo.Models
{
    public class FileDTO
    {
        public string FileType { get; set; }
        public byte[] Data { get; set; }
        public string FileName { get; set; }
    }
}
