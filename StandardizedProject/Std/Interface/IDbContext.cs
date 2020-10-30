using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace StandardizedProject.Std.Interface {
    /// <summary>
    /// All interface's implementation are Required.
    /// </summary>
    public interface IDbContext {
        E GetEntity<E>(string condition) where E:class,new();
        IEnumerable<E> GetEntities<E>() where E : class, new();
        int InsertRange<E>(IEnumerable<E> entities) where E : class, new();
        bool Insert<E>(E e) where E : class, new();
        int DeleteRange<E>(IEnumerable<E> entities) where E : class, new();
        bool Delete<E>(E e) where E : class, new();
        int UpdateRange<E>(IEnumerable<E> entities) where E : class, new();
        bool Update<E>(E e) where E : class, new();
        int ExecuteNonQuery(SqlCommand sqlCommand, SqlConnection connection);
        int ExecuteNonQuery(SqlCommand sqlCommand, SqlConnection connection, SqlParameter[] parameters);
        DataTable ExecuteDataTable(SqlCommand sqlCommand,SqlConnection connection);
        DataTable ExecuteDataTable(SqlCommand sqlCommand, SqlConnection connection, SqlParameter[] parameters);
    }
}
