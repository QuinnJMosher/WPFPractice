using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        public static string folder = "C:\\Users\\quinn.mosher\\Pictures\\kenny_medals\\";
        public static string[] files = {
                            "flat_medal1.png",
                            "flat_medal2.png",
                            "flat_medal3.png"
                         };
        public static string saveFolder = "C:\\Users\\quinn.mosher\\Pictures\\SpritePackTest\\";
        public static string saveName = "SpritePackerTest0.png";

        static void Main(string[] args)
        {
            List<Sprite> sprites = new List<Sprite>();

            for (int i = 0; i < files.Length; i++)
            {
                sprites.Add(new Sprite(new Uri(folder + files[i])));
            }

            Atlas atlas = AtlasBuilder.CreateAtlas(sprites);

            //Console.ReadLine();
        }
    }
}
