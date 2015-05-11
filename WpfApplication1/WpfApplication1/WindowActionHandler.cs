using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media.Imaging;

namespace WpfApplication1
{
    public static class WindowActionHandler
    {

        public static void AddSprites(ListBox imageListBox, string[] FileNames)
        {
            if (imageListBox == null)
            {
                return;
            }
            if (FileNames == null || FileNames.Count() == 0)
            {
                return;
            }
            //add items to list
            for (int i = 0; i < FileNames.Length; i++)
            {
                //imageListBox.Items.Add(dialog1.FileNames.ElementAt(i));
                imageListBox.Items.Add(new Sprite(new Uri(FileNames.ElementAt(i), UriKind.Absolute)));
            }
            //if there wasent anything selected then select the first item
            if (imageListBox.SelectedIndex == -1)
            {
                imageListBox.SelectedIndex = 0;
            }
        }

        public static void ClearAll(ListBox listTarget, Image imageTarget)
        {
            if (listTarget == null)
            {
                return;
            }
            else if (imageTarget == null)
            {
                return;
            }

            //set index to unselected
            listTarget.SelectedIndex = -1;
            listTarget.Items.Clear();
            //clear the currently displayed image
            imageTarget.Source = null;
        }

        public static void ClearSelected(ListBox listTarget, Image imageTarget)
        {
            //make shure we have something selected
            if (listTarget.SelectedIndex != -1)
            {
                //keep old index because removing an item from the list sets the index to -1
                int oldIndex = listTarget.SelectedIndex;

                //remove items
                listTarget.Items.RemoveAt(listTarget.SelectedIndex);

                //if the old index is still acceptable
                if (oldIndex < listTarget.Items.Count)
                {
                    //keep it
                    listTarget.SelectedIndex = oldIndex;
                }
                else
                {
                    //if the list is empty
                    if (listTarget.Items.IsEmpty)
                    {
                        //clear the displayed image
                        imageTarget.Source = null;
                    }
                    else
                    {
                        //set the index to the last item on the list
                        listTarget.SelectedIndex = listTarget.Items.Count - 1;
                    }
                }
            }
        }

        public static void SelectionChange(ListBox listTarget, Image imageTarget)
        {
            //make sure SelectedValue isnt going to send us null
            if (listTarget.SelectedIndex != -1)
            {
                imageTarget.Source = ((Sprite)listTarget.Items.GetItemAt(listTarget.SelectedIndex)).image;
            }
        }

        private static Atlas GenerateAtlas(ListBox targetList)
        {
            //convert listbox list to list of sprites
            List<Sprite> spritelist = new List<Sprite>();
            for (int i = 0; i < targetList.Items.Count; i++)
            {
                spritelist.Add((Sprite)targetList.Items.GetItemAt(i));
            }

            //create image 
            Atlas atl = AtlasBuilder.CreateAtlas(spritelist);

            if (atl == null)
            {
                return null;
            }
            //return
            return atl;
        }


        public static void GeneratePreview(ListBox listTarget, Image imageTarget)
        {
            Atlas atlas = GenerateAtlas(listTarget);
            imageTarget.Source = atlas.GetBitmapImage();
            listTarget.SelectedIndex = -1;
        }

        public static void Export(ListBox listTarget, string filename)
        {
            //create the name for the xml doc
            string xmlName = filename.Substring(0, filename.Length - 4);//subtract the file extention's length to get the filepath w/o the file ext
            xmlName += "_atlas.xml"; //adds a prefix to help asociate this xml doc with the image and also adding the .xml ext

            string fileNameOnly = filename;

            int lastSlash = fileNameOnly.Length - 1;
            //find whaere the last slash in the filename is
            while (lastSlash >= 0 && fileNameOnly[lastSlash] != '\\')
            {
                lastSlash--;
            }

            //we want our string to be from one ahead of the last slash in the path because that's when the actual filename begins
            fileNameOnly = fileNameOnly.Substring(lastSlash + 1, fileNameOnly.Length - lastSlash - 1);

            //generate image
            Atlas atlas = GenerateAtlas(listTarget);

            //set the sheetname of the atlas to point to the bitmap because we didn't know this when we made it
            atlas.xmlDoc.Element("TextureAtlas").SetAttributeValue("SheetName", fileNameOnly);

            BitmapImage savedImage = atlas.GetBitmapImage();

            //these throw exceptions that will need to be handeled outside of this meathod
            //save image
            BitmapSaver.SaveBitmap(savedImage, filename);

            //save bitmap
            atlas.xmlDoc.Save(xmlName);
        }

    }
}
