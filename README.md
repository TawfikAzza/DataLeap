# DataLeap

## Prerequisites
- Docker
- .NET Core 8

## Running ezMigrationTool to execute a test migration

1. **Start Docker Compose**
   - Navigate to the root folder of the app and run:
     ```bash
     docker compose up -d --build
     ```

2. **Ensure Essential Services are Running**
   - Confirm that the following services are running:
     - MySQL
     - MongoDB

3. **Spin Up MySQL and Populate Data**
   - Connect to your MySQL instance.
   - Use the `create_and_populate.sql` script to create and populate the database with mock data simulating a production environment.

4. **Run the EzMigrationTool**
   - Navigate to the `/ezmigrationtool` folder in your command line interface.
   - Execute the console application with the following command:
     ```bash
     dotnet run
     ```
5. **Follow the steps** indicated by the application, your command line interface should prompt something like this:
   
   ![asdasd](https://github.com/TawfikAzza/DataLeap/assets/90683062/28f3eed9-6437-4058-bde8-e5d39c9bfa76)
