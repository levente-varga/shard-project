using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shard
{
    class Scene
    {
        List<GameObject> gameObjects;
        string name;

        public Scene(string name)
        {
            gameObjects = new List<GameObject>();
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

        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
            Debug.Log($"Game object '{gameObject.GetType().ToString()}' added to scene");

        }

        public void RemoveGameObject(GameObject gameObject)
        {
            gameObjects.Remove(gameObject);
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
    }
}
