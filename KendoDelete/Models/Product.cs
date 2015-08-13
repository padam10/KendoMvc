


namespace KendoDelete.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public partial class Product
    {
        [DataType("Integer")]
        [Required]
        public int ID { get; set; }

        [DataType("String")]
        [Required]
        public string Name { get; set; }

        [DataType("Integer")]
        [Required]
        public string PerUnit { get; set; }

        public Nullable<decimal> Price { get; set; }
        public Nullable<short> Units { get; set; }

        [DataType("String")]
        [Required]
        public Nullable<short> Level { get; set; }
        public bool Status { get; set; }
    }
}
