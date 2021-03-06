﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Runtime.InteropServices;
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
        /// Define se o processo é executado na WOW64.
        /// </summary>
        public bool IsWow64 { get; private set; }

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

            IsWow64Process(Handle, out var tempBool);
            IsWow64 = tempBool;

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
        public PrimitiveMemoryDump PrimitiveRead(UIntPtr address, int bufferSize) {
            var output = new PrimitiveMemoryDump {
                Buffer = new byte[bufferSize],
                BytesRead = 0,
            };
            var temp = 0;

            ReadProcessMemory(Handle, address, output.Buffer,
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
        [Obsolete("A função PrimitiveWrite() está depreciada, utilize Write() em seu lugar.")]
        public PrimitiveMemoryWrite PrimitiveWrite(UIntPtr address, byte[] buffer) {
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
        /// <param name="write">Objeto a ser escrito.</param>
        /// <param name="type">Assinatura de tipo do objeto que será escrito.</param>
        /// <returns>Se a função tiver exito, é retornado verdadeiro.</returns>
        public bool Write(UIntPtr address, object write, Type type) =>
            WriteProcessMemory(Handle, address, _getByteArrayFromObject(write), 
                (UIntPtr)Marshal.SizeOf(type), IntPtr.Zero);

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

        #region Helper

        private readonly Func<object, byte[]> _getByteArrayFromObject = obj => {
            var size = Marshal.SizeOf(obj);
            var bytes = new byte[size];
            var ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, ptr, false);
            Marshal.Copy(ptr, bytes, 0, size);
            Marshal.FreeHGlobal(ptr);
            return bytes;
        };

        #endregion

        /// <summary>
        /// Converte um array de bytes em um objeto de determinado tipo.
        /// </summary>
        public Func<byte[], Type, object> GetObjectFromByteArray = (b, t) => {
            var ptr = Marshal.AllocHGlobal(b.Length);
            Marshal.Copy(b, 0, ptr, b.Length);
            var desserialized = Marshal.PtrToStructure(ptr, t);
            Marshal.FreeHGlobal(ptr);
            return desserialized;
        };
    }
}
