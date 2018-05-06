using System;
using System.Runtime.InteropServices;

namespace Uiana.Library.Memmim.Libraries {
    /// <summary>
    /// Implementa as diretivas para bibliotecas de código
    /// não gerenciáveis.
    /// </summary>
    internal static class Kernel32 {

        /// <summary>
        /// Abre um identificador para outro processo em execução no sistema.
        /// Esse identificador pode ser usado para ler e gravar na instância
        /// de memória do processo ou para injetar código no mesmo.
        /// </summary>
        /// <param name="dwDesiredAccess">O acesso ao objeto do processo.</param>
        /// <param name="bInheritHandle">Se verdadeiro, os processos criados por esse,
        /// herdarão o identificador do mesmo.</param>
        /// <param name="dwProcessId">O identificador do processo local a ser aberto.</param>
        /// <returns>Se a função for bem-sucedida, o valor de retorno será um identificador aberto para o processo especificado.</returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(int dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        /// <summary>
        /// Lê dados de uma área de memória em um processo especificado.
        /// </summary>
        /// <param name="hProcess">Um identificador para o processo com memória que está sendo lida.</param>
        /// <param name="lpBaseAddress">Um ponteiro para o endereço base no processo especificado do qual ler.</param>
        /// <param name="lpBuffer">Um ponteiro para um buffer que recebe o conteúdo do espaço de endereço do processo especificado.</param>
        /// <param name="dwSize">O número de bytes a serem lidos do processo especificado.</param>
        /// <param name="lpNumberOfBytesRead">Um ponteiro para uma variável que recebe o número de bytes transferidos para o buffer especificado.</param>
        /// <exception cref="Exception">Toda a área a ser lida deve estar acessível ou a operação falhará.</exception>
        /// <returns></returns>
        [DllImport("kernel32.dll")]
        public static extern bool ReadProcessMemory(int hProcess,
            int lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);
    }
}
