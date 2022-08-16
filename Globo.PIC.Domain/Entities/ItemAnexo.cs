using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class ItemAnexo
    {

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Range(0, long.MaxValue)]
        [Description("Id")]
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id do Pedido Veiculos Item")]
        public long IdItem { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Nome do Arquivo")]
        public string NomeArquivo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        //[Required]
        [Description("Nome Original")]
        public string NomeOriginal { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Tipo do Arquivo")]
        public string Tipo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Data do Arquivo")]
        public DateTime? DataItemAnexo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public ItemAnexo()
        {
        }

        #region Relationship one to many properties

        
        #endregion

        #region Relationship many to one properties


        public virtual Item Item { get; set; }
        

        #endregion
    }

    //public class TipoPedido
    //{
    //    public string Id_Tipo { get; private set; }
    //    public String Nome { get; private set; }
    //}
}
