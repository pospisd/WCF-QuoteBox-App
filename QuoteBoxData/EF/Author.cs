using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuoteBoxData.EF
{
    public partial class Author
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Author()
        {
            Quotes = new HashSet<Quote>();
        }

        public int AuthorId { get; set; }

        [Required]
        [StringLength(256)]
        public string FirstName { get; set; }

        [StringLength(256)]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(256)]
        public string LastName { get; set; }

        public string AdditionalTxt { get; set; }

        public DateTime CreatedDtm { get; set; }

        public int VersionNum { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Quote> Quotes { get; set; }
    }
}
