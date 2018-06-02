namespace Hes.Mpc.Remote
{
    /// <summary>
    ///     Contains the constant used for communication through Windows OS Messaging
    /// </summary>
    public class MpcApiCommands
    {
        public const uint CMD_ADDTOPLAYLIST = 0xA0001000;
        public const uint CMD_CLEARPLAYLIST = 0xA0001001;
        public const uint CMD_CLOSEAPP = 0xA0004006;
        public const uint CMD_CLOSEFILE = 0xA0000002;
        public const uint CMD_CONNECT = 0x50000000;
        public const uint CMD_CURRENTPOSITION = 0x50000007;
        public const uint CMD_DECREASEVOLUME = 0xA0004004;
        public const uint CMD_DISCONNECT = 0x5000000B;
        public const uint CMD_GETAUDIOTRACKS = 0xA0003001;
        public const uint CMD_GETNOWPLAYING = 0xA0003002;
        public const uint CMD_GETPLAYLIST = 0xA0003003;
        public const uint CMD_GETSUBTITLETRACKS = 0xA0003000;
        public const uint CMD_INCREASEVOLUME = 0xA0004003;
        public const uint CMD_JUMPBACKWARDMED = 0xA0004002;
        public const uint CMD_JUMPFORWARDMED = 0xA0004001;
        public const uint CMD_JUMPOFNSECONDS = 0xA0003005;
        public const uint CMD_LISTAUDIOTRACKS = 0x50000005;
        public const uint CMD_LISTSUBTITLETRACKS = 0x50000004;
        public const uint CMD_NOTIFYENDOFSTREAM = 0x50000009;
        public const uint CMD_NOTIFYSEEK = 0x50000008;
        public const uint CMD_NOWPLAYING = 0x50000003;
        public const uint CMD_OPENFILE = 0xA0000000;
        public const uint CMD_PAUSE = 0xA0000005;
        public const uint CMD_PLAY = 0xA0000004;
        public const uint CMD_PLAYLIST = 0x50000006;
        public const uint CMD_PLAYMODE = 0x50000002;
        public const uint CMD_PLAYPAUSE = 0xA0000003;
        public const uint CMD_REMOVEFROMPLAYLIST = 0xA0001003;
        public const uint CMD_SETAUDIODELAY = 0xA0002001;
        public const uint CMD_SETAUDIOTRACK = 0xA0002004;
        public const uint CMD_SETINDEXPLAYLIST = 0xA0002003;
        public const uint CMD_SETPOSITION = 0xA0002000;
        public const uint CMD_SETSUBTITLEDELAY = 0xA0002002;
        public const uint CMD_SETSUBTITLETRACK = 0xA0002005;
        public const uint CMD_SHADER_TOGGLE = 0xA0004005;
        public const uint CMD_STARTPLAYLIST = 0xA0001002;
        public const uint CMD_STATE = 0x50000001;
        public const uint CMD_STOP = 0xA0000001;
        public const uint CMD_TOGGLEFULLSCREEN = 0xA0004000;
    }
}