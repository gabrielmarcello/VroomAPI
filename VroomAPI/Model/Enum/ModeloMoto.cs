using System.ComponentModel;

namespace VroomAPI.Model.Enum {
    /// <summary>
    /// Modelos de motocicleta disponíveis no sistema
    /// </summary>
    public enum ModeloMoto {
        /// <summary>
        /// Modelo popular básico
        /// </summary>
        [Description("Mottu Pop - Modelo básico")]
        MOTTUPOP = 0,
        
        /// <summary>
        /// Modelo esportivo
        /// </summary>
        [Description("Mottu Sport - Modelo esportivo")]
        MOTTUSPORT = 1,
        
        /// <summary>
        /// Modelo elétrico
        /// </summary>
        [Description("Mottu E - Modelo elétrico")]
        MOTTUE = 2
    }
}
