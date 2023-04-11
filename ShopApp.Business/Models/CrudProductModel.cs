namespace ShopApp.Business.Models
{
    public class CrudProductModel
    {
       
        public string Name { get; set; }
        public DateTime CreatedDate { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; } = string.Empty;
        public string image3 { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        
    }
}
