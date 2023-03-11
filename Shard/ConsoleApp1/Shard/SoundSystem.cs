/*
*
*   A very simple implementation of a very simple music player.
*   @author Levente Varga
*   @version 1.0
*   
*/

using SDL2;
using SpaceInvaders;
using System;
using System.Collections.Generic;
using System.IO;

namespace Shard
{
    class SoundSystem : Sound, InputListener
    {
        IntPtr music;

        int audio_rate = SDL_mixer.MIX_DEFAULT_FREQUENCY;
        ushort audio_format = SDL_mixer.MIX_DEFAULT_FORMAT;
        int audio_channels = SDL_mixer.MIX_DEFAULT_CHANNELS;
        int audio_buffers = 4096;

        double musicPosition;
        double lastMusicPosition;
        double lastPlayheadPosition;

        bool audioOpen = false;
        bool musicPlaying = false;

        double correction = 0.0;
        double correctionFactor = 10;

        bool smoothen = true;
        bool debug = false;

        public bool Smoothen
        {
            get => smoothen;
            set => smoothen = value;
        }


        // Debugging
        List<double> correctionSamples = new List<double>();
        List<double> musicPositionChangeSamples = new List<double>();
        int maxSampleCount = 864;

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

            Visible = true;
            Layer = 100;
        } 

        public override double MusicPosition
        {
            get => musicPosition;
        }

        public override double MusicLength
        {
            get => SDL_mixer.Mix_MusicDuration(music);
        }

        public override void update()
        {
            if (!musicPlaying) return;

            double playheadPosition = SDL_mixer.Mix_GetMusicPosition(music);

            lastMusicPosition = musicPosition;
            musicPosition += Bootstrap.getDeltaTime();

            if (playheadPosition != lastPlayheadPosition)
            {
                double difference = playheadPosition - musicPosition;
                correction += difference / correctionFactor;

                if (!smoothen) musicPosition = playheadPosition;
                lastPlayheadPosition = playheadPosition;

                //Debug.Log($"Difference: {difference}");
            }

            double currentCorrection = correction / correctionFactor;
            correction -= currentCorrection;

            if (smoothen)
            {
                musicPosition += currentCorrection;
            }

            

            if (musicPosition >= MusicLength)
            {
                Debug.Log("Music reached end");
                musicPlaying = false;
                musicPosition = MusicLength;
            }

            if (debug)
            {
                Debug.Log($"Correction: {currentCorrection} \t/ {correction}");
                
                AddSample(musicPositionChangeSamples, musicPosition - lastMusicPosition);
                AddSample(correctionSamples, correction);

                for (int i = 0; i < musicPositionChangeSamples.Count; i++)
                {
                    Bootstrap.getDisplay().drawLine(100, i, 100 + (int)(musicPositionChangeSamples[i] * 2000), i, 255, 255, 255, 30);
                }

                for (int i = 0; i < correctionSamples.Count; i++)
                {
                    Bootstrap.getDisplay().drawLine(300, i, 400 + (int)(correctionSamples[i] * 100), i, 255, 255, 255, 10);
                }
            }
        }

        private void AddSample(List<double> sampleList, double sample)
        {
            if (sampleList.Count == maxSampleCount) sampleList.RemoveAt(sampleList.Count - 1);
            sampleList.Insert(0, sample);
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

            SDL_mixer.Mix_VolumeMusic(SDL_mixer.MIX_MAX_VOLUME / 16);
            music = SDL_mixer.Mix_LoadMUS(file);
            SDL_mixer.Mix_PlayMusic(music, 1);

            musicPlaying = true;
            musicPosition = 0;
            lastPlayheadPosition = SDL_mixer.Mix_GetMusicPosition(music);

            Bootstrap.getInput().addListener(this);
        }

        public void handleInput(InputEvent ie)
        {
            switch (ie.Type)
            {
                case InputEventType.KeyDown:
                    if (ie.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_S)
                    {
                        smoothen = !smoothen;
                    }
                    if (ie.Key == (int)SDL.SDL_Scancode.SDL_SCANCODE_D)
                    {
                        debug = !debug;
                    }
                    break;
            }
        }
    }
}