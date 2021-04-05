using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using projemm3.Entities;

namespace projemm3.DataAccessLayer.EntityFramework
{
    public class MyInitializer:CreateDatabaseIfNotExists<DatabaseContext>
    {
        protected override void Seed(DatabaseContext context)
        { //admin ekledim
            ProjemUser admin = new ProjemUser()
            {
                Name = "Büşra",
                Surname = "Ceylan",
                Email = "busraceylan@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = true,

                Username = "busraceylan",
                ProfilImageFileName = "people-profile-icon_24877-40756.jpg",
                Password ="123456",
                CreatedOn=DateTime.Now,
                ModifiedOn=DateTime.Now.AddMinutes(5),
                ModifiedUsername="busraceylan"

            

            };
            //kullanıcı ekledim
            ProjemUser standartUser = new ProjemUser()
            {
                Name = "Hatice",
                Surname = "Ceylan",
                Email = "hbusraceylan@gmail.com",
                ActivateGuid = Guid.NewGuid(),
                IsActive = true,
                IsAdmin = false,
                
                Username = "haticeceylan",
                ProfilImageFileName = "people-profile-icon_24877-40756.jpg",
                Password = "121212",
                CreatedOn = DateTime.Now.AddHours(1),
                ModifiedOn = DateTime.Now.AddMinutes(65),
                ModifiedUsername = "busraceylan"
            };
            context.ProjemUsers.Add(admin);
            context.ProjemUsers.Add(standartUser);
            context.SaveChanges();

            for (int i = 0; i < 8; i++)
            {
                ProjemUser user = new ProjemUser()
                {
                    Name = FakeData.NameData.GetFemaleFirstName(),
                    Surname = FakeData.NameData.GetSurname(),
                    Email = FakeData.NetworkData.GetEmail(),
                    ProfilImageFileName = "people-profile-icon_24877-40756.jpg",
                    ActivateGuid = Guid.NewGuid(),
                    IsActive = true,
                    IsAdmin = false,
                    Username = $"user{i}",
                    Password = "123",
                    CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                    ModifiedUsername = $"user{i}"
                };
                context.ProjemUsers.Add(user);
            }

            context.SaveChanges();
            // kullanıcı için liste
            List<ProjemUser> userlist = context.ProjemUsers.ToList();

            //fake kategori ekledim
            for (int i = 0; i < 10; i++)
            {
                Category cat = new Category()
                {
                    Title = FakeData.PlaceData.GetStreetName(),
                    Description=FakeData.PlaceData.GetAddress(),
                    CreatedOn=DateTime.Now,
                    ModifiedOn=DateTime.Now,
                    ModifiedUsername="busraceylan"
                };
                context.Categories.Add(cat);
                
                //fake not ekledim
                for (int k = 0; k < FakeData.NumberData.GetNumber(5,9); k++)
                
                {
                    ProjemUser owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];

                    Note note = new Note()
                    {
                        Title = FakeData.TextData.GetAlphabetical(FakeData.NumberData.GetNumber(5, 25)),
                        Text = FakeData.TextData.GetSentences(FakeData.NumberData.GetNumber(1, 3)),
                       
                        IsDraft = false,
                        LikeCount = FakeData.NumberData.GetNumber(1, 9),
                        Owner = owner,
                        CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                        ModifiedUsername= owner.Username,
                    };
                    cat.Notes.Add(note);

                    //fake yorum ekledim
                    for (int j = 0; j < FakeData.NumberData.GetNumber(3,5); j++)

                    {
                        ProjemUser comment_owner = userlist[FakeData.NumberData.GetNumber(0, userlist.Count - 1)];

                        Comment comment = new Comment()
                        {
                            Text = FakeData.TextData.GetSentence(),
                          
                            Owner= owner,
                            CreatedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedOn = FakeData.DateTimeData.GetDatetime(DateTime.Now.AddYears(-1), DateTime.Now),
                            ModifiedUsername = comment_owner.Username,
                        };

                        note.Comments.Add(comment);
                    }
                    //like ekledim
                   
                    for (int m = 0; m <note.LikeCount; m++)
                    {
                        Liked liked = new Liked()
                        {
                            LikedUser = userlist[m]
                        };

                        note.Likes.Add(liked);
                    }
                }
            }

            context.SaveChanges();
        }
    }
}
