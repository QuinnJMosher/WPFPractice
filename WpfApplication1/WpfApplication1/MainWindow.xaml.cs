using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Security;

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
     
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            
        }

        private void FindFile(object sender, RoutedEventArgs e)
        {
            //create dialog window
            OpenFileDialog dialog1 = new OpenFileDialog();
            //set dialog settings
            dialog1.Multiselect = true;
            dialog1.Filter = "Image Files (*.bmp;*.jpg;*.gif;*.png)|*.bmp;*.jpg;*.gif;*.png";
            //open window and get how it closed
            System.Windows.Forms.DialogResult result = dialog1.ShowDialog();

            //if dialog was closed by pressing "ok"
            if (result == System.Windows.Forms.DialogResult.OK)
            {
                //add items to list
                for (int i = 0; i < dialog1.FileNames.Length; i++)
                {
                   //imageListBox.Items.Add(dialog1.FileNames.ElementAt(i));
                    imageListBox.Items.Add(new Sprite(new Uri(dialog1.FileNames.ElementAt(i), UriKind.Absolute)));
                }
                //if there wasent anything selected then select the first item
                if (imageListBox.SelectedIndex == -1)
                {
                    imageListBox.SelectedIndex = 0;
                }
            }

        }

        private void ClearAll(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure that you want to clear all images?", "Clear All", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }


            //set index to unselected
            imageListBox.SelectedIndex = -1;
            imageListBox.Items.Clear();
            //clear the currently displayed image
            imageDisplay.Source = null;
        }

        private void imageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //make sure SelectedValue isnt going to send us null
            if (imageListBox.SelectedIndex != -1)
            {
                //create and setup image object
                //BitmapImage Im = new BitmapImage();
                //Im.BeginInit();
                ////Selected value returns an Object so an explicit cast is needed
                //Im.UriSource = new Uri((string)imageListBox.Items.GetItemAt(imageListBox.SelectedIndex), UriKind.Absolute);
                //Im.EndInit();
                //set the image
                imageDisplay.Source = ((Sprite)imageListBox.Items.GetItemAt(imageListBox.SelectedIndex)).image;
            }
        }

        private void ClearSelected(object sender, RoutedEventArgs e)
        {
            //make shure we have something selected
            if (imageListBox.SelectedIndex != -1) {
                //keep old index because removing an item from the list sets the index to -1
                int oldIndex = imageListBox.SelectedIndex;

                //remove items
                imageListBox.Items.RemoveAt(imageListBox.SelectedIndex);

                //if the old index is still acceptable
                if (oldIndex < imageListBox.Items.Count)
                {
                    //keep it
                    imageListBox.SelectedIndex = oldIndex;
                }
                else
                {
                    //if the list is empty
                    if (imageListBox.Items.IsEmpty)
                    {
                        //clear the displayed image
                        imageDisplay.Source = null;
                    }
                    else
                    {
                        //set the index to the last item on the list
                        imageListBox.SelectedIndex = imageListBox.Items.Count - 1;
                    }
                }
            }
        }

        private BitmapImage GenerateImage()
        {
            //convert listbox list to list of sprites
            List<Sprite> spritelist = new List<Sprite>();
            for (int i = 0; i < imageListBox.Items.Count; i++)
            {
                spritelist.Add((Sprite)imageListBox.Items.GetItemAt(i));
            }

            //create image 
            Atlas atl = AtlasBuilder.CreateAtlas(spritelist);

            if (atl == null)
            {
                return null;
            }
            //return
            return atl.GetBitmapImage();
        }

        private void GeneratePreview(object sender, RoutedEventArgs e)
        {
            imageDisplay.Source = GenerateImage();
            imageListBox.SelectedIndex = -1;
        }

        private void ExportFile(object sender, RoutedEventArgs e)
        {
            //check for content
            if (imageListBox.Items.Count < 1)
            {
                System.Windows.MessageBox.Show("Cannot Create files for 0 images", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //open savefiledialoug
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = "PNG Image (*.png)|*.png";

            System.Windows.Forms.DialogResult diagResult = saveDialog.ShowDialog();

            if (diagResult != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            string fileExt = System.IO.Path.GetExtension(saveDialog.FileName);

            if (fileExt.ToLower().CompareTo(".png") != 0)
            {
                System.Windows.MessageBox.Show("Invalid File Type", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            //generate image
            BitmapImage savedImage = GenerateImage();

            //save image (check for exceptions!)
            try
            {
                BitmapSaver.SaveBitmap(savedImage, saveDialog.FileName);
            }
            catch (SecurityException ex)
            {
                System.Windows.MessageBox.Show("Security Error: you may not have the permissions required: " + ex.Message, "Security Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Unknown Error: " + ex.Message, "Unknown Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
