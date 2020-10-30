using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.SqlClient;

namespace StandardizedProject.Std.Auxillary {
    public static class EntityMapping {
        public static E SqlDataReaderToEntity<E>(SqlDataReader reader) where E : class, new() {
            if(reader == null || !reader.HasRows) { throw new ArgumentNullException(); }
            E e = new E();
            PropertyInfo[] properties = typeof(E).GetProperties();
            while (reader.Read()) {
                foreach(var property in properties) {
                    if (!ExistField(reader, property.Name)) { continue; }
                    if(reader[property.Name]==null) { continue; }
                    if (!property.PropertyType.IsGenericType) {
                        property.SetValue(e,
                            reader[property.Name] == DBNull.Value ?
                            null :
                            Convert.ChangeType(reader[property.Name], property.PropertyType),
                            null);
                    }
                    else {
                        Type genericTypeDefinition = property.PropertyType.GetGenericTypeDefinition();
                        if (genericTypeDefinition == typeof(Nullable<>)) {
                            property.SetValue(e,
                                reader[property.Name] == DBNull.Value ?
                                null :
                                Convert.ChangeType(reader[property.Name],
                                property.PropertyType.GetGenericArguments()[0]), null);
                        }
                    }
                }
            }
            return e;
        }
        public static IEnumerable<E> SqlDataReaderToEntityCollection<E>(SqlDataReader reader)where E:class,new() {
            if (reader == null || !reader.HasRows) { throw new ArgumentNullException(); }
            List<E> result = new List<E>();
            PropertyInfo[] properties = typeof(E).GetProperties();
            while (reader.Read()) {
                E e = new E();
                foreach (var property in properties) {
                    if (!ExistField(reader, property.Name)) { continue; }
                    if (reader[property.Name] == null) { continue; }
                    if (!property.PropertyType.IsGenericType) {
                        property.SetValue(e,
                            reader[property.Name] == DBNull.Value ?
                            null :
                            Convert.ChangeType(reader[property.Name], property.PropertyType),
                            null);
                    }
                    else {
                        Type genericTypeDefinition = property.PropertyType.GetGenericTypeDefinition();
                        if (genericTypeDefinition == typeof(Nullable<>)) {
                            property.SetValue(e,
                                reader[property.Name] == DBNull.Value ?
                                null :
                                Convert.ChangeType(reader[property.Name],
                                property.PropertyType.GetGenericArguments()[0]), null);
                        }
                    }
                }
                result.Add(e);
            }
            return result;
        }
        private static bool ExistField(SqlDataReader reader, string field) {
            reader.GetSchemaTable().DefaultView.RowFilter = $"ColumnName= '{field}'";
            return (reader.GetSchemaTable().DefaultView.Count > 0);
        }
    }
}
