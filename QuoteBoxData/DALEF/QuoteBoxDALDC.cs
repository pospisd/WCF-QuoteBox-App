using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace QuoteBoxData.DALEF
{
    class QuoteBoxDALDC
    {
        /// <summary>
        /// Models individual tables in QuoteBox, namely "Authors" and "Quotes".
        /// </summary>
        /// 

        //----------------------------------------------------------------------------
        // Disconnected Layer - Data Adapter.
        //
        // Data Adapter objects function as a bridge between the client tier and a 
        // relational database. Using these objects, you can obtain DataSet objects,
        // manipulate their contents, and send the modified rows back for processing. 
        // The end result is a highly scalable data-centric .NET application.
        //
        // Notice the connection to the database does not need to be explicitly opened 
        // or closed. Also, DataSets were designed for simple forms over data business 
        // applications.
        //
        // If your DataTable maps to or is generated from a single database table, you 
        // can take advantage of the DbCommandBuilder object to automatically 
        // generate the DeleteCommand, InsertCommand, and UpdateCommand objects for the 
        // DataAdapter.
        //
        //----------------------------------------------------------------------------
        private string ConnectionString;
        private SqlDataAdapter Adapter = null;

        /// <summary>
        /// Public constructor.
        /// </summary>
        /// <param name="queryString"></param>
        /// <param name="connectionString"></param>
        public QuoteBoxDALDC(string queryString, string connectionString)
        {
            this.ConnectionString = connectionString;
            ConfigureAdapter(queryString, out this.Adapter);
        }

        /// <summary>
        /// Assign the sql commands to the adapter. (Takes advantage of SqlCommandBuilder.)
        /// Best to provision each instance of QuoteBoxDALDC to query one table only.
        /// </summary>
        /// <param name="sqlStatement"></param>
        /// <param name="adapter"></param>
        private void ConfigureAdapter(string sqlStatement, out SqlDataAdapter adapter)
        {
            // Create the adapter and set up the SelectCommand.
            adapter = new SqlDataAdapter(sqlStatement, ConnectionString);

            // Obtain the remaining command objects at runtime using the SqlCommandBuilder.
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
        }

        /// <summary>
        /// Adds or refreshes rows in the DataSet to match those in the datasource.
        /// (With option to use FillSchema.)
        /// </summary>
        /// <param name="addSchema"></param>
        /// <param name="ds"></param>
        /// <param name="tableName"></param>
        public void FillAdapter(bool addSchema, ref DataSet ds, string tableName)
        {
            // Byt default the table schema is not copied across with Fill method.
            if (addSchema.Equals(true))
            {
                Adapter.FillSchema(ds, SchemaType.Source, tableName);
            }
            Adapter.Fill(ds, tableName);
        }

        /// <summary>
        /// The correct command object is leveraged behind the scenes.
        /// </summary>
        /// <param name="dt"></param>
        public void UpdateAdapter(DataTable dt)
        {
            Adapter.Update(dt);
        }

        //----------------------------------------------------------------------------
        // SERIALIZE:
        //----------------------------------------------------------------------------
        /// <summary>
        /// Serialize DataTable/DataSet objects in binary format.
        /// </summary>
        /// <param name="quotesDS"></param>
        static void SaveAndLoadAsBinary(DataSet quotesDS)
        {
            // Set binay serialization flag.
            quotesDS.RemotingFormat = SerializationFormat.Binary;

            // Save this DataSet as binary.
            FileStream fs = new FileStream("BinaryQuotes.bin", FileMode.Create);
            BinaryFormatter bFormat = new BinaryFormatter();
            bFormat.Serialize(fs, quotesDS);
            fs.Close();

            // Clear out DataSet.
            quotesDS.Clear();

            // Load DataSet from binary file.
            fs = new FileStream("BinaryQuotes.bin", FileMode.Open);
            DataSet data = (DataSet)bFormat.Deserialize(fs);
        }
    }
}
