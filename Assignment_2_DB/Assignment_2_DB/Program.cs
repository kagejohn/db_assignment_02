﻿using System;
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

            Console.WriteLine("Loading data plz wait...");
            await LoadCollection();

            Console.WriteLine("Input a command: ");
            while (true)
            {
                string input = Console.ReadLine();

                if (input.StartsWith("help"))
                {
                    Console.WriteLine("Available commands:");
                    Console.WriteLine("total twitter users");
                    Console.WriteLine("users that link to most other users");
                    Console.WriteLine("most mentioned users");
                    Console.WriteLine("most active users");
                    Console.WriteLine("most polarized users");
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

                if (input.ToLower() == "most mentioned users")
                {
                    foreach (KeyValuePair<string, int> keyValuePair in MostMentionedUsers())
                    {
                        Console.WriteLine("Username: " + keyValuePair.Key + " Mentions: " + keyValuePair.Value);
                    }
                }

                if (input.ToLower() == "most active users")
                {
                    foreach (KeyValuePair<string, int> keyValuePair in MostActiveUsers())
                    {
                        Console.WriteLine("Username: " + keyValuePair.Key + " Tweets: " + keyValuePair.Value);
                    }
                }

                if (input.ToLower() == "most polarized users")
                {
                    List<Dictionary<string, int>> polarizedTweets = MostPolarizedTweets();

                    Console.WriteLine("Negative list:");
                    foreach (KeyValuePair<string, int> keyValuePair in polarizedTweets[0])
                    {
                        Console.WriteLine("Username: " + keyValuePair.Key + " Tweets: " + keyValuePair.Value);
                    }

                    Console.WriteLine("Positive list:");
                    foreach (KeyValuePair<string, int> keyValuePair in polarizedTweets[1])
                    {
                        Console.WriteLine("Username: " + keyValuePair.Key + " Tweets: " + keyValuePair.Value);
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
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            foreach (Dictionary<string, BsonValue> tweet in Tweets)
            {
                if (tweet["text"].AsString.Contains("@"))
                {
                    List<string> textList = tweet["text"].AsString.Split(' ').ToList();
                    int indexOfMention = textList.FindIndex(s => s.Contains("@"));

                    if (textList[indexOfMention].Length > 1)
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
            }

            return dictionary.OrderByDescending(o => o.Value).Take(10).ToDictionary();
        }

        //Who is are the most mentioned Twitter users? (Provide the top five.)
        static Dictionary<string, int> MostMentionedUsers()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            foreach (Dictionary<string, BsonValue> tweet in Tweets)
            {
                if (tweet["text"].AsString.Contains("@"))
                {
                    List<string> textList = tweet["text"].AsString.Split(' ').ToList();
                    int indexOfMention = textList.FindIndex(s => s.Contains("@"));

                    if (textList[indexOfMention].Length > 1)
                    {
                        string mentionedUser = textList[indexOfMention].Substring(1, textList[indexOfMention].Length - 1);
                        if (dictionary.ContainsKey(mentionedUser))
                        {
                            dictionary[mentionedUser] += 1;
                        }
                        else
                        {
                            dictionary[mentionedUser] = 1;
                        }
                    }
                }
            }

            return dictionary.OrderByDescending(o => o.Value).Take(5).ToDictionary();
        }

        //Who are the most active Twitter users (top ten)?
        static Dictionary<string, int> MostActiveUsers()
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();

            foreach (Dictionary<string, BsonValue> tweet in Tweets)
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

            return dictionary.OrderByDescending(o => o.Value).Take(10).ToDictionary();
        }

        //Who are the five most grumpy (most negative tweets) and the most happy (most positive tweets)?
        static List<Dictionary<string, int>> MostPolarizedTweets()
        {
            Dictionary<string, int> dictionaryNegative = new Dictionary<string, int>();
            Dictionary<string, int> dictionaryPositive = new Dictionary<string, int>();

            foreach (Dictionary<string, BsonValue> tweet in Tweets)
            {
                if (tweet["polarity"] == 0)
                {
                    string username = tweet["user"].ToString();
                    if (dictionaryNegative.ContainsKey(username))
                    {
                        dictionaryNegative[username] += 1;
                    }
                    else
                    {
                        dictionaryNegative[username] = 1;
                    }
                }
                else if (tweet["polarity"] == 4)
                {
                    string username = tweet["user"].ToString();
                    if (dictionaryPositive.ContainsKey(username))
                    {
                        dictionaryPositive[username] += 1;
                    }
                    else
                    {
                        dictionaryPositive[username] = 1;
                    }
                }
            }

            List<Dictionary<string, int>> dictionaryList = new List<Dictionary<string, int>>
            {
                dictionaryNegative.OrderByDescending(o => o.Value).Take(5).ToDictionary(),
                dictionaryPositive.OrderByDescending(o => o.Value).Take(5).ToDictionary()
            };

            return dictionaryList;
        }
    }
}
