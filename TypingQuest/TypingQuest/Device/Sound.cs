using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using TypingQuest.Utility;

namespace TypingQuest.Device
{
    class Sound
    {
        private ContentManager contentManager;
        private Dictionary<string, Song> bgms;
        private Dictionary<string, SoundEffect> soundEffects;
        private Dictionary<string, SoundEffectInstance> seInstances;
        private List<SoundEffectInstance> sePlayList;

        private string currentBGM;
        private Timer fadeTimer;

        public Sound(ContentManager content)
        {
            contentManager = content;
            MediaPlayer.IsRepeating = true;

            bgms = new Dictionary<string, Song>();
            soundEffects = new Dictionary<string, SoundEffect>();
            seInstances = new Dictionary<string, SoundEffectInstance>();

            sePlayList = new List<SoundEffectInstance>();

            currentBGM = null;
            fadeTimer = new Timer(1.2f);
            fadeTimer.Initialize();
        }
        private string ErrorMessage(string name)
        {
            return "再生する音データのアセット名（" + name +
                "）がありません\n" +
                "アセット名の確認、Dictionaryに登録されているか確認してください\n";
        }
        #region BGM関連
        public void LoadBGM(string name, string filepath = "./")
        {
            if (bgms.ContainsKey(name))
            {
                return;
            }
            bgms.Add(name, contentManager.Load<Song>(filepath + name));
        }
        public bool IsStoppedBGM()
        {
            return (MediaPlayer.State == MediaState.Stopped);
        }
        public bool IsPlayingBGM()
        {
            return (MediaPlayer.State == MediaState.Playing);
        }
        public bool IsPausedBGM()
        {
            return (MediaPlayer.State == MediaState.Paused);
        }
        public void StopBGM()
        {
            fadeTimer.Update();
            if (fadeTimer.IsTime())
            {
                MediaPlayer.Stop();
                currentBGM = null;
                fadeTimer.Initialize();
            }
            else
            {
                MediaPlayer.Volume = fadeTimer.Rate() * 0.5f;
            }
        }
        public void PlayBGM(string name)
        {
            Debug.Assert(bgms.ContainsKey(name), ErrorMessage(name));

            if (MediaPlayer.Volume < 0.5f)
            {
                MediaPlayer.Volume += 0.004f;
            }

            if (currentBGM == name)
            {
                return;
            }

            if (IsPlayingBGM())
            {
                StopBGM();
                return;
            }

            MediaPlayer.Volume = 0.0f;

            currentBGM = name;

            MediaPlayer.Play(bgms[currentBGM]);
        }
        public void ChangeBGMLoopFlag(bool loopflag)
        {
            MediaPlayer.IsRepeating = loopflag;
        }
        #endregion
        #region WAV関連
        public void LoadSE(string name, string filepath = "./")
        {
            if (soundEffects.ContainsKey(name))
            {
                return;
            }
            soundEffects.Add(name, contentManager.Load<SoundEffect>(filepath + name));
        }
        public void CreateSEInstance(string name)
        {
            if (seInstances.ContainsKey(name))
            {
                return;
            }
            Debug.Assert(
                seInstances.ContainsKey(name),
                "先に" + name + "の読み込み処理をしてください");
            seInstances.Add(name, soundEffects[name].CreateInstance());
        }
        public void PlaySE(string name)
        {
            Debug.Assert(soundEffects.ContainsKey(name), ErrorMessage(name));

            soundEffects[name].Play();
        }
        public void PlaySEInstance(string name, bool loopFlag = false)
        {
            Debug.Assert(seInstances.ContainsKey(name), ErrorMessage(name));
            var data = seInstances[name];
            data.IsLooped = loopFlag;
            data.Play();
            sePlayList.Add(data);
        }
        public void StopSE()
        {
            foreach (var se in sePlayList)
            {
                if (se.State == SoundState.Playing)
                {
                    se.Stop();
                }
            }
        }
        public void PauseSE()
        {
            foreach (var se in sePlayList)
            {
                if (se.State == SoundState.Playing)
                {
                    se.Pause();
                }
            }
        }
        public void RemoveSE()
        {
            sePlayList.RemoveAll(se => (se.State == SoundState.Stopped));
        }
        #endregion

        public void Unload()
        {
            bgms.Clear();
            soundEffects.Clear();
            sePlayList.Clear();
        }
    }
}
