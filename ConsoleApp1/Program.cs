using ConsoleApp1.Data.Model;
using ConsoleApp1.Data.MongoDB.ClusterTesting;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Post item = new Post();
            Post recoveredItem;
            var Items = new List<Post>();
            var user = "UserTesting_change";

            Context.Cluster = "clustertesting_change";
            Context.DatabaseName = "DataBaseTesting_change";
            Context.ConnectionString = string.Format("mongodb+srv://UserTesting:{0}@{1}-wzng5.azure.mongodb.net", Uri.EscapeUriString(user), Context.Cluster);
            
            Context.IsSSL = true;

            using (var dbContext = new Context()){
                try
                {
                    //Add one
                    item.Id = Guid.NewGuid();
                    item.Title = string.Format("Title {0} {1} {2}", DateTime.Now.ToShortDateString(), 0, item.Id.ToString());
                    item.Content = "Content " + item.Id.ToString();
                    dbContext.Posts.InsertOne(item);

                    for (int i = 1; i < 11; i++)
                    {
                        var id = Guid.NewGuid();
                        Items.Add(new Post()
                        {
                            Id = id,
                            Title = string.Format("Title {0} {1} {2}", DateTime.Now.ToShortDateString(), i, id.ToString()),
                            Content = "Content " + id
                        });
                    }
                    dbContext.Posts.InsertMany(Items);

                    //Replace one
                    item.Title = string.Format("Title {0} {1} {2}", DateTime.Now.ToShortDateString(), 100, item.Id.ToString());
                    item.Content = "Content new text " + item.Id.ToString();
                    dbContext.Posts.ReplaceOne(_ => _.Id.Equals(item.Id), item);

                    //Get one
                    recoveredItem = dbContext.Posts.Find(_ => _.Id == Items.Last().Id).FirstOrDefault();

                    //Delete one
                    dbContext.Posts.DeleteOne(_ => _.Id == recoveredItem.Id);

                    //Get All List
                    Items = dbContext.Posts.Find(_ => true).ToList();

                    dbContext.Posts.DeleteMany(_ => _.Title.Contains(DateTime.Now.ToShortDateString()));
                }
                catch (Exception ex)
                {
                    Console.Write(ex.Message);
                }
            }
        }
    }
}
