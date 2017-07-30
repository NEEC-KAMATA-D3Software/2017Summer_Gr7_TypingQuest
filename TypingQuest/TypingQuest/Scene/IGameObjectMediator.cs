using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypingQuest.Scene
{
    interface IGameObjectMediator
    {
        void AddGameObject(GameObject gameObject);
        bool IsPlayerDead();
        GameObject GetPlayer();
        GameObject GetGameObject(GameObjectID id);

        List<GameObject> GetGameObjectList(GameObjectID id);
    }
}
