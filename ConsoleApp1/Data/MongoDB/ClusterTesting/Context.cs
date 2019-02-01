using ConsoleApp1.Data.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
//using System.Text;

//www.mongodb.com/cloud
namespace ConsoleApp1.Data.MongoDB.ClusterTesting
{
    class Context:IDisposable
    {
        public static string ConnectionString { get; set; }
        public static string DatabaseName { get; set; }
        public static string Cluster { get; set; }
        public static bool IsSSL { get; set; }

        private IMongoDatabase _database { get; }

        public Context()
        {
            try
            {
                MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(ConnectionString));
                if (IsSSL)
                {
                    settings.SslSettings = new SslSettings { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };
                    settings.RetryWrites = true;
                }
                var mongoClient = new MongoClient(settings);
                _database = mongoClient.GetDatabase(DatabaseName);
            }
            catch (Exception ex)
            {
                throw new Exception("Can not access to db server.", ex);
            }
        }

        public IMongoCollection<Post> Posts
        {
            get
            {
                return _database.GetCollection<Post>("Posts");
            }
        }

        public void Dispose()
        {
            //Nothing
        }
    }
}
