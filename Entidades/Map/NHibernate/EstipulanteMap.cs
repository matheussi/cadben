namespace cadben.Entidades.Map.NHibernate
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    using FluentNHibernate.Mapping;

    public class EstipulanteMap : ClassMap<Estipulante>
    {
        public EstipulanteMap()
        {
            base.Table("estipulante");
            base.Id(e => e.ID).Column("estipulante_id").GeneratedBy.Identity();
            base.Map(e => e.ContratanteId).Column("estipulante_contratanteId").Not.Nullable();
            base.Map(e => e.Nome).Column("estipulante_descricao").Not.Nullable();

            base.Map(e => e.DataCadastro).Column("estipulante_dataCadastro").Not.Nullable();
            base.Map(e => e.DataAlteracao).Column("estipulante_dataAlteracao").Nullable();
            base.Map(e => e.Ativo).Column("estipulante_ativo");
        }
    }

    public class EstipulanteTaxaMap : ClassMap<EstipulanteTaxa>
    {
        public EstipulanteTaxaMap()
        {
            base.Table("estipulante_taxa");
            base.Id(e => e.ID).Column("estipulantetaxa_id").GeneratedBy.Identity();
            base.Map(e => e.EstipulanteId).Column("estipulantetaxa_estipulanteId").Not.Nullable();
            base.Map(e => e.Valor).Column("estipulantetaxa_valor").Not.Nullable();
            base.Map(e => e.Vigencia).Column("estipulantetaxa_vigencia").Not.Nullable();
            base.Map(e => e.Tipo).Column("estipulantetaxa_tipo").CustomType(typeof(int));

            base.Map(e => e.DataCadastro).Column("estipulantetaxa_dataCadastro").Not.Nullable();
            base.Map(e => e.DataAlteracao).Column("estipulantetaxa_dataAlteracao").Nullable();
        }
    }
}
