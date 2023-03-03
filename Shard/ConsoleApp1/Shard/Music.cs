/*
*
*   A very simple implementation of a very simple music player.
*   @author Levente Varga
*   @version 1.0
*   
*/

using SDL2;
using System;
using System.IO;

namespace Shard
{
    public class Music : Sound
    {
        IntPtr music;

        public Music()
        {
            SDL_mixer.Mix_Init();
        } 

        public override double PlayheadPosition
        {
            get => SDL_mixer.Mix_GetMusicPosition(music);
            set => SDL_mixer.Mix_SetMusicPosition(value);
        }

        public override double Length
        {
            get => SDL_mixer.Mix_MusicDuration(music);
        }

        public override void Load(string path)
        {
            string file = Bootstrap.getAssetManager().getAssetPath(path);
            music = SDL_mixer.Mix_LoadMUS(file);
        }

        public override void Play()
        {
            
            SDL_mixer.Mix_PlayMusic(music, 1);
        }

        public void Free()
        {
            SDL_mixer.Mix_FreeMusic(music);
        }
    }
}