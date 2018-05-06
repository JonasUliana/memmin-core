using System;
using static System.Console;

namespace Uiana.Library.Memmim.Sample {
    class Program {
        static void Main() {
            var processName = "DigOrDie";
            var address = 0x15_2A_72_C4;

            var memmim = new Memmim();
            memmim.SetProcessPidByName(processName);

            var readed = memmim.PrimitiveRead(address, 12);

            WriteLine($"Lidos {readed.BytesRead} byte(s) do total de {readed.Buffer.Length} parametrizados");

            // Caso o sistema for little-endian, o array de bytes precisa
            // ser invertido.
            if (BitConverter.IsLittleEndian)
                Array.Reverse(readed.Buffer);

            WriteLine($"Conteúdo: {BitConverter.ToInt64(readed.Buffer, 0)}");
            WriteLine("Alterando valor...");
            var bytes = BitConverter.GetBytes(99999);
            var readedAfterWrite = memmim.PrimitiveWrite(address, 12);
#if DEBUG
            WriteLine("Pressione uma tecla para sair...");
            ReadKey();
#endif
        }
    }
}
