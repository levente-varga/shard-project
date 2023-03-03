/*
*
*   This class intentionally left blank.  
*   @author Michael Heron
*   @version 1.0
*   
*/

using System;

namespace Shard
{
    abstract public class Sound
    {
        abstract public double PlayheadPosition { get; set; }

        abstract public double Length { get; }

        public void LoadAndPlay(string path)
        {
            Load(path);
            Play();
        }

        abstract public void Load(string path);

        abstract public void Play();
    }
}
