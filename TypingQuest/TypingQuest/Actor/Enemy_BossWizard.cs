using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TypingQuest.Scene;
using TypingQuest.Device;
using TypingQuest.Actor.Magic;
using TypingQuest.Utility;
using TypingQuest.Def;
using TypingQuest.EffectFolder;

namespace TypingQuest.Actor
{
    enum BossMode
    {
        Fire,
        Water,
        Wind,
        Rock,
        Gravity,
        Heal,
        Cyclone,
        Lightning
    }
    class Enemy_BossWizard : Character
    {
        private AI ai;
        private BossMode mode;
        private int saveDistance;
        private MagicSpell magicSpell;

        private Timer timer;
        private string magic;
        private List<string> magicList;
        private int nowSpell;
        private bool triggerMagic;
        private int magicCnt;
        private bool heightLimit;

        private Player player;

        private Motion motion;
        private Vector2 prevPosition;
        private Vector2 thirdPosition;
        private Timer effectTimer;
        float speed = 2.5f;

        public Enemy_BossWizard(Vector2 position, GameDevice gameDevice, MagicManager magicManager, Player player)
            : base("BOSSplayer_motion", position, 64, 64, gameDevice, magicManager)
        {
            maxHp = 1200;
            hp = maxHp;

            magicSpell = new MagicSpell();
            triggerMagic = false;
            heightLimit = true;
            timer = new Timer(0.2f);

            magicList = new List<string>();
            magicList.Add("FIRE ");
            magicList.Add("WATER ");
            magicList.Add("WIND ");
            magicList.Add("ROCK ");
            magicList.Add("GRAVITY ");
            magicList.Add("HEAL ");
            magicList.Add("CYCLONE ");
            magicList.Add("LIGHTNING ");

            mode = BossMode.Rock;
            magic = magicList[(int)mode];
            saveDistance = 200;
            ai = new Boss_TraceAI(player, saveDistance);

            this.player = player;

            motion = new Motion();
            effectTimer = new Timer(0.2f);

            Initialize();
        }

        public override void Initialize()
        {
            magicSpell.Initialize();
            nowSpell = 0;
            timer.Initialize();
            magicCnt = 0;

            for (int i = 0; i < 4; i ++)
            {
                motion.Add(i, new Rectangle(64 * i, 0, 64, 64));
            }
            motion.Initialize(new Range(0, 3), new Timer(0.2f));
            direct = Direct.Right;
            prevPosition = position;
            thirdPosition = position;
            effectTimer.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (hp <= 0)
            {
                isDead = true;
            }

            if (canMove)
            {
                if(!(ai is LazyAI))
                velocity = ai.Think(this);
                if (velocity.Length() > 0)
                {
                    velocity.Normalize();
                }


                if ((position - player.GetPosition()).X < 0)
                {
                    direct = Direct.Right;
                }
                else
                {
                    direct = Direct.Left;
                }
            }

            UpdateState(gameTime);

            //移動系状態異常があるためvelocityとpositionの間に行います

            if (canMove)
            {

                position += (velocity * speed);
            }
            else
            {
                position.X = position.X + velocity.X;
            }

            canMove = true;

            timer.Update();
            JudgeTrigger();
            SpellMagic();

            UpdateEffect();
            motion.Update(gameTime);

            var min = new Vector2(64, 0);
            var max = new Vector2(64 * 21, 64 * 19);
            position = Vector2.Clamp(position, min, max);
        }
        private void UpdateEffect()
        {
            effectTimer.Update();
            if (effectTimer.IsTime())
            {
                effectTimer.Initialize();
                thirdPosition = prevPosition;
                prevPosition = position;
            }
        }
        public void SpellMagic()
        {
            if (timer.IsTime())
            {
                if (nowSpell < magic.Length - 1)
                {
                    magicSpell.AddSpellString(magic[nowSpell].ToString());
                    nowSpell++;
                }
                else if (nowSpell == magic.Length - 1)
                {
                    if (!triggerMagic)
                    {
                        return;
                    }
                    if ((position - player.GetPosition()).X > 0)
                    {
                        direct = Direct.Left;
                    }
                    else
                    {
                        direct = Direct.Right;
                    }
                    LaunchMagic(magicSpell.SpellToString());
                    magicCnt++;
                    nowSpell++;

                }
                else if (nowSpell >= magic.Length)
                {
                    magicSpell.Initialize();
                    nowSpell = 0;
                    Pattern();
                }
                timer.Initialize();
            }
        }

        private void JudgeTrigger()
        {
            if (mode == BossMode.Heal)
            {
                triggerMagic = true;
                return;
            }
            if ((player.GetPosition() - position).Length() < saveDistance)
            {
                if (heightLimit)
                {
                    if (Math.Abs((player.GetPosition() - position).Y) < 30)
                    {
                        triggerMagic = true;
                        return;
                    }
                    triggerMagic = false;
                    return;

                }
                else
                {
                    triggerMagic = true;
                    return;
                }
            }
            else if (magicCnt > 2)
            {
                Vector2 temp = new Vector2(64 * 11, 64 * 13);
                if ((position - temp).Length() < 65)
                {
                    triggerMagic = true;
                }
            }
            triggerMagic = false;
        }

        private void Pattern()
        {
            speed = 2.5f;
            //SpecialSkill 条件 HP > 6%
            if (magicCnt > 2 && hp > maxHp / 15.0f)
            {
                Special();
                if (magicCnt == 12)
                {
                    magicCnt = -1;
                }
                return;
            }
            //HP > 85%
            if (hp > maxHp * 17 / 20.0f)
            {
                switch (magicCnt)
                {
                    case 0:
                        mode = BossMode.Rock;
                        magic = magicList[(int)mode];
                        saveDistance = 400;
                        heightLimit = true;
                        ai = new Boss_TraceAI(player, saveDistance);
                        timer = new Timer(0.3f);
                        break;
                    case 1:
                        mode = BossMode.Wind;
                        magic = magicList[(int)mode];
                        saveDistance = 300;
                        heightLimit = true;
                        ai = new Boss_TraceAI(player, saveDistance);
                        timer = new Timer(0.3f);
                        break;
                    case 2:
                        mode = BossMode.Fire;
                        SwitchMode();
                        magicCnt = -1;
                        break;
                }
            }
            //HP > 70%
            else if (hp > maxHp * 7.0f / 10.0f)
            {
                switch (magicCnt)
                {
                    case 0:
                        mode = BossMode.Rock;
                        break;
                    case 1:
                        mode = BossMode.Wind;
                        break;
                    case 2:
                        mode = BossMode.Fire;
                        break;
                }
                SwitchMode();
            }
            //HP > 50%
            else if (hp > maxHp / 2.0f)
            {
                switch (magicCnt)
                {
                    case 0:
                        mode = BossMode.Gravity;
                        break;
                    case 1:
                        mode = BossMode.Wind;
                        break;
                    case 2:
                        mode = BossMode.Fire;
                        break;
                }
                SwitchMode();
            }
            //HP > 30%
            else if (hp > maxHp * 3.0f / 10.0f)
            {
                switch (magicCnt)
                {
                    case 0:
                        mode = BossMode.Cyclone;
                        SwitchMode();
                        break;

                    case 1:
                        mode = BossMode.Lightning;
                        SwitchMode();
                        break;

                    case 2:
                        mode = BossMode.Fire;
                        magic = magicList[(int)mode];
                        saveDistance = 1000;
                        heightLimit = false;
                        ai = new Boss_TraceAI(player, saveDistance);
                        timer = new Timer(0.1f);
                        break;
                }
            }
            else
            {
                SwitchModeRandom();
            }
        }

        private void Special()
        {
            if (magicCnt < 12)
            {
                mode = BossMode.Fire;
                magic = magicList[(int)mode];
                saveDistance = 3000;
                heightLimit = false;
                ai = new LazyAI(player);
                position = new Vector2(64 * 11, 64 * 13);
                timer = new Timer(0.001f);
                return;
            }
            mode = BossMode.Water;
            speed = 6f;
            magic = magicList[(int)mode];
            saveDistance = 500;
            heightLimit = true;
            ai = new Boss_TraceAI(player, saveDistance);
            timer = new Timer(0.01f);
        }

        public void SwitchMode()
        {
            switch (mode)
            {
                case BossMode.Fire:
                    magic = magicList[(int)mode];
                    saveDistance = 800;
                    heightLimit = false;
                    ai = new Boss_TraceAI(player, saveDistance);
                    timer = new Timer(0.2f);
                    break;
                case BossMode.Gravity:
                    magic = magicList[(int)mode];
                    saveDistance = 200;
                    heightLimit = true;
                    ai = new Boss_TraceAI(player, saveDistance);
                    timer = new Timer(0.15f);
                    break;
                case BossMode.Rock:
                    magic = magicList[(int)mode];
                    saveDistance = 400;
                    heightLimit = true;
                    ai = new Boss_TraceAI(player, saveDistance);
                    timer = new Timer(0.15f);
                    break;
                case BossMode.Water:
                    magic = magicList[(int)mode];
                    saveDistance = 400;
                    heightLimit = true;
                    ai = new Boss_TraceAI(player, saveDistance);
                    timer = new Timer(0.1f);
                    break;
                case BossMode.Wind:
                    magic = magicList[(int)mode];
                    saveDistance = 300;
                    heightLimit = true;
                    ai = new Boss_TraceAI(player, saveDistance);
                    timer = new Timer(0.1f);
                    break;
                case BossMode.Heal:
                    magic = magicList[(int)mode];
                    saveDistance = 1000;
                    heightLimit = false;
                    ai = new EscapeAI(player, saveDistance);
                    timer = new Timer(2.0f);
                    break;
                case BossMode.Cyclone:
                    magic = magicList[(int)mode];
                    saveDistance = 128;
                    heightLimit = false;
                    ai = new Boss_TraceAI(player, saveDistance);
                    timer = new Timer(0.1f);
                    break;
                case BossMode.Lightning:
                    mode = BossMode.Lightning;
                    magic = magicList[(int)mode];
                    saveDistance = 500;
                    heightLimit = false;
                    ai = new Boss_TraceAI(player, saveDistance);
                    timer = new Timer(0.1f);
                    break;
            }

        }
        public void SwitchModeRandom()
        {
            Random rnd = new Random();

            if (hp < (maxHp / 4.0f) && rnd.Next(0, 101) < 10)
            {
                mode = BossMode.Heal;
            }
            else
            {
                mode = (BossMode)rnd.Next(0, 5);
            }

            switch (mode)
            {
                case BossMode.Fire:
                    magic = magicList[(int)mode];
                    saveDistance = 800;
                    heightLimit = false;
                    ai = new Boss_TraceAI(player, saveDistance);
                    timer = new Timer(0.2f);
                    break;
                case BossMode.Gravity:
                    magic = magicList[(int)mode];
                    saveDistance = 200;
                    heightLimit = true;
                    ai = new Boss_TraceAI(player, saveDistance);
                    timer = new Timer(0.15f);
                    break;
                case BossMode.Rock:
                    magic = magicList[(int)mode];
                    saveDistance = 400;
                    heightLimit = true;
                    ai = new Boss_TraceAI(player, saveDistance);
                    timer = new Timer(0.15f);
                    break;
                case BossMode.Water:
                    magic = magicList[(int)mode];
                    saveDistance = 400;
                    heightLimit = true;
                    ai = new Boss_TraceAI(player, saveDistance);
                    timer = new Timer(0.1f);
                    break;
                case BossMode.Wind:
                    magic = magicList[(int)mode];
                    saveDistance = 300;
                    heightLimit = true;
                    ai = new Boss_TraceAI(player, saveDistance);
                    timer = new Timer(0.12f);
                    break;
                case BossMode.Heal:
                    magic = magicList[(int)mode];
                    saveDistance = 1000;
                    heightLimit = false;
                    ai = new EscapeAI(player, saveDistance);
                    timer = new Timer(1.0f);
                    break;
                case BossMode.Cyclone:
                    magic = magicList[(int)mode];
                    saveDistance = 128;
                    heightLimit = false;
                    ai = new Boss_TraceAI(player, saveDistance);
                    timer = new Timer(0.05f);
                    break;
                case BossMode.Lightning:
                    mode = BossMode.Lightning;
                    magic = magicList[(int)mode];
                    saveDistance = 500;
                    heightLimit = false;
                    ai = new Boss_TraceAI(player, saveDistance);
                    timer = new Timer(0.1f);
                    break;
            }

        }

        public override void Draw(Renderer renderer)
        {
            float rate = hp / (maxHp * 1.0f);
            if (direct == Direct.Left)
            {
                renderer.DrawTexture(name, position + gameDevice.GetDisplayModify(), motion.DrawRange());
                renderer.DrawTexture(name, prevPosition + gameDevice.GetDisplayModify(), motion.DrawRange(), 0.3f);
                renderer.DrawTexture(name, thirdPosition + gameDevice.GetDisplayModify(), motion.DrawRange(), 0.2f);
            }
            else
            {
                renderer.DrawTexture(name, position + gameDevice.GetDisplayModify(), motion.DrawRange(), 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.FlipHorizontally);
                renderer.DrawTexture(name, prevPosition + gameDevice.GetDisplayModify(), motion.DrawRange(), 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.FlipHorizontally, 0.3f);
                renderer.DrawTexture(name, prevPosition + gameDevice.GetDisplayModify(), motion.DrawRange(), 0, Vector2.Zero, new Vector2(1, 1), SpriteEffects.FlipHorizontally, 0.2f);
            }

            renderer.DrawTexture("enemy_HP_G", new Vector2(
                    GetCenterWidth() - (108),
                    position.Y + height + 5) + gameDevice.GetDisplayModify(),
                    new Rectangle(0, 0, 72, 19),
                    0,
                    Vector2.Zero,
                    new Vector2(3, 1)
                    );
            renderer.DrawTexture(
                    "enemy_HP",
                    new Vector2(
                        GetCenterWidth() - (108),
                        position.Y + height + 5) +
                        gameDevice.GetDisplayModify(),
                    new Rectangle(0, 0, 72, 19),
                    0,
                    Vector2.Zero,
                    new Vector2(rate * 3, 1)
                    );

            DrawSpell(renderer);

            DrawState(renderer);
        }

        public void DrawSpell(Renderer renderer)
        {
            renderer.DrawString(
                magicSpell.SpellToString(),
                new Vector2(GetCenterWidth() - (magicSpell.SpellCount() * 14), position.Y - 50) + gameDevice.GetDisplayModify());
        }
        public void ScrollDisplayToBoss()
        {
            Vector2 modifyPosition = new Vector2(position.X + 128, position.Y - 128);
            Vector2 cameraPosition = gameDevice.GetCameraPosition();
            if ((modifyPosition - cameraPosition).Length() > 30)
            {
                gameDevice.ScrollCamera(modifyPosition - cameraPosition);
            }
        }
        public override object Clone()
        {
            throw new NotImplementedException();
        }

        public override void Hit(GameObject gameObject)
        {
        }
    }
}
