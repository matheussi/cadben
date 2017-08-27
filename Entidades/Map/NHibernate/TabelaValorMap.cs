namespace cadben.Entidades.Map.NHibernate
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    using FluentNHibernate.Mapping;

    public class TabelaValorMap : ClassMap<TabelaValor>
    {
        public TabelaValorMap()
        {
            base.Table("tabela_valor");
            base.Id(t => t.ID).Column("tabelavalor_id").GeneratedBy.Identity();

            base.Map(t => t.Descricao).Column("tabelavalor_descricao").Nullable();
            base.Map(t => t.DataCadastro).Column("tabelavalor_data").Not.Nullable();
            base.Map(t => t.Fim).Column("tabelavalor_fim").Nullable();
            base.Map(t => t.Inicio).Column("tabelavalor_inicio").Not.Nullable();

            base.Map(t => t.FimVencimento).Column("tabelavalor_vencimentoFim").Nullable();
            base.Map(t => t.InicioVencimento).Column("tabelavalor_vencimentoInicio").Nullable();

            base.References(t => t.Contrato).Column("tabelavalor_contratoId").Not.Nullable();
        }
    }

    public class TabelaValorItemMap : ClassMap<TabelaValorItem>
    {
        public TabelaValorItemMap()
        {
            base.Table("tabela_valor_item");
            base.Id(t => t.ID).Column("tabelavalor_id").GeneratedBy.Identity();

            base.Map(t => t.CalculoAutomatico).Column("tabelavaloritem_calculoAutomatico").Not.Nullable();
            base.Map(t => t.DataCadastro).Column("tabelavaloritem_data").Not.Nullable();
            base.Map(t => t.IdadeFim).Column("tabelavaloritem_idadeFim").Not.Nullable();
            base.Map(t => t.IdadeInicio).Column("tabelavaloritem_idadeInicio").Not.Nullable();

            base.Map(t => t.QCValor).Column("tabelavaloritem_qParticular").Not.Nullable();
            base.Map(t => t.QCValorCompraCarencia).Column("tabelavaloritem_qComumCompraCarencia").Nullable();
            base.Map(t => t.QCValorCondicaoEspecial).Column("tabelavaloritem_qComumEspecial").Nullable();
            base.Map(t => t.QCValorMigracao).Column("tabelavaloritem_qComumMigracao").Nullable();
            base.Map(t => t.QCValorPagamento).Column("tabelavaloritem_qComumPagamento").Nullable();
            base.Map(t => t.QCValorADM).Column("tabelavaloritem_qComumADM").Nullable();

            base.Map(t => t.QPValor).Column("tabelavaloritem_qParticular").Not.Nullable();
            base.Map(t => t.QPValorCompraCarencia).Column("tabelavaloritem_qParticularCompraCarencia").Nullable();
            base.Map(t => t.QPValorCondicaoEspecial).Column("tabelavaloritem_qParticularEspecial").Nullable();
            base.Map(t => t.QPValorMigracao).Column("tabelavaloritem_qParticularMigracao").Nullable();
            base.Map(t => t.QPValorPagamento).Column("tabelavaloritem_qParticularPagamento").Nullable();
            base.Map(t => t.QPValorADM).Column("tabelavaloritem_qParticularADM").Nullable();

            base.References(t => t.Plano).Column("tabelavaloritem_planoId").Not.Nullable();
            base.References(t => t.Tabela).Column("tabelavaloritem_tabelaid").Not.Nullable();
        }
    }
}
