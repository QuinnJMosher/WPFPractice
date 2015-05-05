using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using WpfApplication1;//running project

using System.Windows.Controls;
using System.Windows;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void AddSpriteTest()
        {
            //setup1 [empty list]
            ListBox listBox = new ListBox();
            string[] fileNames = new string[0];

            //run
            WindowActionHandler.AddSprites(listBox, fileNames);

            //setup2 [null listbox]
            listBox = null;
            fileNames = new string[] 
            { 
                "C:\\Users\\quinn.mosher\\Pictures\\kenny_medals\\flat_medal1.png", 
                "C:\\Users\\quinn.mosher\\Pictures\\kenny_medals\\flat_medal2.png", 
                "C:\\Users\\quinn.mosher\\Pictures\\kenny_medals\\flat_medal3.png" 
            };

            //run
            WindowActionHandler.AddSprites(listBox, fileNames);

            //setup [valid data]
            listBox = new ListBox();
            //filenames already set

            //run
            WindowActionHandler.AddSprites(listBox, fileNames);

            //check data
            Assert.AreEqual(listBox.Items.Count, 3);
            Assert.AreEqual(listBox.SelectedIndex, 0);

            Assert.AreEqual(((Sprite)listBox.Items.GetItemAt(0)).safeName, "flat_medal1.png", false);
            Assert.AreEqual(((Sprite)listBox.Items.GetItemAt(1)).safeName, "flat_medal2.png", false);
            Assert.AreEqual(((Sprite)listBox.Items.GetItemAt(2)).safeName, "flat_medal3.png", false);

        }

        [TestMethod]
        public void ClearAllTest()
        {

            string[] fileNames = new string[] 
            { 
                "C:\\Users\\quinn.mosher\\Pictures\\kenny_medals\\flat_medal1.png", 
                "C:\\Users\\quinn.mosher\\Pictures\\kenny_medals\\flat_medal2.png", 
                "C:\\Users\\quinn.mosher\\Pictures\\kenny_medals\\flat_medal3.png" 
            };

            //setup3 [valid data]
            ListBox listbox = new ListBox();
            WindowActionHandler.AddSprites(listbox, fileNames);
            Image image = new Image();


            //run
            WindowActionHandler.ClearAll(listbox, image);

            //check data
            Assert.AreEqual(image.Source, null);
            Assert.AreEqual(listbox.SelectedIndex, -1);
            Assert.AreEqual(listbox.Items.Count, 0);
           
        }

        [TestMethod]
        public void ClearSelectedTest()
        {
            string[] fileNames = new string[] 
            { 
                "C:\\Users\\quinn.mosher\\Pictures\\kenny_medals\\flat_medal1.png", 
                "C:\\Users\\quinn.mosher\\Pictures\\kenny_medals\\flat_medal2.png", 
                "C:\\Users\\quinn.mosher\\Pictures\\kenny_medals\\flat_medal3.png" 
            };

            Image image = new Image();

            //setup1 [no items in list]
            ListBox listBox = new ListBox();

            //test
            WindowActionHandler.ClearSelected(listBox, image);

            //setup2 [no selected item]
            WindowActionHandler.AddSprites(listBox, fileNames);
            listBox.SelectedIndex = -1;

            //test
            WindowActionHandler.ClearSelected(listBox, image);

            //check data
            Assert.AreEqual(listBox.Items.Count, 3);

            //setup3 [deleting end item]
            //listbox already set
            listBox.SelectedIndex = 2;

            //test
            WindowActionHandler.ClearSelected(listBox, image);

            //check data
            Assert.AreEqual(listBox.Items.Count, 2);
            Assert.AreEqual(listBox.SelectedIndex, 1);

            //setup4 [deleteing last item]
            WindowActionHandler.ClearSelected(listBox, image);

            //test
            WindowActionHandler.ClearSelected(listBox, image);

            //check data
            Assert.AreEqual(listBox.Items.Count, 0);
            Assert.AreEqual(listBox.SelectedIndex, -1);

            //seutp5 [standard]
            WindowActionHandler.AddSprites(listBox, fileNames);
            listBox.SelectedIndex = 1;

            //run
            WindowActionHandler.ClearSelected(listBox, image);

            //check data
            Assert.AreEqual(listBox.Items.Count, 2);
            Assert.AreEqual(listBox.SelectedIndex, 1);
            Assert.AreEqual(((Sprite)listBox.Items.GetItemAt(0)).safeName, "flat_medal1.png");
            Assert.AreEqual(((Sprite)listBox.Items.GetItemAt(1)).safeName, "flat_medal3.png");
        }

        [TestMethod]
        public void SelectionChangeTest()
        {
            string[] fileNames = new string[] 
            { 
                "C:\\Users\\quinn.mosher\\Pictures\\kenny_medals\\flat_medal1.png", 
                "C:\\Users\\quinn.mosher\\Pictures\\kenny_medals\\flat_medal2.png", 
                "C:\\Users\\quinn.mosher\\Pictures\\kenny_medals\\flat_medal3.png" 
            };

            //genral setup
            ListBox listbox = new ListBox();
            WindowActionHandler.AddSprites(listbox, fileNames);
            Image image = new Image();
            //setup [valid]
            listbox.SelectedIndex = 1;

            //run
            WindowActionHandler.SelectionChange(listbox, image);

            //check data
            Assert.AreEqual(listbox.SelectedIndex, 1);
            Assert.AreEqual(image.Source, ((Sprite)listbox.Items.GetItemAt(1)).image);
        }

        [TestMethod]
        public void GeneratePreviewTest()
        {
            //setup
            string[] fileNames = new string[] 
            { 
                "C:\\Users\\quinn.mosher\\Pictures\\kenny_medals\\flat_medal1.png", 
                "C:\\Users\\quinn.mosher\\Pictures\\kenny_medals\\flat_medal2.png", 
                "C:\\Users\\quinn.mosher\\Pictures\\kenny_medals\\flat_medal3.png" 
            };

            ListBox listbox = new ListBox();
            WindowActionHandler.AddSprites(listbox, fileNames);
            Image image = new Image();

            //run
            WindowActionHandler.GeneratePreview(listbox, image);

            //check data
            Assert.AreNotEqual(image.Source, null);
            Assert.AreEqual(listbox.SelectedIndex, -1);
        }
    }
}
