﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Media.Imaging;

using System.Xml.Linq;

namespace WpfApplication1
{
    class Atlas
    {
        public RenderTargetBitmap image;

        public int width;
        public int height;

        public List<Sprite> sprites;

        public XDocument xmlDoc;

        public Atlas()
        {
            image = null;
            width = 0;
            height = 0;
            sprites = null;
            xmlDoc = null;
        }

        public BitmapImage GetBitmapImage()
        {

            if (image == null)
            {
                return null;
            } 

            BitmapImage atlasImg = new BitmapImage();

            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(image));

            using (MemoryStream stream = new MemoryStream())
            {
                encoder.Save(stream);
                stream.Seek(0, SeekOrigin.Begin);

                atlasImg.BeginInit();
                atlasImg.CacheOption = BitmapCacheOption.OnLoad;
                atlasImg.StreamSource = stream;
                atlasImg.EndInit();
            }

            return atlasImg;
        }
    }
}
