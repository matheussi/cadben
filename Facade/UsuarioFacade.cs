namespace cadben.Facade
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    using NHibernate;
    using NHibernate.Linq;
    using cadben.Entity;
    using Ent = cadben.Entidades;

    public sealed class UsuarioFacade : FacadeBase
    {
        #region Singleton

        static UsuarioFacade _instancia;
        public static UsuarioFacade Instancia 
        {
            get
            {
                if (_instancia == null) { _instancia = new UsuarioFacade(); }
                return _instancia;
            }
        }
        #endregion

        private UsuarioFacade() { }

        public Usuario Salvar(Usuario usuario)
        {
            using (ISession sessao = ObterSessao())
            {
                using (ITransaction tran = sessao.BeginTransaction())
                {
                    sessao.SaveOrUpdate(usuario);
                    tran.Commit();
                }
            }

            return usuario;
        }

        public Ent.Usuario LogOn(string login, string senha, string contratanteAlias)
        {
            Ent.Usuario ret = null;

            using (ISession sessao = ObterSessao())
            {
                if (string.IsNullOrEmpty(contratanteAlias))
                {
                    ret = sessao.Query<Ent.Usuario>()
                        //.Fetch(u => u.Contratante)
                        //.Fetch(u => u.Unidade)
                        //.ThenFetch(un => un.Owner)
                        .Where(u => u.Login == login && u.Senha == senha && u.Ativo && u.Contratante == null)
                        .SingleOrDefault();
                }
                else
                {
                    var contr = sessao.Query<Contratante>()
                        .Where(c => c.Ativo == true && c.Alias == contratanteAlias)
                        .SingleOrDefault();

                    if (contr == null) return null;

                    ret = sessao.Query<Ent.Usuario>()
                        .Fetch(u => u.Contratante)
                        //.Fetch(u => u.Unidade)
                        //.ThenFetch(un => un.Owner)
                        .Where(u => u.Login == login && u.Senha == senha && u.Ativo && u.Contratante.Id == contr.Id)
                        .SingleOrDefault();
                }
            }

            return ret;
        }

        public Usuario Carregar(string login)
        {
            Usuario ret = null;

            using (ISession sessao = ObterSessao())
            {
                ret = sessao.Query<Usuario>()
                    //.Fetch(u => u.Unidade)
                    //.ThenFetch(un => un.Owner)
                    .Where(u => u.Login == login && u.Ativo).SingleOrDefault();
            }

            return ret;
        }

        public Usuario Carregar(long id)
        {
            Usuario ret = null;

            using (ISession sessao = ObterSessao())
            {
                ret = sessao.Query<Usuario>()
                    //.Fetch(u => u.Unidade)
                    //.ThenFetch(un => un.Owner)
                    .Where(u => Convert.ToInt64(u.ID) == id)
                    .Single();
            }

            return ret;
        }

        public List<Usuario> Carregar(TipoUsuario tipo, string nome)
        {
            List<Usuario> ret = null;

            //using (ISession sessao = ObterSessao())
            //{
            //    if (tipo == TipoUsuario.AdministradorContratante)
            //    {
            //        ret = sessao.Query<Usuario>()
            //            //.Fetch(u => u.Unidade)
            //            //.ThenFetch(un => un.Owner)
            //            .Where(u => u.Tipo == tipo && (u.Nome.Contains(nome) || u.Unidade.Nome.Contains(nome) || u.Unidade.Owner.Nome.Contains(nome)))
            //            .OrderBy(u => u.Unidade.Owner.Nome).OrderBy(u => u.Nome)
            //            .Take(200)
            //            .ToList();
            //    }
            //    else
            //    {
            //        ret = sessao.Query<Usuario>()
            //            //.Fetch(u => u.Unidade)
            //            //.ThenFetch(un => un.Owner)
            //            .Where(u => u.Tipo == tipo && u.Nome.Contains(nome))
            //            .Take(200)
            //            .ToList();
            //    }
            //}

            return ret;
        }

        /// <summary>
        /// True caso o login possa ser usado, False caso o login NÃO possa ser usado
        /// </summary>
        public bool VerificarLogin(long? id, string login)
        {
            Usuario ret = null;

            using (ISession sessao = ObterSessao())
            {
                if (id != null)
                {
                    ret = sessao.Query<Usuario>()
                        .Where(u => Convert.ToInt64(u.ID) != id.Value && u.Login == login)
                        .FirstOrDefault();
                }
                else
                {
                    ret = sessao.Query<Usuario>()
                        .Where(u => u.Login == login)
                        .FirstOrDefault();
                }
            }

            return ret == null;
        }

        public Usuario CarregarPorUnidade(long unidadeId)
        {
            Usuario ret = null;

            using (ISession sessao = ObterSessao())
            {
                ret = sessao.Query<Usuario>()
                    //.Fetch(u => u.Unidade)
                    //.Where(us => us.Unidade.ID == unidadeId)
                    .Where(u => u.FilialID == unidadeId)
                    .OrderBy(u => u.Nome)
                    .FirstOrDefault();
            }

            return ret;
        }
    }
}
