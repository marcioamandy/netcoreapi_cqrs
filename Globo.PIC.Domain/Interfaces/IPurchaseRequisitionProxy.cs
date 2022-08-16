using Globo.PIC.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Domain.Interfaces
{

    /// <summary>
    /// 
    /// </summary>
    public interface IPurchaseRequisitionProxy : IDisposable
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="body"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RequisitionHeader> PostRequisitionHeaderAsync(string body, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerId"></param>
        /// <param name="body"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<RequisitionLine> PostRequisitionLineAsync(string headerId, string body, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerId"></param>
        /// <param name="body"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DistributionLine> PostRequisitionDistributionWithHeaderAsync(string headerId, string body, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerId"></param>
        /// <param name="lineId"></param>
        /// <param name="body"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<DistributionLine> PostRequisitionDistributionWithLineAsync(string headerId, string lineId, string body, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="headerId"></param>
        /// <param name="body"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        Task<HeaderLine> PostRequisitionAsync(string headerId, string body, CancellationToken cancellationToken);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="purchaseRequisition"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
		Task<RequisicaoCompra> PostPurchaseRequisitionAsync(PurchaseRequisition purchaseRequisition, CancellationToken cancellationToken);
    }
}