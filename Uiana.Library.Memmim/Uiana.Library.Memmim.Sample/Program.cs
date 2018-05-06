using System;
using Uiana.Library.Memmim.Enums;
using static System.Console;

namespace Uiana.Library.Memmim.Sample {
    class Program {
        static void Main() {
            var processName = "DigOrDie";
            var address = (UIntPtr)0x1E3FE954;

            var memmim = new Memmim();
            memmim.SetProcessPidByName(processName,
                ProcessAccessFlags.All);

            var readed = memmim.PrimitiveRead(address, sizeof(byte));

            // Caso o sistema for little-endian, o array de bytes precisa
            // ser invertido.
            if (BitConverter.IsLittleEndian)
                Array.Reverse(readed.Buffer);

            WriteLine("Suspendendo threads do processo...");
            memmim.SuspendProcess();

            WriteLine($"Lidos {readed.BytesRead} byte(s) do total de {readed.Buffer.Length} parametrizados");
            WriteLine($"Conteúdo: {memmim.GetObjectFromByteArray(readed.Buffer, typeof(bool))}");

            WriteLine("Gravando novo valor...");
            memmim.Write(address, 12, typeof(int));

            memmim.PrimitiveRead(address, sizeof(int));
            WriteLine($"Lidos {readed.BytesRead} byte(s) do total de {readed.Buffer.Length} parametrizados");
            WriteLine($"Conteúdo: {memmim.GetObjectFromByteArray(readed.Buffer, typeof(bool))}");

            WriteLine("Resumindo threads do processo...");
            memmim.ResumeProcess();
#if DEBUG
            WriteLine("Pressione uma tecla para sair...");
            ReadKey();
#endif
        }
    }
}
