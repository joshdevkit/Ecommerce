using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Ecommerce.Infrastructure.Data
{
    public class DBConnection : IDisposable
    {
        private readonly SqlConnection _connection;
        private SqlTransaction? _transaction;

        public DBConnection(string? connectionString = null)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                var config = new ConfigurationBuilder()
                    .AddJsonFile(Path.Combine(AppContext.BaseDirectory, "appsettings.json"), optional: false, reloadOnChange: true)
                    .Build();

                connectionString = config.GetConnectionString("DefaultConnection")
                    ?? throw new ArgumentNullException("Connection string missing in appsettings.json!");
            }

            _connection = new SqlConnection(connectionString);
            _connection.Open();
        }

        #region Connection & Transaction

        private void Open()
        {
            if (_connection.State != ConnectionState.Open)
                _connection.Open();
        }

        private async System.Threading.Tasks.Task OpenAsync()
        {
            if (_connection.State != ConnectionState.Open)
                await _connection.OpenAsync();
        }

        public void BeginTransaction()
        {
            Open();
            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            _transaction?.Commit();
            _transaction = null;
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            _transaction = null;
        }

        #endregion

        #region Command Builder

        private SqlCommand BuildCommand(
            string sql,
            Dictionary<string, object>? parameters = null,
            CommandType commandType = CommandType.Text,
            int timeout = 30,
            bool useTransaction = false
        )
        {
            var cmd = new SqlCommand(sql, _connection)
            {
                CommandType = commandType,
                CommandTimeout = timeout,
                Transaction = useTransaction ? _transaction : null
            };

            if (parameters != null)
            {
                foreach (var p in parameters)
                    cmd.Parameters.AddWithValue(p.Key, p.Value ?? DBNull.Value);
            }

            return cmd;
        }

        #endregion

        #region ExecuteScalar

        public string ExecuteScalar(
            string sql,
            Dictionary<string, object>? parameters = null,
            CommandType commandType = CommandType.Text
        )
        {
            Open();
            using var cmd = BuildCommand(sql, parameters, commandType);
            return cmd.ExecuteScalar()?.ToString() ?? string.Empty;
        }

        public async System.Threading.Tasks.Task<string> ExecuteScalarAsync(
            string sql,
            Dictionary<string, object>? parameters = null,
            CommandType commandType = CommandType.Text
        )
        {
            await OpenAsync();
            using var cmd = BuildCommand(sql, parameters, commandType);
            return (await cmd.ExecuteScalarAsync())?.ToString() ?? string.Empty;
        }

        #endregion

        #region ExecuteCommand

        public int ExecuteCommand(
            string sql,
            Dictionary<string, object>? parameters = null,
            CommandType commandType = CommandType.Text,
            bool useTransaction = false
        )
        {
            Open();
            using var cmd = BuildCommand(sql, parameters, commandType, useTransaction: useTransaction);
            return cmd.ExecuteNonQuery();
        }

        public async System.Threading.Tasks.Task<int> ExecuteCommandAsync(
            string sql,
            Dictionary<string, object>? parameters = null,
            CommandType commandType = CommandType.Text,
            bool useTransaction = false
        )
        {
            await OpenAsync();
            using var cmd = BuildCommand(sql, parameters, commandType, useTransaction: useTransaction);
            return await cmd.ExecuteNonQueryAsync();
        }

        #endregion



        #region ExecuteToDataTable

        public DataTable ExecuteToDataTable(
            string sql,
            Dictionary<string, object>? parameters = null,
            CommandType commandType = CommandType.Text,
            bool useTransaction = false
        )
        {
            Open();
            using var cmd = BuildCommand(sql, parameters, commandType, useTransaction: useTransaction);
            using var da = new SqlDataAdapter(cmd);

            var dt = new DataTable();
            da.Fill(dt);
            return dt;
        }


        public async System.Threading.Tasks.Task<DataTable> ExecuteToDataTableAsync(
            string sql,
            Dictionary<string, object>? parameters = null,
            CommandType commandType = CommandType.Text,
            bool useTransaction = false
        )
        {
            await OpenAsync();
            using var cmd = BuildCommand(sql, parameters, commandType, useTransaction: useTransaction);
            using var da = new SqlDataAdapter(cmd);

            var dt = new DataTable();
            await System.Threading.Tasks.Task.Run(() => da.Fill(dt));
            return dt;
        }


        #endregion

        #region ExecuteToDataSet

        public List<DataTable> ExecuteToDataSet(
            string sql,
            Dictionary<string, object>? parameters = null,
            CommandType commandType = CommandType.Text
        )
        {
            Open();
            using var cmd = BuildCommand(sql, parameters, commandType);
            using var da = new SqlDataAdapter(cmd);

            var ds = new DataSet();
            da.Fill(ds);

            return ds.Tables.Cast<DataTable>().ToList();
        }

        #endregion

        #region Paging Helper

        public DataTable ExecutePaged(
            string baseQuery,
            int page,
            int pageSize,
            Dictionary<string, object>? parameters = null
        )
        {
            parameters ??= new Dictionary<string, object>();
            parameters["@Offset"] = (page - 1) * pageSize;
            parameters["@PageSize"] = pageSize;

            string sql = $@"
                {baseQuery}
                OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY
            ";

            return ExecuteToDataTable(sql, parameters);
        }

        #endregion

        #region Bulk Insert

        public void BulkInsert(string tableName, DataTable data)
        {
            Open();
            using var bulk = new SqlBulkCopy(_connection)
            {
                DestinationTableName = tableName
            };
            bulk.WriteToServer(data);
        }

        #endregion

        public List<T> ToList<T>(DataTable dt) where T : new()
        {
            var list = new List<T>();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (DataRow row in dt.Rows)
            {
                var obj = new T();
                foreach (var prop in props)
                {
                    if (!dt.Columns.Contains(prop.Name) || row[prop.Name] == DBNull.Value)
                        continue;

                    prop.SetValue(obj, Convert.ChangeType(row[prop.Name], prop.PropertyType));
                }
                list.Add(obj);
            }

            return list;
        }

        public List<T> ConvertDataTableToList<T>(DataTable dt) where T : new()
        {
            var list = new List<T>();
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (DataRow row in dt.Rows)
            {
                var obj = new T();
                foreach (var prop in props)
                {
                    if (!dt.Columns.Contains(prop.Name) || row[prop.Name] == DBNull.Value)
                        continue;

                    try
                    {
                        var value = row[prop.Name];
                        if (value != DBNull.Value)
                        {
                            prop.SetValue(obj, Convert.ChangeType(value, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType));
                        }
                    }
                    catch
                    {
                        // Skip properties that can't be converted
                        continue;
                    }
                }
                list.Add(obj);
            }

            return list;
        }




        #region Dispose

        public void Dispose()
        {
            _transaction?.Dispose();
            _connection.Dispose();
        }

        #endregion
    }
}
