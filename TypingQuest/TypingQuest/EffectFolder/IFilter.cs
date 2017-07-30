using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace TypingQuest.EffectFolder
{
    public interface IFilter
    {
        void Initialize();

        void WriteRenderTarget();//ターゲット書き込み

        void ReleaseRenderTarget();//ターゲット解除
    }
}
