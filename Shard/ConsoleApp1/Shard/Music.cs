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

        int audio_rate = SDL_mixer.MIX_DEFAULT_FREQUENCY;
        ushort audio_format = SDL_mixer.MIX_DEFAULT_FORMAT;
        int audio_channels = SDL_mixer.MIX_DEFAULT_CHANNELS;
        int audio_buffers = 4096;

        string file;

        public Music()
        {
            SDL.SDL_Init(SDL.SDL_INIT_AUDIO);
        } 

        public override double PlayheadPosition
        {
            get => SDL_mixer.Mix_GetMusicPosition(music);
        }

        public override double Length
        {
            get => SDL_mixer.Mix_MusicDuration(music);
        }

        public override void Load(string path)
        {
            file = Bootstrap.getAssetManager().getAssetPath(path);
            //music = SDL_mixer.Mix_LoadMUS(file);
        }

        public override void Play()
        {
            if (SDL_mixer.Mix_OpenAudio(audio_rate, audio_format, audio_channels, audio_buffers) < 0)
            {
                SDL.SDL_Log("Couldn't open audio: " + SDL.SDL_GetError() + "\n");
            }
            else
            {
                SDL_mixer.Mix_QuerySpec(out audio_rate, out audio_format, out audio_channels);
                SDL_mixer.Mix_VolumeMusic(SDL_mixer.MIX_MAX_VOLUME / 2);
                music = SDL_mixer.Mix_LoadMUS(file);
                SDL_mixer.Mix_PlayMusic(music, 1);
            }
        }

        public void Free()
        {
            SDL_mixer.Mix_FreeMusic(music);
        }
    }
}