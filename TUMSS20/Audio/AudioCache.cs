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
        private Dictionary<string, SoundEffect> effects;

        public void Init(ContentManager contentManager)
        {
            this.contentManager = contentManager;
            tracks = new Dictionary<string, Song>();
            effects = new Dictionary<string, SoundEffect>();
        }

        public void Shutdown()
        {
            foreach (var entry in tracks)
            {
                entry.Value.Dispose();
            }

            tracks.Clear();
        }

        private Song getSong(string assetName)
        {
            string fullPath = string.Format("audio/{0}", assetName);

            if (!tracks.ContainsKey(fullPath))
            {
                tracks.Add(fullPath, contentManager.Load<Song>(fullPath));
            }

            return tracks[fullPath];
        }
        private SoundEffect getSoundEffect(string assetName)
        {
            string fullPath = string.Format("audio/{0}", assetName);

            if (!effects.ContainsKey(fullPath))
            {
                effects.Add(fullPath, contentManager.Load<SoundEffect>(fullPath));
            }

            return effects[fullPath];
        }

        public void PlaySong(string assetName, bool repeating)
        {
            Song track = getSong(assetName);
            MediaPlayer.IsRepeating = repeating;
            MediaPlayer.Play(track);
        }

        public void StopSongs()
        {
            MediaPlayer.Stop();
        }

        public void PlaySong(string assetName)
        {
            PlaySong(assetName, false);
        }

        public void PlaySoundEffect(string assetName)
        {
            SoundEffect effect = getSoundEffect(assetName);
            effect.Play();
        }
    }
}
