using StandardizedProject.Std.Interface;
using StandardizedProject.Std.Auxillary;
using StandardizedProject.Std.Services.MsSql;
using System;
using System.Reflection;

namespace HNKLibrary {

    [EntityTag("TABLE_ENTITY")]
    class TestEntity {
        [EntityTag("PRIMARY_KEY",PK =true)]
        public int ID { get; set; }
        [EntityTag("CONTENTS")]
        public string txtField { get; set; }
        [EntityTag("PRICE")]
        public decimal entityPrice { get; set; }
    }
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
        public void TestFindPK() {

            var properties = typeof(TestEntity).GetProperties();

            string pkName = SqlCommandHelper.GetInstance().GetAndRemovePrimaryKey(ref properties);

            Console.WriteLine($"Primary Key in Database is {pkName} .");

            foreach(var p in properties) {
                string name = "";
                if (p.IsDefined(typeof(EntityTagAttribute), false)) { name = p.GetCustomAttribute<EntityTagAttribute>().DisplayName; }
                else { name = p.Name; }
                Console.WriteLine($"Name->{p.Name}");
                Console.WriteLine($"Display Name is ->{name}");
                Console.WriteLine("__________________");
            }
        }
    }
    public class program {
        public static void Main(string[] args) {
            LibTest test = new LibTest();
            test.TestFindPK();
        }
    }
}
