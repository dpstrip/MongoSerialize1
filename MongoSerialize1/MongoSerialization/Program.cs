using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Driver.Builders;

namespace MongoSerialization
{
    class Program
    {
        static void Main(string[] args)
        {
            DAL dal = new DAL();

            //Print all customers in collection
            Console.WriteLine("find all records");
            dal.printCustomers(dal.getCustomers());
            Console.ReadKey();

            //Find one document in collection
            Console.WriteLine("\nfind one record");
            FilterDefinition<Customer> filter = Builders<Customer>.Filter.Eq("FirstName", "David");
            Customer c = dal.getCustomer(filter);
            dal.printCustomers(c);
            Console.ReadKey();

            //Delete the record found and review all documents in the collection
            Console.WriteLine("\ndelete a record and print out remaining records");
            dal.deleteCustomer(c);
            dal.printCustomers(dal.getCustomers());
            Console.ReadKey();


            //make a new record and insert it into the collection.  Then print out all of the documents
            Console.WriteLine("\ninsert a new record");
            Customer c2 = makeCustomer("David", "Stripeik");
            dal.insertCustomer(c2);
            dal.printCustomers(dal.getCustomers());
            Console.ReadKey();

            //update the record 
            Console.WriteLine("\n\nupdate record");
            Console.WriteLine("customer to update");
            dal.printCustomers(c2);
            c2.LastName = "hghgdd";
            //dal.upateCustomer(c2);
            FilterDefinition<Customer> filter2 = Builders<Customer>.Filter.Eq(d => d.Id, c2.Id);
            dal.upateCustomer(filter2, c2);
            Console.WriteLine("\n\nfind all records");
            dal.printCustomers(dal.getCustomers());

            Console.ReadKey();
     
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
