using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.ViewModels
{
    public class AgreementItemsViewModel
    {
        [Description("Id")]
        public string Id { get; set; }
        [Description("Id do Item")]
        public string ItemId { get; set; }
        [Description("Descrição")]
        public string Description { get; set; }
        [Description("Numero da Linha")]
        public int LineNumber { get; set; }
        [Description("Data de Expiração")]
        public string ExpirationDate { get; set; }
        [Description("Valor")]
        public decimal Value { get; set; }
    }
}
