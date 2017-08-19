namespace cadben.Facade
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    using NHibernate;
    using NHibernate.Linq;
    using cadben.Entidades;

    public class EstipulanteFacade : FacadeBase
    {
        #region Singleton  

        static EstipulanteFacade _instancia;
        public static EstipulanteFacade Instancia
        {
            get
            {
                if (_instancia == null) { _instancia = new EstipulanteFacade(); }
                return _instancia;
            }
        }
        #endregion

        private EstipulanteFacade() { }

        public Estipulante Salvar(Estipulante estipulante)
        {
            using (ISession sessao = ObterSessao())
            {
                using (ITransaction tran = sessao.BeginTransaction())
                {
                    sessao.SaveOrUpdate(estipulante);
                    tran.Commit();
                }
            }

            return estipulante;
        }

        public Estipulante Carregar(long id, long? contratanteId = null)
        {
            Estipulante ret = null;

            using (ISession sessao = ObterSessao())
            {
                if (!contratanteId.HasValue || contratanteId.Value == 0)
                {
                    ret = sessao.Query<Estipulante>()
                    .Where(c => c.ID == id).SingleOrDefault();
                }
                else
                {
                    ret = sessao.Query<Estipulante>()
                    .Where(c => c.ID == id && c.ContratanteId == contratanteId.Value).SingleOrDefault();
                }
            }

            return ret;
        }

        public List<Estipulante> CarregarTodos(long contratanteId, string nome = null)
        {
            using (ISession sessao = ObterSessao())
            {
                if (string.IsNullOrEmpty(nome))
                {
                    var ret = sessao.Query<Estipulante>()
                        .Where(o => o.ContratanteId == contratanteId)
                        .OrderBy(o => o.Nome).ToList();

                    return ret;
                }
                else
                {
                    var ret = sessao.Query<Estipulante>()
                        .Where(o => o.Nome.Contains(nome) && o.ContratanteId == contratanteId)
                        .OrderBy(o => o.Nome).ToList();

                    return ret;
                }
            }
        }
    }
}
