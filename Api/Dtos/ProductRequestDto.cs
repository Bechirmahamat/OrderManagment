using System.ComponentModel.DataAnnotations;

namespace Api.Dtos
{

    public class CreateProductRequest
    {
        [Required, Length(minimumLength: 3, maximumLength: 200)]
        public string Name { get; set; } // ✅ Property

        [Required, Length(minimumLength: 3, maximumLength: 200)]
        public string Description { get; set; } // ✅ Property

        [Required]
        public double Price { get; set; } // ✅ Property

        [Required, Range(1, int.MaxValue)] // Use [Range] for numbers, not [Length]
        public int StockCount { get; set; } // ✅ Property
        [Required]
        public Guid CategoryId { get; set; } // ✅ Property
    }
}
