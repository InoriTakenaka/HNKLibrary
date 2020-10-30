using StandardizedProject.Std.Interface;
using StandardizedProject.Std.Auxillary;
using StandardizedProject.Std.Services.MsSql;
using System;

namespace HNKLibrary {
    public class LibTest {
        public void foo() {
            SqlServiceFactory<IDbContext> factory = new SqlServiceFactory<IDbContext>();
            string path = factory.FindSelf();
            Console.WriteLine(path);
        }
        public void ServiceList() {
            SqlServiceFactory<IDbContext> factory = new SqlServiceFactory<IDbContext>();
            factory.ServiceList();
        }
        public void GetService() {
            //usage
            SqlServiceFactory<IDbContext> factory = new SqlServiceFactory<IDbContext>();
            dynamic sqlAccess = factory.CreateService<SqlDbAccess>();
            sqlAccess.SetConnection("connectionString");
            Console.WriteLine(sqlAccess.GetType());
        }
    }
    public class program {
        public static void Main(string[] args) {
            LibTest libTest = new LibTest();
            libTest.foo();
            Console.WriteLine("=========================");
            libTest.ServiceList();
            Console.WriteLine("=========================");
            libTest.GetService();
        }
    }
}
