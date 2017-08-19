namespace cadben.Entidades.Map.NHibernate
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    using FluentNHibernate.Mapping;

    public class OperadoraMap : ClassMap<Operadora>
    {
        public OperadoraMap()
        {
            base.Table("operadora");
            base.Id(o => o.ID).Column("operadora_id").GeneratedBy.Identity();
            base.Map(o => o.ContratanteId).Column("operadora_contratanteId").Not.Nullable();
            base.Map(o => o.Nome).Column("operadora_nome").Not.Nullable();
            base.Map(o => o.CNPJ).Column("operadora_cnpj").Not.Nullable();

            //base.Map(o => o.Email).Column("operadora_email").Nullable();

            //base.Map(o => o.Telefone1).Column("operadora_telefone1").Nullable();
            //base.Map(o => o.DDD1).Column("operadora_ddd1").Nullable();

            //base.Map(o => o.Telefone2).Column("operadora_telefone2").Nullable();
            //base.Map(o => o.DDD2).Column("operadora_ddd2").Nullable();

            //base.Map(o => o.Celular1).Column("operadora_celular").Nullable();
            //base.Map(o => o.DDDCel1).Column("operadora_celularddd").Nullable();
            //base.Map(o => o.OperadoraCel1).Column("operadora_celularOperadora").Nullable();

            base.Map(o => o.DataCadastro).Column("operadora_dataCadastro").Not.Nullable();
            base.Map(o => o.DataAlteracao).Column("operadora_dataAlteracao").Nullable();
            base.Map(o => o.Inativa).Column("operadora_inativa");
        }
    }
}