using System.Runtime.InteropServices;

namespace Hes.Mpc.Remote
{
    public static class MessageSender
    {
        /// <summary>
        /// Used for SendMessage in user32.dll. Defined in Winuser.h
        /// </summary>
        public const int WmCopydata = 0x4A;

        /// <summary>
        ///
        /// </summary>
        /// <param name="senderWindowHandle"></param>
        /// <param name="receiverWindowhandle"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        public static int SendMessage(int receiverWindowhandle, int senderWindowHandle, ref CopyDataStruct lParam){
            return SendMessage(receiverWindowhandle, WmCopydata, senderWindowHandle, ref lParam);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="Msg"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "SendMessageW")]
        private static extern int SendMessage(int hWnd, int Msg, int wParam, ref CopyDataStruct lParam);
    }
}