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
    /// Interaction logic for Uses.xaml
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
                WindowActionHandler.AddSprites(imageListBox, dialog1.FileNames);
            }

        }

        private void ClearAll(object sender, RoutedEventArgs e)
        {

            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure that you want to clear all images?", "Clear All", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes)
            {
                return;
            }

            WindowActionHandler.ClearAll(imageListBox, imageDisplay);
        }

        private void imageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            WindowActionHandler.SelectionChange(imageListBox, imageDisplay);

            ////make sure SelectedValue isnt going to send us null
            //if (imageListBox.SelectedIndex != -1)
            //{
            //    imageDisplay.Source = ((Sprite)imageListBox.Items.GetItemAt(imageListBox.SelectedIndex)).image;
            //}
        }

        private void ClearSelected(object sender, RoutedEventArgs e)
        {
            WindowActionHandler.ClearSelected(imageListBox, imageDisplay);
        }

        private Atlas GenerateAtlas()
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
            return atl;
        }

        private void GeneratePreview(object sender, RoutedEventArgs e)
        {
            if (imageListBox.Items.Count < 1)
            {
                System.Windows.MessageBox.Show("Cannot Generate Preview for 0 images", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            Atlas atlas = GenerateAtlas();
            imageDisplay.Source = atlas.GetBitmapImage();
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

            //create the name for the xml doc
            string xmlName = saveDialog.FileName.Substring(0, saveDialog.FileName.Length - fileExt.Length);//subtract the file extention's length to get the filepath w/o the file ext
            xmlName += "_atlas.xml"; //adds a prefix to help asociate this xml doc with the image and also adding the .xml ext

            string fileNameOnly = saveDialog.FileName;

            int lastSlash = fileNameOnly.Length - 1;
            //find whaere the last slash in the filename is
            while (lastSlash >= 0 && fileNameOnly[lastSlash] != '\\') {
                lastSlash--;
            }

            //we want our string to be from one ahead of the last slash in the path because that's when the actual filename begins
            fileNameOnly = fileNameOnly.Substring(lastSlash + 1, fileNameOnly.Length - lastSlash - 1);

            //generate image
            Atlas atlas = GenerateAtlas();

            //set the sheetname of the atlas to point to the bitmap because we didn't know this when we made it
            atlas.xmlDoc.Element("TextureAtlas").SetAttributeValue("SheetName", fileNameOnly);

            BitmapImage savedImage = atlas.GetBitmapImage();

            //save image (check for exceptions!) //also saves and catches saving the xml
            try
            {
                //save image
                BitmapSaver.SaveBitmap(savedImage, saveDialog.FileName);

                //save bitmap
                atlas.xmlDoc.Save(xmlName);

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
