using EzMigrationTool.Services;

// Constants
const string mysqlConnStr = "server=localhost;user=root;database=EZMoney_test;password=rootpassword;";
const string mongoConnStr = "mongodb://localhost:27017";
const string mongoDatabase = "EZMoney_test";

// Services
var mysqlExtractionService = MysqlExtractionService.GetInstance(mysqlConnStr);
var mongoInjectionService = MongoInjectionService.GetInstance(mongoConnStr, mongoDatabase);
var fileService = FileService.GetInstance();

// ------------ Start ------------

Console.WriteLine("-----------------------------------------------------------");
Console.WriteLine("Welcome to DataLeap, your trustworthy data migration tool.");
Console.WriteLine("-----------------------------------------------------------");

// Extract data from MySQL

Console.WriteLine("+++++++++++");
Console.WriteLine("Starting data extraction from MySQL...");

var users = mysqlExtractionService.ExtractUsers();
var expenses = mysqlExtractionService.ExtractExpenses();
var groups = mysqlExtractionService.ExtractGroups();
Console.WriteLine("Data extraction completed successfully.");

Console.WriteLine("+++++++++++");
Console.WriteLine("Saving data to files...");

fileService.SaveDataToFile(users, "users.json");
fileService.SaveDataToFile(groups, "groups.json");
fileService.SaveDataToFile(expenses, "expenses.json");

Console.WriteLine("Data saving completed successfully.");
Console.WriteLine("-----------------------------------------------");
Console.WriteLine("Would you like to start the migration of the data collected to MongoDB? \n"
                  + "This operation will overwrite any existing data in the MongoDB database \n"
                  + "and the transaction cannot be undone. Proceed with caution. (y/n)");

var step2 = Console.ReadLine();

if (step2 == "n") {
    return;
}

Console.WriteLine("-----------------------------------------------");

// ------------ if continue ------------

// Inject data into MongoDB

Console.WriteLine("+++++++++++");
Console.WriteLine("Starting data migration to MongoDB...");

mongoInjectionService.InjectUsersToMongo();
mongoInjectionService.InjectGroupsToMongo();
mongoInjectionService.InjectExpensesToMongo();

Console.WriteLine("Data migration completed successfully.");
Console.WriteLine("-----------------------------------------------");
Console.WriteLine("Thank you for using DataLeap. Have a great day!");
Console.WriteLine("-----------------------------------------------");

// ------------ End ------------


