using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReslifeFiveBackEnd.Model
{
    public class RLFImage
    {
        public int Id { get; set; }
        public byte[]? ImageData { get; set; }
        public string ImageType { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}
