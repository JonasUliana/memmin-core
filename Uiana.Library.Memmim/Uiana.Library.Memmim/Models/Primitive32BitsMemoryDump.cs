namespace Uiana.Library.Memmim.Models {
    /// <summary>
    /// Indexa informações sobre um dump de memória primitivo.
    /// </summary>
    public class Primitive32BitsMemoryDump {
        /// <summary>
        /// Tamanho do dump.
        /// <para>
        /// Nota: Valor em bytes.
        /// </para>
        /// </summary>
        public long Size { get; set; }

        /// <summary>
        /// Tamanho do dump.
        /// <para>
        /// Nota: Valor em bytes.
        /// </para>
        /// </summary>
        public long BytesRead { get; set; }

        /// <summary>
        /// Tamanho do dump.
        /// <para>
        /// Nota: Valor em bytes.
        /// </para>
        /// </summary>
        public byte[] Buffer { get; set; }
    }
}
