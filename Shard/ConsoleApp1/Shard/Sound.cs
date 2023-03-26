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
    class Sound : GameObject
    {
        virtual public double MusicPosition { get; }

        virtual public double MusicLength { get; }

        virtual public void PlayMusic(string path) { }

        virtual public void PauseMusic() { }

        virtual public void ResumeMusic() { }

        virtual public void PlaySound(string path) { }
    }
}
