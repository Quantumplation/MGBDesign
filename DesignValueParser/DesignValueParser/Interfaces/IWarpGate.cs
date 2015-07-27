
namespace DesignValueParser.Interfaces
{
    public interface IWarpGate
    {
        /// <summary>
        /// The location of the destination warp gate
        /// </summary>
        IStarSystem Destination { get; }

        /// <summary>
        /// The location of this warp gate
        /// </summary>
        IStarSystem Location { get; }

        /// <summary>
        /// Distance from location to destination
        /// </summary>
        decimal Distance { get; }
    }
}
