using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class Music
    {
        double beatPerMinute;
        double beatPerSecond;
        double offsetSeconds;
        string title;
        double musicPositionSeconds;
        double musicPositionBeats;

        public double PositionSeconds
        {
            set
            {
                musicPositionSeconds = value;
                musicPositionBeats = (musicPositionSeconds - offsetSeconds) * beatPerSecond;

            }
            get => musicPositionSeconds;
        }

        public double PositionBeats
        {
            set 
            {
                musicPositionBeats = value;
                musicPositionSeconds = musicPositionBeats / beatPerSecond + offsetSeconds;
            }
            get => musicPositionBeats;
        }

        public double BeatPerMinute { get => beatPerMinute; }
        public double BeatPerSecond { get => beatPerSecond; }
        public double OffsetSeconds { get => offsetSeconds; }
        public string Title { get => title; }

        List<Note> notes;

        public Music(string title, double beatPerMinute, double offsetSeconds)
        {
            this.beatPerMinute = beatPerMinute;
            this.offsetSeconds = offsetSeconds;
            beatPerSecond = beatPerMinute / 60.0;
            notes = new List<Note>();
        }

        public void AddNote(Note note)
        {
            notes.Add(note);
        }

        public void UpdateNotes(double musicPosition)
        {
            notes.ForEach(note => { note.update(); });
        }
    }
}
