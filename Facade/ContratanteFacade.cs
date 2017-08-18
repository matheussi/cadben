namespace cadben.Facade
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    using NHibernate;
    using NHibernate.Linq;
    using cadben.Entidades;

    public class ContratanteFacade : FacadeBase
    {
        #region Singleton 

        static ContratanteFacade _instancia;
        public static ContratanteFacade Instancia
        {
            get
            {
                if (_instancia == null) { _instancia = new ContratanteFacade(); }
                return _instancia;
            }
        }
        #endregion

        private ContratanteFacade() { }

        public Contratante Salvar(Contratante contratante)
        {
            using (ISession sessao = ObterSessao())
            {
                using (ITransaction tran = sessao.BeginTransaction())
                {
                    sessao.SaveOrUpdate(contratante);
                    tran.Commit();
                }
            }

            return contratante;
        }

        public Contratante Carregar(long id)
        {
            Contratante ret = null;

            using (ISession sessao = ObterSessao())
            {
                ret = sessao.Query<Contratante>()
                    .Where(c => c.Id == id).SingleOrDefault();
            }

            return ret;
        }

        public Contratante CarregarPorAlias(string alias)
        {
            Contratante ret = null;

            using (ISession sessao = ObterSessao())
            {
                ret = sessao.Query<Contratante>()
                    .Where(c => c.Alias == alias).SingleOrDefault();
            }

            return ret;
        }

        public List<Contratante> CarregarTodos(string nome = null)
        {
            using (ISession sessao = ObterSessao())
            {
                if (string.IsNullOrEmpty(nome))
                {
                    var ret = sessao.Query<Contratante>().OrderBy(c => c.Nome).ToList();

                    return ret;
                }
                else
                {
                    var ret = sessao.Query<Contratante>()
                        .Where(c => c.Nome.Contains(nome))
                        .OrderBy(c => c.Nome).ToList();

                    return ret;
                }
            }
        }
    }
}
