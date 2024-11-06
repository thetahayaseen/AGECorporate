using AGECorporate.Models;

namespace AGECorporate.View_Models
{
    public class ProductFormViewModel
    {
        public Product? product {get; set;}
        public List<ProductCategory>? productCategories { get; set; }
    }
}
