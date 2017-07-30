using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Def;

namespace TypingQuest.Device
{
    class Camera
    {
        private Vector2 position;
        private GameDevice gameDevice;
        private Vector2 min;
        private Vector2 max;

        public Camera(GameDevice gameDevice)
        {
            position = Vector2.Zero;
            this.gameDevice = gameDevice;
        }
        public void Initialize()
        {

            position = Vector2.Zero;
        }
        public void SetCameraPosition(Vector2 position)
        {
            this.position = new Vector2(position.X - Screen.Width / 2, position.Y - Screen.Height / 2);
            min = new Vector2(Screen.Width / 2, Screen.Height / 2);
            max = new Vector2(gameDevice.GetStageLength() - Screen.Width / 2, gameDevice.GetStageHeight() - Screen.Height / 2 + Screen.UI);
            this.position = Vector2.Clamp(position, min, max);
        }
        public Vector2 GetDisplayModify()
        {
            return Vector2.Zero - position + new Vector2(Screen.Width / 2, Screen.Height / 2);
        }

        public void Scroll(Vector2 velocity)
        {
            float speed = velocity.Length() * 0.03f;
            velocity.Normalize();
            position += velocity * speed;
            this.position = Vector2.Clamp(position, min, max);
        }
        public Vector2 GetPosition()
        {
            return position;
        }
    }
}
