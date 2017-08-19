namespace cadben.Facade
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    using NHibernate;
    using NHibernate.Linq;
    using cadben.Entidades;

    public class OperadoraFacade : FacadeBase
    {
        #region Singleton  

        static OperadoraFacade _instancia;
        public static OperadoraFacade Instancia
        {
            get
            {
                if (_instancia == null) { _instancia = new OperadoraFacade(); }
                return _instancia;
            }
        }
        #endregion

        private OperadoraFacade() { }

        public Operadora Salvar(Operadora operadora)
        {
            using (ISession sessao = ObterSessao())
            {
                using (ITransaction tran = sessao.BeginTransaction())
                {
                    sessao.SaveOrUpdate(operadora);
                    tran.Commit();
                }
            }

            return operadora;
        }

        public Operadora Carregar(long id, long? contratanteId = null)
        {
            Operadora ret = null;

            using (ISession sessao = ObterSessao())
            {
                if (!contratanteId.HasValue || contratanteId.Value == 0)
                {
                    ret = sessao.Query<Operadora>()
                    .Where(c => c.ID == id).SingleOrDefault();
                }
                else
                {
                    ret = sessao.Query<Operadora>()
                    .Where(c => c.ID == id && c.ContratanteId == contratanteId.Value).SingleOrDefault();
                }
            }

            return ret;
        }

        public List<Operadora> CarregarTodas(long contratanteId, string nome = null)
        {
            using (ISession sessao = ObterSessao())
            {
                if (string.IsNullOrEmpty(nome))
                {
                    var ret = sessao.Query<Operadora>()
                        .Where(o => o.ContratanteId == contratanteId)
                        .OrderBy(o => o.Nome).ToList();

                    return ret;
                }
                else
                {
                    var ret = sessao.Query<Operadora>()
                        .Where(o => o.Nome.Contains(nome) && o.ContratanteId == contratanteId)
                        .OrderBy(o => o.Nome).ToList();

                    return ret;
                }
            }
        }
    }
}
