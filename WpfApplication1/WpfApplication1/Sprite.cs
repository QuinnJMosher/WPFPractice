using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media.Imaging;

namespace WpfApplication1
{
    class Sprite
    {
        //vars
        public string safeName;
        public BitmapImage image;

        public float posX;
        public float posY;

        public int width;
        public int height;

        //constructors
        public Sprite(Uri path)
        {
            image = new BitmapImage(path);
            safeName = path.Segments[path.Segments.Length - 1].Substring(1);
            width = image.PixelWidth;
            height = image.PixelHeight;
        }

        //meathods

        public override string ToString() {
            return safeName;
        }
        
   }
}
