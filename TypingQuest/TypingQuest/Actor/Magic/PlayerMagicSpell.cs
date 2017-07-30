using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using TypingQuest.Device;

namespace TypingQuest.Actor.Magic
{
    class PlayerMagicSpell : MagicSpell
    {
        private Keys[] pushedKeys;
        private InputState input;
        public PlayerMagicSpell(InputState input)
            :base()
        {
            spell = string.Empty;
            this.input = input;
        }
        public void Update()
        {
            AddSpellString();
        }
        private void AddSpellString()
        {
            if (spell.Length > 15)
            {
                return;
            }
            pushedKeys = Keyboard.GetState().GetPressedKeys();
            foreach (Keys key in pushedKeys)
            {
                if (input.GetKeyTrigger(key))
                {
                    if (key >= Keys.A && key <= Keys.Z)
                    {
                        spell += key.ToString();
                    }
                }
            }
        }
    }
}
