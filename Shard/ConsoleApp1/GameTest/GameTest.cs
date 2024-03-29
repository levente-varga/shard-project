﻿using GameTest;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;

namespace Shard
{
    class GameTest : Game, InputListener
    {
        GameObject background;
        Music music;
        private Button scoreButton;
        private Action<string> selectedSongSetup;

        public override void Initialize()
        {
            LoadMenuScene();
            Bootstrap.GetInput().AddListener(this);
        }

        public void LoadGameScene(Action setupMusic = null)
        {
            Scene gameScene = new Scene();

            SceneManager.GetInstance().LoadScene(gameScene);

            CreateBackground();

            if (setupMusic != null)
            {
                setupMusic();
            }

            Button backToMenuButton = new Button(
                Bootstrap.GetDisplay().GetWidth() / 2 - 550,
                Bootstrap.GetDisplay().GetHeight() / 2 - 300,
                120, 44, "Back to menu", 16,
                Color.FromArgb(255, 255, 87, 51),
                Color.FromArgb(255, 255, 136, 34),
                Color.FromArgb(255, 255, 255, 255),
                Color.FromArgb(255, 255, 255, 255)
                );

            scoreButton = new Button(
                Bootstrap.GetDisplay().GetWidth() / 2 + 450,
                Bootstrap.GetDisplay().GetHeight() / 2 - 300,
                120, 44, "Score: " + ScoreManager.totalScorePoints.ToString(), 16,
                Color.FromArgb(255, 255, 87, 51),
                Color.FromArgb(255, 255, 136, 34),
                Color.FromArgb(255, 255, 255, 255),
                Color.FromArgb(255, 255, 255, 255)
                );

            backToMenuButton.OnClick += () =>
            {
                SceneManager.GetInstance().RemoveScene(gameScene);
                Bootstrap.GetSound().PauseMusic();
                ScoreManager.ResetScore();
                LoadMenuScene();
            };

            scoreButton.OnClick += () =>
            {
                
            };

            Bootstrap.GetSound().PlayMusic(music.FilePath);

            ScoreManager.OnScoreChanged += UpdateScoreText;
        }

        void UpdateScoreText(int newScore)
        {
            if (scoreButton == null)
            {
                return;
            }
            Console.WriteLine($"Updating score text to: {newScore}");
            scoreButton.SetText("Score: " + newScore.ToString());
        }

        public void LoadSongSelectionScene()
        {
            Scene songSelectionScene = new Scene();

            SceneManager.GetInstance().LoadScene(songSelectionScene);

            CreateBackground();

            int buttonSpacing = 10;
            int buttonWidth = 200;
            int buttonHeight = 44;
            int buttonYOffset = -110;

            Button chooseSongButton = new Button(
                Bootstrap.GetDisplay().GetWidth() / 2 - buttonWidth / 2,
                Bootstrap.GetDisplay().GetHeight() / 2 + buttonYOffset,
                buttonWidth, buttonHeight, "Choose your song", 16,
                Color.FromArgb(255, 255, 87, 51),
                Color.FromArgb(255, 255, 136, 34),
                Color.FromArgb(255, 255, 255, 255),
                Color.FromArgb(255, 255, 255, 255)
            );

            songSelectionScene.AddGameObject(chooseSongButton);

            Button song1Button = CreateLevelButton("Coldplay - Clocks", buttonYOffset + buttonHeight + buttonSpacing);
            Button song2Button = CreateLevelButton("Riot - Overkill", buttonYOffset + 2 * (buttonHeight + buttonSpacing));
            Button song3Button = CreateLevelButton("XX - XX", buttonYOffset + 3 * (buttonHeight + buttonSpacing));

            songSelectionScene.AddGameObject(song1Button);
            songSelectionScene.AddGameObject(song2Button);
            songSelectionScene.AddGameObject(song3Button);

            chooseSongButton.OnClick += () =>
            {

            };
            song1Button.OnClick += () =>
            {
                selectedSongSetup = (difficulty) =>
                {
                    if (difficulty == "Easy") SetupEasyClocks();
                    else if (difficulty == "Medium") SetupMediumClocks();
                    else if (difficulty == "Hard") SetupHardClocks();
                };
                LoadLevelSelectionScene();
            };
            song2Button.OnClick += () =>
            {
                selectedSongSetup = (difficulty) =>
                {
                    if (difficulty == "Easy") SetupEasyOverkill();
                    else if (difficulty == "Medium") SetupMediumOverkill();
                    else if (difficulty == "Hard") SetupHardOverkill();
                };
                LoadLevelSelectionScene();
            };
            song3Button.OnClick += () =>
            {
                selectedSongSetup = (difficulty) =>
                {
                    if (difficulty == "Easy") SetupEasyXX();
                    else if (difficulty == "Medium") SetupMediumXX();
                    else if (difficulty == "Hard") SetupHardXX();
                };
                LoadLevelSelectionScene();
            };
        }

        public void LoadLevelSelectionScene()
        {
            Scene levelSelectionScene = new Scene();

            SceneManager.GetInstance().LoadScene(levelSelectionScene);

            CreateBackground();

            int buttonSpacing = 10;
            int buttonWidth = 200;
            int buttonHeight = 44;
            int buttonYOffset = -110;

            Button chooseLevelButton = new Button(
                Bootstrap.GetDisplay().GetWidth() / 2 - buttonWidth / 2,
                Bootstrap.GetDisplay().GetHeight() / 2 + buttonYOffset,
                buttonWidth, buttonHeight, "Choose your difficulty", 16,
                Color.FromArgb(255, 255, 87, 51),
                Color.FromArgb(255, 255, 136, 34),
                Color.FromArgb(255, 255, 255, 255),
                Color.FromArgb(255, 255, 255, 255)
            );

            levelSelectionScene.AddGameObject(chooseLevelButton);

            Button easyButton = CreateLevelButton("Easy", buttonYOffset + buttonHeight + buttonSpacing);
            Button mediumButton = CreateLevelButton("Medium", buttonYOffset + 2 * (buttonHeight + buttonSpacing));
            Button hardButton = CreateLevelButton("Hard", buttonYOffset + 3 * (buttonHeight + buttonSpacing));

            levelSelectionScene.AddGameObject(easyButton);
            levelSelectionScene.AddGameObject(mediumButton);
            levelSelectionScene.AddGameObject(hardButton);

            chooseLevelButton.OnClick += () =>
            {

            };
            easyButton.OnClick += () =>
            {
                LoadGameWithDifficulty(levelSelectionScene, () => selectedSongSetup("Easy"));
            };
            mediumButton.OnClick += () =>
            {
                LoadGameWithDifficulty(levelSelectionScene, () => selectedSongSetup("Medium"));
            };
            hardButton.OnClick += () =>
            {
                LoadGameWithDifficulty(levelSelectionScene, () => selectedSongSetup("Hard"));
            };
        }

        private void LoadGameWithDifficulty(Scene levelSelectionScene, Action setupMusic)
        {
            SceneManager.GetInstance().RemoveScene(levelSelectionScene);
            LoadGameScene(setupMusic);
        }

        private Button CreateLevelButton(string text, int yOffset)
        {
            int buttonWidth = 120;
            int buttonHeight = 44;

            return new Button(
                Bootstrap.GetDisplay().GetWidth() / 2 - buttonWidth / 2,
                Bootstrap.GetDisplay().GetHeight() / 2 + yOffset,
                buttonWidth, buttonHeight, text, 16,
                Color.FromArgb(255, 255, 87, 51),
                Color.FromArgb(255, 255, 136, 34),
                Color.FromArgb(255, 255, 255, 255),
                Color.FromArgb(255, 255, 255, 255)
            );
        }
        private void LoadGameWithDifficulty(Scene levelSelectionScene)
        {
            SceneManager.GetInstance().RemoveScene(levelSelectionScene);
            LoadGameScene();
        }

        public void LoadMenuScene()
        {
            Scene menuScene = new Scene();

            SceneManager.GetInstance().LoadScene(menuScene);

            CreateBackground();

            Button startButton = new Button(
                    Bootstrap.GetDisplay().GetWidth() / 2 - 60,
                    Bootstrap.GetDisplay().GetHeight() / 2 - 22,
                    120, 44, "New game", 20,
                    Color.FromArgb(255, 255, 87, 51),
                    Color.FromArgb(255, 255, 136, 34),
                    Color.FromArgb(255, 255, 255, 255),
                    Color.FromArgb(255, 255, 255, 255)
                    );

            startButton.OnClick += () => {
                SceneManager.GetInstance().RemoveScene(menuScene);
                LoadSongSelectionScene();
            };
        }

        public override void Update()
        {
            if (music != null) music.PositionSeconds = Bootstrap.GetSound().MusicPosition;

            //Debug.Log(Bootstrap.getSound().MusicPosition + " / " + Bootstrap.getSound().MusicLength);
            //Bootstrap.getDisplay().showText("FPS: " + Bootstrap.getSecondFPS() + " / " + Bootstrap.getFPS(), 10, 10, 12, 255, 255, 255, 255);
            //Bootstrap.getDisplay().showText($"Position: {Bootstrap.getSound().MusicPosition} / {Bootstrap.getSound().MusicLength}", 10, 30, 12, 255, 255, 255, 255);
            //Bootstrap.getDisplay().showText($"Beat: {(int)music.PositionBeats + (int)(music.PositionBeats * 100) / 25 % 4 * 25 * 0.01 }", 10, 50, 12, 255, 255, 255, 255);
        }

        public override int GetTargetFrameRate()
        {
            return 75;

        }
        public void CreateBackground()
        {
            background = new GameObject();
            background.Transform.SpritePath = GetAssetManager().GetAssetPath ("rhythm_game_background.png");
            background.Transform.X = 0;
            background.Transform.Y = 0;
            background.Visible = true;
            background.Layer = 0;
            background.Transform.SetSize(
                Bootstrap.GetDisplay().GetWidth(),
                Bootstrap.GetDisplay().GetHeight());
        }

        public void HandleInput(InputEvent ie)
        {
            switch (ie.Type)
            {
                case InputEventType.MouseDown:
                    break;
            }
        }

        private void SetupEasyClocks()
        {
            int displayWidth = Bootstrap.GetDisplay().GetWidth();
            int displayHeight = Bootstrap.GetDisplay().GetHeight();
            Random random = new Random();

            music = new Music("Coldplay - Clocks", "clocks.wav", 131.0, 0.24);

            music.StartCreating();
            
            for (int i = 0; i < 600; i += 4)
            {
                music.AddNoteAndPause(1.5);
                music.AddNoteAndPause(1.5);
                music.AddNoteAndPause(1);
            }
        }

        private void SetupMediumClocks()
        {
            int displayWidth = Bootstrap.GetDisplay().GetWidth();
            int displayHeight = Bootstrap.GetDisplay().GetHeight();
            Random random = new Random();

            music = new Music("Coldplay - Clocks", "clocks.wav", 131.0, 0.24 + 0.10);

            music.StartCreating();

            music.AddPause(16);
            
            music.AddNoteAndPause(
                4, 
                Bootstrap.GetDisplay().GetWidth() / 2,
                Bootstrap.GetDisplay().GetHeight() / 2
                );

            music.AddNoteAndPause(4);
            music.AddNoteAndPause(4);
            music.AddNoteAndPause(4);

            //return;

            for (int i = 0; i < 8; i++)
            {
                music.AddNoteAndPause(1.5, 500, 400);
                music.AddNoteAndPause(1.5, 600, 400);
                music.AddNoteAndPause(1.0, 700, 400);
            }

            // Refrain 1

            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(0.5);

            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(2.5);



            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);

            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(2.5);



            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);

            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(2.5);



            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);

            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(0.5);



            music.AddPause(1.0);     
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);

            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(2.5);



            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);

            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(0.5);



            music.AddPause(1.0);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);

            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(2.5);



            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);

            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(0.5);



            for (int i = 0; i < 16; i++)
            {
                music.AddNoteAndPause(1.5);
                music.AddNoteAndPause(1.5);
                music.AddNoteAndPause(1.0);
            }



            // Refrain 2

            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(2.0);


            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(2.5);



            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);

            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(0.5);



            music.AddPause(1.0);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);

            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(2.0);
            music.AddNoteAndPause(0.5);



            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);

            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(0.5);



            music.AddPause(1.0);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);

            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(2.5);



            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);

            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(0.5);



            music.AddPause(1.0);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);

            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(2.0);
            music.AddNoteAndPause(0.5);



            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);

            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(0.5);
            music.AddNoteAndPause(0.5);



            for (int i = 0; i < 16; i++)
            {
                music.AddNoteAndPause(1.5);
                music.AddNoteAndPause(1.5);
                music.AddNoteAndPause(1.0);
            }



            for (int i = 0; i < 16; i++)
            {
                music.AddNoteAndPause(1.5, 300, 300);
               
                music.AddNoteAndPause(1.5, 400, 300);
                
                music.AddNoteAndPause(0.5, 500, 300);
                music.AddNoteAndPause(0.5, 400, 300);
            }
        }

        private void SetupHardClocks()
        {
            int displayWidth = Bootstrap.GetDisplay().GetWidth();
            int displayHeight = Bootstrap.GetDisplay().GetHeight();
            Random random = new Random();

            music = new Music("Coldplay - Clocks", "clocks.wav", 131.0, 0.24 + 0.10);

            music.StartCreating();

            music.AddNoteAndPause(12);
            music.AddNoteAndPause(4);

        }
        private void SetupEasyOverkill()
        {
            int displayWidth = Bootstrap.GetDisplay().GetWidth();
            int displayHeight = Bootstrap.GetDisplay().GetHeight();
            Random random = new Random();

            music = new Music("RIOT - Overkill", "overkill.wav", 174.0, 4.90 + 0.07);

            music.StartCreating();

            for (int i = 0; i < 600; i += 4)
            {
                music.AddNoteAndPause(1.5);
                music.AddNoteAndPause(1.5);
                music.AddNoteAndPause(1);
            }
        }

        private void SetupMediumOverkill()
        {
            int displayWidth = Bootstrap.GetDisplay().GetWidth();
            int displayHeight = Bootstrap.GetDisplay().GetHeight();
            Random random = new Random();

            music = new Music("RIOT - Overkill", "overkill.wav", 174.0, 4.90 + 0.07);

            music.StartCreating();

            music.AddPause(64);
            music.AddNoteAndPause(12);
            music.AddNoteAndPause(4);
            music.AddNoteAndPause(8);
            music.AddNoteAndPause(8);

            music.AddNoteAndPause(12);
            music.AddNoteAndPause(4);
            music.AddNoteAndPause(8);
            music.AddNoteAndPause(8);

            music.AddNoteAndPause(12);
            music.AddNoteAndPause(4);
            music.AddNoteAndPause(8);
            music.AddNoteAndPause(8);



            music.AddNoteAndPause(0.25, 400, 400);
            music.AddNoteAndPause(0.25, 470, 400);
            music.AddNoteAndPause(1.0, 540, 400);

            music.AddNoteAndPause(0.25, 800, 600);
            music.AddNoteAndPause(0.25, 870, 600);
            music.AddNoteAndPause(6.0, 940, 600);

            music.AddNoteAndPause(0.25, 900, 500);
            music.AddNoteAndPause(0.25, 830, 500);
            music.AddNoteAndPause(1.0, 760, 500);

            music.AddNoteAndPause(0.25, 500, 300);
            music.AddNoteAndPause(0.25, 430, 300);
            music.AddNoteAndPause(6.0, 360, 300);

            music.AddNoteAndPause(0.25, 900, 500);
            music.AddNoteAndPause(0.25, 880, 450);
            music.AddNoteAndPause(1.0, 860, 400);

            music.AddNoteAndPause(0.25, 800, 400);
            music.AddNoteAndPause(0.25, 780, 450);

            music.AddNoteAndPause(0.5, 760, 500);
            music.AddNoteAndPause(0.5, 720, 400);
            music.AddNoteAndPause(0.5, 680, 500);
            music.AddNoteAndPause(0.5, 640, 400);

            music.AddNoteAndPause(0.5, 600, 500);
            music.AddNoteAndPause(0.5, 560, 400);
            music.AddNoteAndPause(0.5, 520, 500);
            music.AddNoteAndPause(0.5, 480, 400);

            music.AddNoteAndPause(0.5, 440, 500);
            music.AddNoteAndPause(0.5, 400, 400);
            music.AddNoteAndPause(0.5, 360, 500);
            music.AddNoteAndPause(0.5, 320, 400);

            music.AddNoteAndPause(8.0, 280, 500);

            music.AddNoteAndPause(3.5);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(0.5);

            music.AddPause(3.5);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(0.5);

            music.AddPause(3.5);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(0.5);

            music.AddPause(8.0);

            music.AddPause(3.5);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(0.5);

            music.AddPause(3.5);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(0.5);

            music.AddPause(3.5);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(1.5);
            music.AddNoteAndPause(1.0);
            music.AddNoteAndPause(0.5);
        }


        private void SetupHardOverkill()
        {
            int displayWidth = Bootstrap.GetDisplay().GetWidth();
            int displayHeight = Bootstrap.GetDisplay().GetHeight();
            Random random = new Random();

            music = new Music("RIOT - Overkill", "overkill.wav", 174.0, 4.90 + 0.07);

            music.StartCreating();

            music.AddNoteAndPause(4);
            music.AddNoteAndPause(8);
            music.AddNoteAndPause(8);

        }
        private void SetupEasyXX()
        {
            int displayWidth = Bootstrap.GetDisplay().GetWidth();
            int displayHeight = Bootstrap.GetDisplay().GetHeight();
            Random random = new Random();

            music = new Music("RIOT - Overkill", "overkill.wav", 174.0, 4.90 + 0.07);

            music.StartCreating();

            for (int i = 0; i < 600; i += 4)
            {
                music.AddNoteAndPause(1.5);
                music.AddNoteAndPause(1.5);
                music.AddNoteAndPause(1);
            }
        }

        private void SetupMediumXX()
        {
            int displayWidth = Bootstrap.GetDisplay().GetWidth();
            int displayHeight = Bootstrap.GetDisplay().GetHeight();
            Random random = new Random();

            music = new Music("RIOT - Overkill", "overkill.wav", 174.0, 4.90 + 0.07);

            music.StartCreating();


            music.AddNoteAndPause(
                4,
                Bootstrap.GetDisplay().GetWidth() / 2,
                Bootstrap.GetDisplay().GetHeight() / 2
                );

            music.AddNoteAndPause(4);
            music.AddNoteAndPause(4);
            music.AddNoteAndPause(4);


        }

        private void SetupHardXX()
        {
            int displayWidth = Bootstrap.GetDisplay().GetWidth();
            int displayHeight = Bootstrap.GetDisplay().GetHeight();
            Random random = new Random();

            music = new Music("RIOT - Overkill", "overkill.wav", 174.0, 4.90 + 0.07);

            music.StartCreating();

            music.AddNoteAndPause(4);
            music.AddNoteAndPause(8);
            music.AddNoteAndPause(8);

        }
    }
}
