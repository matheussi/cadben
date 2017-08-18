namespace cadben.Entidades
{
    using System;
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;

    using cadben.Entidades.Enuns;

    public class AgendaExportacaoCartaoItem : EntidadeBase
    {
        public virtual void Inicializar()
        {
            Agenda = new AgendaExportacaoCartao();
            Titular = new ContratoBeneficiario();
        }

        public virtual AgendaExportacaoCartao Agenda { get; set; }
        public virtual ContratoBeneficiario Titular { get; set; }
    }
}