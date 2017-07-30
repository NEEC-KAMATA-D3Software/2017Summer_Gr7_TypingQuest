using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TypingQuest.Def;

namespace TypingQuest.EffectFolder
{
    class DamageEffect : EffectBase
    {
        private EffectParameter damageMatrix;
        private EffectParameter texture;

        public override void AddEffect(Effect effect)
        {
            this.effect = effect;
            Initialize();
        }
        public override void Initialize()
        {
            base.Initialize();

            projection.SetValue(Projection.projection);
            damageMatrix = effect.Parameters["DamageMatrix"];
            texture = effect.Parameters["Texture"];
        }
        public void Update(Texture2D writeTarget, float positionX, float positionY)
        {
            damageMatrix.SetValue(Matrix.CreateTranslation(positionX, positionY, 0));
            texture.SetValue(writeTarget);
        }
    }
}
