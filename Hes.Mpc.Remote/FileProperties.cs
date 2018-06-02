using System;

namespace Hes.Mpc.Remote
{
    /// <summary>
    ///     This class is used to store the information about the currently playing file.
    /// </summary>
    public class FileProperties
    {
        /// <summary>
        ///     Title of the current file.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     Author of the curent file.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        ///     Description of the current file.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        ///     Absolute path of the current file.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     Length of the current file.
        /// </summary>
        public TimeSpan Duration { get; set; }
    }
}