using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class Music : InputListener
    {
        double beatPerMinute;
        double beatPerSecond;
        double offsetSeconds;
        string title;
        double musicPositionSeconds;
        double musicPositionBeats;
        double creatingAtBeat = 0; // the beat we are adding notes at with AddNoteAndPauseForBeat()

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

            Bootstrap.getInput().addListener(this);
        }

        /*
         * 
         */

        public void StartCreating()
        {
            notes.Clear();
            creatingAtBeat = 0;
        }

        public void AddNoteAndPause(double pauseForBeats)
        {
            Random random = new Random();
            int displayWidth = Bootstrap.getDisplay().getWidth();
            int displayHeight = Bootstrap.getDisplay().getHeight();

            AddNoteAndPause(pauseForBeats, new Vector2(
                random.Next(displayWidth / 2) + displayWidth / 4,
                random.Next(displayHeight / 2) + displayHeight / 4
                ));
        }

        public void AddNoteAndPause(double pauseForBeats, int x, int y) => AddNoteAndPause(pauseForBeats, new Vector2(x, y));

        public void AddNoteAndPause(double pauseForBeats, Vector2 position)
        {
            notes.Add(new Note(this, creatingAtBeat, position));
            creatingAtBeat += pauseForBeats;
        }

        public void AddPause(double pauseForBeats)
        {
            creatingAtBeat += pauseForBeats;
        }



        public void UpdateNotes(double musicPosition)
        {
            notes.ForEach(note => { note.update(); });
        }

        public void handleInput(InputEvent ie)
        {
            if (ie.Type != InputEventType.MouseDown) return;
            
            List<Note> candidates = new List<Note>();

            foreach (Note note in notes)
            {
                if (note.Visible && note.IsPositionInsideArea(ie.X, ie.Y))
                {
                    candidates.Add(note);
                }
            }

            if (candidates.Count == 0) return;

            if (candidates.Count > 1) candidates.Sort((a, b) => Math.Sign(a.PositionBeats - b.PositionBeats));

            candidates[0].Fire();
        }
    }
}
