using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Data.SqlClient;

namespace LibraryAPI
{
    public class SQLConnection
    {
        public string _connectionString;

        public SQLConnection(string connectionString)
        {
            _connectionString = connectionString;
        }

        public T ExecuteQuery<T>(string query, List<KeyValuePair<string, object>> @params)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                // Add parameters to the command
                foreach (var param in @params)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (typeof(T) == typeof(DataTable))
                    {
                        var table = new DataTable();
                        table.Load(reader);
                        return (T)(object)table;
                    }
                    else if (typeof(T) == typeof(DataRow))
                    {
                        var table = new DataTable();
                        table.Load(reader);
                        return (T)(object)table.Rows[0];
                    }
                    else if (typeof(T) == typeof(string))
                    {
                        return (T)(object)reader.GetString(0);
                    }
                    else if (typeof(T) == typeof(int))
                    {
                        return (T)(object)reader.GetInt32(0);
                    }
                    else if (typeof(T) == typeof(bool))
                    {
                        return (T)(object)reader.GetBoolean(0);
                    }
                    else
                    {
                        throw new NotSupportedException("Unsupported return type.");
                    }
                }

                connection.Close();
            }
        }



        public int ExecuteNonQuery(string query, List<KeyValuePair<string, object>> @params, Boolean GetScopeIdentity = false)
        {
            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(query, connection))
            {
                // Add parameters to the command
                foreach (var param in @params)
                {
                    command.Parameters.AddWithValue(param.Key, param.Value);
                }
                connection.Open();

                if (GetScopeIdentity == true)
                {
                   
                    command.CommandText += "; SELECT SCOPE_IDENTITY()";
                    
                    try
                    {
                        return (int)(decimal)command.ExecuteScalar();
                    }
                    catch (Exception ex)
                    {
                        return -1;
                    }


                }
                else
                {
                    var returnResult = command.ExecuteNonQuery();
                    return returnResult;
                }

                connection.Close();


            }
        }
    }
}
