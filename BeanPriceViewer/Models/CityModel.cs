using System.ComponentModel.DataAnnotations;

namespace BeanPriceViewer.Models
{
    public class City
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int? Temperature { get; set; }
        public int? Humidity { get; set; }
        public int? BluePrice { get; set; }
        public int? RedPrice { get; set; }
        public int? GreenPrice { get; set; }
        public int? YellowPrice { get; set; }
    }
}