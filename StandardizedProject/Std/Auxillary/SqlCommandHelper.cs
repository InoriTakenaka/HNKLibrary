using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StandardizedProject.Std.Auxillary {
    public class SqlCommandHelper {
        private static Dictionary<string, string> sqlCache_;
        private static SqlCommandHelper instance_;
        private SqlCommandHelper() {
            sqlCache_ = new Dictionary<string, string>();
        }
        protected virtual string GenerateInsertSql<E>(E e) where E: class, new() {
            // Get Entity's Type Declare
            Type type = e.GetType();

            // Get TableName in Database
            string tableName = type.IsDefined(typeof(EntityTagAttribute), false) ?
                               type.GetCustomAttribute<EntityTagAttribute>().DisplayName :
                               type.Name.ToUpper();
            // Ignore primary key in Entity
            string[] colNames = type.GetProperties()
                .Where(p => {
                    return
                    p.IsDefined(typeof(EntityTagAttribute), false) ?
                    !(p.GetCustomAttribute<EntityTagAttribute>().PK) :
                    !(p.Name.ToUpper().Contains("ID"));
                })
                .Select(p => {
                    return
                    p.IsDefined(typeof(EntityTagAttribute), false) ?
                    (p.GetCustomAttribute<EntityTagAttribute>().DisplayName) :
                    (p.Name.ToUpper());
                })
                .ToArray();

            // Building the Sql Command Text
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append($"INSERT INTO {tableName} ")
                .Append($" ({string.Join(",", colNames)}) ")
                .Append($" VALUES (@{string.Join(",@", colNames)})");

            return sqlBuilder.ToString();
        }
        protected virtual string GenerateUpdateSql<E>(E e) where E: class, new() {
            // Get Entity's Type Declare
            Type type = e.GetType();

            // Get Property List of Entity
            PropertyInfo[] properties = type.GetProperties();

            // Get TableName in Database
            string tableName = type.IsDefined(typeof(EntityTagAttribute), false) ?
                               type.GetCustomAttribute<EntityTagAttribute>().DisplayName :
                               type.Name.ToUpper();

            // Get Primary Key's Name in Database
            string pkName = GetAndRemovePrimaryKey(ref properties);

            // Get All Column's Name in Database
            string[] colName = properties.Select(p => $"{p.Name}=@{p.Name}").ToArray();
               
            // Building Sql Command Text
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append($" UPDATE {tableName} ")
                .Append($" SET {string.Join(",", colName)} ")
                .Append(" WHERE ID=@ID ");

            return sqlBuilder.ToString();
        }
        protected virtual string GenerateDeleteSql<E>(E e)where E : class, new() {
            string tableName = typeof(E).Name;
            return $"DELETE FROM {tableName} WHERE ID=@ID";
        }
        protected virtual Task<string> GenerateInserSqlAsync<E>(E e) where E: class, new() {
            return Task.Run(() => {
                int a = 1;
                return GenerateInsertSql(e);//klklklklklk
            });
        }
        protected virtual Task<string> GenerateUpdateSqlAsync<E>(E e)where E : class, new() {
            return  Task.Run(() => {
                return GenerateUpdateSql(e);
            });
        }

        /// <summary>
        /// Get Primary Key's Name in Database,
        /// then Remove it from Property List.
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public virtual string GetAndRemovePrimaryKey(ref PropertyInfo[] properties) {

            int i = 0;
            string result = string.Empty;
            PropertyInfo[] temp = new PropertyInfo[properties.Length];
            properties.CopyTo(temp, 0);

            PropertyInfo property = null;
            for (; i < properties.Length; i++) {
                property = properties[i];
                if (property.IsDefined(typeof(EntityTagAttribute), false)) {
                    var attr = property.GetCustomAttribute<EntityTagAttribute>();
                    if (attr.PK) {
                        result = attr.DisplayName;
                        break;                        
                    }
                }
                if (property.Name.ToUpper().Contains("ID")) {
                    result = property.Name.ToUpper();
                    break;
                }                
            }

            if (null == property) { throw new InvalidOperationException(); }
            properties = temp.SkipWhile(p => p.Name == property.Name).ToArray();
            return result;
        }
        public string InsertSql<E>(E e) where E : class, new() {
            string key = typeof(E).Name + "_INSERT";
            if (sqlCache_.ContainsKey(key)) { return sqlCache_[key]; }
            else {
                string inserSql = GenerateInsertSql(e);
                sqlCache_.Add(key, inserSql);
                return inserSql;
            }
        }
        public string UpdateSql<E>(E e)where E : class, new() {
            string key = typeof(E).Name + "_UPDATE";
            if (sqlCache_.ContainsKey(key)) { return sqlCache_[key]; }
            else {
                string updateSql = GenerateUpdateSql(e);
                sqlCache_.Add(key, updateSql);
                return updateSql;
            }               
        }
        public string DeleteSql<E>(E e)where E : class, new() {
            string key = typeof(E).Name + "_DELETE";
            if (sqlCache_.ContainsKey(key)) { return sqlCache_[key]; }
            else {
                string deleteSql = GenerateDeleteSql(e);
                sqlCache_.Add(key, deleteSql);
                return deleteSql;
            }
        }
        public static SqlCommandHelper GetInstance() {
            if(instance_ == null) {
                instance_ = new SqlCommandHelper();
            }
            return instance_;
        }
    }
}
