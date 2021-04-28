﻿using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace reAudioPlayerML.HttpServer.API
{
    public class ControlAPI: WebApiController
    {
        [Route(HttpVerbs.Get, "/next")]
        public async Task RNext()
        {
            next();
        }

        public string next()
        {
            PlayerManager.next();
            return null;
        }

        [Route(HttpVerbs.Get, "/last")]
        public async Task RLast()
        {
            last();
        }
        public string last()
        {
            PlayerManager.last();
            return null;
        }

        [Route(HttpVerbs.Get, "/volume/{value}")]
        public async Task RSetVolume(int value)
        {
            volume(value);
        }
        public string volume(int value)
        {
            PlayerManager.volume = value;
            return value.ToString();
        }

        [Route(HttpVerbs.Get, "/playPause")]
        public async Task RPlayPause(int value)
        {
            await Static.SendStringAsync(HttpContext, playPause());
        }
        public string playPause()
        {
            PlayerManager.playPause();
            if (PlayerManager.isPlaying)
            {
                //return GetStream("ressources/controls/webPlay.png");
                return Static.GetStream("ressources/controls/webPlay.png");
            }
            else
            {
                //return GetStream("ressources/controls/webPause.png");
                return Static.GetStream("ressources/controls/webPause.png");
            }
        }

        [Route(HttpVerbs.Get, "/load/playlist/{index}")]
        public async Task<string> RLoadPlaylist(int index)
        {
            return loadPlaylist(index);
        }
        public string loadPlaylist(int index)
        {
            return PlayerManager.loadPlaylist(index);
        }

        [Route(HttpVerbs.Get, "/load/{index}")]
        public async Task RLoadSong(int index)
        {
            loadSong(index);
        }
        public string loadSong(int index)
        {
            PlayerManager.load(index);
            return null;
        }

        public void handleWebsocket(ref Modules.WebSocket.MessageObject msg)
        {
            int value = 0;
            bool isInt = int.TryParse(msg.data, out value);

            switch (msg.endpoint)
            {
                case "next":
                    msg.data = next();
                    break;

                case "last":
                    msg.data = last();
                    break;

                case "volume":
                    msg.data = isInt ? volume(value) : null;
                    break;

                case "playPause":
                    msg.data = playPause();
                    break;

                case "load/playlist":
                    msg.data = isInt ? loadPlaylist(value) : null;
                    break;

                case "load":
                    msg.data = isInt ? loadSong(value) : null;
                    break;

                default:
                    msg.data = "404";
                    break;
            }
        }
    }
}