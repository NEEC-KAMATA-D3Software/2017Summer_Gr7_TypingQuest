using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using TypingQuest.Device;

namespace TypingQuest.EffectFolder
{
    class HpEffect : EffectBase
    {
        private GameDevice gameDevice;

        private float scrollSpeed;
        private EffectParameter speed;
        public HpEffect(GameDevice gameDevice)
        {
            this.gameDevice = gameDevice;
            effect = gameDevice.GetContentManager().Load<Effect>("./hpEffect");

            Initialize();
        }
        public override void AddEffect(Effect effect)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            speed = effect.Parameters["Speed"];

            scrollSpeed = 0.3f;
        }
        public void Update()
        {
            scrollSpeed += 0.006f;
            if (scrollSpeed >= 0.8f)
            {
                scrollSpeed = 0.3f;
            }
            speed.SetValue(scrollSpeed);
        }
    }
}
