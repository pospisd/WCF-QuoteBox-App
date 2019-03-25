using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace QuoteBoxData.DALC
{

    /// <summary>
    /// Connected Layer - Utilises the DataReader.
    /// 
    /// Data readers represent a read-only, forward-only stream of data returned one record at a time.
    /// Useful when you need to iterate over large amounts of data quickly and you do not need to maintain
    /// an in-memory representation.
    /// 
    /// The connected Layer of ADO.NET allows you to interact with the database using the connection, command
    /// and data reader objects of your data provider.
    /// </summary>
    /// 
    public class QuoteBoxDALC
    {
        private SqlConnection _sqlConnection = null;
    
        public void OpenConnection(string connectionString)
        {
            _sqlConnection = new SqlConnection { ConnectionString = connectionString };
            _sqlConnection.Open();
        }
    
        public void CloseConnection()
        {
            _sqlConnection.Close();
        }
    
        /*********************************************************************
         * 
         * CREATE:
         * 
         *********************************************************************/
    
        /// <summary>
        /// Create new messages. 
        /// </summary>
        /// <param name="message"></param>
        public void CreateMessage(string message)
        {
            string sql = "INSERT INTO Messages" +
                   $"(MessageTxt, CreatedDtm) VALUES (@Message, '{(DateTime.Now).ToString("yyyy-MM-dd HH:mm:ss.fff")}')";
    
            // Execute using our connection.
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                // Fill params collection.
                SqlParameter parameter = new SqlParameter
                {
                    ParameterName = "@Message",
                    Value = message,
                    SqlDbType = SqlDbType.NVarChar
                };
                command.Parameters.Add(parameter);
    
                command.ExecuteNonQuery();
            }
        }
    
    
        /*********************************************************************
         * 
         * READ:
         * 
         *********************************************************************/
    
        /// <summary>
        /// Read one message.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string ReadMessage(int id)
        {
            string message;
    
            // Establish name of stored procedure.
            using (SqlCommand command = new SqlCommand("udsp_ReadMessage", _sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
    
                // Input param.
                SqlParameter param = new SqlParameter
                {
                    ParameterName = "@MessageId",
                    SqlDbType = SqlDbType.Int,
                    Value = id,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(param);
    
                // Output param.
                param = new SqlParameter
                {
                    ParameterName = "@MessageTxt",
                    SqlDbType = SqlDbType.VarChar,
                    Size = 4000,
                    Direction = ParameterDirection.Output
                };
                command.Parameters.Add(param);
    
                // Execute the stored proc.
                command.ExecuteNonQuery();
    
                // Return output param.
                message = (string)command.Parameters["@MessageTxt"].Value;
    
            }
    
            return message;
        }
    
        /// <summary>
        /// ReadQuote.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DataTable ReadQuote(int id)
        {
            // Create DataTable to hold values.
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("QuoteId", typeof(int));
            dataTable.Columns.Add("AuthorId", typeof(int));
            dataTable.Columns.Add("FirstName", typeof(string));
            dataTable.Columns.Add("MiddleName", typeof(string));
            dataTable.Columns.Add("LastName", typeof(string));
            dataTable.Columns.Add("AdditionalTxt", typeof(string));
            dataTable.Columns.Add("AuthorCreatedDtm", typeof(DateTime));
            dataTable.Columns.Add("QuoteTxt", typeof(string));
            dataTable.Columns.Add("QuoteCreatedDtm", typeof(DateTime));
    
            // Establish name of stored procedure.
            using (SqlCommand command = new SqlCommand("udsp_ReadQuote", _sqlConnection))
            {
                command.CommandType = CommandType.StoredProcedure;
    
                // Input param.
                SqlParameter param = new SqlParameter
                {
                    ParameterName = "@QuoteId",
                    SqlDbType = SqlDbType.Int,
                    Value = id,
                    Direction = ParameterDirection.Input
                };
                command.Parameters.Add(param);
    
                SqlDataReader reader = command.ExecuteReader();
    
                dataTable.Load(reader);
    
            }
    
            return dataTable;
        }
    
        public List<string> ReadAllMessages()
        {
            // This will hold the records.
            List<string> messageList = new List<string>();
    
            // Prep command object.
            string sql = "SELECT MessageTxt FROM Messages";
    
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                SqlDataReader dataReader = command.ExecuteReader();
                while (dataReader.Read())
                {
                    messageList.Add((string)dataReader["MessageTxt"]);
                }
                dataReader.Close();
            }
    
            return messageList;
        }
    
    
        /*********************************************************************
         * 
         * UPDATE:
         * 
         *********************************************************************/
    
        /// <summary>
        /// Update message text.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newText"></param>
        public void UpdateMessage(int id, string newText)
        {
            // Update the text of the message with the specified MessageId.
            string sql = $"UPDATE Messages SET MessageTxt = '{newText}' WHERE MessageId = {id}";
    
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                command.ExecuteNonQuery();
            }
        }
    
    
        /*********************************************************************
         * 
         * DELETE:
         * 
         *********************************************************************/
    
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void DeleteMessage(int id)
        {
            // Delete the message with the specified id.
            string sql = $"DELETE FROM Messages WHERE MessageId = {id}";
    
            using (SqlCommand command = new SqlCommand(sql, _sqlConnection))
            {
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    Exception error = new Exception("Sorry, encountered a Sql exception!", ex);
                    throw error;
                }
            }
        }
    
    }
}