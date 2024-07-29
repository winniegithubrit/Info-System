namespace InfoSystem.Models
{
    public class Product
    {
        public int Id { get; set; }               
        public string? ProductName { get; set; }  
        public string? Category { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }   
        public string? Supplier { get; set; }      
        public string? Description { get; set; }   
    }
}
