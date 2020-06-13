using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace TUMSS20.Audio
{
    public sealed class AudioCache
    {
        private static AudioCache instance = null;
        private static readonly object lockObj = new object();

        private AudioCache()
        {

        }

        public static AudioCache Instance
        {
            get
            {
                lock(lockObj)
                {
                    if (instance == null)
                    {
                        instance = new AudioCache();
                    }

                    return instance;
                }
            }
        }

        private ContentManager contentManager;
        private Dictionary<string, Song> tracks;

        public void Init(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            tracks = new Dictionary<string, Song>();
        }

        public void Shutdown()
        {
            foreach (var entry in tracks)
            {
                entry.Value.Dispose();
            }

            tracks.Clear();
        }

        private Song getSoundEffect(string assetName)
        {
            string fullPath = string.Format("audio/{0}", assetName);

            if (!tracks.ContainsKey(fullPath))
            {
                tracks.Add(fullPath, contentManager.Load<Song>(fullPath));
            }

            return tracks[fullPath];
        }

        public void Play(string assetName)
        {
            Song track = getSoundEffect(assetName);
            MediaPlayer.Play(track);
        }
    }
}
