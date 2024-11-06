using AGECorporate.Models;

namespace AGECorporate.View_Models
{
    public class ProductCategoryDisplayViewModel
    {
        public ProductCategory? productCategory { get; set; }
        public List<Product>? products { get; set; }
    }
}
