using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;

namespace TypingQuest.Scene
{
    interface IScene
    {
        void Initialize();
        void Update(GameTime gameTime);
        void Draw(Renderer renderer);
        void Shutdown();

        bool IsEnd();
        Scene Next();
    }
}
