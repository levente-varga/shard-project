using SDL2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class Note : GameObject, InputListener
    {
        enum Score
        {
            None,       // no data yet
            Perfect,    // best time, most amount of points
            Great,      // reasonable time, normal amount of points
            Good,       // worst time, least amount of points
            Ok,         // no points, but not missed
            Miss        // no hit
        }

        const int HIGHLIGHT_SIZE = 120;
        const int FLARE_SIZE = 160;
        const int NOTE_SIZE = 50;
        const double DEFAULT_FADE_IN_DURATION_BEATS = 2;
        const double DEFAULT_FADE_OUT_DURATION_BEATS = 0.5;
        const double DEFAULT_FLARE_DURATION_BEATS = 1;
        
        double positionBeats;
        double fadeInDurationBeats;
        double fadeOutDurationBeats;
        double fadeInStart;
        double fadeOutEnd;
        double flareStart;
        double accuracy;
        bool fired = false;
        Score score;
        Music music;
        GameObject highlight;
        GameObject flare;

        public double FadeInDurationBeats
        {
            set => fadeInDurationBeats = value > 0 ? value : 1;
        }

        public double FadeOutDurationBeats
        {
            set => fadeOutDurationBeats = value > 0 ? value : 1;
        }

        public Note(Music music, double positionBeat, Vector2 position) 
        {
            this.music = music;
            this.positionBeats = positionBeat;
            FadeInDurationBeats = DEFAULT_FADE_IN_DURATION_BEATS;
            FadeOutDurationBeats = DEFAULT_FADE_OUT_DURATION_BEATS;
            fadeInStart = positionBeats - fadeInDurationBeats;
            fadeOutEnd = positionBeats + fadeOutDurationBeats;

            Transform.SetSize(NOTE_SIZE, NOTE_SIZE);
            Transform.Centre = position;
            Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("note.png");
            Layer = 3;

            SetupHighlight();

            Bootstrap.getInput().addListener(this);
        }

        private void SetupHighlight()
        {
            highlight = new GameObject();
            highlight.Transform.SetSize(HIGHLIGHT_SIZE, HIGHLIGHT_SIZE);
            highlight.Transform.Centre = Transform.Centre;
            highlight.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath("highlight.png");
            highlight.Layer = 2;
        }

        private void SetupFlare()
        {
            flare = new GameObject();
            flare.Transform.SetSize(NOTE_SIZE, NOTE_SIZE);
            flare.Transform.Centre = Transform.Centre;

            string flareName = score.ToString().ToLower() + "_flare.png";

            flare.Transform.SpritePath = Bootstrap.getAssetManager().getAssetPath(flareName);
            flare.Layer = 2;

            flareStart = music.PositionBeats;
        }

        private void UpdateFlare()
        {
            if (flare == null) return;

            double diffBeat = music.PositionBeats - flareStart;
            flare.Visible = diffBeat < DEFAULT_FLARE_DURATION_BEATS;
            if (!flare.Visible) return;

            double ratio = diffBeat / DEFAULT_FLARE_DURATION_BEATS;
            int currentFlareSize = NOTE_SIZE + (int)(ratio * (FLARE_SIZE - NOTE_SIZE));

            flare.Transform.SetSize((int)currentFlareSize, (int)currentFlareSize);
            flare.Transform.Centre = Transform.Centre;
            flare.Alpha = (int)(255 * 2 * Math.Pow((1 - ratio), 2));
        }

        public override void update()
        {
            UpdateFlare();

            bool visibleBefore = Visible;
            Visible = fadeInStart < music.PositionBeats && music.PositionBeats < fadeOutEnd;
            highlight.Visible = fadeInStart < music.PositionBeats && music.PositionBeats < positionBeats;

            if (music.PositionBeats > fadeOutEnd && !fired) Fail();

            if (!Visible) return;

            double diffBeat = music.PositionBeats - positionBeats;

            if (music.PositionBeats < positionBeats)
            {
                double ratio = -diffBeat / fadeInDurationBeats;
                Alpha = (int)(255 * Math.Pow((1 - ratio), 2.5));

                highlight.Alpha = Alpha;
                int currentHighlightSize = NOTE_SIZE + (int)(ratio * (HIGHLIGHT_SIZE - NOTE_SIZE));
                highlight.Transform.SetSize(currentHighlightSize, currentHighlightSize);
                highlight.Transform.Centre = Transform.Centre;
            }
            else
            {
                double ratio = diffBeat / fadeOutDurationBeats;
                Alpha = (int)(255 * Math.Pow((1 - ratio), 2));
            }
            Bootstrap.getDisplay().addToDraw(this);
        }

        private void Fire()
        {
            if (fired) return;
            fired = true;

            double positionSeconds = positionBeats / music.BeatPerSecond + music.OffsetSeconds;
            accuracy = Math.Abs(positionSeconds - music.PositionSeconds);

            if (accuracy < 0.05) score = Score.Perfect;
            else if (accuracy < 0.11) score = Score.Great;
            else if (accuracy < 0.18) score = Score.Good;
            else if (accuracy < 0.25) score = Score.Ok;
            else score = Score.Miss;

            Debug.Log($"Hit! Accuracy: {(accuracy * 1000).ToString("0")} ms   {score.ToString()}");

            SetupFlare();
        }

        private void Fail()
        {
            if (fired) return;
            fired = true;

            score = Score.Miss;
            Debug.Log($"Miss!");

            SetupFlare();
        }

        public void handleInput(InputEvent ie)
        {
            switch (ie.Type)
            {
                case InputEventType.MouseDown:
                    if (!Visible) break;

                    if (IsPositionInsideNote(ie.X, ie.Y)) {
                        Fire();
                    }
                    break;
            }
        }

        private bool IsPositionInsideNote(int x, int y) => Math.Pow(x - Transform.Centre.X, 2) + Math.Pow(y - Transform.Centre.Y, 2) < Math.Pow(NOTE_SIZE / 2, 2);
    }
}
