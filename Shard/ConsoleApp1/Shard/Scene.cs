using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Formats.Asn1.AsnWriter;

namespace Shard
{
    class Scene
    {
        List<GameObject> gameObjects;
        List<SceneCommand> commands;
        string name;

        public Scene(string name = "")
        {
            gameObjects = new List<GameObject>();
            commands = new List<SceneCommand>();
            this.name = name;
        }

        public string Name { get => name; }

        public List<GameObject> GameObjects { get => gameObjects; }

        public void Load()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].Initialize();
                Bootstrap.GetInput().AddListener(gameObjects[i]);
            }
        }

        private void ImmediatelyAddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
            //Debug.Log($"Game object '{gameObject.GetType()}' added to scene");

        }

        private void ImmediatelyRemoveGameObject(GameObject gameObject)
        {
            gameObjects.Remove(gameObject);
        }

        public void AddGameObject(GameObject gameObject)
        {
            commands.Add(new AddGameObjectCommand(gameObject, this));

            Bootstrap.GetInput().RemoveListener(gameObject);
            if (SceneManager.GetInstance().LoadedScene == this)
            {
                Bootstrap.GetInput().AddListener(gameObject);
            }
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            commands.Add(new RemoveGameObjectCommand(gameObject, this));

            if (SceneManager.GetInstance().LoadedScene == this)
            {
                Bootstrap.GetInput().RemoveListener(gameObject);
            }
        }

        public void Update()
        {
            //Debug.Log($"Updating {gameObjects.Count} game objects...");

            List<GameObject> toBeDestroyed = new List<GameObject>();

            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (gameObjects[i].ToBeDestroyed)
                {
                    toBeDestroyed.Add(gameObjects[i]);
                }
                else
                {
                    gameObjects[i].Update();
                }
            }

            for (int i = 0; i < toBeDestroyed.Count; i++)
            {
                toBeDestroyed[i].OnDestroy();
                gameObjects.Remove(toBeDestroyed[i]);
                Debug.Log("Game object destroyed!");
            }

            toBeDestroyed.Clear();
        }

        public void PhysicsUpdate()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].PhysicsUpdate();
            }
        }

        public void PrePhysicsUpdate()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                gameObjects[i].PrePhysicsUpdate();
            }
        }

        public void Unload()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                Bootstrap.GetInput().RemoveListener(gameObjects[i]);
                //gameObjects[i].OnDestroy();
            }
        }

        public override string ToString()
        {
            return $"Scene '{name}' contains {gameObjects.Count} game objects";
        }

        public void ExecuteCommands()
        {
            List<SceneCommand> commands = new List<SceneCommand>(this.commands);
            this.commands.Clear();

            foreach (SceneCommand command in commands)
            {
                command.Execute();
            }
        }



        private abstract class SceneCommand
        {
            protected Scene scene;

            public SceneCommand(Scene scene)
            {
                this.scene = scene;
            }

            public abstract void Execute();
        }

        private class AddGameObjectCommand : SceneCommand
        {
            GameObject gameObject;
            

            public AddGameObjectCommand(GameObject gameObject, Scene scene) : base(scene)
            {
                this.gameObject = gameObject;
            }

            public override void Execute()
            {
                scene.ImmediatelyAddGameObject(gameObject);
            }
        }

        private class RemoveGameObjectCommand : SceneCommand
        {
            GameObject gameObject;
            string sceneName;

            public RemoveGameObjectCommand(GameObject gameObject, Scene scene) : base(scene)
            {
                this.gameObject = gameObject;
            }

            public override void Execute()
            {
                scene.ImmediatelyRemoveGameObject(gameObject);
            }
        }
    }
}
