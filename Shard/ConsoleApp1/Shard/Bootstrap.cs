/*
*
*   The Bootstrap - this loads the config file, processes it and then starts the game loop
*   @author Michael Heron
*   @version 1.0
*   
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using SDL2;

namespace Shard
{
    class Bootstrap
    {
        public static string DEFAULT_CONFIG = "config.cfg";


        private static Game runningGame;
        private static Display displayEngine;
        private static Sound soundEngine;
        private static InputSystem input;
        private static PhysicsManager phys;
        private static AssetManagerBase asset;
        private static SceneManager scene;

        private static int targetFrameRate;
        private static int millisPerFrame;
        private static double deltaTime;
        private static double timeElapsed;
        private static int frames;
        private static List<long> frameTimes;
        private static long startTime;
        private static string baseDir;
        private static Dictionary<string,string> enVars;

        private static bool running = true;

        public static bool CheckEnvironmentalVariable (string id) {
            return enVars.ContainsKey (id);
        }

        
        public static string GetEnvironmentalVariable (string id) {
            if (CheckEnvironmentalVariable (id)) {
                return enVars[id];
            }

            return null;
        }


        public static double TimeElapsed { get => timeElapsed; set => timeElapsed = value; }

        public static string GetBaseDir() {
            return baseDir;
        }

        public static void Setup()
        {
            string workDir = Environment.CurrentDirectory;
            baseDir = Directory.GetParent(workDir).Parent.Parent.Parent.Parent.FullName;;

            SetupEnvironmentalVariables(baseDir + "\\" + "envar.cfg");
            Setup(baseDir + "\\" + DEFAULT_CONFIG);

        }

        public static void SetupEnvironmentalVariables (String path) {
                Console.WriteLine("Path is " + path);

                Dictionary<string, string> config = BaseFunctionality.GetInstance().ReadConfigFile(path);

                enVars = new Dictionary<string,string>();

                foreach (KeyValuePair<string, string> kvp in config)
                {
                    enVars[kvp.Key] = kvp.Value;
                }
        }
        public static double GetDeltaTime()
        {

            return deltaTime;
        }

        public static Display GetDisplay()
        {
            return displayEngine;
        }

        public static Sound GetSound()
        {
            return soundEngine;
        }

        public static InputSystem GetInput()
        {
            return input;
        }

        public static AssetManagerBase GetAssetManager() {
            return asset;
        }

        public static SceneManager GetSceneManager()
        {
            return scene;
        }

        public static Game GetRunningGame()
        {
            return runningGame;
        }

        public static void Setup(string path)
        {
            Console.WriteLine ("Path is " + path);

            Dictionary<string, string> config = BaseFunctionality.GetInstance().ReadConfigFile(path);
            Type t;
            object ob;
            bool bailOut = false;

            phys = PhysicsManager.GetInstance();

            foreach (KeyValuePair<string, string> kvp in config)
            {
                t = Type.GetType("Shard." + kvp.Value);

                if (t == null)
                {
                    Debug.GetInstance().Log("Missing Class Definition: " + kvp.Value + " in " + kvp.Key, Debug.DEBUG_LEVEL_ERROR);
                    Environment.Exit(0);
                }

                ob = Activator.CreateInstance(t);

                switch (kvp.Key)
                {
                    case "display":
                        displayEngine = (Display)ob;
                        displayEngine.Initialize();
                        break;
                    case "sound":
                        soundEngine = (Sound)ob;
                        break;
                    case "asset":
                        asset = (AssetManagerBase)ob;
                        asset.RegisterAssets();
                        break;
                    case "game":
                        runningGame = (Game)ob;
                        targetFrameRate = runningGame.GetTargetFrameRate();
                        millisPerFrame = 1000 / targetFrameRate;
                        break;
                    case "input":
                        input = (InputSystem)ob;
                        input.Initialize();
                        break;
                }

                scene = SceneManager.GetInstance();

                Debug.GetInstance().log("Config file... setting " + kvp.Key + " to " + kvp.Value);
            }

            if (runningGame == null)
            {
                Debug.GetInstance().Log("No game set", Debug.DEBUG_LEVEL_ERROR);
                bailOut = true;
            }

            if (displayEngine == null)
            {
                Debug.GetInstance().Log("No display engine set", Debug.DEBUG_LEVEL_ERROR);
                bailOut = true;
            }

            if (soundEngine == null)
            {
                Debug.GetInstance().Log("No sound engine set", Debug.DEBUG_LEVEL_ERROR);
                bailOut = true;
            }

            if (bailOut)
            {
                Environment.Exit(0);
            }
        }

        public static long GetCurrentMillis()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }

        public static int GetFPS()
        {
            int fps;
            double seconds;

            seconds = (GetCurrentMillis() - startTime) / 1000.0;

            fps = (int)(frames / seconds);

            return fps;
        }

        public static int GetSecondFPS()
        {
            int count = 0;
            long now = GetCurrentMillis();
            int lastEntry;



            // Debug.Log ("Frametimes is " + frameTimes.Count);

            if (frameTimes.Count == 0) {
                return -1;
            }

            lastEntry = frameTimes.Count - 1;

            while (frameTimes[lastEntry] > (now - 1000) && lastEntry > 0) {
                lastEntry -= 1;
                count += 1;
            }

            if (lastEntry > 0) {
                frameTimes.RemoveRange (0, lastEntry);
            }

            return count;
        }

        public static int GetCurrentFrame()
        {
            return frames;
        }

        static void Main(string[] args)
        {
            long timeInMillisecondsStart, lastTick, timeInMillisecondsEnd;
            long interval;
            int sleep;
            int tfro = 1;
            bool physUpdate = false;
            bool physDebug = false;



            // Setup the engine.
            Setup();

            // When we start the program running.
            startTime = GetCurrentMillis();
            frames = 0;
            frameTimes = new List<long>();
            // Start the game running.
            runningGame.Initialize();

            timeInMillisecondsStart = startTime;
            lastTick = startTime;

            phys.GravityModifier = 0.1f;
            // This is our game loop.

            if (GetEnvironmentalVariable("physics_debug") == "1")
            {
                physDebug = true;
            }

            while (running)
            {
                //Debug.Log($"------------------------------------");

                frames += 1;

                timeInMillisecondsStart = GetCurrentMillis();
                
                // Clear the screen.
                Bootstrap.GetDisplay().Clear();

                //Debug.Log("> Scene Management");
                // Scene management
                SceneManager.GetInstance().ExecuteCommands();

                // Sound
                GetSound().Update();

                //Debug.Log("> Game Update");
                // Update 
                runningGame.Update();
                // Input

                if (runningGame.IsRunning() == true)
                {
                    // Get input, which works at 50 FPS to make sure it doesn't interfere with the 
                    // variable frame rates.
                    Debug.Log("> Input");
                    running = input.GetInput();

                    // Update runs as fast as the system lets it.  Any kind of movement or counter 
                    // increment should be based then on the deltaTime variable.
                    //GameObjectManager.GetInstance().Update();
                    Debug.Log("> Scene Management");
                    SceneManager.GetInstance().Update();

                    // This will update every 20 milliseconds or thereabouts.  Our physics system aims 
                    // at a 50 FPS cycle.
                    if (phys.WillTick())
                    {
                        //GameObjectManager.GetInstance().PrePhysicsUpdate();
                        //Debug.Log("> Pre-Physics Update");
                        SceneManager.GetInstance().PrePhysicsUpdate();
                    }

                    // Update the physics.  If it's too soon, it'll return false.   Otherwise 
                    // it'll return true.
                    //Debug.Log("> General Physics Update");
                    physUpdate = phys.Update();

                    if (physUpdate)
                    {
                        // If it did tick, give every object an update
                        // that is pinned to the timing of the physics system.
                        //GameObjectManager.GetInstance().PhysicsUpdate();
                        //Debug.Log("> Physics Update");
                        SceneManager.GetInstance().PhysicsUpdate();
                    }

                    if (physDebug) {
                        phys.DrawDebugColliders();
                    }
                }

                //Debug.Log("> Present");
                

                // Render the screen.
                Bootstrap.GetDisplay().Present();

                timeInMillisecondsEnd = GetCurrentMillis();

                frameTimes.Add (timeInMillisecondsEnd);

                interval = timeInMillisecondsEnd - timeInMillisecondsStart;

                sleep = (int)(millisPerFrame - interval);


                TimeElapsed += deltaTime;

                if (sleep >= 0)
                {
                    // Frame rate regulator.  Bear in mind since this is millisecond precision, and we 
                    // only get whole numbers from our interval, it will only rarely match a target 
                    // FPS.  Milliseconds just aren't precise enough.
                    //
                    //  (I'm hinting if this bothers you, you might have found an engine modification to make...)

                    //Thread.Sleep(sleep);
                    SDL.SDL_Delay((uint)sleep);
                }

                timeInMillisecondsEnd = GetCurrentMillis();
                deltaTime = (timeInMillisecondsEnd - timeInMillisecondsStart) / 1000.0f;

                millisPerFrame = 1000 / targetFrameRate;

                lastTick = timeInMillisecondsStart;
            } 
        }
    }
}
