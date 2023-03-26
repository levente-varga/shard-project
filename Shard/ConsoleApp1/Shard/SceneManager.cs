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
        Scene loadedScene;
        List<SceneManagerCommand> commands;
        
        private SceneManager() 
        { 
            scenes = new List<Scene>();
            commands = new List<SceneManagerCommand>();
        }

        private static SceneManager instance;

        public Scene LoadedScene
        {
            get => instance.loadedScene;
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

        public bool DoesContainScene(Scene scene)
        {
            for (int i = 0; i < scenes.Count; i++)
            {
                if (scenes[i] == scene) return true;
            }

            return false;
        }

        public List<Scene> GetAllScenes()
        {
            return new List<Scene>(scenes);
        }

        public void AddScene(Scene scene)
        {
            Debug.Log($"Added new scene called '{scene.Name}'");

            scenes.Add(scene);
        }

        public void RemoveScene(Scene scene)
        {
            scenes.Remove(scene);
        }

        private void ImmediatelyLoadScene(Scene scene)
        {
            if (loadedScene != null) ImmediatelyUnloadScene(loadedScene);

            scene.Load();

            Debug.Log("Loaded scene changed! Status of scenes is the following:");
            List<Scene> scenes = GetInstance().GetAllScenes();
            foreach (Scene s in scenes) { Debug.Log($" {(s == loadedScene ? ">>>" : " - ")} {s}"); }

            loadedScene = scene;
        }

        private void ImmediatelyUnloadScene(Scene scene)
        {
            scene.Unload();
            loadedScene = null;
        }

        private void ImmediatelyAddGameObject(GameObject gameObject, Scene scene)
        {
            scene.AddGameObject(gameObject);

            

            //Debug.Log($"Game object {gameObject.GetType()} has been added to scene '{scene.Name}'");
        }

        private void ImmediatelyRemoveGameObject(GameObject gameObject, Scene scene)
        {
            scene.RemoveGameObject(gameObject);
        }

        public void PhysicsUpdate()
        {
            loadedScene.PhysicsUpdate();
        }

        public void PrePhysicsUpdate()
        {
            loadedScene.PrePhysicsUpdate();
        }

        public void Update()
        {
            //Debug.Log($"Updating scene '{loadedScene}'");
            loadedScene.Update();
        }



        public void LoadScene(Scene scene)
        {
            commands.Add(new LoadSceneCommand(scene));
            if (!scenes.Contains(scene)) AddScene(scene);
        }

        public void UnloadScene(Scene scene)
        {
            commands.Add(new UnloadSceneCommand(scene));
        }
        
        public void AddGameObject(GameObject gameObject)
        {
            commands.Add(new AddGameObjectToCurrentSceneCommand(gameObject));
        }

        public void AddGameObject(GameObject gameObject, Scene scene)
        {
            Debug.Log($"ADD command added for {gameObject.GetType()}");
            commands.Add(new AddGameObjectCommand(gameObject, scene));
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            commands.Add(new RemoveGameObjectFromCurrentSceneCommand(gameObject));
        }

        public void RemoveGameObject(GameObject gameObject, Scene scene)
        {
            commands.Add(new RemoveGameObjectCommand(gameObject, scene));
        }

        public void ExecuteCommands()
        {
            List<SceneManagerCommand> commands = new List<SceneManagerCommand>(this.commands);
            this.commands.Clear();

            foreach (SceneManagerCommand command in commands)
            {
                command.Execute();
            }

            foreach (Scene scene in scenes)
            {
                scene.ExecuteCommands();
            }
        }



        private abstract class SceneManagerCommand
        {
            public abstract void Execute();
        }

        private class AddGameObjectCommand : SceneManagerCommand
        {
            GameObject gameObject;
            Scene scene;

            public AddGameObjectCommand(GameObject gameObject, Scene scene)
            {
                this.gameObject = gameObject;
                this.scene = scene;
            }

            public override void Execute()
            {
                instance.ImmediatelyAddGameObject(gameObject, scene);
            }
        }

        private class AddGameObjectToCurrentSceneCommand : SceneManagerCommand
        {
            GameObject gameObject;

            public AddGameObjectToCurrentSceneCommand(GameObject gameObject)
            {
                this.gameObject = gameObject;
            }

            public override void Execute()
            {
                instance.ImmediatelyAddGameObject(gameObject, instance.LoadedScene);
            }
        }

        private class RemoveGameObjectCommand : SceneManagerCommand
        {
            GameObject gameObject;
            Scene scene;

            public RemoveGameObjectCommand(GameObject gameObject, Scene scene)
            {
                this.gameObject = gameObject;
                this.scene = scene;
            }

            public override void Execute()
            {
                instance.ImmediatelyRemoveGameObject(gameObject, scene);
            }
        }

        private class RemoveGameObjectFromCurrentSceneCommand : SceneManagerCommand
        {
            GameObject gameObject;

            public RemoveGameObjectFromCurrentSceneCommand(GameObject gameObject)
            {
                this.gameObject = gameObject;
            }

            public override void Execute()
            {
                instance.ImmediatelyRemoveGameObject(gameObject, instance.loadedScene);
            }
        }

        private class LoadSceneCommand : SceneManagerCommand
        {
            Scene scene;

            public LoadSceneCommand(Scene scene)
            {
                this.scene = scene;
            }

            public override void Execute()
            {
                instance.ImmediatelyLoadScene(scene);
            }
        }

        private class UnloadSceneCommand : SceneManagerCommand
        {
            Scene scene;

            public UnloadSceneCommand(Scene scene)
            {
                this.scene = scene;
            }

            public override void Execute()
            {
                instance.ImmediatelyUnloadScene(scene);
            }
        }
    }
}
