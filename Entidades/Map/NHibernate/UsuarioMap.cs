namespace cadben.Entidades.Map.NHibernate
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    using FluentNHibernate.Mapping;

    public class UsuarioMap : ClassMap<Usuario>
    {
        public UsuarioMap()
        {
            base.Table("usuario");
            base.Id(c => c.ID).Column("usuario_id").GeneratedBy.Identity();

            base.Map(c => c.Login).Column("usuario_login");
            base.Map(c => c.Nome).Column("usuario_nome");
            base.Map(c => c.Email).Column("usuario_email");
            base.Map(c => c.Senha).Column("usuario_senha");
            base.Map(c => c.Tipo).Column("usuario_tipo").CustomType(typeof(int));

            base.Map(c => c.Ativo).Column("usuario_ativo");
            base.Map(c => c.DataCadastro).Column("usuario_data");

            base.Map(c => c.FilialID).Column("usuario_filialId").Nullable();

            base.References(u => u.Contratante).Column("usuario_contratanteId");
        }
    }
}
