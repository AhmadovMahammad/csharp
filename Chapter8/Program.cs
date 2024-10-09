﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Net.NetworkInformation;

namespace Chapter8
{
    public class Program
    {
        private static void Main(string[] args)
        {
            // Using EF Core

            /* EF Core Entity Classes
             
            EF Core lets you use any class to represent data, 
            as long as it contains a public property for each column that you want to query.
            
            For instance, we could define the following entity class to query and update a Customers table in the database:

            public class Customer
            {
                public Guid CustomerID { get; set; }
                public string FirstName { get; set; } = string.Empty;
                public string LastName { get; set; } = string.Empty;
                public string Address { get; set; } = string.Empty;
            }

            */

            /* DbContext
             
            After defining entity classes, the next step is to subclass DbContext. 
            An instance of that class represents your sessions working with the database.

            Typically, your DbContext subclass will contain one DbSet<T> property 
            for each entity in your model:

            public class NutshellContext : DbContext
            {
                public DbSet<Customer> Customers { get; set; } = null!;
                // properties for other tables
            }

            A DbContext object does three things:

            1. It acts as a factory for generating DbSet<> objects that you can query.
            2. It keeps track of any changes that you make to your entities so that you can write them back.
            3. It provides virtual methods that you can override to configure the connection and model.

            */

            /* Configuring the connection
             
            By overriding the OnConfiguring method, you can specify the database provider and connection string:
            
            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                if (!optionsBuilder.IsConfigured)
                {
                    optionsBuilder.UseSqlServer(@"Server=(local);Database=Nutshell;Trusted_Connection=True");
                }
            }

            In this example, the connection string is specified as a string literal. 
            Production applications would typically retrieve it from a configuration file such as appsettings.json.

            If you’re using ASP.NET, you can allow its dependency injection framework to preconfigure optionsBuilder; 
            in most cases, this lets you avoid overriding OnConfiguring altogether.

            In the OnConfiguring method, you can enable other options, including lazy loading.

            */

            /* Configuring the Model
             
            By default, EF Core is convention based, 
            meaning that it infers the database schema from your class and property names.

            You can override the defaults by overriding OnModelCreating and 
            calling extension methods on the ModelBuilder parameter.

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Customer>()
                    .ToTable("Customer", "DefaultSchema");
            }

            Without this code, EF Core would map this entity to a table named “Customers” rather than “Customer”, 
            because we have a DbSet<Customer> property in our DbContext called Customers:

            */

            /* Creating the database
             
            A better approach is to use EF Core’s migrations feature, 
            which not only creates the database but configures it such that 
            EF Core can automatically update the schema in the future when your entity classes change.

            Install-Package Microsoft.EntityFrameworkCore.Tools
            Add-Migration InitialCreate
            Update-Database

            1. The first command installs tools to manage EF Core from within Visual Studio
            2. The second command generates a special C# class known as a code migration that 
               contains instructions to create the database.
            3. The final command runs those instructions against the database connection string specified in 
               the project’s application configuration file.
            */

            /* Using DbContext
            
            After you’ve defined Entity classes and subclassed DbContext, 
            you can instantiate your DbContext and query the database, as follows:

            using var dbContext = new NutshellContext();
            Console.WriteLine(dbContext.Customers.Count());
            // Executes "SELECT COUNT(*) FROM [Customer] AS [c]"

            You can also use your DbContext instance to write to the database. 
            The following code inserts a row into the Customer table:
            
            using var dbContext = new NutshellContext();
            Customer cust = new Customer()
            {
                Name = "Sara Wells"
            };
            
            dbContext.Customers.Add (cust);
            dbContext.SaveChanges(); // Writes changes back to database
            */

            /* Object Tracking
            
            Object tracking in Entity Framework Core (EF Core) refers to the ability of the DbContext 
            to keep track of the state of entity objects retrieved from the database.

            1. Header: Entity State Management

            When you retrieve entities from the database, EF Core tracks the entity's state 
            (Added, Unchanged, Modified, Deleted, or Detached). 
            
            This state management allows EF Core to know if an entity needs to be 
            1. inserted, 2. updated, or 3. deleted when SaveChanges is called.

            using var dbContext = new NutshellContext();

            // fake db seed
            //var customer = new Customer
            //{
            //    CustomerID = Guid.NewGuid(),
            //    FirstName = "Mahammad",
            //    LastName = "Ahmadov"
            //};

            //dbContext.Customers.Add(customer);
            //dbContext.SaveChanges();

            //---------- First version with tracking

            //// 1. EF Core tracks the customer after retrieving it from the database
            //var customerFromDb = dbContext.Customers.First();
            //Console.WriteLine($"First fetch: {customerFromDb.FirstName} {customerFromDb.LastName}");

            //// 2. Change the customer's name (EF Core will mark it as Modified)
            //customerFromDb.LastName = "Ahmadli";

            //// Check the entity state before calling SaveChanges
            //var entry = dbContext.Entry(customerFromDb);
            //Console.WriteLine($"Customer State: {entry.State}"); // Output: Modified

            //// Save the changes, so the modification is saved in the database
            //dbContext.SaveChanges();

            //// 3. Fetch the customer again, object tracking ensures we get the updated data
            //var updatedCustomer = dbContext.Customers.First();
            //Console.WriteLine($"Updated fetch: {updatedCustomer.FirstName} {updatedCustomer.LastName}");

            //---------- Second version with no tracking

            // 4. No-tracking query: fetch without tracking
            
            //var customerNoTracking = dbContext.Customers.AsNoTracking().First();
            //Console.WriteLine($"No-tracking fetch: {customerNoTracking.FirstName} {customerNoTracking.LastName}");

            //// Modify the entity (though it's not being tracked)
            //customerNoTracking.LastName = "Updated LastName";

            //// Attempt to check the entity state
            //var entry = dbContext.Entry(customerNoTracking);

            //// Checking the entity state will raise an exception or report "Detached" since it's not tracked.
            //Console.WriteLine($"Customer State: {entry.State}"); // Output: Detached

            2. Header: Identity Map Pattern

            EF Core follows the Identity Map pattern to ensure that each entity is unique within the scope of a DbContext

            using var dbContext = new NutshellContext();

            Customer a = dbContext.Customers.OrderBy(c => c.FirstName).First();
            Customer b = dbContext.Customers.OrderBy(c => c.CustomerID).First();
            Console.WriteLine(Object.ReferenceEquals(a, b)); // True

            The query may retrieve the same customer from the database twice, 
            but EF Core ensures that it gives you the same instance of the Customer object. 
            This helps in preventing inconsistent data states across your application.

            3. Header: AsNoTracking Queries

            If you are fetching data for read-only purposes and you don't need to track changes to entities, 
            using the AsNoTracking method improves performance by not keeping track of the entities returned by the query.

            No Changes Are Tracked: Any modifications you make to this entity will not be tracked by EF Core. 
            Therefore, if you try to call SaveChanges(), 
            EF Core won't know that anything has changed because the entity is not in the Modified state.

            Read-Only Purpose: AsNoTracking() is primarily intended for read-only purposes where 
            you don't intend to make changes to the data or persist those changes back to the database. 
            It improves performance by avoiding the overhead of tracking entities.

            using var dbContext = new NutshellContext();

            var customerNoTracking = dbContext.Customers.AsNoTracking().First();
            Console.WriteLine($"No-tracking fetch: {customerNoTracking.FirstName} {customerNoTracking.LastName}");

            // Modify the entity (though it's not being tracked)
            customerNoTracking.LastName = "Updated LastName";

            // Attempt to check the entity state
            var entry = dbContext.Entry(customerNoTracking);
            Console.WriteLine($"Customer State: {entry.State}"); // Output: Detached

            // No changes will be saved because customer is not tracked.
            dbContext.SaveChanges();

            If you want to persist changes to a non-tracked entity, you should change its state manually

            entry.State = EntityState.Modified;
            dbContext.SaveChanges(); // Now data is changed
            */

            /* Change Tracking
             
            */

            //using var dbContext = new NutshellContext();

            //var customerNoTracking = dbContext.Customers.AsNoTracking().First();
            //Console.WriteLine($"No-tracking fetch: {customerNoTracking.FirstName} {customerNoTracking.LastName}");


            /* Navigation Properties
             
            */

            /* Loading navigation properties
             
            */

            /* Lazy loading
             
            */

            /* Deferred Execution
             
            */

            /* Expression Trees (437)
             
            */
        }
    }
}