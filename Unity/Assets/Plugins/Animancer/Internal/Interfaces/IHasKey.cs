// Animancer // Copyright 2019 Kybernetik //

namespace Animancer
{
    /// <summary>
    /// Exposes a <see cref="Key"/> object that can be used for dictionaries and hash sets.
    /// </summary>
    public interface IHasKey
    {
        /************************************************************************************************************************/

        /// <summary>
        /// An identifier object that can be used for dictionaries and hash sets.
        /// </summary>
        object Key { get; }

        /************************************************************************************************************************/
    }
}
