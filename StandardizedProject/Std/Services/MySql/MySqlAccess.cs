using StandardizedProject.Std.Interface;
using StandardizedProject.Std.Auxillary;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace StandardizedProject.Std.Services.MySql {
    public class MySqlAccess : IDbContext {
        public bool Delete<E>(E e) where E : class, new() {
            throw new System.NotImplementedException();
        }

        public int DeleteRange<E>(IEnumerable<E> entities) where E : class, new() {
            throw new System.NotImplementedException();
        }

        public DataTable ExecuteDataTable(SqlCommand sqlCommand, SqlConnection connection) {
            throw new System.NotImplementedException();
        }

        public DataTable ExecuteDataTable(SqlCommand sqlCommand, SqlConnection connection, SqlParameter[] parameters) {
            throw new System.NotImplementedException();
        }

        public int ExecuteNonQuery(SqlCommand sqlCommand, SqlConnection connection) {
            throw new System.NotImplementedException();
        }

        public int ExecuteNonQuery(SqlCommand sqlCommand, SqlConnection connection, SqlParameter[] parameters) {
            throw new System.NotImplementedException();
        }

        public IEnumerable<E> GetEntities<E>() where E : class, new() {
            throw new System.NotImplementedException();
        }

        public E GetEntity<E>(string condition) where E : class, new() {
            throw new System.NotImplementedException();
        }

        public bool Insert<E>(E e) where E : class, new() {
            throw new System.NotImplementedException();
        }

        public int InsertRange<E>(IEnumerable<E> entities) where E : class, new() {
            throw new System.NotImplementedException();
        }

        public bool Update<E>(E e) where E : class, new() {
            throw new System.NotImplementedException();
        }

        public int UpdateRange<E>(IEnumerable<E> entities) where E : class, new() {
            throw new System.NotImplementedException();
        }
    }
}
