using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Utility;
using TypingQuest.Device;

namespace TypingQuest.Scene
{
    class SceneFader : IScene
    {
        public enum SceneFadeState
        {
            In,
            Out,
            None
        };

        private Timer timer;
        private static float FADE_TIMER = 2.0f;
        private SceneFadeState firstState; 
        private SceneFadeState state;
        private IScene scene;
        private bool isEnd = false;

        public SceneFader(IScene scene, SceneFadeState state = SceneFadeState.In)
        {
            this.scene = scene;
            firstState = state;
            this.state = state;
        }

        public void Initialize()
        {
            state = firstState;
            scene.Initialize();
            timer = new Timer(FADE_TIMER);
            timer.Initialize();
            isEnd = false;
        }

        public void UpdateFadeIn(GameTime gameTime)
        {
            scene.Update(gameTime);
            if(scene.IsEnd())
            {
                state = SceneFadeState.Out;
            }

            timer.Update();
            if(timer.IsTime())
            {
                state = SceneFadeState.None;
            }
        }

        public void UpdateFadeOut(GameTime gameTime)
        {
            //scene.Update(gameTime);
            if(scene.IsEnd())
            {
                state = SceneFadeState.Out;
            }

            timer.Update();
            if(timer.IsTime())
            {
                isEnd = true;
            }
        }

        public void UpdateFadeNone(GameTime gameTime)
        {
            scene.Update(gameTime);
            if(scene.IsEnd())
            {
                state = SceneFadeState.Out;

                timer.Initialize();
            }
        }

        public void Update(GameTime gameTime)
        {
            switch (state)
            {
                case SceneFadeState.In:
                    UpdateFadeIn(gameTime);
                    break;

                case SceneFadeState.Out:
                    UpdateFadeOut(gameTime);
                    break;

                case SceneFadeState.None:
                    UpdateFadeNone(gameTime);
                    break;

            }
        }

        private void DrawEffect(Renderer renderer, float alpha)
        {
            renderer.Begin();
            renderer.DrawTexture(
                "fade",
                Vector2.Zero,
                new Vector2(1024, 768),
                alpha);

            renderer.End();
        }

        private void DrawFadeIn(Renderer renderer)
        {
            scene.Draw(renderer);
            DrawEffect(renderer, timer.Rate());
        }

        private void DrawFadeOut(Renderer renderer)
        {
            scene.Draw(renderer);
            DrawEffect(renderer, 1.0f - timer.Rate());
        }

        private void DrawFadeNone(Renderer renderer)
        {
            scene.Draw(renderer);
        }

        public bool IsEnd()
        {
            return isEnd;
        }

        public Scene Next()
        {
            return scene.Next();
        }

    

        public void Draw(Renderer renderer)
        {
            switch(state)
            {
                case SceneFadeState.In:
                    DrawFadeIn(renderer);
                    break;

                case SceneFadeState.Out:
                    DrawFadeOut(renderer);
                    break;

                case SceneFadeState.None:
                    DrawFadeNone(renderer);
                    break;
            }
        }

        public void Shutdown()
        {
            scene.Shutdown();
        }
    }
}
