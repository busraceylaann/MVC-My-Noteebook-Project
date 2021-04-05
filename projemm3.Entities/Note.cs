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
    [Table("Notes")]
    public class Note : MyEntityBase
    {
        [DisplayName("Başlık"), Required, StringLength(60)]
        public string Title { get; set; }

        [DisplayName("Açıklama"), Required, StringLength(2000)]
        public string Text { get; set; }
        [DisplayName("Taslak")]
        public bool IsDraft { get; set; }
        [DisplayName("Beğenme")]
        public int LikeCount { get; set; }
        [DisplayName("Kategorisi")]
        public int CategoryId { get; set; }

        [DisplayName("Fotoğraf Ekleyiniz"),StringLength(500)]
        public string Notresim { get; set; }


        public virtual ProjemUser  Owner { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<Comment> Comments { get; set; }
        public virtual List<Liked> Likes { get; set; }

        public Note()
        {
            Comments = new List<Comment>();
            Likes = new List<Liked>();
        }

    }
}
