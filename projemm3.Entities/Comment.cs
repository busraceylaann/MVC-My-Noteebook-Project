using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projemm3.Entities
{
    [Table("Comments")]
    public class Comment:MyEntityBase
    {
        [Required,StringLength(250)]
        public string Text { get; set; }

        public virtual Note Note { get; set; }
        public virtual ProjemUser Owner { get; set; }

    }
}
