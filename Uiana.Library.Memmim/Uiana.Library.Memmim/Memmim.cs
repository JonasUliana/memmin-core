using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using Uiana.Library.Memmim.Enums;
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

        /// <summary>
        /// Lista de módulos carregados e herdados por <see cref="Process"/>.
        /// </summary>
        public Dictionary<string, IntPtr> Modules { get; private set; } =
        new Dictionary<string, IntPtr>();

        /// <summary>
        /// Módulo atualmente trabalhado pela instância.
        /// </summary>
        public ProcessModule Module { get; private set; }

        /// <summary>
        /// Módulo principal de <see cref="Process"/>.
        /// </summary>
        public ProcessModule MainModule { get; private set; }

        #endregion

        /// <summary>
        /// Parametriza <see cref="Process"/> com a assinatura de nome
        /// especificada na função.
        /// </summary>
        /// <param name="signature">No do processo que irá ser indexado.</param>
        /// <param name="accessRoles">Lista de roles necessárias para manejo do processo.</param>
        public void SetProcessPidByName(string signature, ProcessAccessFlags accessRoles)
        {
            Process = GetProcessesByName(signature ?? throw new ArgumentException(nameof(signature)))
                .Where(p => p.ProcessName == signature)
                .Select(p => p)
                .FirstOrDefault();
            
            if (Process == null)
                throw new NullReferenceException($"Processo não encontrado com a assinatura de nome \"{signature}\".");

            Handle = OpenProcess(accessRoles,
                false, Process.Id);

            Module = Process.MainModule;

            GetModules();
        }

        /// <summary>
        /// Enumera os módulos carregado pela thread principal do processo.
        /// </summary>
        public void GetModules() {
            if (Modules.Any())
                Modules.Clear();

            if (Process == null)
                throw new ArgumentNullException(nameof(Process));

            if (!string.IsNullOrEmpty(Module.ModuleName) &&
                !Modules.ContainsKey(Module.ModuleName))
                Modules
                    .Cast<ProcessModule>()
                    .ToList()
                    .ForEach(m => Modules.Add(m.ModuleName,
                        m.BaseAddress));
        }

        /// <summary>
        /// Realiza a leitura de um enderço de até N bits no processo
        /// instânciado por <see cref="SetProcessPidByName"/>.
        /// </summary>
        /// <param name="address">Endereço absoluto para leitura.</param>
        /// <param name="bufferSize">Tamanho do buffer da leitura.</param>
        public PrimitiveMemoryDump PrimitiveRead(IntPtr address, int bufferSize) {
            var output = new PrimitiveMemoryDump {
                Buffer = new byte[bufferSize],
                BytesRead = 0,
            };
            var temp = 0;

            ReadProcessMemory((int)Handle, (int)address, output.Buffer,
                output.Buffer.Length, ref temp);

            output.BytesRead = temp;

            return output;
        }

        /// <summary>
        /// Realiza a gravação de um enderço de até N bits no processo
        /// instânciado por <see cref="SetProcessPidByName"/>.
        /// </summary>
        /// <param name="address">Endereço absoluto para escrita.</param>
        /// <param name="buffer">Tamanho do buffer da escrita.</param>
        public PrimitiveMemoryWrite PrimitiveWrite(IntPtr address, byte[] buffer) {
            var output = new PrimitiveMemoryWrite {
                BufferWrited = buffer
            };
            var temp = 0;

            WriteProcessMemory((int)Handle, (int)address, buffer,
                buffer.Length, ref temp);

            output.BytesWrited = temp;

            return output;
        }

        /// <summary>
        /// Realiza a gravação de um byte de um endereço na memória
        /// no processo instânciado por <see cref="SetProcessPidByName"/>.
        /// </summary>
        /// <param name="address">Endereço absoluto para escrita.</param>
        /// <param name="write">Valor a ser escrito.</param>
        /// <returns>Se a função tiver exito, é retornado verdadeiro.</returns>
        public bool WriteByte(IntPtr address, byte write)
        {
            // TODO:
            // Implementar função.
            return false;
        }

        /// <summary>
        /// Suspende o processo manuseado nessa instância.
        /// </summary>
        public void SuspendProcess() =>
            Process
                .Threads
                .Cast<ProcessThread>()
                .ToList()
                .ForEach(t => {
                    var opened = OpenThread(ThreadAccess.SuspendResume, false, (uint)t.Id);
                    SuspendThread(opened);
                    CloseHandle(opened);
                });

        /// <summary>
        /// Retoma o processo manuseado nessa instância.
        /// </summary>
        public void ResumeProcess() =>
            Process
                .Threads
                .Cast<ProcessThread>()
                .ToList()
                .ForEach(t => {
                    var opened = OpenThread(ThreadAccess.SuspendResume, false, (uint)t.Id);
                    int toResume;
                    do {
                        toResume = ResumeThread(opened);
                    } while (toResume > 0);
                    CloseHandle(opened);
                });
    }
}
