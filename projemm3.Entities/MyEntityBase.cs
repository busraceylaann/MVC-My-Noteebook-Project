using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projemm3.Entities
{
    
   public class MyEntityBase
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] //bir kolonun oto artmasını sağlar
        public int Id { get; set; }
        [DisplayName("Düzenlenme Tarihi"), Required]
        public DateTime CreatedOn { get; set; }
        [DisplayName("Oluşturulma Tarihi"), Required]
        public DateTime ModifiedOn { get; set; }
        [DisplayName("Oluşturan Kişi"), Required,StringLength(30)]
        public string ModifiedUsername { get; set; }
    }
}
