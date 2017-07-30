using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TypingQuest.Device;
using TypingQuest.Actor.Magic;
using TypingQuest.Actor;
using TypingQuest.Actor.Magic.ComboSystem;
using TypingQuest.Def;
using TypingQuest.Utility;
using TypingQuest.EffectFolder;

namespace TypingQuest.Scene
{
    class Boss : IScene
    {
        private bool isEnd;
        private bool startFlag;
        private Timer eventTimer;
        private float alpha;

        private GameDevice gameDevice;
        private Sound sound;
        private InputState input;
        private CharacterManager characterManager;
        private Map map;
        private Player player;
        private Enemy_Boss boss;
        private Enemy_BossWizard bossWizard;
        private MagicManager magicManager;
        private ComboSystem comboSystem;

        private DamageFilter damageFilter;

        public Boss(GameDevice gameDevice, CharacterManager characterManager, MagicManager magicManager, DamageFilter damageFilter)
        {
            isEnd = false;
            startFlag = true;
            this.gameDevice = gameDevice;
            sound = gameDevice.GetSound();
            input = gameDevice.GetInputState();
            this.characterManager = characterManager;
            this.magicManager = magicManager;
            map = new Map(gameDevice);
            comboSystem = gameDevice.GetComboSystem();
            eventTimer = new Timer(3);

            this.damageFilter = damageFilter;
        }
        public void Draw(Renderer renderer)
        {
            List<RenderTarget2D> targetList = new List<RenderTarget2D>();

            if (damageFilter.IsStart())
            {
                damageFilter.WriteRenderTarget();
            }

            renderer.Begin();
            renderer.DrawTexture("background", Vector2.Zero);

                renderer.DrawTexture(
                "mask",
                Vector2.Zero,
                new Vector2(1024, 768),
                alpha);

            map.Draw(renderer);
            characterManager.Draw(renderer);
            magicManager.Draw(renderer);
            renderer.DrawTexture("typingUI", new Vector2(0, Screen.Height - Screen.UI));
            characterManager.DrawPlayerUI(renderer);
            comboSystem.Draw(renderer);

            if (!isEnd)
            {
                characterManager.DrawPlayerHP(renderer);
            }

            renderer.End();

            if (damageFilter.IsStart())
            {
                targetList.Add(damageFilter.GetRenderTarget());
                damageFilter.ReleaseRenderTarget();
                damageFilter.Draw(renderer);
            }

        }

        public void Initialize()
        {
            isEnd = false;
            startFlag = true;
            alpha = 0;

            characterManager.Initialize();
            magicManager.Initialize();
            map.Load("./bossStage.csv");
            player = characterManager.TargetPlayer();
            player.Initialize();
            player.StagePosition(new Vector2(gameDevice.GetStageLength() - (Screen.Width) + 256, gameDevice.GetStageHeight() - 128));
            boss = new Enemy_Boss(player, new Vector2(gameDevice.GetStageLength() - (Screen.Width) + 128, 256f), gameDevice, magicManager);
            bossWizard = new Enemy_BossWizard(new Vector2(gameDevice.GetStageLength() - (Screen.Width) + 32, 300f), gameDevice, magicManager, player);
            characterManager.AddCharacter(boss);
            characterManager.AddCharacter(bossWizard);
            magicManager.AddCharacterManager(characterManager);
            eventTimer.Initialize();

            damageFilter.Initialize();
        }

        public bool IsEnd()
        {
            return isEnd;
        }

        public Scene Next()
        {
            if (player.IsDead())
            {
                comboSystem.Initialize();
                return Scene.GameOver;
            }
            return Scene.Ending;
        }

        public void Shutdown()
        {
        }

        public void ShutdownEnding()
        {
            comboSystem.Initialize();
            magicManager.Initialize();
            map.Unload();
        }

        public void Update(GameTime gameTime)
        {
            if (!isEnd)
            {
                sound.PlayBGM("Battle-BloodyCrescent");
            }
            else
            {
                alpha -= 0.001f;
                alpha = (alpha <= 0) ? 0 : alpha;
            }

            comboSystem.OffDamageEffect();
            if (startFlag)
            {
                map.Update(gameTime);
                bossWizard.ScrollDisplayToBoss();
                boss.SetAlpha(1 - eventTimer.Rate());
                alpha = (1 - eventTimer.Rate()) * 0.4f;
                eventTimer.Update();
                if (eventTimer.IsTime())
                {
                    startFlag = false;
                }
                return;
            }

            map.Update(gameTime);
            map.Hit(player);
            characterManager.Enemies().ForEach((Character enemy) => map.Hit(enemy));
            characterManager.Update(gameTime);
            magicManager.Update(gameTime);
            magicManager.GetMagicList().ForEach((MagicAbstract magic) => map.Hit(magic));
            comboSystem.Update(gameTime);

            damageFilter.Update();

            if (player.IsDead() || (boss.IsDead() && bossWizard.IsDead()))
            {
                isEnd = true;
            }
            if (comboSystem.IsDamageEffect())
            {
                damageFilter.GetStart();
            }
        }
    }
}
