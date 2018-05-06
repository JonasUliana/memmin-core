using System;
using System.Linq;
using System.Diagnostics;
using Uiana.Library.Memmim.Models;
using static System.Diagnostics.Process;

using static Uiana.Library.Memmim.Libraries.Kernel32;
using static Uiana.Library.Memmim.Enums.ProcessAccessFlags;

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

        /// <summary>
        /// Handle da thread primária de <see cref="Process"/>.
        /// </summary>
        public IntPtr Handle { get; private set; }

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

            if (Process == null)
                throw new NullReferenceException($"Processo não encontrado com a assinatura de nome \"{signature}\".");

            Handle = OpenProcess(VirtualMemoryRead | VirtualMemoryRead,
                false, Process.Id);
        }

        /// <summary>
        /// Realiza a leitura de um enderço de até N bits no processo
        /// instânciado por <see cref="SetProcessPidByName"/>.
        /// </summary>
        /// <param name="address">Endereço absoluto para leitura.</param>
        /// <param name="bufferSize">Tamanho do buffer da leitura.</param>
        public PrimitiveMemoryDump PrimitiveRead(int address, int bufferSize) {
            var output = new PrimitiveMemoryDump {
                Buffer = new byte[bufferSize],
                BytesRead = 0,
            };
            var temp = 0;

            ReadProcessMemory((int)Handle, address, output.Buffer,
                output.Buffer.Length, ref temp);

            output.BytesRead = temp;

            return output;
        }

        /// <summary>
        /// Realiza a gravação de um enderço de até N bits no processo
        /// instânciado por <see cref="SetProcessPidByName"/>.
        /// </summary>
        /// <param name="address">Endereço absoluto para escrita.</param>
        /// <param name="bufferSize">Tamanho do buffer da escrita.</param>
        public PrimitiveMemoryDump PrimitiveWrite(int address, int bufferSize) {
            var output = new PrimitiveMemoryDump {
                Buffer = new byte[bufferSize],
                BytesRead = 0,
            };
            var temp = 0;

            WriteProcessMemory((int)Handle, address, output.Buffer,
                output.Buffer.Length, ref temp);

            output.BytesRead = temp;

            return output;
        }
    }
}
