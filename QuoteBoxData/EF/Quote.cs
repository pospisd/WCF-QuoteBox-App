using System;
using System.ComponentModel.DataAnnotations;

namespace QuoteBoxData.EF
{
    public partial class Quote
    {
        public int QuoteId { get; set; }

        public int AuthorId { get; set; }

        [Required]
        public string QuoteTxt { get; set; }

        public DateTime CreatedDtm { get; set; }

        public int VersionNum { get; set; }

        public virtual Author Author { get; set; }
    }
}
