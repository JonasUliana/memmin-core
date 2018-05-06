using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using static System.Diagnostics.Process;
using System.Threading.Tasks;

namespace Uiana.Library.Memmim {
    /// <summary>
    /// Permite o gerenciamento remoto de outro processo.
    /// </summary>
    public class Memmim {
        #region Propriedades

        /// <summary>
        /// Instância de processo remota atualmente consumida.
        /// </summary>
        public Process Process { get; private set; }

        #endregion

        /// <summary>
        /// Parametriza <see cref="Process"/> com a assinatura de nome
        /// especificada na função.
        /// </summary>
        /// <param name="signature">No do processo que irá ser indexado.</param>
        public void SetProcessPidByName(string signature)
        {
            Process = GetProcessesByName(signature ?? throw new ArgumentException(nameof(signature)))
                .Where(p => p.ProcessName == signature)
                .Select(p => p)
                .FirstOrDefault();
        }
    }
}
