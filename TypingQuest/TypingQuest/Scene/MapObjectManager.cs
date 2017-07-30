using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;

namespace TypingQuest.Scene
{
    class MapObjectManager : IGameObjectMediator
    {
        private List<GameObject> gameObjectList;
        private List<GameObject> addGameObjects;

        private GameDevice gameDevice;
        private Map map;
        public MapObjectManager(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            Initialize();
        }
        public void Initialize()
        {
            if (gameObjectList != null)
            {
                gameObjectList.Clear();
            }
            else
            {
                gameObjectList = new List<GameObject>();
            }
            if (addGameObjects != null)
            {
                addGameObjects.Clear();
            }
            else
            {
                addGameObjects = new List<GameObject>();
            }
        }
        public void Add(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }
            addGameObjects.Add(gameObject);
        }
        public void Add(Map map)
        {
            if (map == null)
            {
                return;
            }
            this.map = map;
        }
        private void HitToMap()
        {
            if (map == null)
            {
                return;
            }
            gameObjectList.ForEach((GameObject c) => map.Hit(c));
        }
        private void HitToGameObject()
        {
            foreach (var c1 in gameObjectList)
            {
                foreach (var c2 in gameObjectList)
                {
                    if (c1 == c2 || c1.IsDead() || c2.IsDead())
                    {
                        continue;
                    }
                    if (c1.Collision(c2))
                    {
                        c1.Hit(c2);
                        c2.Hit(c1);
                    }
                }
            }
        }

        public void CharaHitGameObject(GameObject chara)
        {
            foreach (var obj in gameObjectList)
            {
                if (obj.IsDead() || chara.IsDead())
                {
                    continue;
                }
                if (chara.Collision(obj))
                {
                    chara.Hit(obj);
                    obj.Hit(chara);
                }
            }
        }

        private void RemoveDeadCharacter()
        {
            gameObjectList.RemoveAll(c => c.IsDead());
        }


        public void Update(GameTime gameTime)
        {
            gameObjectList.ForEach((GameObject c) => c.Update(gameTime));
            addGameObjects.ForEach((GameObject c) => gameObjectList.Add(c));
            addGameObjects.Clear();

            //HitToMap();
            //HitToGameObject();

            RemoveDeadCharacter();
        }
        public void Draw(Renderer renderer)
        {
            gameObjectList.ForEach((GameObject c) => c.Draw(renderer));
        }
        public void AddGameObject(GameObject gameObject)
        {
            if (gameObject == null)
            {
                return;
            }
            addGameObjects.Add(gameObject);
        }

        public GameObject GetGameObject(GameObjectID id)
        {
            GameObject find = gameObjectList.Find(c => c.GetID() == id);
            if (find != null && !find.IsDead())
            {
                return find;
            }
            return null;
        }
        public List<GameObject> GetGameObjectList(GameObjectID id)
        {
            List<GameObject> list = gameObjectList.FindAll(c => c.GetID() == id);
            List<GameObject> aliveList = new List<GameObject>();
            foreach (var c in list)
            {
                if (!c.IsDead())
                {
                    aliveList.Add(c);
                }
            }
            return aliveList;
        }

        public bool IsPlayerDead()
        {
            throw new NotImplementedException();
        }

        public GameObject GetPlayer()
        {
            throw new NotImplementedException();
        }
    }
}
