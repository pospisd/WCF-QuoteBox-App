using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using QuoteBoxData.DALC;

namespace QuoteBoxTestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("***** QuoteBox Test Console *****\n");

            // Get connection string from App.config.
            string connectionString =
                ConfigurationManager.ConnectionStrings["QuoteBoxSqlProvider"].ConnectionString;

            bool userDone = false;
            string userCommand = "";

            // Create our QuoteBoxDAL obeject.
            QuoteBoxDALC quoteBoxDALC = new QuoteBoxDALC();
            quoteBoxDALC.OpenConnection(connectionString);

            // Keep asking for input until the user presses the [Q] key.
            try
            {
                ShowInstructions();
                do
                {
                    Console.WriteLine("\nPlease enter your command: ");
                    userCommand = Console.ReadLine();
                    Console.WriteLine();

                    switch (userCommand?.ToUpper() ?? "")
                    {
                        case "I":
                            InsertNewMessage(quoteBoxDALC);
                            break;

                        case "U":
                            UpdateMessage(quoteBoxDALC);
                            break;

                        case "D":
                            DeleteMessage(quoteBoxDALC);
                            break;

                        case "L":
                            ShowAllMessages(quoteBoxDALC);
                            break;

                        case "S":
                            ShowInstructions();
                            break;

                        case "M":
                            LookUpMessage(quoteBoxDALC);
                            break;

                        case "Q":
                            LookUpQuote(quoteBoxDALC);
                            break;

                        case "X":
                            userDone = true;
                            break;

                        default:
                            Console.WriteLine("Bad data! Try again.");
                            break;
                    }

                } while (!userDone);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                quoteBoxDALC.CloseConnection();
            }

            Console.ReadLine();
        }

        private static void ShowError(string objectName)
        {
            Console.WriteLine($"There is an issue creating the {objectName}");
            Console.ReadLine();
        }


        /*********************************************************************
         * 
         * USER INTERFACE METHODS:
         * 
         *********************************************************************/

        /// Show Instructions.
        /// <summary>
        /// </summary>
        private static void ShowInstructions()
        {
            Console.WriteLine("I: Inserts a new message");
            Console.WriteLine("U: Updates an existing  message");
            Console.WriteLine("D: Deletes a message");
            Console.WriteLine("L: Lists all the messages");
            Console.WriteLine("S: Shows these instructions");
            Console.WriteLine("M: Looks up a message");
            Console.WriteLine("Q: Looks up a quote");
            Console.WriteLine("X: Exit the program");
        }

        /// <summary>
        /// Show All Messages.
        /// </summary>
        /// <param name="messageDAL"></param>
        private static void ShowAllMessages(QuoteBoxDALC QuoteBoxDALC)
        {
            // Get the collection of messages.
            List<string> messageList = QuoteBoxDALC.ReadAllMessages();

            // Print out the messages.
            Console.WriteLine("\n------------------------------------------------");

            foreach (string message in messageList)
            {
                Console.WriteLine($"--> \"{message}\"");
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Delete Message.
        /// </summary>
        /// <param name="messageDAL"></param>
        private static void DeleteMessage(QuoteBoxDALC QuoteBoxDALC)
        {
            // Get Id of message to delete.
            Console.WriteLine("Enter ID of Message to delete: ");
            int messageId = int.Parse(Console.ReadLine() ?? "0");

            // Just in case you have a referential integrity violation!
            try
            {
                if (messageId > 0)
                {
                    Console.WriteLine("Deleting message...\n");
                    QuoteBoxDALC.DeleteMessage(messageId);
                }
                else
                    Console.WriteLine("Error, could not update message!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Insert New Message.
        /// </summary>
        /// <param name="messageDAL"></param>
        private static void InsertNewMessage(QuoteBoxDALC QuoteBoxDALC)
        {
            Console.WriteLine("Enter new message text: ");
            string newMessageTxt = Console.ReadLine() ?? "";

            // Now pass to data access lib.
            Console.WriteLine("Inserting new message...\n");
            QuoteBoxDALC.CreateMessage(newMessageTxt);
        }

        /// <summary>
        /// Update Message.
        /// </summary>
        /// <param name="messageDAL"></param>
        private static void UpdateMessage(QuoteBoxDALC QuoteBoxDALC)
        {
            // Get user data.
            Console.WriteLine("Enter message ID: ");
            int messageId = int.Parse(Console.ReadLine() ?? "0");

            Console.WriteLine("Enter new message: ");
            string newMessageTxt = Console.ReadLine();

            // Now pass to data access lib.
            if (messageId > 0)
            {
                Console.WriteLine("Updating message...\n");
                QuoteBoxDALC.UpdateMessage(messageId, newMessageTxt);
            }
            else
                Console.WriteLine("Error, could not update message!");
        }

        /// <summary>
        /// Look Up Message.
        /// </summary>
        /// <param name="messageDAL"></param>
        private static void LookUpMessage(QuoteBoxDALC QuoteBoxDALC)
        {
            // Get Id of message to look up.
            Console.WriteLine("Enter ID of message to look up: ");
            int messageId = int.Parse(Console.ReadLine() ?? "0");

            if (messageId > 0)
            {
                Console.WriteLine("Looking up message...");
                Console.WriteLine("\n------------------------------------------------");
                Console.WriteLine($"--> \"{QuoteBoxDALC.ReadMessage(messageId).TrimEnd()}\".");
            }
            else
                Console.WriteLine("Error, could not look up message!");

        }

        private static void LookUpQuote(QuoteBoxDALC QuoteBoxDALC)
        {
            // Get Id of message to look up.
            Console.WriteLine("Enter ID of quote to look up: ");
            int messageId = int.Parse(Console.ReadLine() ?? "1");

            if (messageId > 0)
            {
                Console.WriteLine("Looking up quote...\n");
                DataTable dataTable = QuoteBoxDALC.ReadQuote(messageId);

                if (dataTable.Rows.Count > 0)
                {
                    DataRow dr = dataTable.Rows[0];

                    Console.WriteLine("\n--------------------------------------------------------------------");
                    Console.WriteLine("\nLook Up Quote:");
                    Console.WriteLine("\n--------------------------------------------------------------------");
                    Console.WriteLine($"QuoteId: {dr["QuoteId"].ToString()}");
                    Console.WriteLine($"AuthorId: {dr["AuthorId"].ToString()}");
                    Console.WriteLine($"FirstName: {dr["FirstName"].ToString()}");
                    Console.WriteLine($"MiddleName: {dr["MiddleName"].ToString()}");
                    Console.WriteLine($"LastName: {dr["LastName"].ToString()}");
                    Console.WriteLine($"AdditionalTxt: {dr["AdditionalTxt"].ToString()}");
                    Console.WriteLine($"AuthorCreatedDtm: {dr["AuthorCreatedDtm"].ToString()}");
                    Console.WriteLine($"QuoteTxt: {dr["QuoteTxt"].ToString()}");
                    Console.WriteLine($"QuoteCreatedDtm: {dr["QuoteCreatedDtm"].ToString()}");
                    Console.WriteLine("\n--------------------------------------------------------------------\n");
                }
                else
                    Console.WriteLine("Error: No rows to process!");



            }
            else
                Console.WriteLine("Error, could not look up message!");
        }
    }
}
