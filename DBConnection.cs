using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace Mlshinon
{
    internal static class DBConnection
    {
        public static MySqlConnection Connect(string? cs = null)
        {
            var connStr = string.IsNullOrWhiteSpace(cs)
                ? "server=127.0.0.1;uid=root;database=malshinon"
                : cs;

            var conn = new MySqlConnection(connStr);
            conn.Open();
            return conn;
        }

        public static void Disconnect(MySqlConnection conn) => conn.Close();

        public static MySqlCommand Command(string sql, Parameter[] prms)
        {
            var cmd = new MySqlCommand { CommandText = sql };
            foreach (var p in prms)
                cmd.Parameters.AddWithValue(p.parameter, p.parameterValue);
            return cmd;
        }

        private static MySqlDataReader Send(MySqlConnection conn, MySqlCommand cmd)
        {
            cmd.Connection = conn;
            return cmd.ExecuteReader(System.Data.CommandBehavior.CloseConnection);
        }

        private static List<Dictionary<string, object?>> Parse(MySqlDataReader rdr)
        {
            var rows = new List<Dictionary<string, object?>>();

            using (rdr)
            {
                while (rdr.Read())
                {
                    var row = new Dictionary<string, object?>(rdr.FieldCount);
                    for (int i = 0; i < rdr.FieldCount; i++)
                        row[rdr.GetName(i)] = rdr.IsDBNull(i) ? null : rdr.GetValue(i);

                    rows.Add(row);
                }
            }
            return rows;
        }

        public static List<Dictionary<string, object?>> Execute(
            string sql,
            Parameter[] prms,
            string? connectionString = null)
        {
            var conn = Connect(connectionString);
            var cmd = Command(sql, prms);
            var rdr = Send(conn, cmd); 
            return Parse(rdr);
        }
    }
}
