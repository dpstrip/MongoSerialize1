using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver.Builders;
using MongoDB.Driver;
using MongoDB.Bson;
using System.Xml.Linq;

namespace MongoSerialization
{
    class DAL
    {
        private MongoClient client;
        private IMongoDatabase database;
        private IMongoCollection<Customer> collection;
        public FilterDefinition<Customer> filter;

        public DAL()
        {
            client = new MongoClient("mongodb://localhost");
            //gets the database within client, if it does not exist, make it
            database = client.GetDatabase("customers");
            //get collecton from database, if does not exists creates it
            collection = database.GetCollection<Customer>("customers");
        }

        /*
         * Brings back all objects
         * */
        public  List<Customer> getCustomers()
        {
            var filter = new BsonDocument();
            var documents = collection.Find(filter).ToList<Customer>();

            return documents;
        }

        public void printCustomers(List<Customer> customerList)
        {
            foreach (Customer c in customerList)
            {
                printCustomers(c);
            }
        }

        public void printCustomers(Customer customer)
        {
            if(customer != null)
            {
                Console.WriteLine("Customer " + customer.FirstName + ", " + customer.LastName + " is found");
                Console.WriteLine("the date the record was entered was " + customer.TimeStamp);
                Console.WriteLine("the id is: " + customer.Id);
            }
            
        }


        /*
         * Pass a FilterDefinition<Customer> filter object
         * to find the object you want.
         * FilterDefinition<Customer> filter = Builders<Customer>.Filter.Eq(c => c.Id, customer.Id);
         * is the id of teh object you are looking for
         * Brings back one object
         * */
        public Customer getCustomer(FilterDefinition<Customer> filter)
        {
            //run filter to find it
            var query = collection.Find(filter);
            //get customer
            var customer = query.FirstOrDefault();
            return customer;
        }

        /*
         * give it the object to delete  Uses ID to find
         * object
         * */
        public async void deleteCustomer(Customer customer)
        {
            var filter = Builders<Customer>.Filter.Eq(c => c.Id, customer.Id);
            await collection.DeleteOneAsync(filter);
        }

        /*
         * Create a new object and it is
         * inserted.  The Object Id after insertion
         * is given back
         * */

        public async void insertCustomer(Customer c)
        {
            await collection.InsertOneAsync(c);
        }

        /*
         * This update replaces the entire object, except for the id, and 
         * uses the ID to find the object.
         * */
        public async void upateCustomer(Customer customer)
        {
            var filter = Builders<Customer>.Filter.Eq(c => c.Id, customer.Id);
            //var update = Builders<Customer>.Update.Set("LastName", "American (New)");
            await collection.ReplaceOneAsync(filter, customer);
            //await collection.UpdateOneAsync(filter, update);
        }


        /*This update is used if I need something other then the ID obkect to find the object.
         * Still doing a full replacement of the object.
         * Must create a FilterDefinition<Customer> filter object with search critera defined 
         * to use this function. 
         * */
        public async void upateCustomer(FilterDefinition<Customer> filter,Customer customer)
        {
            //var filter = Builders<Customer>.Filter.Eq(c => c.Id, customer.Id);

            await collection.ReplaceOneAsync(filter, customer);

        }
    }
}
