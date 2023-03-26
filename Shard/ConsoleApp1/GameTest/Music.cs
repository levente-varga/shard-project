using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class Music : GameObject
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
        List<Vector2> trailPositions = new List<Vector2>();
        List<double> trailTimeStamps = new List<double>();
        const double trailDurationSeconds = 0.2;

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

            Bootstrap.GetInput().AddListener(this);
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
            int displayWidth = Bootstrap.GetDisplay().GetWidth();
            int displayHeight = Bootstrap.GetDisplay().GetHeight();

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
            notes.ForEach(note => { note.Update(); });
        }

        public override void Update()
        {
            for (int i = trailPositions.Count - 1; i > 0; i--)
            {
                double elapsedSeconds = Bootstrap.TimeElapsed - trailTimeStamps[i];

                if (elapsedSeconds > trailDurationSeconds)
                {
                    trailTimeStamps.RemoveAt(i);
                    trailPositions.RemoveAt(i);
                    continue;
                }
            }

            for (int i = 0; i < trailPositions.Count - 1; i++)
            {
                Vector2 start = trailPositions[i];
                Vector2 end = trailPositions[i + 1];

                double elapsedSeconds = Bootstrap.TimeElapsed - trailTimeStamps[i];
                double strength = Math.Pow(1 - (elapsedSeconds / trailDurationSeconds), 2.0);
                Bootstrap.GetDisplay().DrawLine((int)start.X, (int)start.Y, (int)end.X, (int)end.Y, 255, 255, 255, (int)(255 * strength));
            }
        }

        private void HitNote(Vector2 p)
        {
            List<Note> candidates = new List<Note>();

            foreach (Note note in notes)
            {
                if (note.Visible && !note.Fired && note.IsPositionInsideArea((int)p.X, (int)p.Y))
                {
                    candidates.Add(note);
                }
            }

            if (candidates.Count == 0) return;

            if (candidates.Count > 1) candidates.Sort((a, b) => Math.Sign(a.PositionBeats - b.PositionBeats));

            candidates[0].Fire();
        }

        public override void HandleInput(InputEvent ie)
        {
            Debug.Log($"Music HandleInput");

            switch (ie.Type)
            {
                case InputEventType.MouseDown:
                    mousePosition = new Vector2(ie.X, ie.Y);
                    HitNote(mousePosition);
                    break;
                case InputEventType.MouseUp:
                    break;
            }
        }
    }
}
