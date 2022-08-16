using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Globo.PIC.Domain.Enums;
using Globo.PIC.Domain.Extensions;

namespace Globo.PIC.Domain.ViewModels
{
    public class ItemViewModel
    {

        /// <summary>
        /// 
        /// </summary>
        [Range(0, long.MaxValue)]
        [Description("Id")]
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(0, long.MaxValue)]
        public long IdTipo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id da SubCategoria")]
        [Range(0, long.MaxValue)]
        public long IdSubCategoria { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [DefaultValue(0.00)]
        [Required]
        [Description("Valor do item")]
        public decimal Valor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Nome do Fornecedor")]
        [Required]
        [MaxLength(200)]
        public string Fornecedor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("CNPJ do Fornecedor")]
        [Required]
        [MaxLength(14)]
        public string CodigoFornecedor { get; set; }       

        /// <summary>
        /// 
        /// </summary>
        [Description("Nome do Item")]
        [MaxLength(200)]
        public string NomeItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Descricao do Item")]
        [MaxLength(500)]
        public string Descricao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Unidade de medida para o Item")]
        [MaxLength(20)]
        public string UnidadeMedida { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Nro Acordo")]
        [MaxLength(100)]
        public string NroAcordo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Código do item")]
        [MaxLength(100)]
        public string CodItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Range(0, long.MaxValue)]
        [Description("Linha Acordo")]
        public long LinhaAcordo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Observações do Item")]
        [MaxLength(500)]
        public string Observacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Acordo Jurídico")]
        [MaxLength(100)]
        public string AcordoJuridico { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tipo de Negociação")]
        public string TipoNegociacao { get { return ((TipoNegociacao)IdTipo).GetEnumDescription(); } }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tipo de Negociação")]
        [Range(1, long.MaxValue)]
        public long IdTipoNegociacao { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Lista PedidoAnexosViewModel")]
        public List<ArquivoViewModel> Arquivos { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tipo")]
        public CategoriaViewModel Tipo { get; set; }


        /// <summary>
        /// 
        /// </summary>
        [Description("SubCategoria")]
        public CategoriaViewModel SubCategoria { get; set; }

    }
}
