using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{
    public class Categoria
    {

        /// <summary>
        /// 
        /// </summary>
        [Key]
        [Range(1, long.MaxValue)]
        [Description("Id")]
        public long Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Id Categoria Pai")]
        [Range(1, long.MaxValue)]
        public long? IdCategoria { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [Description("Nome da Categoria")]
        public string Nome { get; set; }

        #region Relationship one to many properties

        public virtual IEnumerable<PedidoItemVeiculo> PedidosVeiculosSubCategorias { get; set; }

        public virtual IEnumerable<PedidoItemVeiculo> PedidosVeiculosTipos { get; set; }

        public virtual IEnumerable<Item> ItemSubCategorias { get; set; }

        public virtual IEnumerable<Item> ItemTipos { get; set; }

        public virtual IEnumerable<Categoria> Categorias { get; set; }

        #endregion

        #region Relationship many to one properties

        public virtual Categoria CategoriaPai { get; set; }

        #endregion
        /// <summary>
        /// 
        /// </summary>  
        public Categoria()
        {
        }
    }
}
