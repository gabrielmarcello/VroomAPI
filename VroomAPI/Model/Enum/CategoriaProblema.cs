using System.ComponentModel;

namespace VroomAPI.Model.Enum {
    /// <summary>
    /// Categorias de problemas que podem ser identificados nas motocicletas
    /// </summary>
    public enum CategoriaProblema {
        /// <summary>
        /// Problemas relacionados a componentes mecânicos
        /// </summary>
        [Description("Problema mecânico")]
        MECANICO = 0,
        
        /// <summary>
        /// Problemas relacionados ao sistema elétrico
        /// </summary>
        [Description("Problema elétrico")]
        ELETRICO = 1,
        
        /// <summary>
        /// Problemas relacionados à documentação
        /// </summary>
        [Description("Problema de documentação")]
        DOCUMENTACAO = 2,
        
        /// <summary>
        /// Problemas relacionados à aparência e estética
        /// </summary>
        [Description("Problema estético")]
        ESTETICO = 3,
        
        /// <summary>
        /// Problemas relacionados à segurança
        /// </summary>
        [Description("Problema de segurança")]
        SEGURANCA = 4,
        
        /// <summary>
        /// Múltiplos problemas identificados
        /// </summary>
        [Description("Múltiplos problemas")]
        MULTIPLO = 5,
        
        /// <summary>
        /// Motocicleta em conformidade, sem problemas
        /// </summary>
        [Description("Conforme - sem problemas")]
        CONFORME = 6
    }
}
