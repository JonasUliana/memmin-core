using System;
using Uiana.Library.Memmim.Enums;
using static System.Console;

namespace Uiana.Library.Memmim.Sample {
    class Program {
        static void Main() {
            var processName = "DigOrDie";
            var address = 0x17BC722C;

            var memmim = new Memmim();
            memmim.SetProcessPidByName(processName, ProcessAccessFlags.VirtualMemoryRead);

            var readed = memmim.PrimitiveRead((IntPtr)address, sizeof(int));

            // Caso o sistema for little-endian, o array de bytes precisa
            // ser invertido.
            if (BitConverter.IsLittleEndian)
                Array.Reverse(readed.Buffer);

            WriteLine($"Lidos {readed.BytesRead} byte(s) do total de {readed.Buffer.Length} parametrizados");
            WriteLine($"Conteúdo: {BitConverter.ToInt32(readed.Buffer, 0)}");

#if DEBUG
            WriteLine("Pressione uma tecla para sair...");
            ReadKey();
#endif
        }
    }
}
