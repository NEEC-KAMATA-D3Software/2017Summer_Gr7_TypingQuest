using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TypingQuest.Device;

namespace TypingQuest.Actor.Magic
{
    class MagicSpell
    {
        protected string spell;
        public MagicSpell()
        {
            spell = string.Empty;
        }
        public void AddSpellString(string spellChar)
        {
            spell += spellChar;
        }
        public void Initialize()
        {
            spell = string.Empty;
        }
        public string SpellToString()
        {
            return spell;
        }
        public int SpellCount()
        {
            return spell.Length;
        }
    }
}
