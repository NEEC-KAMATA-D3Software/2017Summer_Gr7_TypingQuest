using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;

namespace TypingQuest.Scene
{
    class Window : GameObject
    {
        private int number;

        public Window(Vector2 position, GameDevice gameDevice, int number)
            :base("window", position, 64, 64, gameDevice)
        {
            this.number = number;
        }
        public Window(Window other)
            :this(other.position, other.gameDevice, other.number)
        {
        }
        public override object Clone()
        {
            return new Window(this);
        }

        public override void Hit(GameObject gameObject)
        {
        }

        public override void Update(GameTime gameTime)
        {
        }
        public override void Draw(Renderer renderer)
        {
            renderer.DrawTexture(
                name,
                position + gameDevice.GetDisplayModifyMap(),
                new Rectangle((number % 3) * 64, (number / 3) * 64, 64, 64));
        }
    }
}
