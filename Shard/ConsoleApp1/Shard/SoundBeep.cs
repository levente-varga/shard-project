/*
*
*   A very simple implementation of a very simple sound system.
*   @author Michael Heron
*   @version 1.0
*   
*/

using SDL2;
using System;
using System.IO;

namespace Shard
{
    public class SoundSDL : Sound
    {
        SDL.SDL_AudioSpec have, want;
        uint length, dev;
        IntPtr buffer;

        public override double Length => throw new NotImplementedException();

        public override double PlayheadPosition 
        { 
            get => throw new NotImplementedException(); 
            set => throw new NotImplementedException(); 
        }

        public override void Load(string path)
        {
            string file = Bootstrap.getAssetManager().getAssetPath(path);
            SDL.SDL_LoadWAV(file, out have, out buffer, out length);
            dev = SDL.SDL_OpenAudioDevice(IntPtr.Zero, 0, ref have, out want, 0);
        }

        public override void Play()
        {
            int success = SDL.SDL_QueueAudio(dev, buffer, length);
            SDL.SDL_PauseAudioDevice(dev, 0);
        }
    }
}

