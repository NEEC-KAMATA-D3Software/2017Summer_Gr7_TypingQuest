using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using TypingQuest.Device;

namespace TypingQuest.Scene
{
    class Map
    {
        private List<List<GameObject>> mapList;

        private GameDevice gameDevice;


        public Map(GameDevice gameDevice)
        {
            mapList = new List<List<GameObject>>();
            this.gameDevice = gameDevice;
        }
        private List<GameObject> AddBlock(int lineCnt, string[] line)
        {
            Dictionary<string, GameObject> objectList = new Dictionary<string, GameObject>();
            objectList.Add("0", new SpaceWall(Vector2.Zero, gameDevice));
            objectList.Add("1", new Block(Vector2.Zero, gameDevice));
            objectList.Add("2", new SpaceFire(Vector2.Zero, gameDevice));
            objectList.Add("3", new Space(Vector2.Zero, gameDevice));
            objectList.Add("4", new CreditBlock(Vector2.Zero, gameDevice));
            objectList.Add("80", new Window(Vector2.Zero, gameDevice, 0));
            objectList.Add("81", new Window(Vector2.Zero, gameDevice, 1));
            objectList.Add("82", new Window(Vector2.Zero, gameDevice, 2));
            objectList.Add("83", new Window(Vector2.Zero, gameDevice, 3));
            objectList.Add("84", new Window(Vector2.Zero, gameDevice, 4));
            objectList.Add("85", new Window(Vector2.Zero, gameDevice, 5));
            objectList.Add("86", new Window(Vector2.Zero, gameDevice, 6));
            objectList.Add("87", new Window(Vector2.Zero, gameDevice, 7));
            objectList.Add("88", new Window(Vector2.Zero, gameDevice, 8));
            objectList.Add("90", new Gate(Vector2.Zero, gameDevice, 0));
            objectList.Add("91", new Gate(Vector2.Zero, gameDevice, 1));
            objectList.Add("92", new Gate(Vector2.Zero, gameDevice, 2));
            objectList.Add("93", new Gate(Vector2.Zero, gameDevice, 3));
            objectList.Add("94", new Gate(Vector2.Zero, gameDevice, 4));
            objectList.Add("95", new Gate(Vector2.Zero, gameDevice, 5));
            objectList.Add("96", new Gate(Vector2.Zero, gameDevice, 6));
            objectList.Add("97", new Gate(Vector2.Zero, gameDevice, 7));
            objectList.Add("98", new Gate(Vector2.Zero, gameDevice, 8));

            List<GameObject> workList = new List<GameObject>();

            int colCnt = 0;
            foreach (var s in line)
            {
                try
                {
                    GameObject work = (GameObject)objectList[s].Clone();
                    work.SetPosition(new Vector2(colCnt * 64, lineCnt * 64));
                    workList.Add(work);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
                colCnt++;
            }

            return workList;
        }
        public void Load(string filename)
        {
            if (!filename.Contains(".csv"))
            {
                filename += ".csv";
            }
            CSVReader csv = new CSVReader();
            csv.Read(filename);

            var data = csv.GetData(); // List<string[]>
            gameDevice.SetStageSize(data[0].Length, data.Count);
            for (int lineCnt = 0; lineCnt < data.Count(); lineCnt++)
            {
                mapList.Add(AddBlock(lineCnt, data[lineCnt]));
            }
        }
        public void Unload()
        {
            mapList.Clear();
        }
        public void Update(GameTime gameTime)
        {
            foreach (List<GameObject> list in mapList)
            {
                foreach (GameObject obj in list)
                {
                    if (obj is Space || obj is SpaceWall || obj is CreditBlock)
                    {
                        continue;
                    }

                    obj.Update(gameTime);
                }
            }
        }
        public void Hit(GameObject gameObject)
        {
            //座標変換 obj => map[x, y]
            Point work = gameObject.GetRectangle().Location;
            int x = work.X / 64;
            int y = work.Y / 64;

            if (x < 1) x = 1;
            if (y < 1) y = 1;

            for (int i = y - 1; i <= (y + 1); i++)
            {
                for (int j = x - 1; j <= (x + 1); j++)
                {
                    try
                    {
                        GameObject obj = mapList[i][j];
                        if (obj is Space || obj is SpaceFire || obj is SpaceWall || obj is CreditBlock)
                        {
                          continue;
                        }
                        if (obj.Collision(gameObject))
                        {
                          gameObject.Hit(obj);
                        }
                    }
                    catch
                    {
                    }
                    
                }
            }
        }

        public void Draw(Renderer renderer)
        {
            foreach (List<GameObject> list in mapList)
            {
                foreach (GameObject obj in list)
                {
                    if (obj is Space)
                    {
                        continue;
                    }
                    obj.Draw(renderer);
                }
            }
        }
    }
}
