using Model.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Models
{
    public class SaleItem : Auditable
    {
        [Key,Column(Order =1)]
        public Guid Id { get; set; }

        [Key,Column(Order = 2)]
        public Guid SaleId { get; set; }

        public string Name { get; set; }
    }
}
