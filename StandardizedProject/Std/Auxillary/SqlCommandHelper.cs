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
            string tableName = typeof(E).Name;
            string[] colName = typeof(E).GetProperties().Select(p=>p.Name).ToArray();
            StringBuilder sqlBuilder = new StringBuilder();
            sqlBuilder.Append($"INSERT INTO {tableName} ")
                .Append($" ({string.Join(",", colName)}) ")
                .Append($" VALUES (@{string.Join(",@", colName)})");
            return sqlBuilder.ToString();
        }
        protected virtual string GenerateUpdateSql<E>(E e) where E: class, new() {
            string tableName = typeof(E).Name;
            string[] colName = typeof(E).GetProperties()
                .Where(p => p.Name != "ID")
                .Select(p => $"{p.Name}=@{p.Name}")
                .ToArray();
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
        protected virtual async Task<string> GenerateInserSqlAsync<E>(E e) where E: class, new() {
            return await Task.Run(() => {
                return GenerateInsertSql(e);
            });
        }
        protected virtual async Task<string> GenerateUpdateSqlAsync<E>(E e)where E : class, new() {
            return await Task.Run(() => {
                return GenerateUpdateSql(e);
            });
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
