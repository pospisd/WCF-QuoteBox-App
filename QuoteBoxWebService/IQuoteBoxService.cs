// IQuoteBoxService.cs
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.ServiceModel;
//using QuoteBoxDAL.EF;

namespace QuoteBoxWebService
{

    [ServiceContract]
    public interface IQuoteBoxService
    {

        [OperationContract]
        List<QuoteRecord> GetQuote(int id);
    }

    [DataContract]
    public class QuoteRecord
    {
        [DataMember]
        public int QuoteId;

        [DataMember]
        public int AuthorId;

        [DataMember]
        public string FirstName;

        [DataMember]
        public string MiddleName;

        [DataMember]
        public string LastName;

        [DataMember]
        public string AdditionalTxt;

        [DataMember]
        public DateTime AuthorCreatedDtm;

        [DataMember]
        public string QuoteTxt;

        [DataMember]
        public DateTime QuoteCreatedDtm;

        [DataMember]
        public Author Author;

    }
}