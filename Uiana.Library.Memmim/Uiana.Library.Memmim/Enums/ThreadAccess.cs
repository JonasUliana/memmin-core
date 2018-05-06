namespace Uiana.Library.Memmim.Enums
{
    /// <summary>
    /// Enumera o tipo de acesso a determinado thread.
    /// </summary>
    public enum ThreadAccess {
        /// <summary>
        /// Acesso de finalização,
        /// </summary>
        Terminate = 0x0001,

        /// <summary>
        /// Acesso para gerenciamento de instância.
        /// </summary>
        SuspendResume = 0x0002,

        /// <summary>
        /// Acesso para ler o contexto de execução.
        /// </summary>
        GetContext = 0x0008,

        /// <summary>
        /// Acesso para gravar o contexto de execução.
        /// </summary>
        SetContext = 0x0010,

        /// <summary>
        /// Acesso para ler o contexto de informação.
        /// </summary>
        SetInformation = 0x0020,

        /// <summary>
        /// Acesso para ler o contexto de query de informação.
        /// </summary>
        QueryInformation = 0x0040,

        /// <summary>
        /// Acesso para parametrização do token.
        /// </summary>
        SetThreadToken = 0x0080,

        /// <summary>
        /// Acesso para representação em outra instância.
        /// </summary>
        Impersonate = 0x0100,

        /// <summary>
        /// Acesso direto para representação em outra instância.
        /// </summary>
        DirectImpersonation = 0x0200
    }
}