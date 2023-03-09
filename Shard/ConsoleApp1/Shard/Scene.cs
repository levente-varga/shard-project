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

        public Scene()
        {
            gameObjects = new List<GameObject>();
        }

        public void Load()
        {
            foreach (GameObject obj in gameObjects)
            {
                obj.initialize();
            }
        }

        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
        }
    }
}
