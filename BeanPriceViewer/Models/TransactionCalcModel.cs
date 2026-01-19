using System.ComponentModel.DataAnnotations;

namespace BeanPriceViewer.Models
{
    public class CalculationData
    {
        public int? amount { get; set; }
        public string? BeanType { get; set; }
        public int CityId { get; set; }
        public int Id { get; set; }
        public int? Cash { get; set; }
        public int? Turn { get; set; }
        public int? MaxTurns { get; set; }
        public int? BlueStock { get; set; }
        public int? RedStock { get; set; }
        public int? GreenStock { get; set; }
        public int? YellowStock { get; set; }
    }
}