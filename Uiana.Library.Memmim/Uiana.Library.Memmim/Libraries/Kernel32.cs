using System;
using System.Runtime.InteropServices;

using Uiana.Library.Memmim.Enums;

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
        public static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, int dwProcessId);

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
        public static extern bool ReadProcessMemory(IntPtr hProcess,
            UIntPtr lpBaseAddress, byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesRead);

        /// <summary>
        /// Usado para gravar dados em um processo remoto.
        /// </summary>
        /// <param name="hProcess">Um identificador para a memória do processo a ser modificada.</param>
        /// <param name="lpBaseAddress">Um ponteiro para o endereço base no processo especificado no qual os dados são gravados.</param>
        /// <param name="lpBuffer">Um ponteiro para o buffer que contém dados a serem gravados no espaço de endereço do processo especificado.</param>
        /// <param name="dwSize">O número de bytes a serem gravados no processo especificado.</param>
        /// <param name="lpNumberOfBytesWritten">Um ponteiro para uma variável que recebe o número de bytes transferidos para o processo especificado.</param>
        /// <returns>Se a função tiver êxito, o valor de retorno é diferente de zero.</returns>
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteProcessMemory(int hProcess, int lpBaseAddress,
            byte[] lpBuffer, int dwSize, ref int lpNumberOfBytesWritten);

        /// <summary>
        /// Usado para gravar dados em um processo remoto.
        /// </summary>
        /// <param name="hProcess">Um identificador para a memória do processo a ser modificada.</param>
        /// <param name="lpBaseAddress">Um ponteiro para o endereço base no processo especificado no qual os dados são gravados.</param>
        /// <param name="lpBuffer">Um ponteiro para o buffer que contém dados a serem gravados no espaço de endereço do processo especificado.</param>
        /// <param name="nSize">O número de bytes a serem gravados no processo especificado.</param>
        /// <param name="lpNumberOfBytesWritten">Um ponteiro para uma variável que recebe o número de bytes transferidos para o processo especificado.</param>
        /// <returns>Se a função tiver êxito, o valor de retorno é diferente de zero.</returns>
        [DllImport("kernel32.dll")]
        public static extern bool WriteProcessMemory(IntPtr hProcess, 
            UIntPtr lpBaseAddress, byte[] lpBuffer, UIntPtr nSize, 
            IntPtr lpNumberOfBytesWritten);

        /// <summary>
        /// Abre um objeto de thread existente.
        /// </summary>
        /// <param name="dwDesiredAccess">O acesso ao objeto de thread.</param>
        /// <param name="bInheritHandle">Caso verdadeiro, os processos criados por esse processo herdarão o identificador.</param>
        /// <param name="dwThreadId">O identificador do segmento a ser aberto.</param>
        /// <returns>Se a função tiver êxito, o valor de retorno é um identificador aberto para o segmento especificado.</returns>
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenThread(ThreadAccess dwDesiredAccess, bool bInheritHandle, uint dwThreadId);

        /// <summary>
        /// Um aplicativo de 64 bits pode suspender um thread WOW64 usando a função Wow64SuspendThread.
        /// </summary>
        /// <param name="hThread">Um identificador para o segmento que deve ser suspenso.</param>
        /// <returns>Se a função for bem-sucedida, o valor de retorno é a contagem de suspensões anterior do thread; caso contrário, é (DWORD) -1.</returns>
        [DllImport("kernel32.dll")]
        public static extern uint SuspendThread(IntPtr hThread);

        /// <summary>
        /// Decrementa a contagem de suspensões de um thread. Quando a contagem suspensa é diminuída para zero, a execução do encadeamento é retomada.
        /// </summary>
        /// <param name="hThread">Um identificador para o segmento a ser reiniciado.</param>
        /// <returns>Se a função for bem-sucedida, o valor de retorno é a contagem de suspensões anterior do encadeamento.</returns>
        [DllImport("kernel32.dll")]
        public static extern int ResumeThread(IntPtr hThread);

        /// <summary>
        /// Fecha o handle de um objeto aberto.
        /// </summary>
        /// <param name="hObject">Um identificador válido para um objeto aberto.</param>
        /// <returns>Se a função tiver êxito, o valor de retorno é diferente de zero.</returns>
        [DllImport("kernel32.dll")]
        public static extern int CloseHandle(
            IntPtr hObject
        );

        /// <summary>
        /// Determina se o processo especificado está sendo executado no WOW64.
        /// </summary>
        /// <param name="hProcess">Um identificador para o processo.</param>
        /// <param name="wow64Process">Um ponteiro para um valor definido como verdadeiro se o processo estiver sendo executado no WOW64.</param>
        /// <returns>Se a função tiver êxito, o valor de retorno é um valor diferente de zero.</returns>
        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process(
            [In] IntPtr hProcess,
            [Out] out bool wow64Process
        );
    }
}
