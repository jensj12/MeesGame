using Microsoft.Xna.Framework;
using System;
using System.IO;
using MeesGame.Gameplay.UIObjects;

namespace MeesGame
{
    public class FileExplorer : UIList
    {
        /// <summary>
        /// distance between each of the buttons in the file explorer
        /// </summary>
        private int buttonDistance = 10;

        private String fileExtension;

        ///returns the index of the selected button
        private int selected = 0;

        /// <summary>
        /// directory the file explorer is displaying
        /// </summary>
        private String currentDirectory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="location"></param>
        /// <param name="dimensions"></param>
        /// <param name="parent"></param>
        /// <param name="content"></param>
        /// <param name="fileExtension">extensions the file explorer looks for, for example .lvl</param>
        /// <param name="path">path of the folder the files are located in</param>
        public FileExplorer(Vector2 location, Vector2 dimensions, UIContainer parent, String fileExtension, string path) : base(location, dimensions, parent)
        {
            currentDirectory = path;
            this.fileExtension = fileExtension;

            generateFileList();
            scrollBar.ChangeTotalElementsSize();
        }

        /// <summary>
        /// lists the file in the directory and puts them into buttons for the file explorer
        /// </summary>
        public void generateFileList()
        {
            children.Reset();
            String[] tmpFileList = Directory.GetFiles(currentDirectory);
            int index = 0;
            for (int i = 0; i < tmpFileList.Length; i++)
            {
                if (tmpFileList[i].EndsWith("." + fileExtension))
                {
                    if (index == 0)
                    {
                        children.Add(new Button(new Vector2(0, 0), Dimensions, this, tmpFileList[i].Substring(currentDirectory.Length + 1, tmpFileList[i].Length - currentDirectory.Length - fileExtension.Length - 2), ItemSelect));
                    }
                    else
                    {
                        children.Add(new Button(new Vector2(0, buttonDistance), Dimensions, this, tmpFileList[i].Substring(currentDirectory.Length + 1, tmpFileList[i].Length - currentDirectory.Length - fileExtension.Length - 2), ItemSelect));
                    }
                    index++;
                }
            }
        }

        /// <summary>
        /// method called when a button is pressed. Changes the selected item in the list and
        /// updates the selected variable
        /// </summary>
        /// <param name="button">Button that was just pressed</param>
        public void ItemSelect(Button button)
        {
            ((Button)children[selected]).Selected = false;
            button.Selected = true;
            selected = children.IndexOf(button);
        }
    }
}
