
namespace DesignValueParser.Interfaces
{
    public interface ICarrier
    {
        /// <summary>
        /// The location of this carrier
        /// </summary>
        IStarSystem Location { get; }
    }
}
