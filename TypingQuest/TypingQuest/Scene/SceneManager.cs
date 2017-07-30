using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;

namespace TypingQuest.Scene
{
    class SceneManager
    {
        private Dictionary<Scene, IScene> scenes = new Dictionary<Scene, IScene>();

        private IScene currentScene = null;

        public SceneManager()
        {

        }

        public void Add(Scene name, IScene scene)
        {
            if(scenes.ContainsKey(name))
            {
                return;
            }
            scenes.Add(name, scene);
        }

        public void Change(Scene name)
        {
            if(currentScene != null)
            {
                currentScene.Shutdown();
            }
            currentScene = scenes[name];
            currentScene.Initialize();
        }

        public void Update(GameTime gameTime)
        {
            if(currentScene == null)
            {
                return;
            }

            currentScene.Update(gameTime);

            if(currentScene.IsEnd())
            {
                Change(currentScene.Next());
            }

        }

        public void Draw(Renderer renderer)
        {
            if(currentScene == null)
            {
                return;
            }
            currentScene.Draw(renderer);
        }
    }
}
