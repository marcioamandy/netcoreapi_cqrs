using Globo.PIC.Domain.Models;
using Globo.PIC.Domain.Types.Queries;
using Globo.PIC.Domain.Types.Queries.Filters;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Globo.PIC.Application.QueryHandlers
{
    public class CategoriaQueryHandler :

        IRequestHandler<GetByItensCategoriaGroupFilter, List<CategoriaCatalogoItem>>
    {
        Task<List<CategoriaCatalogoItem>> IRequestHandler<GetByItensCategoriaGroupFilter,
            List<CategoriaCatalogoItem>>.Handle(GetByItensCategoriaGroupFilter request, CancellationToken cancellationToken)
        {

            List<Arquivo> arquivos = new List<Arquivo>();
            arquivos.Add(new Arquivo
            {
                Id = 1,
                Name = "image name 1",
                OriginalName = "Original name 1",
                SentAt = DateTime.Now,
                Type = "none",
                Url = "https://staticmobly.akamaized.net/p/Mobly-SofC3A1-3-Lugares-Davos-Base-de-Madeira-Linho-Cotton-Cru-1579-509294-1-zoom.jpg"
            });
            arquivos.Add(new Arquivo
            {
                Id = 2,
                Name = "image name 2",
                OriginalName = "Original name 2",
                SentAt = DateTime.Now,
                Type = "none",
                Url = "https://staticmobly.akamaized.net/p/Mobly-SofC3A1-3-Lugares-Davos-Base-de-Madeira-Linho-Cotton-Azul-Marinho-3740-709294-1-zoom.jpg"
            });
            List<CategoriaCatalogoItem> categoriaCatalogoItem = new List<CategoriaCatalogoItem>();
             
            CatalogoItem catalogoItem1 = new CatalogoItem();
            catalogoItem1.Nome = "nome";
            catalogoItem1.Descricao = "descrição";
            catalogoItem1.FornecedorId = 1;
            catalogoItem1.FornecedorNome = "Fornecedor";
            catalogoItem1.UnitPrice = 123;
            catalogoItem1.Arquivos = arquivos;

            CatalogoItem catalogoItem2 = new CatalogoItem();
            catalogoItem2.Nome = "nome";
            catalogoItem2.Descricao = "descrição";
            catalogoItem2.FornecedorId = 1;
            catalogoItem2.FornecedorNome = "Fornecedor";
            catalogoItem2.UnitPrice = 123;
            catalogoItem2.Arquivos = arquivos;

            List<CatalogoItem> catalogos = new List<CatalogoItem>();
            catalogos.Add(catalogoItem1);
            catalogos.Add(catalogoItem2);

            categoriaCatalogoItem.Add(new CategoriaCatalogoItem
            {
                IdCategoria =1,
                NomeCategoria = "categoria 1",
                Itens = catalogos
            }) ;

            categoriaCatalogoItem.Add(new CategoriaCatalogoItem
            {
                IdCategoria = 2,
                NomeCategoria = "categoria 2",
                Itens = catalogos
            });


            return Task.FromResult(categoriaCatalogoItem); 
        }
    }
}
