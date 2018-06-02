using System;

namespace Hes.Mpc.Remote
{
    public struct CopyDataStruct
    {
        /// <summary>
        ///     The data to be passed to the receiving application.
        /// </summary>
        public UIntPtr dwData;

        /// <summary>
        ///     The size, in bytes, of the data pointed to by the lpData member.
        /// </summary>
        public int cbData;

        /// <summary>
        ///     The data to be passed to the receiving application. This member can be NULL.
        /// </summary>
        public IntPtr lpData;
    }
}