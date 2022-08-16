using System;
using Globo.PIC.Domain.Entities;
using Globo.PIC.Domain.Interfaces;
using System.Linq;
using System.Data;
using MediatR;
using System.Threading.Tasks;
using System.Threading;
using Globo.PIC.Domain.Types.Commands;
using System.Collections.Generic;
using Globo.PIC.Domain.Types.Events;

namespace Globo.PIC.Application.Services.Commands
{

    /// <summary>
    /// 
    /// </summary>
    public class UsuarioCommandHandler :
        IRequestHandler<UpdateUsuario>,
        IRequestHandler<CreateUsuario>
    {

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<UserRole> userRoleRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<Usuario> usuarioRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<Departamento> departamentoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<UnidadeNegocio> unidadeNegocioRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<Conteudo> conteudoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IRepository<UsuarioConteudo> usuarioConteudoRepository;

        /// <summary>
        /// 
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// 
        /// </summary>
        private readonly IMediator mediator;

        /// <summary>
        /// 
        /// </summary>  
        public UsuarioCommandHandler(
            IUnitOfWork _unitOfWork,
            IRepository<UserRole> _userRoleRepository,
            IRepository<Usuario> _userRepository,
            IRepository<Departamento> _departamentoRepository,
            IRepository<UnidadeNegocio> _unidadeNegocioRepository,
            IRepository<Conteudo> _conteudoRepository,
            IRepository<UsuarioConteudo> _usuarioConteudoRepository,
            IMediator _mediator)
        {
            unitOfWork = _unitOfWork;
            userRoleRepository = _userRoleRepository;
            usuarioRepository = _userRepository;
            departamentoRepository = _departamentoRepository;
            unidadeNegocioRepository = _unidadeNegocioRepository;
            conteudoRepository = _conteudoRepository;
            usuarioConteudoRepository = _usuarioConteudoRepository;
            mediator = _mediator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<Unit> IRequestHandler<CreateUsuario, Unit>.Handle(CreateUsuario request, CancellationToken cancellationToken)
        {
            var userCreate = await usuarioRepository.GetByLogin(request.Usuario.Login, cancellationToken);

            if (userCreate != null)
                throw new ApplicationException("Login já cadastrado!");

            var userEmail = usuarioRepository.GetAll().Where(a => a.Email.Equals(request.Usuario.Email)).FirstOrDefault();

            if (!String.IsNullOrEmpty(userEmail.Email))
                throw new ApplicationException("Email já cadastrado!");

            var rolesUsers = userRoleRepository.GetAll().Where(s => s.Login == request.Usuario.Login).ToList();

            if (rolesUsers.Count > 0)
            {
                userRoleRepository.Remove(rolesUsers);

                var resultRoles = unitOfWork.SaveChanges();

                if (!resultRoles) throw new ApplicationException("Erro removendo roles associadas ao Usuário.");
            }

            if (request.Usuario.IdUnidadeNegocio.HasValue)
            {
                var unidadeNegocio = await
                    unidadeNegocioRepository.GetById(request.Usuario.IdUnidadeNegocio.Value, cancellationToken);

                if (unidadeNegocio == null)
                    throw new ApplicationException("Unidade Negócio não encontrada!");

                request.Usuario.UnidadeNegocio = null;
            }

            if (request.Usuario.IdDepartamento > 0)
            {
                var departamento = await
                    departamentoRepository.GetById(request.Usuario.IdDepartamento.Value, cancellationToken);

                if (departamento == null)
                    throw new ApplicationException("Departamento não encontrado!");

                request.Usuario.Departamento = null;
            }

            if (request.Usuario.UsuariosConteudos != null && request.Usuario.UsuariosConteudos.Count() > 0)
            {
                var newItens = new List<UsuarioConteudo>();

                foreach (var item in request.Usuario.UsuariosConteudos)
                {
                    var conteudo = await conteudoRepository.GetById(item.IdConteudo, cancellationToken);

                    if (conteudo == null)
                        throw new ApplicationException($"O conteúdo de Id {item.IdConteudo} não foi encontrado!");

                    newItens.Add(new UsuarioConteudo() { IdConteudo = conteudo.Id });
                }

                request.Usuario.UsuariosConteudos = newItens;
            }

            usuarioRepository.AddOrUpdate(request.Usuario, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("Erro atualizando/adicionando Usuário.");

            await mediator.Publish<OnUsuarioCriado>(new OnUsuarioCriado() { Usuario = request.Usuario });

            return Unit.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        async Task<Unit> IRequestHandler<UpdateUsuario, Unit>.Handle(UpdateUsuario request, CancellationToken cancellationToken)
        {
            var userUpdate = await usuarioRepository.GetByLogin(request.Usuario.Login, cancellationToken);

            if(userUpdate == null)
                    throw new ApplicationException("Usuário não encontrado!");

            if (request.Usuario.IdUnidadeNegocio.HasValue)
            {
                var unidadeNegocio = await
                    unidadeNegocioRepository.GetById(request.Usuario.IdUnidadeNegocio.Value, cancellationToken);

                if (unidadeNegocio == null)
                    throw new ApplicationException("Unidade Negócio não encontrada!");

                userUpdate.IdUnidadeNegocio = request.Usuario.IdUnidadeNegocio.Value;
            }
            else
                userUpdate.IdUnidadeNegocio = null;

            if (request.Usuario.IdDepartamento.HasValue)
            {
                var departamento = await
                    departamentoRepository.GetById(request.Usuario.IdDepartamento.Value, cancellationToken);

                if (departamento == null)
                    throw new ApplicationException("Departamento não encontrado!");

                userUpdate.IdDepartamento = request.Usuario.IdDepartamento.Value;
            }
            else
                userUpdate.IdDepartamento = null;

            userUpdate.Roles =
                request.Usuario.Roles.Select(r =>
                new UserRole()
                {
                    User = request.Usuario,
                    Name = r.Name,
                    Login = request.Usuario.Login.ToLower()
                }
            ).ToList();

            if (request.Usuario.UsuariosConteudos != null && request.Usuario.UsuariosConteudos.Count() > 0)
            {
                usuarioConteudoRepository.Remove(userUpdate.UsuariosConteudos);

                unitOfWork.SaveChanges();

                var newItens = new List<UsuarioConteudo>();

                foreach (var item in request.Usuario.UsuariosConteudos)
                {
                    var conteudo = await conteudoRepository.GetById(item.IdConteudo, cancellationToken);

                    if (conteudo == null)
                        throw new ApplicationException($"O conteúdo de Id {item.IdConteudo} não foi encontrado!");

                    newItens.Add(new UsuarioConteudo() { IdConteudo = conteudo.Id });
                }

                userUpdate.UsuariosConteudos = newItens;
            }
            else
                userUpdate.UsuariosConteudos = new List<UsuarioConteudo>();

            usuarioRepository.AddOrUpdate(userUpdate, cancellationToken);

            var result = unitOfWork.SaveChanges();

            if (!result) throw new ApplicationException("Erro atualizando/adicionando Usuário.");

            return Unit.Value;
        }
    }
}
