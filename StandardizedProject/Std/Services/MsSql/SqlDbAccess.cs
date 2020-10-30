using StandardizedProject.Std.Auxillary;
using StandardizedProject.Std.Interface;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace StandardizedProject.Std.Services.MsSql {
    public class SqlDbAccess : IDbContext {
        private string connectionString_;
        public SqlDbAccess(string connectionString) {
            connectionString_ = connectionString;
        }      
        public SqlDbAccess() {
            connectionString_ = string.Empty;
        }
        public void SetConnection(string connectionString) {
            connectionString_ = connectionString;
        }
        public bool Delete<E>(E e) where E : class, new() {
            string commandText = SqlCommandHelper.GetInstance().DeleteSql(e);
            using(SqlConnection connection = new SqlConnection(connectionString_)) {
                using(SqlCommand command = new SqlCommand()) {
                    command.CommandText = commandText;
                    return (ExecuteNonQuery(command,connection)==1);
                }
            }
        }
        public int DeleteRange<E>(IEnumerable<E> entities) where E : class, new() {
            string commandText = SqlCommandHelper.GetInstance().DeleteSql(entities.First());
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectionString_)) {
                using (SqlCommand sqlCommand = new SqlCommand()) {
                    sqlCommand.CommandText = commandText;
                    foreach (var e in entities) {
                        SqlParameter[] sqlParameters
                            = GetParameters(e);
                        result += ExecuteNonQuery(sqlCommand, connection, sqlParameters);
                        sqlCommand.Parameters.Clear();
                    }
                    return result;
                }
            }
        }
        public SqlDataReader ExecuteDataReader(SqlCommand sqlCommand, SqlConnection connection) {
            return ExecuteDataReader(sqlCommand, connection, null);
        }
        public SqlDataReader ExecuteDataReader(SqlCommand sqlCommand, SqlConnection connection, SqlParameter[] parameters) {
            if(parameters!=null) { 
                sqlCommand.Parameters.AddRange(parameters); 
            }
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandTimeout = 3;
            if (connection.State != ConnectionState.Open) {
                connection.Open();
            }
            SqlDataReader reader = sqlCommand.ExecuteReader();
            connection.Close();
            return reader;
        }
        public DataTable ExecuteDataTable(SqlCommand sqlCommand, SqlConnection connection) {
            return ExecuteDataTable(sqlCommand, connection, null);
        }
        public DataTable ExecuteDataTable(SqlCommand sqlCommand, SqlConnection connection, SqlParameter[] parameters) {
            if (parameters != null) { sqlCommand.Parameters.AddRange(parameters); }
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandTimeout = 3;
            if (connection.State != ConnectionState.Open) { connection.Open(); }
            SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
            DataTable table = new DataTable();
            adapter.Fill(table);
            return table;
        }
        public int ExecuteNonQuery(SqlCommand sqlCommand, SqlConnection connection) {
            return ExecuteNonQuery(sqlCommand, connection,null);            
        }
        public int ExecuteNonQuery(SqlCommand sqlCommand, SqlConnection connection, SqlParameter[] parameters) {
            sqlCommand.Connection = connection;
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandTimeout = 3;
            if (parameters != null) {
                sqlCommand.Parameters.AddRange(parameters);
            }
            if (connection.State != ConnectionState.Open) {
                connection.Open();
            }
            return sqlCommand.ExecuteNonQuery();
        }
        public IEnumerable<E> GetEntities<E>() where E : class, new() {
            using (SqlConnection connection = new SqlConnection(connectionString_)) {
                using (SqlCommand sqlCommand = new SqlCommand()) {
                    sqlCommand.CommandText = $"SELECT * FROM {typeof(E).Name}";
                    using (SqlDataReader reader = ExecuteDataReader(sqlCommand, connection)) {
                        return EntityMapping.SqlDataReaderToEntityCollection<E>(reader);
                    }
                }
            }
        }
        public E GetEntity<E>(string condition) where E : class, new() {
            using(SqlConnection connection=new SqlConnection(connectionString_)) {
                using (SqlCommand sqlCommand = new SqlCommand()) {
                    sqlCommand.CommandText = condition;
                    using(SqlDataReader reader = ExecuteDataReader(sqlCommand, connection)) {
                       return EntityMapping.SqlDataReaderToEntity<E>(reader);
                    }
                }
            }
        }
        public bool Insert<E>(E e) where E : class, new() {
           string commandText = SqlCommandHelper.GetInstance().InsertSql(e);
            using(SqlConnection connection=new SqlConnection(connectionString_)) {
                using(SqlCommand sqlCommand = new SqlCommand()) {
                    sqlCommand.CommandText = commandText;
                    var parameters = GetParameters(e);
                    return (ExecuteNonQuery(sqlCommand, connection, parameters) == 1);
                }
            }
        }
        public int InsertRange<E>(IEnumerable<E> entities) where E : class, new() {
            string commandText = SqlCommandHelper.GetInstance().InsertSql(entities.First());
            int result = 0;
            using (SqlConnection connection = new SqlConnection(connectionString_)) {
                using (SqlCommand sqlCommand = new SqlCommand()) {
                    sqlCommand.CommandText = commandText;
                    foreach (var e in entities) {
                        var parameters = GetParameters(e);
                        result += ExecuteNonQuery(sqlCommand, connection, parameters);
                        sqlCommand.Parameters.Clear();
                    }
                    return result;
                }
            }
        }
        public bool Update<E>(E e) where E : class, new() {
            string commandText = SqlCommandHelper.GetInstance().UpdateSql(e);
            using(SqlConnection connection=new SqlConnection(connectionString_)) {
                using(SqlCommand sqlCommand = new SqlCommand()) {
                    sqlCommand.CommandText = commandText;
                    SqlParameter[] sqlParameters = GetParameters(e);
                    return (ExecuteNonQuery(sqlCommand, connection) == 1);
                }
            }
        }
        public int UpdateRange<E>(IEnumerable<E> entities) where E : class, new() {
            string commandText = SqlCommandHelper.GetInstance().UpdateSql(entities.First());
            using (SqlConnection connection=new SqlConnection(connectionString_)) {
                using(SqlCommand sqlCommand = new SqlCommand()) {
                    int result = 0;
                    sqlCommand.CommandText = commandText;
                    foreach(var e in entities) {
                        SqlParameter[] sqlParameters = GetParameters(e);
                        result += ExecuteNonQuery(sqlCommand, connection,sqlParameters);
                        sqlCommand.Parameters.Clear();
                    }                    
                    return result;
                }
            }
        }

        protected SqlParameter[] GetParameters<E>(E e) {
            return typeof(E).GetProperties().Select(p => {
                return new SqlParameter($"@{p.Name}", p.GetValue(e, null));
            }).ToArray();
        }
       
    }
}
