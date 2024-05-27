using IntegrityTest.Repositories;

namespace IntegrityTest.Repositories.Factories;

public static class MongoRepositoryFactory
{
    public static MongoRepository Create()
    {
        return new MongoRepository("mongodb://localhost:27017", "EZMoney_test");
    }
}