using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using TypingQuest.Scene;
using TypingQuest.Actor.Magic;
using TypingQuest.Device;
using TypingQuest.Def;
using TypingQuest.Actor;
using TypingQuest.Actor.Magic.ComboSystem;
using TypingQuest.EffectFolder;

namespace TypingQuest
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphicsDeviceManager;
        private GameDevice gameDevice;

        private Renderer renderer;
        private Sound sound;
        private SceneManager sceneManager;
        private CharacterManager characterManager;
        private MagicManager magicManager;

        private DamageEffect damageEffect;
        private DamageFilter damageFilter;

        public Game1()
        {
            graphicsDeviceManager = new GraphicsDeviceManager(this);
            graphicsDeviceManager.PreferredBackBufferWidth = Screen.Width;
            graphicsDeviceManager.PreferredBackBufferHeight = Screen.Height;
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            gameDevice = new GameDevice(Content, GraphicsDevice);
            base.Window.Title = "TypingQuest";

            renderer = gameDevice.GetRenderer();
            sound = gameDevice.GetSound();

            damageEffect = new DamageEffect();
            damageFilter = new DamageFilter(GraphicsDevice, damageEffect);

            #region SceneManager
            characterManager = new CharacterManager();
            magicManager = new MagicManager(gameDevice);

            sceneManager = new SceneManager();
            Stage1 stage1 = new Stage1(gameDevice, characterManager, magicManager, damageFilter);
            Stage2 stage2 = new Stage2(gameDevice, characterManager, magicManager, damageFilter);
            Boss bossScene = new Boss(gameDevice, characterManager, magicManager, damageFilter);
            sceneManager.Add(Scene.Scene.Title, new SceneFader(new Title(gameDevice, magicManager)));
            sceneManager.Add(Scene.Scene.Stage1, new SceneFader(stage1));
            sceneManager.Add(Scene.Scene.Stage2, new SceneFader(stage2));
            sceneManager.Add(Scene.Scene.BOSS, bossScene);
            sceneManager.Add(Scene.Scene.Ending, new SceneFader(new Ending(gameDevice, bossScene), SceneFader.SceneFadeState.None));
            sceneManager.Add(Scene.Scene.GameOver, new SceneFader(new GameOver(gameDevice)));
            sceneManager.Add(Scene.Scene.Credit, new SceneFader(new Credit(gameDevice, magicManager)));
            sceneManager.Change(Scene.Scene.Title);
            #endregion

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            renderer.LoadFontContent();
            renderer.LoadTexture("black");
            renderer.LoadTexture("BOSS_motion");
            renderer.LoadTexture("BOSS_light");
            renderer.LoadTexture("BOSSplayer_motion");
            renderer.LoadTexture("water");
            renderer.LoadTexture("fire");
            renderer.LoadTexture("gravity");
            renderer.LoadTexture("wind");
            renderer.LoadTexture("rock");
            renderer.LoadTexture("heal");
            renderer.LoadTexture("cyclone");
            renderer.LoadTexture("lightningSingle");
            renderer.LoadTexture("lightningLast");
            renderer.LoadTexture("recovery");
            renderer.LoadTexture("timer");
            renderer.LoadTexture("player_motion2");
            renderer.LoadTexture("space0");
            renderer.LoadTexture("block1");
            renderer.LoadTexture("space2");
            renderer.LoadTexture("gate");
            renderer.LoadTexture("window");
            renderer.LoadTexture("creditBlock");
            renderer.LoadTexture("hp_in");
            renderer.LoadTexture("magicHP_in");
            renderer.LoadTexture("hp_out");
            renderer.LoadTexture("hp_out_2");
            renderer.LoadTexture("enemy_HP");
            renderer.LoadTexture("enemy_HP_G");
            renderer.LoadTexture("typingUI");
            renderer.LoadTexture("credit1");
            renderer.LoadTexture("credit2");
            renderer.LoadTexture("credit3");
            renderer.LoadTexture("credit4");
            renderer.LoadTexture("credit5");
            renderer.LoadTexture("title");
            renderer.LoadTexture("title_light");
            renderer.LoadTexture("background");
            renderer.LoadTexture("fire_enemy_motion");
            renderer.LoadTexture("water_enemy_motion");
            renderer.LoadTexture("rock_enemy_motion");
            renderer.LoadTexture("wind_enemy_motion");
            renderer.LoadTexture("GameOver");
            renderer.LoadTexture("space0_Rock");
            renderer.LoadTexture("space0_Wind");
            renderer.LoadTexture("space0_Water");
            renderer.LoadTexture("light");
            renderer.LoadTexture("spark");
            renderer.LoadTexture("warning");

            sound.LoadBGM("amayonofukurou", "./BGM/");
            sound.LoadBGM("Moon_Grav", "./BGM/");
            sound.LoadBGM("Eos", "./BGM/");
            sound.LoadBGM("Battle-BloodyCrescent", "./BGM/");
            sound.LoadBGM("tomoshibi", "./BGM/");

            sound.LoadSE("switch", "./SE/");
            sound.LoadSE("fire", "./SE/");
            sound.LoadSE("water", "./SE/");
            sound.LoadSE("wind", "./SE/");
            sound.LoadSE("lightning", "./SE/");
            sound.LoadSE("heal", "./SE/");
            sound.LoadSE("recovery", "./SE/");
            sound.LoadSE("rock", "./SE/");

            damageEffect.AddEffect(Content.Load<Effect>("./damageEffect"));

            Texture2D fade = new Texture2D(GraphicsDevice, 1, 1);
            Texture2D mask = new Texture2D(GraphicsDevice, 1, 1);
            Color[] date = new Color[1 * 1];
            Color[] data = new Color[1 * 1];
            date[0] = new Color(30, 20, 50);
            data[0] = new Color(255, 0, 0);
            fade.SetData(date);
            mask.SetData(data);
            renderer.LoadTexture("fade", fade);
            renderer.LoadTexture("mask", mask);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            renderer.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (Keyboard.GetState().IsKeyDown(Keys.Escape) || 
                GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            gameDevice.Update();
            sceneManager.Update(gameTime);
            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            sceneManager.Draw(renderer);

            base.Draw(gameTime);
        }
    }
}
