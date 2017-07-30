using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using TypingQuest.Device;
using TypingQuest.Actor;
using TypingQuest.Actor.Magic;
using TypingQuest.Actor.Magic.ComboSystem;
using TypingQuest.Def;
using TypingQuest.EffectFolder;

namespace TypingQuest.Scene
{
    class Stage2 : IScene
    {
        private bool isEnd;
        private GameDevice gameDevice;
        private Sound sound;
        private InputState input;

        private Map map;
        private MapObjectManager mapObjectManager;

        private static Random rnd;
        private CharacterManager characterManager;
        private Player player;
        private MagicManager magicManager;

        private ComboSystem comboSystem;

        private DamageFilter damageFilter;

        public Stage2(GameDevice gameDevice, CharacterManager characterManager, MagicManager magicManager, DamageFilter damageFilter)
        {
            isEnd = false;
            this.gameDevice = gameDevice;
            sound = gameDevice.GetSound();
            input = gameDevice.GetInputState();

            this.characterManager = characterManager;
            this.magicManager = magicManager;

            map = new Map(gameDevice);
            mapObjectManager = new MapObjectManager(gameDevice);

            comboSystem = gameDevice.GetComboSystem();

            this.damageFilter = damageFilter;
        }
        public void Draw(Renderer renderer)
        {
            if (damageFilter.IsStart())
            {
                damageFilter.WriteRenderTarget();
            }

            renderer.Begin();

            renderer.DrawTexture("background", Vector2.Zero);

            map.Draw(renderer);
            mapObjectManager.Draw(renderer);

            characterManager.Draw(renderer);
            magicManager.Draw(renderer);
            renderer.DrawTexture("typingUI", new Vector2(0, Screen.Height - Screen.UI));
            characterManager.DrawPlayerUI(renderer);
            comboSystem.Draw(renderer);
            characterManager.DrawPlayerHP(renderer);

            renderer.End();

            if (damageFilter.IsStart())
            {
                damageFilter.ReleaseRenderTarget();
                damageFilter.Draw(renderer);
            }

        }

        public void Initialize()
        {
            isEnd = false;

            rnd = new Random();
            characterManager.Initialize();
            magicManager.Initialize();

            map.Load("./stage2.csv");
            mapObjectManager.Initialize();

            player = characterManager.TargetPlayer();
            player.Initialize();
            player.StagePosition(new Vector2(gameDevice.GetStageLength() / 2 - 32, gameDevice.GetStageHeight() - 128));
            EnemyInitialize();
            magicManager.AddCharacterManager(characterManager);

            damageFilter.Initialize();

            InitializeMapObject();

            player.SetNext(false);
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
            return Scene.BOSS;
        }

        public void Shutdown()
        {
            map.Unload();
        }

        public void Update(GameTime gameTime)
        {
            sound.PlayBGM("amayonofukurou");

            comboSystem.OffDamageEffect();

            map.Update(gameTime);
            map.Hit(player);
            mapObjectManager.Update(gameTime);
            mapObjectManager.CharaHitGameObject(player);

            characterManager.Update(gameTime);
            foreach (Character enemy in characterManager.Enemies())
            {
                map.Hit(enemy);
                mapObjectManager.CharaHitGameObject(enemy);
            }

            magicManager.Update(gameTime);
            foreach (MagicAbstract magic in magicManager.GetMagicList())
            {
                map.Hit(magic);
                mapObjectManager.CharaHitGameObject(magic);
            }

            comboSystem.Update(gameTime);

            damageFilter.Update();

            if (player.IsDead() || (player.IsNext() && player.GetPosition().Y < 500))
            {
                isEnd = true;
            }
            if (comboSystem.IsDamageEffect())
            {
                damageFilter.GetStart();
            }

        }

        private void InitializeMapObject()
        {
            GameObject tempGameObj;

            #region DoorA~C Bottom

            tempGameObj = new Button("space0_Water", new Vector2(64 * 19, 64 * 34), gameDevice, mapObjectManager, typeof(Water));
            ((Button)tempGameObj).SetLinkedGameObjectID(GameObjectID.Door_A);
            mapObjectManager.Add(tempGameObj);

            for (int i = 0; i < 3; i++)
            {
                tempGameObj = new Door("block1", new Vector2(64 * (17 + i), 64 * 35), gameDevice, mapObjectManager, true);
                tempGameObj.SetID(GameObjectID.Door_A);
                mapObjectManager.Add(tempGameObj);
            }

            tempGameObj = new Button("space0_Water", new Vector2(64 * 19, 64 * 32), gameDevice, mapObjectManager, typeof(Water));
            ((Button)tempGameObj).SetLinkedGameObjectID(GameObjectID.Door_B);
            mapObjectManager.Add(tempGameObj);

            for (int i = 0; i < 2; i++)
            {
                tempGameObj = new Door("block1", new Vector2(64 * (18 + i), 64 * 33), gameDevice, mapObjectManager, true);
                tempGameObj.SetID(GameObjectID.Door_B);
                mapObjectManager.Add(tempGameObj);
            }
            #endregion

            #region SlidingBlockA Bottom

            tempGameObj = new Button("space0_Wind", new Vector2(64 * 14, 64 * 30), gameDevice, mapObjectManager, typeof(Wind));
            ((Button)tempGameObj).SetLinkedGameObjectID(GameObjectID.SlidingBlock_A);
            mapObjectManager.Add(tempGameObj);

            tempGameObj = new SwitchBlock("block1", new Vector2(64 * 9, 64 * 31), new Vector2(64 * 9, 64 * 31), new Vector2(64 * 9, 64 * 34), new Vector2(0, 3), gameDevice);
            tempGameObj.SetID(GameObjectID.SlidingBlock_A);
            mapObjectManager.Add(tempGameObj);

            tempGameObj = new SwitchBlock("block1", new Vector2(64 * 10, 64 * 31), new Vector2(64 * 10, 64 * 31), new Vector2(64 * 10, 64 * 34), new Vector2(0, 3), gameDevice);
            tempGameObj.SetID(GameObjectID.SlidingBlock_A);
            mapObjectManager.Add(tempGameObj);

            #endregion

            #region SlidingBlock NoneTag

            tempGameObj = new SlidingBlock(new Vector2(64 * 6, 64 * 26), new Vector2(64 * 8, 64 * 26), new Vector2(1, 0), gameDevice);
            mapObjectManager.Add(tempGameObj);
            tempGameObj = new SlidingBlock(new Vector2(64 * 7, 64 * 26), new Vector2(64 * 9, 64 * 26), new Vector2(1, 0), gameDevice);
            mapObjectManager.Add(tempGameObj);

            tempGameObj = new SlidingBlock(new Vector2(64 * 6, 64 * 24), new Vector2(64 * 8, 64 * 24), new Vector2(3, 0), gameDevice);
            mapObjectManager.Add(tempGameObj);
            tempGameObj = new SlidingBlock(new Vector2(64 * 7, 64 * 24), new Vector2(64 * 9, 64 * 24), new Vector2(3, 0), gameDevice);
            mapObjectManager.Add(tempGameObj);

            tempGameObj = new SlidingBlock(new Vector2(64 * 15, 64 * 26), new Vector2(64 * 17, 64 * 26), new Vector2(2, 0), gameDevice);
            mapObjectManager.Add(tempGameObj);
            tempGameObj = new SlidingBlock(new Vector2(64 * 16, 64 * 26), new Vector2(64 * 18, 64 * 26), new Vector2(2, 0), gameDevice);
            mapObjectManager.Add(tempGameObj);

            tempGameObj = new SlidingBlock(new Vector2(64 * 15, 64 * 24), new Vector2(64 * 17, 64 * 24), new Vector2(1.5f, 0), gameDevice);
            mapObjectManager.Add(tempGameObj);
            tempGameObj = new SlidingBlock(new Vector2(64 * 16, 64 * 24), new Vector2(64 * 18, 64 * 24), new Vector2(1.5f, 0), gameDevice);
            mapObjectManager.Add(tempGameObj);
            #endregion

            #region DoorD TimerDoor

            tempGameObj = new Button("space0_Rock", new Vector2(64 * 12, 64 * 25), gameDevice, mapObjectManager, typeof(Rock));
            tempGameObj.SetID(GameObjectID.Button_A);
            ((Button)tempGameObj).SetLinkedGameObjectID(GameObjectID.Door_D);
            mapObjectManager.Add(tempGameObj);

            for (int i = 0; i < 7; i++)
            {
                tempGameObj = new Door("block1", new Vector2(64 * (9 + i), 64 * 20), gameDevice, mapObjectManager, true, true);
                tempGameObj.SetID(GameObjectID.Door_D);
                ((Door)tempGameObj).SetLinkedGameObjectID(GameObjectID.Button_A);
                mapObjectManager.Add(tempGameObj);
            }

            #endregion

            #region DoorE Center

            tempGameObj = new Button("space0_Water", new Vector2(64 * 9, 64 * 19), gameDevice, mapObjectManager, typeof(Water));
            ((Button)tempGameObj).SetLinkedGameObjectID(GameObjectID.Door_E);
            mapObjectManager.Add(tempGameObj);

            tempGameObj = new Button("space0_Water", new Vector2(64 * 15, 64 * 19), gameDevice, mapObjectManager, typeof(Water));
            ((Button)tempGameObj).SetLinkedGameObjectID(GameObjectID.Door_E);
            mapObjectManager.Add(tempGameObj);

            tempGameObj = new Button("space0_Rock", new Vector2(64 * 11, 64 * 12), gameDevice, mapObjectManager, typeof(Rock));
            ((Button)tempGameObj).SetLinkedGameObjectID(GameObjectID.Door_E);
            mapObjectManager.Add(tempGameObj);

            tempGameObj = new Button("space0_Rock", new Vector2(64 * 11, 64 * 12), gameDevice, mapObjectManager, typeof(Rock));
            ((Button)tempGameObj).SetLinkedGameObjectID(GameObjectID.SwitchDoor_A);
            mapObjectManager.Add(tempGameObj);

            for (int i = 0; i < 4; i++)
            {
                tempGameObj = new Door("block1", new Vector2(64 * (11 + i), 64 * 13), gameDevice, mapObjectManager, true);
                tempGameObj.SetID(GameObjectID.Door_E);
                mapObjectManager.Add(tempGameObj);
            }

            for (int i = 0; i < 3; i++)
            {
                tempGameObj = new Door("block1", new Vector2(64 * (14 + i), 64 * 14), gameDevice, mapObjectManager, true);
                tempGameObj.SetID(GameObjectID.Door_E);
                mapObjectManager.Add(tempGameObj);
            }

            for (int i = 0; i < 3; i++)
            {
                tempGameObj = new Door("block1", new Vector2(64 * (11 + i), 64 * 19), gameDevice, mapObjectManager, false, true, 10, TimerType.Space);
                tempGameObj.SetID(GameObjectID.SwitchDoor_A);
                mapObjectManager.Add(tempGameObj);
            }

            #endregion

            #region DoorF Top

            tempGameObj = new Button("space0_Rock", new Vector2(64 * 11, 64 * 12), gameDevice, mapObjectManager, typeof(Rock));
            ((Button)tempGameObj).SetLinkedGameObjectID(GameObjectID.Door_F);
            mapObjectManager.Add(tempGameObj);

            tempGameObj = new Button("space0_Wind", new Vector2(64 * 7, 64 * 12), gameDevice, mapObjectManager, typeof(Wind));
            ((Button)tempGameObj).SetLinkedGameObjectID(GameObjectID.Door_F);
            mapObjectManager.Add(tempGameObj);

            for (int i = 0; i < 5; i++)
            {
                tempGameObj = new Door("block1", new Vector2(64 * (10 + i), 64 * 9), gameDevice, mapObjectManager, true);
                tempGameObj.SetID(GameObjectID.Door_F);
                mapObjectManager.Add(tempGameObj);
            }

            #endregion

            tempGameObj = new TrapBall(new Vector2(64 * 10, 64 * 8), gameDevice, 128);
            mapObjectManager.Add(tempGameObj);

            #region DoorG Gate1

            tempGameObj = new Button("space0_Rock", new Vector2(64 * 14, 64 * 2), gameDevice, mapObjectManager, typeof(Rock));
            ((Button)tempGameObj).SetLinkedGameObjectID(GameObjectID.Door_G);
            mapObjectManager.Add(tempGameObj);

            for (int i = 0; i < 4; i++)
            {
                tempGameObj = new Door("block1", new Vector2(64 * 8, 64 * (i)), gameDevice, mapObjectManager);
                tempGameObj.SetID(GameObjectID.Door_G);
                mapObjectManager.Add(tempGameObj);
            }

            for (int i = 0; i < 4; i++)
            {
                tempGameObj = new Door("block1", new Vector2(64 * 16, 64 * (i)), gameDevice, mapObjectManager);
                tempGameObj.SetID(GameObjectID.Door_G);
                mapObjectManager.Add(tempGameObj);
            }

            #endregion

            #region DoorH Gate2

            tempGameObj = new Button("space0_Wind", new Vector2(64 * 10, 64 * 2), gameDevice, mapObjectManager, typeof(Wind));
            ((Button)tempGameObj).SetLinkedGameObjectID(GameObjectID.Door_H);
            mapObjectManager.Add(tempGameObj);

            for (int i = 0; i < 4; i++)
            {
                tempGameObj = new Door("block1", new Vector2(64 * 16, 64 * (i)), gameDevice, mapObjectManager);
                tempGameObj.SetID(GameObjectID.Door_H);
                mapObjectManager.Add(tempGameObj);
            }

            for (int i = 0; i < 4; i++)
            {
                tempGameObj = new Door("block1", new Vector2(64 * 8, 64 * (i)), gameDevice, mapObjectManager);
                tempGameObj.SetID(GameObjectID.Door_H);
                mapObjectManager.Add(tempGameObj);
            }

            #endregion

            #region DoorI Gate3

            tempGameObj = new Button("space0_Water", new Vector2(64 * 12, 64 * 5), gameDevice, mapObjectManager, typeof(Water));
            ((Button)tempGameObj).SetLinkedGameObjectID(GameObjectID.Door_I);
            mapObjectManager.Add(tempGameObj);

            for (int i = 0; i < 4; i++)
            {
                tempGameObj = new Door("block1", new Vector2(64 * 16, 64 * (i)), gameDevice, mapObjectManager);
                tempGameObj.SetID(GameObjectID.Door_I);
                mapObjectManager.Add(tempGameObj);
            }

            for (int i = 0; i < 4; i++)
            {
                tempGameObj = new Door("block1", new Vector2(64 * 8, 64 * (i)), gameDevice, mapObjectManager);
                tempGameObj.SetID(GameObjectID.Door_I);
                mapObjectManager.Add(tempGameObj);
            }

            #endregion
        }

        private void EnemyInitialize()
        {
            characterManager.AddCharacter(new Enemy(player, new LazyAI(player), new Vector2(64 * 7, 64 * 35), gameDevice, magicManager, "WATER ", rnd));
            characterManager.AddCharacter(new Enemy(player, new LazyAI(player), new Vector2(64 * 6, 64 * 34), gameDevice, magicManager, "WIND ", rnd));
            characterManager.AddCharacter(new Enemy(player, new LazyAI(player), new Vector2(64 * 5, 64 * 33), gameDevice, magicManager, "WATER ", rnd));
            characterManager.AddCharacter(new Enemy(player, new LazyAI(player), new Vector2(64 * 12, 64 * 27), gameDevice, magicManager, "ROCK ", rnd));
            characterManager.AddCharacter(new Enemy(player, new LazyAI(player), new Vector2(64 * 12, 64 * 23), gameDevice, magicManager, "ROCK ", rnd));
            characterManager.AddCharacter(new Enemy(player, new LazyAI(player), new Vector2(64 * 8, 64 * 15), gameDevice, magicManager, "ROCK ", rnd));
            characterManager.AddCharacter(new Enemy(player, new LazyAI(player), new Vector2(64 * 6, 64 * 14), gameDevice, magicManager, "GRAVITY ", rnd));
            characterManager.AddCharacter(new Enemy(player, new LazyAI(player), new Vector2(64 * 18, 64 * 11), gameDevice, magicManager, "FIRE ", rnd));
            characterManager.AddCharacter(new Enemy(player, new LazyAI(player), new Vector2(64 * 9, 64 * 11), gameDevice, magicManager, "FIRE ", rnd));
            characterManager.AddCharacter(new Enemy(player, new LazyAI(player), new Vector2(64 * 6, 64 * 9), gameDevice, magicManager, "WIND ", rnd));
            characterManager.AddCharacter(new Enemy(player, new LazyAI(player), new Vector2(64 * 19, 64 * 4), gameDevice, magicManager, "ROCK ", rnd));
            characterManager.AddCharacter(new Enemy(player, new LazyAI(player), new Vector2(64 * 5, 64 * 4), gameDevice, magicManager, "ROCK ", rnd));
        }
    }
}
