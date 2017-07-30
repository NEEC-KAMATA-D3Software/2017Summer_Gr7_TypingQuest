using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TypingQuest.Def
{
    static class Projection
    {
        public static readonly Matrix projection = Matrix.CreateOrthographicOffCenter(
            0,
            Screen.Width,
            Screen.Height,
            0,
            0, 1);
    }
}
