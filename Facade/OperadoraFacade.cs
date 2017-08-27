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

        //CONTRATO ADM
        /*********************************************************************************/

        public ContratoADM SalvarContratoAdm(ContratoADM contrato)
        {
            using (ISession sessao = ObterSessao())
            {
                using (ITransaction tran = sessao.BeginTransaction())
                {
                    sessao.SaveOrUpdate(contrato);
                    tran.Commit();
                }
            }

            return contrato;
        }

        public void ExcluirContratoAdm(long contratoadmId)
        {
            using (ISession sessao = ObterSessao())
            {
                using (ITransaction tran = sessao.BeginTransaction())
                {
                    var contrato = sessao.Get<ContratoADM>(contratoadmId);
                    sessao.Delete(contrato);
                    tran.Commit();
                }
            }
        }

        public ContratoADM CarregarContratoAdm(long id, long? contratanteId = null)
        {
            ContratoADM ret = null;

            using (ISession sessao = ObterSessao())
            {
                ret = sessao.Query<ContratoADM>()
                    .Fetch(c => c.Operadora)
                    .Fetch(c => c.AssociadoPJ) //estipulante
                    .Where(c => c.ID == id).SingleOrDefault();

                if (contratanteId.HasValue && contratanteId.Value > 0)
                {
                    var operadora = sessao.Query<Operadora>()
                    .Where(o => o.ID == ret.Operadora.ID && o.ContratanteId == contratanteId.Value).SingleOrDefault();

                    if (operadora == null)
                    {
                        throw new ApplicationException("Security exception.");
                    }
                }
            }

            return ret;
        }

        public List<ContratoADM> CarregarContratosAdm(long operadoraId, long? contratanteId)
        {
            using (ISession sessao = ObterSessao())
            {
                if (contratanteId.HasValue)
                {
                    var operadora = sessao.Query<Operadora>()
                        .Where(o => o.ID == operadoraId && o.ContratanteId == contratanteId.Value).SingleOrDefault();

                    if (operadora == null)
                    {
                        throw new ApplicationException("Security exception.");
                    }
                }

                return sessao.Query<ContratoADM>()
                    .Fetch(c => c.AssociadoPJ)
                    .Where(c => c.Operadora.ID == operadoraId)
                    .OrderBy(c => c.Descricao)
                    .ToList();
            }
        }

        //ADICIONAIS
        /*********************************************************************************/

        public Adicional SalvarAdicional(Adicional adicional, IList<AdicionalFaixa> faixas)
        {
            using (ISession sessao = ObterSessao())
            {
                using (ITransaction tran = sessao.BeginTransaction())
                {
                    sessao.SaveOrUpdate(adicional);

                    if(faixas != null && faixas.Count > 0)
                    {
                        foreach(var faixa in faixas)
                        {
                            faixa.Adicional = adicional;
                            sessao.SaveOrUpdate(faixa);
                        }
                    }

                    tran.Commit();
                }
            }

            return adicional;
        }

        public void ExcluirAdicional(long adicionalId)
        {
            using (ISession sessao = ObterSessao())
            {
                using (ITransaction tran = sessao.BeginTransaction())
                {
                    var adicional = sessao.Get<Adicional>(adicionalId);
                    sessao.Delete(adicional);
                    tran.Commit();
                }
            }
        }

        public Adicional CarregarAdicional(long id, long? contratanteId = null)
        {
            Adicional ret = null;

            using (ISession sessao = ObterSessao())
            {
                ret = sessao.Query<Adicional>()
                    .Fetch(c => c.Operadora)
                    .Where(c => c.ID == id).SingleOrDefault();

                if (contratanteId.HasValue && contratanteId.Value > 0)
                {
                    var operadora = sessao.Query<Operadora>()
                    .Where(o => o.ID == ret.Operadora.ID && o.ContratanteId == contratanteId.Value).SingleOrDefault();

                    if (operadora == null)
                    {
                        throw new ApplicationException("Security exception.");
                    }
                }
            }

            return ret;
        }

        public List<Adicional> CarregarAdicionais(long operadoraId, long? contratanteId)
        {
            using (ISession sessao = ObterSessao())
            {
                if (contratanteId.HasValue)
                {
                    var operadora = sessao.Query<Operadora>()
                        .Where(o => o.ID == operadoraId && o.ContratanteId == contratanteId.Value).SingleOrDefault();

                    if (operadora == null)
                    {
                        throw new ApplicationException("Security exception.");
                    }
                }

                return sessao.Query<Adicional>()
                    .Where(c => c.Operadora.ID == operadoraId)
                    .OrderBy(c => c.Descricao)
                    .ToList();
            }
        }

        /********/

        public AdicionalFaixa SalvarAdicionalFaixa(AdicionalFaixa adicional)
        {
            using (ISession sessao = ObterSessao())
            {
                using (ITransaction tran = sessao.BeginTransaction())
                {
                    sessao.SaveOrUpdate(adicional);
                    tran.Commit();
                }
            }

            return adicional;
        }

        public void ExcluirAdicionalFaixa(long adicionalFaixaId)
        {
            using (ISession sessao = ObterSessao())
            {
                using (ITransaction tran = sessao.BeginTransaction())
                {
                    var adicional = sessao.Get<AdicionalFaixa>(adicionalFaixaId);
                    sessao.Delete(adicional);
                    tran.Commit();
                }
            }
        }

        public AdicionalFaixa CarregarAdicionalFaixa(long id, long? contratanteId = null)
        {
            AdicionalFaixa ret = null;

            using (ISession sessao = ObterSessao())
            {
                ret = sessao.Query<AdicionalFaixa>()
                    .Fetch(c => c.Adicional)
                    //.ThenFetch(a => a.Operadora)
                    .Where(c => c.ID == id).SingleOrDefault();

                if (contratanteId.HasValue && contratanteId.Value > 0)
                {
                    var operadora = sessao.Query<Operadora>()
                    .Where(o => o.ID == ret.Adicional.Operadora.ID && o.ContratanteId == contratanteId.Value).SingleOrDefault();

                    if (operadora == null)
                    {
                        throw new ApplicationException("Security exception.");
                    }
                }
            }

            return ret;
        }

        public List<AdicionalFaixa> CarregarAdicionailFaixas(long adicionalId, long? contratanteId)
        {
            using (ISession sessao = ObterSessao())
            {
                var faixas = sessao.Query<AdicionalFaixa>()
                    .Fetch(af => af.Adicional)
                    .Where(af => af.Adicional.ID == adicionalId)
                    .OrderBy(af => af.Vigencia)
                    .OrderBy(af => af.IdadeInicio)
                    .ToList();

                if(faixas != null && faixas.Count > 0 && contratanteId.HasValue)
                {
                    var operadora = sessao.Query<Operadora>()
                        .Where(o => o.ID == faixas[0].Adicional.Operadora.ID && o.ContratanteId == contratanteId.Value)
                        .SingleOrDefault();

                    if (operadora == null)
                    {
                        throw new ApplicationException("Security exception.");
                    }
                }

                return faixas;
            }
        }

        //PLANOS
        /*********************************************************************************/

        public Plano SalvarPlano(Plano plano)
        {
            using (ISession sessao = ObterSessao())
            {
                using (ITransaction tran = sessao.BeginTransaction())
                {
                    plano.ContratoAdm = sessao.Get<ContratoADM>(plano.ContratoAdm.ID);
                    sessao.SaveOrUpdate(plano);
                    tran.Commit();
                }
            }

            return plano;
        }

        public void ExcluirPlano(long planoId)
        {
            using (ISession sessao = ObterSessao())
            {
                using (ITransaction tran = sessao.BeginTransaction())
                {
                    var plano = sessao.Get<Plano>(planoId);
                    sessao.Delete(plano);
                    tran.Commit();
                }
            }
        }

        public Plano CarregarPlano(long id, long? contratanteId = null)
        {
            Plano ret = null;

            using (ISession sessao = ObterSessao())
            {
                ret = sessao.Query<Plano>()
                    .Fetch(c => c.ContratoAdm)
                    .Where(c => c.ID == id).SingleOrDefault();

                if (contratanteId.HasValue && contratanteId.Value > 0)
                {
                    var operadora = sessao.Query<Operadora>()
                    .Where(o => o.ID == ret.ContratoAdm.Operadora.ID && o.ContratanteId == contratanteId.Value).SingleOrDefault();

                    if (operadora == null)
                    {
                        throw new ApplicationException("Security exception.");
                    }
                }
            }

            return ret;
        }

        public List<Plano> CarregarPlanos(long contratoAdmId, long? contratanteId)
        {
            using (ISession sessao = ObterSessao())
            {
                //if (contratanteId.HasValue)
                //{
                //    var operadora = sessao.Query<Operadora>()
                //        .Where(o => o.ID == operadoraId && o.ContratanteId == contratanteId.Value).SingleOrDefault();

                //    if (operadora == null)
                //    {
                //        throw new ApplicationException("Security exception.");
                //    }
                //}

                return sessao.Query<Plano>()
                    .Fetch(c => c.ContratoAdm)
                    .Where(c => c.ContratoAdm.ID == contratoAdmId)
                    .OrderBy(c => c.Descricao)
                    .ToList();
            }
        }

        //TABELAS DE VALOR
        /*********************************************************************************/
        public TabelaValor SalvarTabelaDeValor(TabelaValor tabela, IList<TabelaValorItem> itens)
        {
            using (ISession sessao = ObterSessao())
            {
                using (ITransaction tran = sessao.BeginTransaction())
                {
                    tabela.Contrato = sessao.Get<ContratoADM>(tabela.Contrato.ID);
                    sessao.SaveOrUpdate(tabela);

                    if (itens != null && itens.Count > 0)
                    {
                        foreach (var item in itens)
                        {
                            item.Tabela = tabela;
                            item.Plano = sessao.Get<Plano>(item.Plano.ID);
                            sessao.SaveOrUpdate(item);
                        }
                    }

                    tran.Commit();
                }
            }

            return tabela;
        }

        public void ExcluirTabela(long tabelaId)
        {
            using (ISession sessao = ObterSessao())
            {
                using (ITransaction tran = sessao.BeginTransaction())
                {
                    var tabela = sessao.Get<TabelaValor>(tabelaId);
                    sessao.Delete(tabela);
                    tran.Commit();
                }
            }
        }

        public TabelaValor CarregarTabela(long id, long? contratanteId = null)
        {
            TabelaValor ret = null;

            using (ISession sessao = ObterSessao())
            {
                ret = sessao.Query<TabelaValor>()
                    .Fetch(c => c.Contrato)
                    .Where(c => c.ID == id).SingleOrDefault();

                if (contratanteId.HasValue && contratanteId.Value > 0)
                {
                    var operadora = sessao.Query<Operadora>()
                    .Where(o => o.ID == ret.Contrato.Operadora.ID && o.ContratanteId == contratanteId.Value).SingleOrDefault();

                    if (operadora == null)
                    {
                        throw new ApplicationException("Security exception.");
                    }
                }
            }

            return ret;
        }

        public List<TabelaValor> CarregarTabelas(long contratoAdmId, long? contratanteId)
        {
            using (ISession sessao = ObterSessao())
            {
                //if (contratanteId.HasValue)
                //{
                //    var operadora = sessao.Query<Operadora>()
                //        .Where(o => o.ID == operadoraId && o.ContratanteId == contratanteId.Value).SingleOrDefault();

                //    if (operadora == null)
                //    {
                //        throw new ApplicationException("Security exception.");
                //    }
                //}

                return sessao.Query<TabelaValor>()
                    .Fetch(t => t.Contrato)
                    .Where(t => t.Contrato.ID == contratoAdmId)
                    .OrderBy(t => t.Contrato.Descricao)
                    .ToList();
            }
        }

        /********/

        public TabelaValorItem SalvarTabelaValorItem(TabelaValorItem item)
        {
            using (ISession sessao = ObterSessao())
            {
                using (ITransaction tran = sessao.BeginTransaction())
                {
                    sessao.SaveOrUpdate(item);
                    tran.Commit();
                }
            }

            return item;
        }

        public void ExcluirTabelaValorItem(long itemId)
        {
            using (ISession sessao = ObterSessao())
            {
                using (ITransaction tran = sessao.BeginTransaction())
                {
                    var item = sessao.Get<TabelaValorItem>(itemId);
                    sessao.Delete(item);
                    tran.Commit();
                }
            }
        }

        public TabelaValorItem CarregarTabelaValorItem(long id, long? contratanteId = null)
        {
            TabelaValorItem ret = null;

            using (ISession sessao = ObterSessao())
            {
                ret = sessao.Query<TabelaValorItem>()
                    .Fetch(i => i.Tabela)
                    .ThenFetch(t => t.Contrato)
                    .Where(i => i.ID == id).SingleOrDefault();

                if (contratanteId.HasValue && contratanteId.Value > 0)
                {
                    var operadora = sessao.Query<Operadora>()
                    .Where(o => o.ID == ret.Tabela.Contrato.Operadora.ID && o.ContratanteId == contratanteId.Value).SingleOrDefault();

                    if (operadora == null)
                    {
                        throw new ApplicationException("Security exception.");
                    }
                }
            }

            return ret;
        }

        public List<TabelaValorItem> CarregarTabelaValorItens(long tabelaId, long planoId, long? contratanteId)
        {
            using (ISession sessao = ObterSessao())
            {
                var itens = sessao.Query<TabelaValorItem>()
                    .Fetch(i => i.Tabela)
                    .ThenFetch(t => t.Contrato)
                    .Fetch(i => i.Plano)
                    .Where(i => i.Tabela.ID == tabelaId && i.Plano.ID == planoId)
                    .OrderBy(i => i.Plano.ID)
                    .OrderBy(i => i.IdadeInicio)
                    .ToList();

                if (itens != null && itens.Count > 0 && contratanteId.HasValue)
                {
                    var operadora = sessao.Query<Operadora>()
                        .Where(o => o.ID == itens[0].Tabela.Contrato.Operadora.ID && o.ContratanteId == contratanteId.Value)
                        .SingleOrDefault();

                    if (operadora == null)
                    {
                        throw new ApplicationException("Security exception.");
                    }
                }

                return itens;
            }
        }
    }
}
