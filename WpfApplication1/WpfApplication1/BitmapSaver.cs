using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace WpfApplication1
{
    static class BitmapSaver
    {
        //this throwes exceptions that it doesn't handle
        public static void SaveBitmap(BitmapImage savedImage, string savePath)
        {
            string fileExt = System.IO.Path.GetExtension(savePath);

            BitmapEncoder imageEnc;

            if (fileExt.ToLower().CompareTo(".png") == 0)
            {
                imageEnc = new PngBitmapEncoder();
            }
            else
            {
                throw new System.Exception("Invalid File Type");
            }

            imageEnc.Frames.Add(BitmapFrame.Create(savedImage));


            FileStream saveStream;

            saveStream = new FileStream(savePath, FileMode.Create);

            imageEnc.Save(saveStream);
            saveStream.Close();

        }
    }
}
