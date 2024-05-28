# BenchmarkingTool
This is a tool to benchmark queries in MySQL and MongoDB, based on the project [fast-and-querious](https://github.com/patrickstolc/fast-and-queryous).

## Run
To run the benchmarks, issue the following commands, from the root of the BenchmarkingTool project:

### MongoDB
```
dotnet run --connectionString "mongodb://localhost:27017" --databaseName EZMoney_test --iterations 100 --benchmarkConfig benchmark.mongodb.config.json
```
### MySQL
```
dotnet run --connectionString "server=localhost;user=root;database=EZMoney_test;password=rootpassword;" --databaseName EZMoney_test --iterations 100 --benchmarkConfig benchmark.mysql.config.json
```