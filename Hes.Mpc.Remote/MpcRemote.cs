using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Hes.Mpc.Remote
{
    public class MpcRemote : Control
    {
        /// <summary>
        /// </summary>
        private readonly string _mpcPlayerPath;

        /// <summary>
        ///
        /// </summary>
        /// <param name="mpcPlayerPath"></param>
        public MpcRemote(string mpcPlayerPath)
        {
            _mpcPlayerPath = mpcPlayerPath;
        }

        /// <summary>
        ///     Index for the currently selected audio track
        /// </summary>
        public int MpcActiveAudioTrack { get; private set; }

        /// <summary>
        ///     Playlist index of the currently playing media file.
        /// </summary>
        public int MpcActivePlaylistItem { get; private set; }

        /// <summary>
        ///     Index for the currently selected subtitle track
        /// </summary>
        public int MpcActiveSubtitleTrack { get; private set; }

        /// <summary>
        ///     Available audio tracks for the currently palying media
        /// </summary>
        public List<string> MpcAudioTracks { get; private set; }

        /// <summary>
        ///     Window Handle of the Main MPC-HC window
        /// </summary>
        public int MPCHandle { get; private set; }

        /// <summary>
        ///     Current load state of the media file.
        /// </summary>
        public LoadState MpcLoadState { get; private set; }

        /// <summary>
        ///     A list of strings whose elements are the abolute file paths of the current playlist of MPCHC
        /// </summary>
        public List<string> MpcPlaylist { get; private set; }

        /// <summary>
        ///     Current play state of the media file.
        /// </summary>
        public PlayState MpcPlayState { get; private set; }

        /// <summary>
        ///     Available subtitle tracks for the currently palying media
        /// </summary>
        public List<string> MpcSubtitleTracks { get; private set; }

        /// <summary>
        ///     Contains information about the currently playing file
        /// </summary>
        public FileProperties NowPlayingatMPC { get; private set; }

        /// <summary>
        ///     Window Handle of the Form which will control (i.e send commands to) MPC-HC
        /// </summary>
        public int RemoteControlHandle => Handle.ToInt32();

        /// <summary>
        ///     <para>This functions handles Windows OS messages between MPCHC instance and remote controller</para>
        ///     <para>It is meant to be used in followwing function. Please refer to the webpage for an example</para>
        ///     <para> </para>
        ///     <para>protected override void WndProc(ref Message m)</para>
        ///     <para>{</para>
        ///     <para>//Listen for operating system messages.</para>
        ///     <para>switch (m.Msg)</para>
        ///     <para>{</para>
        ///     <para>case MPCRemoteV2.WM_COPYDATA:</para>
        ///     <para>    remote.HandleMessage(ref m);</para>
        ///     <para>    break;</para>
        ///     <para>}</para>
        ///     <para> base.WndProc(ref m);</para>
        ///     <para>}</para>
        /// </summary>
        /// <param name="m"></param>
        public void HandleMessage(ref Message m)
        {
            var cds = (CopyDataStruct)Marshal.PtrToStructure(m.LParam, typeof(CopyDataStruct));
            var command = cds.dwData.ToUInt32();
            var param = Marshal.PtrToStringAuto(cds.lpData);
            param = param.Replace(@"\|", " - ");
            var multiParam = param.Split(new[] { "|" }, StringSplitOptions.None);

            switch (command)
            {
                case MpcApiCommands.CMD_CONNECT:
                    MPCHandle = int.Parse(param);
                    break;

                case MpcApiCommands.CMD_STATE:
                    MpcLoadState = (LoadState)Enum.Parse(typeof(LoadState), param);
                    break;

                case MpcApiCommands.CMD_PLAYMODE:
                    MpcPlayState = (PlayState)int.Parse(param);
                    break;

                case MpcApiCommands.CMD_NOWPLAYING:
                    NowPlayingatMPC = new FileProperties
                    {
                        Title = multiParam[0],
                        Author = multiParam[1],
                        Description = multiParam[2],
                        Path = multiParam[3],
                        Duration = TimeSpan.FromSeconds(double.Parse(multiParam[4]))
                    };
                    break;

                case MpcApiCommands.CMD_LISTSUBTITLETRACKS:
                    if (param == "-1" || param == "-2")
                    {
                        MpcSubtitleTracks = new List<string>();
                        MpcActiveSubtitleTrack = int.Parse(param);
                    }
                    else
                    {
                        MpcSubtitleTracks = multiParam.ToList();
                        MpcSubtitleTracks.RemoveAt(MpcSubtitleTracks.Count - 1);
                    }

                    break;

                case MpcApiCommands.CMD_LISTAUDIOTRACKS:
                    if (param == "-1" || param == "-2")
                    {
                        MpcAudioTracks = new List<string>();
                        MpcActiveAudioTrack = int.Parse(param);
                    }
                    else
                    {
                        MpcAudioTracks = multiParam.ToList();
                        MpcAudioTracks.RemoveAt(MpcAudioTracks.Count - 1);
                    }

                    break;

                case MpcApiCommands.CMD_PLAYLIST:
                    MpcPlaylist = multiParam.ToList();
                    MpcActivePlaylistItem = int.Parse(multiParam[multiParam.Length - 1]) + 1;
                    MpcPlaylist.RemoveAt(MpcPlaylist.Count - 1);
                    break;

                case MpcApiCommands.CMD_NOTIFYSEEK:
                    //DO SOMETHING AFTER SEEKING IS FINISHED
                    break;

                case MpcApiCommands.CMD_NOTIFYENDOFSTREAM:
                    //DO SOMETHING AFTER PLAYLIST IS FINISHED
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="command"></param>
        /// <param name="parameter"></param>
        private void SendMpcMessage(uint command, string parameter = "")
        {
            if (MPCHandle <= 0) return;

            parameter += (char) 0;
            CopyDataStruct cds;
            cds.dwData = (UIntPtr) command;
            cds.lpData = Marshal.StringToCoTaskMemAuto(parameter);
            cds.cbData = parameter.Length * Marshal.SystemDefaultCharSize;
            MessageSender.SendMessage(MPCHandle, RemoteControlHandle, ref cds);
        }

        #region Get Methods

        /// <summary>
        ///     Gets the current audio tracks. Tracks can be accessed through MpcAudioTracks
        /// </summary>
        public void GetAudioTracks()
        {
            SendMpcMessage(MpcApiCommands.CMD_GETAUDIOTRACKS);
        }

        /// <summary>
        ///     Gets information about currently palying file. Information can be accessed through NowPlayingatMPC
        /// </summary>
        public void GetNowPlaying()
        {
            SendMpcMessage(MpcApiCommands.CMD_GETNOWPLAYING);
        }

        /// <summary>
        ///     Gets the current playlist tracks. Tracks can be accessed through MpcPlaylist
        /// </summary>
        public void GetPlaylist()
        {
            SendMpcMessage(MpcApiCommands.CMD_GETPLAYLIST);
        }

        /// <summary>
        ///     Gets the current subtitle tracks. Tracks can be accessed through MpcSubtitleTracks
        /// </summary>
        public void GetSubtitleTracks()
        {
            SendMpcMessage(MpcApiCommands.CMD_GETSUBTITLETRACKS);
        }

        #endregion Get Methods

        #region Set Methods

        /// <summary>
        /// *NOT IMPLEMETNED ON MPC-HC SIDE*
        /// Removes the given element from the playlist.
        /// </summary>
        /// <param name="index">Which playlist entry to be removed</param>
        public void RemoveFromPlaylist(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// *NOT IMPLEMETNED ON MPC-HC SIDE*
        /// Sets the active file in the playlist.
        /// </summary>
        /// <param name="index">Index of the active file, -1 for no file selected</param>
        public void SetIndexPlaylist(int index)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Jumps Forward (Negative Backwards)
        /// </summary>
        /// <param name="seconds">Jump amount in seconds</param>
        public void JumpOfGivenSeconds(double seconds)
        {
            SendMpcMessage(MpcApiCommands.CMD_JUMPOFNSECONDS, seconds.ToString());
        }

        /// <summary>
        /// Jumps to the end of the currently playing file. Effectively playing next file in the playlist.
        /// </summary>
        public void JumpToTheEndOfPlayingFile()
        {
            //throw new NotImplementedException();
            SetPosition(NowPlayingatMPC.Duration.TotalSeconds);
        }

        /// <summary>
        /// Starts MPC-HC in slave mode.
        /// </summary>
        public void StartApplication()
        {
                Process p = new Process();
                p.StartInfo.FileName = _mpcPlayerPath;
                p.StartInfo.Arguments = "/slave " + RemoteControlHandle;
                p.Start();
        }

        /// <summary>
        /// Opens the specified file in MPC-HC.
        /// </summary>
        /// <param name="path">The absolute path of the media file.</param>
        public void OpenFile(string path)
        {
            SendMpcMessage(MpcApiCommands.CMD_OPENFILE, path);
        }

        /// <summary>
        /// Stops playback, but keep file / playlist
        /// </summary>
        public void StopPlayback()
        {
            SendMpcMessage(MpcApiCommands.CMD_STOP);
        }

        /// <summary>
        /// Stops playback and closes file / playlist
        /// </summary>
        public void CloseFile()
        {
            SendMpcMessage(MpcApiCommands.CMD_CLOSEFILE);
        }

        /// <summary>
        /// Unpauses playback.
        /// </summary>
        public void Play()
        {
            SendMpcMessage(MpcApiCommands.CMD_PLAY);
        }

        /// <summary>
        /// Pauses playback.
        /// </summary>
        public void Pause()
        {
            SendMpcMessage(MpcApiCommands.CMD_PAUSE);
        }

        /// <summary>
        /// Pauses or restarts playbacks
        /// </summary>
        public void PlayPausePlayback()
        {
            SendMpcMessage(MpcApiCommands.CMD_PLAYPAUSE);
        }

        /// <summary>
        /// Adds a new file to playlist but does not start playing.
        /// </summary>
        /// <param name="path">Absolute file path to be added to the playlist.</param>
        public void AddToPlaylist(string path)
        {
            SendMpcMessage(MpcApiCommands.CMD_ADDTOPLAYLIST, path);
        }

        /// <summary>
        /// Remove all files from the playlist.
        /// </summary>
        public void ClearPlaylist()
        {
            SendMpcMessage(MpcApiCommands.CMD_CLEARPLAYLIST);
        }

        /// <summary>
        /// Starts playing the playlist.
        /// </summary>
        public void StartPlaylist()
        {
            SendMpcMessage(MpcApiCommands.CMD_STARTPLAYLIST);
        }

        /// <summary>
        /// Cues current file to specific position.
        /// </summary>
        /// <param name="seconds">New position in seconds</param>
        public void SetPosition(double seconds)
        {
            SendMpcMessage(MpcApiCommands.CMD_SETPOSITION, seconds.ToString());
        }

        /// <summary>
        /// Set the audio delay in miliseconds.
        /// </summary>
        /// <param name="milliseconds">New audio delay in miliseconds</param>
        public void SetAudioDelay(int milliseconds)
        {
            SendMpcMessage(MpcApiCommands.CMD_SETAUDIODELAY, milliseconds.ToString());
        }

        /// <summary>
        /// Sets the subtitle delay.
        /// </summary>
        /// <param name="milliseconds">New subtitle delay in miliseconds</param>
        public void SetSubtitleDelay(int milliseconds)
        {
            SendMpcMessage(MpcApiCommands.CMD_SETSUBTITLEDELAY, milliseconds.ToString());
        }

        /// <summary>
        /// Sets the audio track.
        /// </summary>
        /// <param name="index">Index of the audio track</param>
        public void SetAudioTrack(int index)
        {
            SendMpcMessage(MpcApiCommands.CMD_SETAUDIOTRACK, index.ToString());
        }

        /// <summary>
        /// Sets the subtitle track.
        /// </summary>
        /// <param name="index">Index of the subtitle track, -1 for disabling subtitles</param>
        public void SetSubtitleTrack(int index)
        {
            SendMpcMessage(MpcApiCommands.CMD_SETSUBTITLETRACK, index.ToString());
        }

        /// <summary>
        /// Jump forward - medium, defined by internal design of MPCHC -
        /// </summary>
        public void JumpForward()
        {
            SendMpcMessage(MpcApiCommands.CMD_JUMPFORWARDMED);
        }

        /// <summary>
        /// Jump backward - medium, defined by internal design of MPCHC -
        /// </summary>
        public void JumpBackward()
        {
            SendMpcMessage(MpcApiCommands.CMD_JUMPBACKWARDMED);
        }

        /// <summary>
        /// Toggles fullscreen.
        /// </summary>
        public void ToggleFullscreen()
        {
            SendMpcMessage(MpcApiCommands.CMD_TOGGLEFULLSCREEN);
        }

        /// <summary>
        /// Increases Volume.
        /// </summary>
        public void IncreaseVolume()
        {
            SendMpcMessage(MpcApiCommands.CMD_INCREASEVOLUME);
        }

        /// <summary>
        /// Decreases volume.
        /// </summary>
        public void DecreaseVolume()
        {
            SendMpcMessage(MpcApiCommands.CMD_DECREASEVOLUME);
        }

        /// <summary>
        /// Closes the instance of MPCHC.
        /// </summary>
        public void CloseApplication()
        {
            SendMpcMessage(MpcApiCommands.CMD_CLOSEAPP);
        }

        /// <summary>
        /// Toggles shader.
        /// </summary>
        public void ToggleShader()
        {
            SendMpcMessage(MpcApiCommands.CMD_SHADER_TOGGLE);
        }

        #endregion Set Methods
    }
}