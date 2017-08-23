namespace cadben.Entidades.Map.NHibernate
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    using FluentNHibernate.Mapping;

    public class AdicionalMap : ClassMap<Adicional>
    {
        public AdicionalMap()
        {
            base.Table("adicional");
            base.Id(a => a.ID).Column("adicional_id").GeneratedBy.Identity();

            base.Map(a => a.Ativo).Column("adicional_ativo");

            base.Map(a => a.Codigo).Column("adicional_codTitular");
            base.Map(a => a.Descricao).Column("adicional_descricao");
            base.Map(a => a.ParaTodaProposta).Column("adicional_paraTodaProposta");

            base.Map(a => a.DataCadastro).Column("adicional_data");

            base.References(a => a.Operadora).Column("adicional_operadoraId");
        }
    }

    public class AdicionalFaixaMap : ClassMap<AdicionalFaixa>
    {
        public AdicionalFaixaMap()
        {
            base.Table("adicional_faixa");
            base.Id(a => a.ID).Column("adicionalfaixa_id").GeneratedBy.Identity();

            base.Map(a => a.IdadeFim).Column("adicionalfaixa_idadeFim");
            base.Map(a => a.IdadeInicio).Column("adicionalfaixa_idadeInicio");

            base.Map(a => a.Valor).Column("adicionalfaixa_valor");
            base.Map(a => a.Vigencia).Column("adicionalfaixa_vigencia");

            base.References(a => a.Adicional).Column("adicionalfaixa_adicionalid");
        }
    }
}
