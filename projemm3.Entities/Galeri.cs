using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projemm3.Entities
{
    public class Galeri
    {
        [Key]
        public int ResimId { get; set; }

        public string ResimUrl { get; set; }
    }
}
