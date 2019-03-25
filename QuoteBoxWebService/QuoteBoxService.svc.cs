using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using QuoteBoxData.DALC;

namespace QuoteBoxWebService
{
    // NOTE: In order to launch WCF Test Client for testing this service, please select MessageBoxService.svc or MessageBoxService.svc.cs 
    // at the Solution Explorer and start debugging.
    public class QuoteBoxService : IQuoteBoxService
    {
        private string _connectionString;


        // Ctor.
        public QuoteBoxService()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MessageBoxSqlProvider"].ConnectionString;
        }

        //Methods.
        List<QuoteRecord> IQuoteBoxService.GetQuote(int id)
        {
            // Create list.
            List<QuoteRecord> quoteRecordList = new List<QuoteRecord> { };

            if (id > 0)
            {
                // Get connection string from App.config.
                //_connectionString = ConfigurationManager.ConnectionStrings["MessageBoxSqlProvider"].ConnectionString;

                // Create our MessageBoxDAL obeject.
                QuoteBoxDALC quoteBoxDALC = new QuoteBoxDALC();
                try
                {
                    quoteBoxDALC.OpenConnection(_connectionString);

                    //Logfile.WriteLine("Looking up quote...\n");
                    DataTable dataTable = quoteBoxDALC.ReadQuote(id);

                    if (dataTable.Rows.Count > 0)
                    {
                        DataRow dr = dataTable.Rows[0];
                        QuoteRecord quoteRecord = new QuoteRecord
                        {
                            QuoteId = (int)dr["QuoteId"],
                            AuthorId = (int)dr["AuthorId"],
                            FirstName = dr["FirstName"].ToString(),
                            MiddleName = dr["MiddleName"].ToString(),
                            LastName = dr["LastName"].ToString(),
                            AdditionalTxt = dr["AdditionalTxt"].ToString(),
                            AuthorCreatedDtm = (DateTime)dr["AuthorCreatedDtm"],
                            QuoteTxt = dr["QuoteTxt"].ToString(),
                            QuoteCreatedDtm = (DateTime)dr["QuoteCreatedDtm"]
                        };

                        quoteRecordList.Add(quoteRecord);
                        return quoteRecordList;
                    }
                    else
                    {
                        //Logfile.WriteLine("Error: No rows to process!");
                        return quoteRecordList;
                    }
                }
                catch (Exception ex)
                {
                    // Logfile: Exception.
                    throw new Exception("Error: GetQuote encountered an unexpected exception!", ex);
                }
                finally
                {
                    quoteBoxDALC.CloseConnection();
                }
            }
            else
            {
                //Logfile.WriteLine("Error: No rows to process!");
                return quoteRecordList;
            }
        }
    }
}