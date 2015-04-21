﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Media;
using System.Windows.Media.Imaging;

using System.Windows.Rect;

namespace ConsoleApplication1
{
    static class AtlasBuilder
    {
        //options
        public bool powerOfTwo = false;
        public int _padding = 2;
        public int Padding
        {
            get
            {
                return _padding;
            }
            set
            {
                if (value >= 0)
                {
                    _padding = value;
                }
                else
                {
                    _padding = 0;
                }
            }
        }

        private Atlas working_atlas;

        //primary meathod
        public Atlas CreateAtlas(List<Sprite> in_spriteList)
        {
            working_atlas = new Atlas();
            working_atlas.sprites = new List<Sprite>(in_spriteList);

            CalculateSize();
            ReadySprites();
            //draw Atlas

            Atlas output = working_atlas;
            working_atlas = null;
            return output;
        }

        //helper meathods
        private void CalculateSize()
        {
            int atlas_width = 0;
            for (int i = 0; i < working_atlas.sprites.Count; i++)
            {
                atlas_width += working_atlas.sprites[i].width;

                atlas_width += Padding;
            }

            int atlas_height = 0;
            for (int i = 0; i < working_atlas.sprites.Count; i++)
            {
                atlas_height = Math.Max(atlas_height, working_atlas.sprites[i].height);
            }

            //ignoreing pow of 2

            working_atlas.width = atlas_width;
            working_atlas.height = atlas_height;
        }

        private void ReadySprites()
        {
            int currentX = 0, currentY = 0;
            int lastLineTallest = 0;
            for (int i = 0; i < working_atlas.sprites.Count; i++)
            {
                //set the current sprite into this variabel for brevity
                Sprite CurrentSprite = working_atlas.sprites[i];

                //if will go off of the end
                if (currentX + CurrentSprite.width > working_atlas.width)
                {
                    //reset X
                    currentX = 0;
                    //move the y value down by the tallest item on the last line
                    currentY += lastLineTallest + Padding;
                    //reset the tallest
                    lastLineTallest = 0;
                }

                //set the value to the current position
                CurrentSprite.posX = currentX;
                CurrentSprite.posY = currentY;

                //move the x value (if we go off the end because of the padding then we'll find on the next check)
                currentX += CurrentSprite.width + Padding;

                //check for the tallest
                lastLineTallest = Math.Max(lastLineTallest, CurrentSprite.height);
            }
        }

        private void BuildAtlasImmage()
        {
            //create bitmap frames
            BitmapFrame[] frames = new BitmapFrame[working_atlas.sprites.Count];
            for (int i = 0; i < working_atlas.sprites.Count; i++)
            {
                frames[i] = BitmapDecoder.Create(working_atlas.sprites[i].image.UriSource, BitmapCreateOptions.DelayCreation, BitmapCacheOption.OnLoad).Frames.First();
            }

            //create drawing visual (a kind of canvas)
            DrawingVisual dv = new DrawingVisual();

            //draw to dv
            using (DrawingContext context = dv.RenderOpen()) 
            {
                for (int i = 0; i < frames.Length; i++)
                {
                    context.DrawImage(frames[i], new System.Windows.Rect(working_atlas.sprites[i].posX, working_atlas.sprites[i].posY, working_atlas.sprites[i].width, working_atlas.height));
                }
            }

            //convert dv to a bitmap
            RenderTargetBitmap atlas_bmp = new RenderTargetBitmap(working_atlas.width, working_atlas.height, 96, 96, PixelFormats.Pbgra32);
            atlas_bmp.Render(dv);
            atlas_bmp.Freeze();

            //store
            working_atlas.image = atlas_bmp;
        }

    }
}
