using System.Text.Json.Serialization;
using ShopApp.Core.Entities.Abstract;

namespace ShopApp.DataAccess.Entities
{
    public class Product:IEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; }= DateTime.Now;
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string image1 { get; set; }
        public string image2 { get; set; }= string.Empty;
        public string image3 { get; set; }=string.Empty;
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
