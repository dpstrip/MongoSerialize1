using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;

namespace MongoSerialize1
{
    class Program
    {
        static void Main(string[] args)
        {
            //create MongoDatabase from connection string
            var client = new MongoClient("mongodb://localhost");
            //gets the database within client, if it does not exist, make it
            var database = client.GetDatabase("customers");
            //get collecton from database, if does not exists creates it
            var collection = database.GetCollection<Customer>("customers");
            var collection2 = database.GetCollection<BsonDocument>("customers");
            Customer c = makeCustomer("David", "Stripeik");
          
            //Task.WaitAll(InsertCustomer(collection, c));

             //Task.WaitAll(findCustomer(collection));
            Task.WaitAll(getCustomers(collection));

            //Task.WaitAll(upateDateCustomer(collection,c));
            //Task.WaitAll(getCustomers(collection));
            //Task.WaitAll(deleteCustomer(collection, c));

        }

        

        static async Task getCustomers(IMongoCollection<Customer> collection)
        {
            var filter = new BsonDocument();

            var documents = collection.Find(filter).ToList<Customer>();

            foreach (Customer c in documents)
            {
                Console.WriteLine("Customer " + c.FirstName + ", " + c.LastName + " is found");
                Console.WriteLine("the date the record was entered was " + c.TimeStamp);
                Console.WriteLine("the id is: " + c.Id);
            }

            Console.ReadKey();
        }

        static async Task deleteCustomer(IMongoCollection<Customer> collection, Customer customer)
        {
            var filter = Builders<Customer>.Filter.Eq(c => c.FirstName, "David");
            collection.DeleteOneAsync(filter);
        }

        static async Task upateDateCustomer(IMongoCollection<Customer> collection, Customer customer)
        {
            //var filter = Builders<Customer>.Filter.Eq(c => c.FirstName, "David");
            var filter = Builders<Customer>.Filter.Eq("FirstName", "David");
            var update = Builders<Customer>.Update
                .Set("LastName", "American (New)");
            var result = await collection.UpdateOneAsync(filter, update);
        }

        static async Task findCustomer(IMongoCollection<Customer> collection)
        {
            //create filter to find bson document
            var filter = Builders<Customer>.Filter.Eq(c => c.FirstName, "David");
            //run filter to find it
            var query = collection.Find(filter);
            //get customer
            var customer = query.FirstOrDefault();
            Console.WriteLine("Customer " + customer.FirstName + ", " + customer.LastName + " is found");
            Console.WriteLine("the date the record was entered was " + customer.TimeStamp);
            Console.WriteLine("the id is: " + customer.Id);
            Console.ReadKey();

        }

        static async Task InsertCustomer(IMongoCollection<Customer> collection, Customer c)
        {
            await collection.InsertOneAsync(c);
        }

        public static Customer makeCustomer(string fName, string lName)
        {
            Customer c = new Customer();

            c.FirstName = fName;
            c.LastName = lName;
            c.TimeStamp = DateTime.UtcNow;

            return c;
        }
    }

}
