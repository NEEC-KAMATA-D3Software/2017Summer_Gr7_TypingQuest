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
    class Stage1 : IScene
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

        public Stage1(GameDevice gameDevice, CharacterManager characterManager, MagicManager magicManager, DamageFilter damageFilter)
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

            map.Load("./stage.csv");
            mapObjectManager.Initialize();

            player = new Player(new Vector2(gameDevice.GetStageLength() - 128, gameDevice.GetStageHeight() - 128), gameDevice, magicManager);
            player.Initialize();
            characterManager.AddPlayer(player);

            EnemyInitialize();

            magicManager.AddCharacterManager(characterManager);

            InitializeMapObject();

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
            return Scene.Stage2;
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

            tempGameObj = new SlidingBlock(new Vector2(64 * 50, 64 * 7), new Vector2(64 * 50, 64 * 13), new Vector2(0, 2), gameDevice);
            mapObjectManager.Add(tempGameObj);
            tempGameObj = new SlidingBlock(new Vector2(64 * 51, 64 * 7), new Vector2(64 * 51, 64 * 13), new Vector2(0, 2), gameDevice);
            mapObjectManager.Add(tempGameObj);

            #region DoorA RightGate

            tempGameObj = new Button("space0_Water", new Vector2(64 * 50, 64 * 6), gameDevice, mapObjectManager, typeof(Water));
            tempGameObj.SetID(GameObjectID.Button_B);
            ((Button)tempGameObj).SetLinkedGameObjectID(GameObjectID.Door_A);
            mapObjectManager.Add(tempGameObj);


            for (int i = 0; i < 5; i++)
            {
                tempGameObj = new Door("block1", new Vector2(64 * 23, 64 * i), gameDevice, mapObjectManager);
                tempGameObj.SetID(GameObjectID.Door_A);
                mapObjectManager.Add(tempGameObj);
            }

            

            #endregion

            #region SlidingBlockA Center

            tempGameObj = new SwitchBlock("block1", new Vector2(64 * 23, 64 * 15), new Vector2(64 * 23, 64 * 15), new Vector2(64 * 32, 64 * 15), new Vector2(3, 0), gameDevice);
            tempGameObj.SetID(GameObjectID.SlidingBlock_A);
            mapObjectManager.Add(tempGameObj);

            tempGameObj = new SwitchBlock("block1", new Vector2(64 * 24, 64 * 15), new Vector2(64 * 24, 64 * 15), new Vector2(64 * 33, 64 * 15), new Vector2(3, 0), gameDevice);
            tempGameObj.SetID(GameObjectID.SlidingBlock_A);
            mapObjectManager.Add(tempGameObj);

            tempGameObj = new Button("space0_Water", new Vector2(64 * 21, 64 * 12), gameDevice, mapObjectManager, typeof(Water));
            tempGameObj.SetID(GameObjectID.Button_B);
            ((Button)tempGameObj).SetLinkedGameObjectID(GameObjectID.SlidingBlock_A);
            mapObjectManager.Add(tempGameObj);

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < i + 1; j++)
                {
                    tempGameObj = new Door("block1", new Vector2(64 * (j + 1), 64 * (i + 15)), gameDevice, mapObjectManager, true);
                    tempGameObj.SetID(GameObjectID.SlidingBlock_A);
                    mapObjectManager.Add(tempGameObj);
                }
            }
            #endregion

            #region DoorB RightDoor

            tempGameObj = new Button("space0_Water", new Vector2(64 * 33, 64 * 14), gameDevice, mapObjectManager, typeof(Water));
            ((Button)tempGameObj).SetLinkedGameObjectID(GameObjectID.Door_B);
            mapObjectManager.Add(tempGameObj);

            for (int i = 0; i < 8; i++)
            {
                tempGameObj = new Door("block1", new Vector2(64 * 49, 64 * (6 + i)), gameDevice, mapObjectManager);
                tempGameObj.SetID(GameObjectID.Door_B);
                mapObjectManager.Add(tempGameObj);
            }

            #endregion

            tempGameObj = new TrapBall(new Vector2(64 * 20, 64 * 12), gameDevice, 64);
            mapObjectManager.Add(tempGameObj);

            #region DoorD LeftDoor

            tempGameObj = new Button("space0_Water", new Vector2(64 * 2, 64 * 14), gameDevice, mapObjectManager, typeof(Water));
            ((Button)tempGameObj).SetLinkedGameObjectID(GameObjectID.Door_D);
            mapObjectManager.Add(tempGameObj);

            tempGameObj = new Door("block1", new Vector2(64 * 7, 64 * 13), gameDevice, mapObjectManager, true);
            tempGameObj.SetID(GameObjectID.Door_D);
            mapObjectManager.Add(tempGameObj);

            for (int i = 0; i < 3; i++)
            {
                tempGameObj = new Door("block1", new Vector2(64 * (7 + i), 64 * 14), gameDevice, mapObjectManager, true);
                tempGameObj.SetID(GameObjectID.Door_D);
                mapObjectManager.Add(tempGameObj);
            }
            for (int i = 0; i < 5; i++)
            {
                tempGameObj = new Door("block1", new Vector2(64 * 10, 64 * i), gameDevice, mapObjectManager);
                tempGameObj.SetID(GameObjectID.Door_D);
                mapObjectManager.Add(tempGameObj);
            }

            #endregion

            #region DoorC LeftGate

            tempGameObj = new Button("space0_Water", new Vector2(64 * 2, 64 * 3), gameDevice, mapObjectManager, typeof(Water));
            tempGameObj.SetID(GameObjectID.Button_B);
            ((Button)tempGameObj).SetLinkedGameObjectID(GameObjectID.Door_C);
            mapObjectManager.Add(tempGameObj);

            for (int i = 0; i < 5; i++)
            {
                tempGameObj = new Door("block1", new Vector2(64 * 29, 64 * i), gameDevice, mapObjectManager);
                tempGameObj.SetID(GameObjectID.Door_C);
                mapObjectManager.Add(tempGameObj);
            }

            #endregion

        }

        private void EnemyInitialize()
        {
            characterManager.AddCharacter(new Enemy(player, new LazyAI(player), new Vector2(64 * 1, 64 * 10), gameDevice, magicManager, "WATER ", rnd));
            characterManager.AddCharacter(new Enemy(player, new LazyAI(player), new Vector2(64 * 47, 64 * 3), gameDevice, magicManager, "HEAL ", rnd));
            characterManager.AddCharacter(new Enemy(player, new LazyAI(player), new Vector2(64 * 50, 64 * 3), gameDevice, magicManager, "RECOVERY ", rnd));
            characterManager.AddCharacter(new Enemy(player, new BoundAI(), new Vector2(64 * 42, 64 * 19), gameDevice, magicManager, "WATER ", rnd));
            characterManager.AddCharacter(new Enemy(player, new BoundAI(), new Vector2(64 * 29, 64 * 19), gameDevice, magicManager, "WATER ", rnd));
            characterManager.AddCharacter(new Enemy(player, new BoundAI(), new Vector2(64 * 18, 64 * 19), gameDevice, magicManager, "WATER ", rnd));
            characterManager.AddCharacter(new Enemy(player, new LazyAI(player), new Vector2(64 * 40, 64 * 9), gameDevice, magicManager, "WATER ", rnd));
        }
    }
}
