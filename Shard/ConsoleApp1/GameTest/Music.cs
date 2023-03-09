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
        string filePath;
        double musicPositionSeconds;
        double musicPositionBeats;
        double creatingAtBeat = 0; // the beat we are adding notes at with AddNoteAndPauseForBeat()
        Vector2 mousePosition;

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
        public string FilePath { get => filePath; }

        List<Note> notes;

        public Music(string title, string filePath, double beatPerMinute, double offsetSeconds)
        {
            this.beatPerMinute = beatPerMinute;
            this.offsetSeconds = offsetSeconds;
            this.filePath = filePath;
            this.title = title;
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
                (int)(random.Next(displayWidth / 4) + displayWidth * (3.0 / 8.0)),
                (int)(random.Next(displayHeight / 4) + displayHeight * (3.0 / 8.0))
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
            switch (ie.Type)
            {
                case InputEventType.MouseMotion:
                    mousePosition = new Vector2(ie.X, ie.Y);
                    break;
                case InputEventType.MouseDown:
                case InputEventType.KeyDown:
                    if (ie.Type == InputEventType.MouseDown) mousePosition = new Vector2(ie.X, ie.Y);

                    List<Note> candidates = new List<Note>();

                    foreach (Note note in notes)
                    {
                        if (note.Visible && !note.Fired && note.IsPositionInsideArea((int)mousePosition.X, (int)mousePosition.Y))
                        {
                            candidates.Add(note);
                        }
                    }

                    if (candidates.Count == 0) return;

                    if (candidates.Count > 1) candidates.Sort((a, b) => Math.Sign(a.PositionBeats - b.PositionBeats));

                    candidates[0].Fire();
                    break;
            }
        }
    }
}
