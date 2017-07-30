using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TypingQuest.Actor.Magic.ComboSystem;


namespace TypingQuest.Device
{
    class GameDevice
    {
        private Renderer renderer;
        private InputState input;
        private Sound sound;

        private Camera camera;
        private int stageLength;
        private int stageHeight;
        private ContentManager contentManager;


        private ComboSystem comboSystem;

        public GameDevice(ContentManager contentManager, GraphicsDevice graphicsDevice)
        {
            this.contentManager = contentManager;
            renderer = new Renderer(graphicsDevice, contentManager);
            sound = new Sound(contentManager);
            input = new InputState();

            comboSystem = new ComboSystem();

            camera = new Camera(this);
        }
        public void Initialize()
        {
        }
        public void Update()
        {
            input.Update();
            sound.RemoveSE();
        }
        public ContentManager GetContentManager()
        {
            return contentManager;
        }
        public Renderer GetRenderer()
        {
            return renderer;
        }
        public InputState GetInputState()
        {
            return input;
        }
        public Sound GetSound()
        {
            return sound;
        }
        public ComboSystem GetComboSystem()
        {
            return comboSystem;
        }

        #region Camera
        public void SetDisplayModify(Vector2 position)
        {
            camera.SetCameraPosition(position);
        }
        public Vector2 GetDisplayModify()
        {
            //return displayModify;
            return camera.GetDisplayModify();
        }
        public Vector2 GetDisplayModifyMap()
        {
            return camera.GetDisplayModify();
        }
        public Vector2 GetCameraPosition()
        {
            return camera.GetPosition();
        }
        public void ScrollCamera(Vector2 velocity)
        {
            camera.Scroll(velocity);
        }
        public void SetStageSize(int stageLength, int stageHeight)
        {
            this.stageLength = stageLength * 64;
            this.stageHeight = stageHeight * 64;
        }
        #endregion

        #region Stage
        public int GetStageLength()
        {
            return stageLength;
        }
        public int GetStageHeight()
        {
            return stageHeight;
        }
        #endregion
    }
}
