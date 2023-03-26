using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class SceneManager
    {
        List<Scene> scenes;
        string loadedSceneName = "";
        List<SceneCommand> commands;

        private class SceneCommand
        {
            public enum CommandType
            {
                LoadScene,
                UnloadScene,
                AddGameObject,
                RemoveGameObject,
            }

            string name;
            CommandType type;
            GameObject gameObject;

            public SceneCommand(string name, CommandType type, GameObject gameObject)
            {
                this.name = name;
                this.type = type;
                this.gameObject = gameObject;
            }

            public string SceneName => name;
            public CommandType Type => type;
            public GameObject GameObject => gameObject;
        }

        private SceneManager() 
        { 
            scenes = new List<Scene>();
            commands = new List<SceneCommand>();
        }

        private static SceneManager instance;

        public Scene LoadedScene
        {
            get { return instance.GetScene(loadedSceneName); }
        }

        public static SceneManager GetInstance()
        {
            if (instance == null)
            {
                instance = new SceneManager();
                Debug.Log($"Scene manager created.");
            }

            return instance;
        }

        public Scene GetScene(string name)
        {
            for (int i = 0; i < scenes.Count; i++)
            {
                if (scenes[i].Name == name) return scenes[i];
            }

            return null;
        }

        public List<Scene> GetAllScenes()
        {
            return new List<Scene>(scenes);
        }

        public void CreateScene(string name)
        {
            try
            {
                if (SceneExists(name))
                {
                    throw new Exception($"A scene with the name '{name}' already exists!");
                }

                if (name.Length == 0)
                {
                    throw new Exception($"A scene name must be at least 1 character long!");
                }

                foreach (char c in name)
                {
                    if (('a' > c || c > 'z') && ('A' > c || c > 'Z') && ('0' > c || c > '9'))
                    {
                        throw new Exception($"A scene name may only contain letters and numbers!");
                    }
                }
            }
            finally { }

            scenes.Add(new Scene(name));
        }

        public void DeleteScene(string name)
        {
            if (!SceneExists(name)) return;

            for (int i = 0; i < scenes.Count; i++)
            {
                if (scenes[i].Name == name)
                {
                    scenes.RemoveAt(i);
                    return;
                }
            }
        }

        private void LoadScene(string name)
        {
            if (!SceneExists(name) || name == loadedSceneName) return;

            if (loadedSceneName != "")
            {
                UnloadScene(loadedSceneName);
            }

            GetScene(name).Load();
            loadedSceneName = name;

            Debug.Log("Loaded scene changed! Status of scenes is the following:");
            List<Scene> scenes = GetInstance().GetAllScenes();
            foreach (Scene scene in scenes) { Debug.Log($" {(scene.Name == loadedSceneName ? ">>>" : " - ")} {scene.ToString()}"); }
        }

        private void UnloadScene(string name)
        {
            if (!SceneExists(name)) return;

            GetScene(name).Unload();
            loadedSceneName = "";
        }

        private void AddGameObject(GameObject gameObject) => AddGameObject(gameObject, loadedSceneName);
        private void AddGameObject(GameObject gameObject, string sceneName)
        {
            if (!SceneExists(sceneName)) return;

            GetScene(sceneName).AddGameObject(gameObject);
            if (loadedSceneName == sceneName) Bootstrap.GetInput().AddListener(gameObject);
        }

        private void RemoveGameObject(GameObject gameObject) => RemoveGameObject(gameObject, loadedSceneName);
        private void RemoveGameObject(GameObject gameObject, string sceneName)
        {
            if (!SceneExists(sceneName)) return;

            GetScene(sceneName).RemoveGameObject(gameObject);
        }

        private bool SceneExists(string name)
        {
            for (int i = 0; i < scenes.Count; i++)
            {
                if (scenes[i].Name == name)
                {
                    return true;
                }
            }

            return false;
        }

        public void PhysicsUpdate()
        {
            GetScene(loadedSceneName).PhysicsUpdate();
        }

        public void PrePhysicsUpdate()
        {
            GetScene(loadedSceneName).PrePhysicsUpdate();
        }

        public void Update()
        {
            //Debug.Log($"Updating scene '{loadedScene}'");
            GetScene(loadedSceneName).Update();
        }



        public void AskForLoadScene(string name)
        {
            commands.Add(new SceneCommand(name, SceneCommand.CommandType.LoadScene, null));
        }

        public void AskForUnloadScene(string name)
        {
            commands.Add(new SceneCommand(name, SceneCommand.CommandType.UnloadScene, null));
        }

        public void AskForAddGameObject(GameObject gameObject)
        {
            commands.Add(new SceneCommand("", SceneCommand.CommandType.AddGameObject, gameObject));
        }

        public void AskForRemoveGameObject(GameObject gameObject)
        {
            commands.Add(new SceneCommand("", SceneCommand.CommandType.RemoveGameObject, gameObject));
        }

        public void RunCommands()
        {
            List<SceneCommand> commands = new List<SceneCommand>(this.commands);
            this.commands.Clear();

            foreach (SceneCommand command in commands)
            {
                switch (command.Type) 
                {
                    case SceneCommand.CommandType.LoadScene:
                        LoadScene(command.SceneName);
                        break;
                    case SceneCommand.CommandType.UnloadScene:
                        UnloadScene(command.SceneName);
                        break;
                    case SceneCommand.CommandType.AddGameObject:
                        AddGameObject(command.GameObject);
                        break;
                    case SceneCommand.CommandType.RemoveGameObject:
                        RemoveGameObject(command.GameObject);
                        break;
                }
            }
        }
    }
}
