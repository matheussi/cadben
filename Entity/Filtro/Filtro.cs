using System;
using System.Collections.Generic;
using System.Text;

namespace cadben.Entity.Filtro
{
    [Serializable()]
    public abstract class Filtro
    {
        /// <summary>
        /// Método para Verificar se um Filtro é Válido.
        /// </summary>
        /// <returns>True se for Válido, False se não for Válido.</returns>
        public abstract Boolean IsValid();
    }
}
