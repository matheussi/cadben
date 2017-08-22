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

        /*********************************************/

        public EstipulanteTaxa SalvarTaxa(EstipulanteTaxa taxa)
        {
            using (ISession sessao = ObterSessao())
            {
                using (ITransaction tran = sessao.BeginTransaction())
                {
                    sessao.SaveOrUpdate(taxa);
                    tran.Commit();
                }
            }

            return taxa;
        }

        public void ExcluirTaxa(long taxaId)
        {
            using (ISession sessao = ObterSessao())
            {
                using (ITransaction tran = sessao.BeginTransaction())
                {
                    var taxa = sessao.Get<EstipulanteTaxa>(taxaId);
                    sessao.Delete(taxa);
                    tran.Commit();
                }
            }
        }

        public EstipulanteTaxa CarregarTaxa(long id, long? contratanteId = null)
        {
            EstipulanteTaxa ret = null;

            using (ISession sessao = ObterSessao())
            {
                ret = sessao.Query<EstipulanteTaxa>()
                .Where(c => c.ID == id).SingleOrDefault();

                if (contratanteId.HasValue && contratanteId.Value > 0)
                {
                    var estipulante = sessao.Query<Estipulante>()
                    .Where(c => c.ID == ret.EstipulanteId && c.ContratanteId == contratanteId.Value).SingleOrDefault();

                    if(estipulante == null)
                    {
                        throw new ApplicationException("Security exception.");
                    }
                }
            }

            return ret;
        }

        public List<EstipulanteTaxa> CarregarTaxas(long estipulanteId, long? contratanteId)
        {
            using (ISession sessao = ObterSessao())
            {
                if (contratanteId.HasValue)
                {
                    var estipulante = sessao.Query<Estipulante>()
                    .Where(c => c.ID == estipulanteId && c.ContratanteId == contratanteId.Value).SingleOrDefault();

                    if (estipulante == null)
                    {
                        throw new ApplicationException("Security exception.");
                    }
                }

                return sessao.Query<EstipulanteTaxa>()
                    .Where(t => t.EstipulanteId == estipulanteId)
                    .OrderByDescending(t => t.Vigencia)
                    .Take(20)
                    .ToList();
            }
        }

        public bool ValidarVigenciaTaxa(long? taxaId, long estipulanteId, DateTime vigencia)
        {
            bool ok = true;

            using (ISession sessao = ObterSessao())
            {
                if(taxaId.HasValue)
                {
                    var ret = sessao.Query<EstipulanteTaxa>()
                        .Where(t => t.ID != taxaId && 
                                    t.EstipulanteId == estipulanteId && 
                                    t.Vigencia.Day == vigencia.Day && 
                                    t.Vigencia.Month == vigencia.Month && 
                                    t.Vigencia.Year == vigencia.Year)

                        .FirstOrDefault();

                    if (ret != null) ok = false;
                }
                else
                {
                    var ret = sessao.Query<EstipulanteTaxa>()
                        .Where(t => t.EstipulanteId == estipulanteId && 
                                    t.Vigencia.Day == vigencia.Day && 
                                    t.Vigencia.Month == vigencia.Month && 
                                    t.Vigencia.Year == vigencia.Year)

                        .FirstOrDefault();

                    if (ret != null) ok = false;
                }
            }

            return ok;
        }
    }
}
