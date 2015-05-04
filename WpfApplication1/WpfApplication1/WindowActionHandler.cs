using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace WpfApplication1
{
    static class WindowActionHandler
    {

        public static void AddSprites(ListBox imageListBox, string[] FileNames)
        {
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


    }
}
