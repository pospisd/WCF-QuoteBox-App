using System.ComponentModel.DataAnnotations;

namespace QuoteBoxData.EF
{
    public partial class Quote
    {
        //public virtual ICollection<Quote> Quotes { get; set; } = new HashSet<Quote>();
        [Timestamp]
        public byte[] Timestamp { set; get; }

        /// <summary>
        /// A more readable output that includes the Author name...
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{this.Author.FirstName} " +
                    $"{this.Author.MiddleName ?? ""} " +
                    $"{this.Author.LastName} " +
                    $"quoted \"{this.QuoteTxt}\"";
        }
    }
}