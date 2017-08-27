namespace cadben.Entidades
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    public class TabelaValor : EntidadeBaseData
    {
        public TabelaValor() { DataCadastro = DateTime.Now; }
        public TabelaValor(long id) : this() { this.ID = id; }

        public virtual string Descricao { get; set; }
        public virtual ContratoADM Contrato { get; set; }

        public virtual DateTime Inicio { get; set; }
        public virtual DateTime Fim { get; set; }

        public virtual DateTime? InicioVencimento { get; set; }
        public virtual DateTime? FimVencimento { get; set; }
    }

    public class TabelaValorItem : EntidadeBaseData
    {
        public TabelaValorItem(long id) : this() { this.ID = id; }
        public TabelaValorItem() { DataCadastro = DateTime.Now; IdadeInicio = 0; IdadeFim = 0; }

        public virtual TabelaValor Tabela { get; set; }
        public virtual Plano Plano { get; set; }

        public virtual int IdadeInicio { get; set; }
        public virtual int IdadeFim { get; set; }

        public virtual bool CalculoAutomatico { get; set; }

        public virtual Decimal QCValor { get; set; }
        public virtual Decimal QPValor { get; set; }

        public virtual Decimal QCValorPagamento { get; set; }
        public virtual Decimal QPValorPagamento { get; set; }

        public virtual Decimal QCValorCompraCarencia { get; set; }
        public virtual Decimal QPValorCompraCarencia { get; set; }

        public virtual Decimal QCValorMigracao { get; set; }
        public virtual Decimal QPValorMigracao { get; set; }

        public virtual Decimal QCValorCondicaoEspecial { get; set; }
        public virtual Decimal QPValorCondicaoEspecial { get; set; }

        public virtual Decimal QCValorADM { get; set; }
        public virtual Decimal QPValorADM { get; set; }

        public virtual bool ValidaIdades()
        {
            if (this.IdadeFim < this.IdadeInicio)
                return false;

            if (this.IdadeFim == 0 && this.IdadeInicio == 0)
                return false;

            return true;
        }
    }
}