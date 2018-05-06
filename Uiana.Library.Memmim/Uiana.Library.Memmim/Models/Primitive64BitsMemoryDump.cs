namespace Uiana.Library.Memmim.Models {
    /// <summary>
    /// Indexa informações sobre um dump de memória primitivo.
    /// </summary>
    public class Primitive64BitsMemoryDump {
        /// <summary>
        /// Tamanho do dump.
        /// <para>
        /// Nota: Valor em bytes.
        /// </para>
        /// </summary>
        public ulong Size { get; set; }

        /// <summary>
        /// Tamanho do dump.
        /// <para>
        /// Nota: Valor em bytes.
        /// </para>
        /// </summary>
        public ulong BytesRead { get; set; }

        /// <summary>
        /// Tamanho do dump.
        /// <para>
        /// Nota: Valor em bytes.
        /// </para>
        /// </summary>
        public byte[] Buffer { get; set; }
    }
}
