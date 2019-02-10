using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
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

            Console.WriteLine("Loading data plz wait...");
            await LoadCollection();

            Console.WriteLine("Input a command: ");
            while (true)
            {
                string input = Console.ReadLine();

                if (input.StartsWith("help"))
                {
                    Console.WriteLine("Examples:");
                    Console.WriteLine("total twitter users");
                    Console.WriteLine("users that link to most other users");
                }

                if (input.ToLower() == "total twitter users")
                {
                    Console.WriteLine(UniqueUsers());
                }

                if (input.ToLower() == "users that link to most other users")
                {
                    foreach (KeyValuePair<string, int> keyValuePair in UsersThatLinkToMost())
                    {
                        Console.WriteLine("Username: " + keyValuePair.Key + " Links: " + keyValuePair.Value);
                    }
                }
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

        //How many Twitter users are in the database?
        static int UniqueUsers()
        {
            return Tweets.DistinctBy(t => t["id"]).Count();
        }

        //Which Twitter users link the most to other Twitter users? (Provide the top ten.)
        static Dictionary<string, int> UsersThatLinkToMost()
        {
            //db.tweets.aggregate(
            //{$match: { text:/@\w +\/} },
            //{$group: { _id: null,text: {$push: "$text"} }
            //})

            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            foreach (Dictionary<string, BsonValue> tweet in Tweets)
            {
                if (tweet["text"].AsString.Contains("@"))
                {
                    string username = tweet["user"].ToString();
                    if (dictionary.ContainsKey(username))
                    {
                        dictionary[username] += 1;
                    }
                    else
                    {
                        dictionary[username] = 1;
                    }
                }
            }

            return dictionary.OrderByDescending(o => o.Value).Take(10).ToDictionary();
        }
    }
}
