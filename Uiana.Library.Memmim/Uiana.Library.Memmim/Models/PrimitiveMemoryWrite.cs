namespace Uiana.Library.Memmim.Models {
    /// <summary>
    /// Indexa informações sobre um dump de memória primitivo.
    /// </summary>
    public class PrimitiveMemoryWrite {
        /// <summary>
        /// Tamanho do dump.
        /// <para>
        /// Nota: Valor em bytes.
        /// </para>
        /// </summary>
        public int BytesWrited { get; set; }

        /// <summary>
        /// Tamanho do dump.
        /// <para>
        /// Nota: Valor em bytes.
        /// </para>
        /// </summary>
        public byte[] BufferWrited { get; set; }
    }
}
