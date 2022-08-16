using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Globo.PIC.Domain.Entities
{

	/// <summary>
	/// 
	/// </summary>
	public class Usuario
	{

		/// <summary>
		/// 
		/// </summary>
		[Key]
		[Required]
		[Description("Login do Usuário")]
		public string Login { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Nome")]
		public string Name { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Apelido")]
		public string Apelido { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Sobrenome")]
		public string LastName { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("Email")]
		public string Email { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("IdDepartamento")]
		public long? IdDepartamento{ get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Description("IdDepartamento")]
		public long? IdUnidadeNegocio { get; set; }

		/// <summary>
		/// 
		/// </summary>
		[Required]
		[Description("Ativo")]
		public bool IsActive { get; set; }
		

		#region Relationship one to many properties

		public virtual IEnumerable<PedidoItemArteTracking> TrackingArte { get; set; }

		public virtual IEnumerable<PedidoItemVeiculoTracking> TrackingVeiculo { get; set; }

		public virtual IEnumerable<UserRole> Roles { get; set; }

		public virtual IEnumerable<Equipe> PedidoEquipe { get; set; }

		public virtual IEnumerable<Pedido> PedidosLoginCancelamento { get; set; }

		public virtual IEnumerable<Pedido> PedidosLoginCriadoPor { get; set; }

		public virtual IEnumerable<Pedido> PedidosLoginAtualizadoPor { get; set; }

		public virtual IEnumerable<Pedido> PedidosLoginDevolucao { get; set; }

		public virtual IEnumerable<PedidoArte> PedidosLoginArteBase { get; set; }

		public virtual IEnumerable<PedidoVeiculo> PedidosLoginVeiculoComprador { get; set; }

		public virtual IEnumerable<PedidoVeiculo> PedidosLoginVeiculoAcionamento { get; set; }

		public virtual IEnumerable<PedidoItemArte> PedidosItemArteLoginComprador { get; set; }

		public virtual IEnumerable<PedidoItem> PedidosItemLoginCancelamento { get; set; }

		public virtual IEnumerable<PedidoItem> PedidosItemLoginDevolucao { get; set; }

		public virtual IEnumerable<PedidoItem> PedidosItemLoginAprovacao { get; set; }
		
		public virtual IEnumerable<PedidoItem> PedidosItemLoginReprovacao { get; set; }

		public virtual IEnumerable<PedidoItemConversa> PedidoItemConversa { get; set; }

		public virtual IEnumerable<PedidoItemArteDevolucao> Devolucao { get; set; }

		public virtual IEnumerable<PedidoItemArteAtribuicao> CompradorAtribuicao { get; set; }

		public virtual IEnumerable<PedidoItemArteAtribuicao> CompradorAnteriorAtribuicao { get; set; }

		public virtual IEnumerable<PedidoItemArteCompra> Compras { get; set; }

		public virtual IEnumerable<PedidoItemArteCompraDocumento> Documentos { get; set; }

		public virtual IEnumerable<PedidoItemArteEntrega> Entregas { get; set; }

		public virtual IEnumerable<AcionamentoItem> AcionamentoItemAprovacao { get; set; }

		public virtual IEnumerable<AcionamentoItem> AcionamentoItemReprovacao { get; set; }

		public virtual IEnumerable<UsuarioConteudo> UsuariosConteudos { get; set; }

		#endregion


		#region Relationship many to one

		public virtual Departamento Departamento { get; set; }

		public virtual UnidadeNegocio UnidadeNegocio { get; set; }

		#endregion

		/// <summary>
		/// 
		/// </summary>
		public Usuario()
		{
		}
    }
}
