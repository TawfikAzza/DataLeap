using IntegrityTest.Repositories;

namespace IntegrityTest.Repositories.Factories;

public static class SqlRepositoryFactory
{
    public static SqlRepository Create()
    {
        return new SqlRepository("server=localhost;user=root;database=EZMoney_test;password=rootpassword;");
    }
}