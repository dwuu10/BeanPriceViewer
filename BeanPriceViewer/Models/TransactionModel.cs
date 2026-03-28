using System.ComponentModel.DataAnnotations;

namespace BeanPriceViewer.Models
{
    public class TransactionData
    {
        public int? Amount { get; set; }
        public int? Type { get; set; }
    }
}