using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMongo
{
    public class Program
    {
        static void Main(string[] args)
        {
            MainAsync().Wait();

            Console.WriteLine("Press enter to exit");

            Console.ReadLine();
        }

        static async Task MainAsync()
        {
            //OR - THIS:
            //var client = new MongoClient(new MongoUrl("mongodb://localhost:27017"));
            try
            {
                var client = new MongoClient();

                Console.WriteLine("-<Getting the database...>-");
                IMongoDatabase db = client.GetDatabase("schoool");

                Console.WriteLine("-<Getting the collection...>-");
                var collection = db.GetCollection<Student>("students");

                var newStudents = CreateNewStudents();

                Console.WriteLine("-<Creating many students...>-");
                await collection.InsertManyAsync(newStudents);

                //DISPLAY TO CONSOLE
                var retrievedCollection = db.GetCollection<BsonDocument>("students");

                Console.WriteLine("-----------------------------");
                Console.WriteLine("-<This are the students created...>-");
                using (IAsyncCursor<BsonDocument> cursor = await retrievedCollection.FindAsync(new BsonDocument()))
                {
                    while (await cursor.MoveNextAsync())
                    {
                        IEnumerable<BsonDocument> batch = cursor.Current;
                        foreach (BsonDocument document in batch)
                        {
                            Console.WriteLine(document);
                            Console.WriteLine();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        private static IEnumerable<Student> CreateNewStudents()
        {
            var student1 = new Student
            {
                FirstName = "Gregor",
                LastName = "Felix",
                Subjects = new List<string>() { "English", "Mathematics", "Physics", "Biology" },
                Class = "JSS 7",
                Age = 13
            };

            var student2 = new Student
            {
                FirstName = "Machiko",
                LastName = "Elkberg",
                Subjects = new List<string> { "English", "Mathematics", "Spanish" },
                Class = "JSS 8",
                Age = 14
            };

            var student3 = new Student
            {
                FirstName = "Julie",
                LastName = "Sandal",
                Subjects = new List<string> { "English", "Mathematics", "Physics", "Chemistry" },
                Class = "JSS 4",
                Age = 11
            };

            var newStudents = new List<Student> { student1, student2, student3 };

            return newStudents;
        }
    }

    internal class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Class { get; set; }
        public int Age { get; set; }
        public IEnumerable<string> Subjects { get; set; }
    }
}
