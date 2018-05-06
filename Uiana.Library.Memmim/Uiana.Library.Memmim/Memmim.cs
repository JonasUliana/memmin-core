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
        /// Realiza a leitura de um enderço de até 32 bits no processo
        /// instânciado por <see cref="SetProcessPidByName"/>.
        /// </summary>
        /// <param name="address">Endereço absoluto para escrita.</param>
        /// <param name="bufferSize">Tamanho do buffer da escrita.</param>
        public Primitive32BitsMemoryDump PrimitiveRead32Bits(IntPtr address, long bufferSize) {
            var output = new Primitive32BitsMemoryDump
            {
                Buffer = new byte[bufferSize],
                BytesRead = 0,
                Size = 0
            };
            ulong temp = 0;

            ReadProcessMemory((int)Handle, address, output.Buffer, 
                output.Buffer.Length, ref temp);

            output.BytesRead = (long)temp;

            return output;
        }

        /// <summary>
        /// Realiza a leitura de um enderço de até 64 bits no processo
        /// instânciado por <see cref="SetProcessPidByName"/>.
        /// </summary>
        /// <param name="address">Endereço absoluto para escrita.</param>
        /// <param name="bufferSize">Tamanho do buffer da escrita.</param>
        public Primitive64BitsMemoryDump PrimitiveRead64Bits(IntPtr address, uint bufferSize) {
            var output = new Primitive64BitsMemoryDump {
                Buffer = new byte[bufferSize],
                BytesRead = 0,
                Size = 0
            };
            ulong temp = 0;

            ReadProcessMemory((int)Handle, address, output.Buffer,
                output.Buffer.Length, ref temp);

            output.BytesRead = temp;

            return output;
        }
    }
}
