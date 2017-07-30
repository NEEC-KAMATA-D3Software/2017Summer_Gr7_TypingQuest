using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;

namespace TypingQuest.Actor.Magic
{
    class MagicManager
    {
        private List<MagicAbstract> magicList;
        private GameDevice gameDevice;
        private CharacterManager characterManager;

        public MagicManager(GameDevice gameDevice)
        {
            magicList = new List<MagicAbstract>();
            this.gameDevice = gameDevice;
        }
        public void Initialize()
        {
            magicList.Clear();
        }
        public void AddCharacterManager(CharacterManager characterManager)
        {
            this.characterManager = characterManager;
        }
        public void AddMagic(Character castChara ,string spell, Vector2 position)
        {
            switch (spell)
            {
                case "FIRE":
                    Fire fireMagic = new Fire(castChara, gameDevice, characterManager);
                    fireMagic.Initialize();
                    magicList.Add(fireMagic);
                    break;
                case "WATER":
                    Water waterMagic = new Water(castChara, gameDevice, characterManager);
                    waterMagic.Initialize();
                    magicList.Add(waterMagic);
                    break;
                case "ROCK":
                    Rock rockMagic = new Rock(castChara, gameDevice, characterManager);
                    rockMagic.Initialize();
                    magicList.Add(rockMagic);
                    break;
                case "HEAL":
                    Heal healMagic = new Heal(castChara, gameDevice, characterManager);
                    healMagic.Initialize();
                    magicList.Add(healMagic);
                    break;
                case "RECOVERY":
                    Recovery recoveryMagic = new Recovery(castChara, gameDevice, characterManager);
                    recoveryMagic.Initialize();
                    magicList.Add(recoveryMagic);
                    break;
                case "GRAVITY":
                    MagicGravity gravityMagic = new MagicGravity(castChara, gameDevice, characterManager);
                    gravityMagic.Initialize();
                    magicList.Add(gravityMagic);
                    break;
                case "WIND":
                    Wind windMagic = new Wind(castChara, gameDevice, characterManager);
                    windMagic.Initialize();
                    magicList.Add(windMagic);
                    break;

                case "LIGHTNING":
                    Lightning lightningMagic = new Lightning(castChara, gameDevice, characterManager);
                    lightningMagic.Initialize();
                    magicList.Add(lightningMagic);
                    break;

                case "CYCLONE":
                    Cyclone cycloneMagic = new Cyclone(castChara, gameDevice, characterManager);
                    cycloneMagic.Initialize();
                    magicList.Add(cycloneMagic);
                    break;

                default:
                    break;
            }
        }

        public void Update(GameTime gameTime)
        {
            if (magicList.Count <= 0)
            {
                return;
            }
            magicList.ForEach((MagicAbstract ma) => ma.Update(gameTime));
            magicList.RemoveAll((MagicAbstract ma) => ma.IsEnd());
        }
        public void Draw(Renderer renderer)
        {
            magicList.ForEach((MagicAbstract ma) => ma.Draw(renderer));
        }

        public List<MagicAbstract> GetMagicList()
        {
            return magicList;
        }
    }
}
