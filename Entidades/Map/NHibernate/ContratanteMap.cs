namespace cadben.Entidades.Map.NHibernate
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    using FluentNHibernate.Mapping;

    public class ContratanteMap : ClassMap<Contratante>
    {
        public ContratanteMap()
        {
            base.Table("contratante");
            base.Id(c => c.Id).Column("contratante_id").GeneratedBy.Identity();

            base.Map(c => c.Nome).Column("contratante_nome");
            base.Map(c => c.Alias).Column("contratante_alias");
            base.Map(c => c.Email).Column("contratante_email");
            base.Map(c => c.DataCadastro).Column("contratante_dataCadastro");
            base.Map(c => c.DataAlteracao).Column("contratante_dataAlteracao");
            base.Map(c => c.Ativo).Column("contratante_ativo");
        }
    }
}
