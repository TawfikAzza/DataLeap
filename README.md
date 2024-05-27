## Run
### 1. Start docker compose
```
docker-compose up
```
### 2. Create the SQL database (through phpMyAdmin)
#### 2.1 Open phpMyAdmin in the browser on http://localhost:9000
#### 2.2 Paste Db.sql content in the SQL tab and run it
### 3. Run the EzMigrationTool project
```
cd EzMigrationTool
dotnet run
```

### Addtionnal information
- Use BIN_TO_UID method when working with the IDs.
- One can access the MongoDB via ```localhost:27017``` (via eg. MongoDB compass) and the phpMyAdmin via ```localhost:9000```