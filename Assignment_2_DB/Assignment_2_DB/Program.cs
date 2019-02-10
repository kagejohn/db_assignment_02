using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MoreLinq;

namespace Assignment_2_DB
{
    class Program
    {
        private static IMongoDatabase _mongoDatabase;
        private static IMongoCollection<BsonDocument> _mongoCollection;
        private static readonly List<Dictionary<string, BsonValue>> Tweets = new List<Dictionary<string, BsonValue>>();

        static void Main()
        {
            MainAsync().Wait();
        }

        static async Task MainAsync()
        {
            string connectionString = "mongodb://10.0.75.2:27017";
            MongoClient mongoClient = new MongoClient(connectionString);

            _mongoDatabase = mongoClient.GetDatabase("social_net");
            _mongoCollection = _mongoDatabase.GetCollection<BsonDocument>("tweets");

            //using (IAsyncCursor<BsonDocument> asyncCursor = await _mongoCollection.FindAsync(new BsonDocument()))
            //{
            //    int count = 0;

            //    while (await asyncCursor.MoveNextAsync())
            //    {
            //        IEnumerable<BsonDocument> batch = asyncCursor.Current;
            //        foreach (BsonDocument document in batch)
            //        {
            //            if (count < 10)
            //            {
            //                Console.WriteLine(document + "\n");
            //            }
            //            count++;
            //        }
            //    }
            //}

            await LoadCollection();

            Console.WriteLine("Input a command: ");
            string input = Console.ReadLine();

            if (input.StartsWith("help"))
            {
                Console.WriteLine("Examples:");
                Console.WriteLine("total twitter users");
            }

            if (input.ToLower() == "total twitter users")
            {
                Console.WriteLine(UniqueUsers());
            }
        }

        static async Task LoadCollection()
        {
            using (IAsyncCursor<BsonDocument> asyncCursor = await _mongoCollection.FindAsync(new BsonDocument()))
            {
                while (await asyncCursor.MoveNextAsync())
                {
                    IEnumerable<BsonDocument> batch = asyncCursor.Current;
                    foreach (BsonDocument document in batch)
                    {
                        Dictionary<string, BsonValue> elements = new Dictionary<string, BsonValue>();
                        foreach (BsonElement element in document)
                        {
                            elements.Add(element.Name, element.Value);
                        }
                        Tweets.Add(elements);
                    }
                }
            }
        }

        static int UniqueUsers()
        {
            return Tweets.DistinctBy(t => t["id"]).Count();
        }
    }
}
