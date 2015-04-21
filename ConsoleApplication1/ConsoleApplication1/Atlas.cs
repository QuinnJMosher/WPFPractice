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
        public RenderTargetBitmap image;

        public int width;
        public int height;

        public List<Sprite> sprites;

        public Atlas()
        {
            image = null;
            width = 0;
            height = 0;
            sprites = null;
        }
    }
}
