using projemm3.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;




namespace projemm3.DataAccessLayer.EntityFramework
{
    public class DatabaseContext :DbContext
    {
       public DbSet<ProjemUser> ProjemUsers{ get; set; }
        public DbSet<Note>Notes { get; set; }
        public DbSet<Comment> Comments{ get; set; }
        public DbSet<Category> Categories{ get; set; }
        public DbSet<Liked> Likeds { get; set; }
        public DbSet<Galeri> Galeris { get; set; }

        public DatabaseContext()
        {
            Database.SetInitializer(new MyInitializer());
        }



    }
}
