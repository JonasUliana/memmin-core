using System;
using static System.Console;

namespace Uiana.Library.Memmim.Sample {
    class Program {
        static void Main() {
            var processName = "DigOrDie";
            var address = 0x152A72C4;

            var memmim = new Memmim();
            memmim.SetProcessPidByName(processName);

            var readed = memmim.PrimitiveRead(address, sizeof(int));

            // Caso o sistema for little-endian, o array de bytes precisa
            // ser invertido.
            if (BitConverter.IsLittleEndian)
                Array.Reverse(readed.Buffer);

            WriteLine($"Lidos {readed.BytesRead} byte(s) do total de {readed.Buffer.Length} parametrizados");
            WriteLine($"Conteúdo: {BitConverter.ToInt32(readed.Buffer, 0)}");

            WriteLine("Alterando valor...");
            var bytes = BitConverter.GetBytes(9999);
            var readedAfterWrite = memmim.PrimitiveWrite(address, bytes);
            WriteLine($"Conteúdo escrito: {readedAfterWrite.BytesWrited} byte(s)");


            readed = memmim.PrimitiveRead(address, sizeof(int));
            WriteLine($"Conteúdo: {BitConverter.ToInt32(readed.Buffer, 0)}");
#if DEBUG
            WriteLine("Pressione uma tecla para sair...");
            ReadKey();
#endif
        }
    }
}
