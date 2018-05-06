using System;
using Uiana.Library.Memmim.Enums;
using static System.Console;

namespace Uiana.Library.Memmim.Sample {
    class Program {
        static void Main() {
            var processName = "DigOrDie";
            var address = (UIntPtr)0x1E3FE954;

            var memmim = new Memmim();
            memmim.SetProcessPidByName(processName, ProcessAccessFlags.VirtualMemoryRead);

            var readed = memmim.PrimitiveRead(address, sizeof(byte));

            // Caso o sistema for little-endian, o array de bytes precisa
            // ser invertido.
            if (BitConverter.IsLittleEndian)
                Array.Reverse(readed.Buffer);
            WriteLine("Suspendendo threads do processo...");
            memmim.SuspendProcess();
            WriteLine($"Lidos {readed.BytesRead} byte(s) do total de {readed.Buffer.Length} parametrizados");
            WriteLine($"Conteúdo: {readed.Buffer.Length} byte(s)");
            WriteLine($"Gravando novo valor no endereço {address}...");
            WriteLine($"Output: {memmim.WriteByte((UIntPtr)address, byte.MaxValue)}");
            memmim.PrimitiveRead(address, sizeof(byte));
            WriteLine($"Lidos {readed.BytesRead} byte(s) do total de {readed.Buffer.Length} parametrizados");
            WriteLine($"Conteúdo: {readed.Buffer.Length} byte(s)");
            WriteLine("Resumindo threads do processo...");
            memmim.ResumeProcess();
#if DEBUG
            WriteLine("Pressione uma tecla para sair...");
            ReadKey();
#endif
        }
    }
}
