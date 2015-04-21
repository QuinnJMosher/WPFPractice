using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Imaging;

namespace ConsoleApplication1
{
    class Atlas
    {
        public BitmapImage image;

        public int width;
        public int height;

        public List<Sprite> sprites;

        private Atlas()
        {
            image = new BitmapImage();
            width = 0;
            height = 0;
            sprites = new List<Sprite>();
        }
    }
}
