using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using TypingQuest.Def;

namespace TypingQuest.EffectFolder
{
    abstract class EffectBase
    {
        protected Effect effect;
        protected EffectParameter projection;

        public abstract void AddEffect(Effect effect);

        public virtual void Initialize()
        {
            projection = effect.Parameters["Projection"];
            projection.SetValue(Projection.projection);

        }

        public Effect GetEffect()
        {
            return effect;
        }
    }
}
