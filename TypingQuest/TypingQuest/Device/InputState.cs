using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace TypingQuest.Device
{
    class InputState
    {
        private float velocity = 0;

        private KeyboardState currentKey;
        private KeyboardState previousKey;

        public InputState()
        {
        }
        public float Velocity()
        {
            return velocity;
        }
        private void UpdateVelocity(KeyboardState keyState)
        {
            velocity = 0.0f;



            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                velocity += 1.0f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                velocity -= 1.0f;
            }
        }
        public void Update()
        {
            var keyState = Keyboard.GetState();

            UpdateVelocity(keyState);

            UpdateKey(keyState);
        }
        private void UpdateKey(KeyboardState keyState)
        {
            previousKey = currentKey;
            currentKey = keyState;
        }
        public bool IsKeyDown(Keys key)
        {
            bool current = currentKey.IsKeyDown(key);
            bool previous = previousKey.IsKeyDown(key);
            return current && !previous;
        }
        public bool GetKeyTrigger(Keys key)
        {
            return IsKeyDown(key);
        }
        public bool GetKeyState(Keys key)
        {
            return currentKey.IsKeyDown(key);
        }
    }
}
