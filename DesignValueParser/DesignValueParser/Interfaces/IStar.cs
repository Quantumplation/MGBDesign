
namespace DesignValueParser.Interfaces
{
    public interface IStar
    {
        /// <summary>
        /// Total mass of this star (Exa-Kg)
        /// </summary>
        decimal Mass { get; }

        /// <summary>
        /// Total radius of this star (m)
        /// </summary>
        decimal Radius { get; }
    }
}
