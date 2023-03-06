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
        abstract public double MusicPosition { get; }

        abstract public double MusicLength { get; }

        abstract public void PlayMusic(string path);

        abstract public void PlaySound(string path);
    }
}
