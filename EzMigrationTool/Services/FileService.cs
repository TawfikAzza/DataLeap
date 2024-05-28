using MongoDB.Bson;
using MongoDB.Bson.IO;

namespace EzMigrationTool.Services;

public class FileService {
    private static FileService _instance = null!;

    private FileService() {
    }

    public void SaveDataToFile(List<BsonDocument> document, string fileName) {
        try {
            // Convert the list of BsonDocuments to a BsonArray and then to JSON
            var usersJson = new BsonArray(document).ToJson(new JsonWriterSettings { Indent = true });

            // Get the current working directory
            string currentDirectory = Environment.CurrentDirectory;

            // Navigate to the desired target directory relative to the current directory
            string parentDirectory = Directory.GetParent(currentDirectory)!.Parent!.Parent!.FullName;
            string targetDirectory = Path.Combine(parentDirectory, "jsonData");

            // Ensure the directory exists
            if (!Directory.Exists(targetDirectory)) {
                Directory.CreateDirectory(targetDirectory);
            }

            // Define the file path
            string filePath = Path.Combine(targetDirectory, fileName);

            // Open a StreamWriter to write the JSON string to a file
            using (var writer = new StreamWriter(filePath)) {
                writer.Write(usersJson);
            }
            Console.WriteLine("Data saved to file: " + fileName);
        }
        catch (Exception ex) {
            Console.WriteLine("An error occurred while saving data to file: " + ex.Message);
        }
    }

    #region Singleton

    public static FileService GetInstance() {
        if (_instance == null!) {
            _instance = new FileService();
        }
        return _instance;
    }

    #endregion
}
