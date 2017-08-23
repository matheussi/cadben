namespace cadben.Entidades.Map.NHibernate
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    using FluentNHibernate.Mapping;

    public class ContratoADMMap : ClassMap<ContratoADM>
    {
        public ContratoADMMap()
        {
            base.Table("contratoADM");
            base.Id(c => c.ID).Column("contratoadm_id").GeneratedBy.Identity();

            base.Map(c => c.Ativo).Column("contratoadm_ativo");
            base.Map(c => c.Descricao).Column("contratoadm_descricao");

            base.Map(c => c.Numero).Column("contratoadm_numero");
            base.Map(c => c.DataCadastro).Column("contratoadm_data");
            base.Map(c => c.ContratoSaude).Column("contratoadm_contratoSaude");
            base.Map(c => c.ContratoDental).Column("contratoadm_contratoDental");

            base.Map(c => c.CodigoFilial).Column("contratoadm_codFilial");
            base.Map(c => c.CodigoUnidade).Column("contratoadm_codUnidade");
            base.Map(c => c.CodigoAdministradora).Column("contratoadm_codAdministradora");

            base.References(c => c.Operadora).Column("contratoadm_operadoraId");
            base.References(c => c.AssociadoPJ).Column("contratoadm_estipulanteId");
        }
    }
}
