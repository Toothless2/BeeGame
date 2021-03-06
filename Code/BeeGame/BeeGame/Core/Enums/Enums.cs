﻿namespace BeeGame.Core.Enums
{
    /// <summary>
    /// Honey Comb Types
    /// </summary>
    public enum HoneyCombType
    {
        HONEY, ICEY
    };

    #region BeeStuff
    /// <summary>
    /// The different possible bee Species
    /// </summary>
    public enum BeeSpecies
    {
        FOREST, MEADOWS, TROPICAL, WINTRY, MODEST, MARSHY, ENDER, MONASTIC, STEADFAST, VALIANT, COMMON, CULTIVATED, DILIGENT, RURAL, FARMERLY, AGRARIAN, UNWEARY, INDUSTRIOUS, ICY, GLACIAL, NOBLE, IMPERIAL, MAJESTIC, MIRY, BOGGY, HERIOC, PHANTASMAL, SPECTRAL, HERMETIC, SECLUDED, SINISTER, FIENDISH, DEMONIC, FRUGAL, AUSTER, VINDICTIVE, EXOTIC, ENDEMIC, VENGEFUL, AVENGING, SETADFAST, HEROIC
    };

    /// <summary>
    /// The different bee types
    /// </summary>
    public enum BeeType
    {
        QUEEN, DRONE, PRINCESS
    };

    /// <summary>
    /// The different bee temp preferences
    /// </summary>
    public enum BeeTempPreferance
    {
        FROZEN, COLD, TEMPERATE, HOT, HELL
    };

    /// <summary>
    /// The lifespan of the bee
    /// </summary>
    public enum BeeLifeSpan
    {
        HUMMINGBIRD, SHORTEST, SHORT, NORMAL, LONG, LONGEST, SEATURTLE
    };

    /// <summary>
    /// How fast the bee produces items
    /// </summary>
    public enum BeeProductionSpeed
    {
        SLOW, NORMAL, FAST
    };

    /// <summary>
    /// Any effects of the bee
    /// </summary>
    public enum BeeEffect
    {
        NONE, POISON
    }

    /// <summary>
    /// Humidity preferences of the bee
    /// </summary>
    public enum BeeHumidityPreferance
    {
        ARID, DRY, TEMPERATE, MOIST, HUMID
    };
    #endregion BeeStuff

    /// <summary>
    /// Direction in the game
    /// </summary>
    public enum Direction
    {
        NORTH, EAST, SOUTH, WEST, UP, DOWN
    };
}
