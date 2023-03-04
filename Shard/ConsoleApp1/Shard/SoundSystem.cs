/*
*
*   A very simple implementation of a very simple sound system.
*   @author Levente Varga
*   @version 1.0
*   
*/

using SDL2;
using System;
using System.IO;

namespace Shard
{
    public class SoundSystem : Sound
    {
        IntPtr music;

        int audio_rate = SDL_mixer.MIX_DEFAULT_FREQUENCY;
        ushort audio_format = SDL_mixer.MIX_DEFAULT_FORMAT;
        int audio_channels = SDL_mixer.MIX_DEFAULT_CHANNELS;
        int audio_buffers = 4096;

        bool audioOpen = false;

        public SoundSystem()
        {
            SDL.SDL_Init(SDL.SDL_INIT_AUDIO);
            if (SDL_mixer.Mix_OpenAudio(audio_rate, audio_format, audio_channels, audio_buffers) < 0)
            {
                SDL.SDL_Log("Couldn't open audio: " + SDL.SDL_GetError() + "\n");
            }
            else
            {
                SDL_mixer.Mix_QuerySpec(out audio_rate, out audio_format, out audio_channels);
                audioOpen = true;
            }
        } 

        public override double MusicPosition
        {
            get => SDL_mixer.Mix_GetMusicPosition(music);
        }

        public override double MusicLength
        {
            get => SDL_mixer.Mix_MusicDuration(music);
        }

        public override void PlaySound(string path)
        {
            string file = Bootstrap.getAssetManager().getAssetPath(path);
            if (!audioOpen || !File.Exists(file)) return;
            
            IntPtr chunk = SDL_mixer.Mix_LoadWAV(file);
            SDL_mixer.Mix_PlayChannel(-1, chunk, 0);
        }

        public override void PlayMusic(string path)
        {
            string file = Bootstrap.getAssetManager().getAssetPath(path);
            if (!audioOpen || !File.Exists(file)) return;

            SDL_mixer.Mix_VolumeMusic(SDL_mixer.MIX_MAX_VOLUME / 2);
            music = SDL_mixer.Mix_LoadMUS(file);
            SDL_mixer.Mix_PlayMusic(music, 1);
        }
    }
}