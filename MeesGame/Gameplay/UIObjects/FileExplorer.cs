using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Content;
using System.IO;
using MeesGame.Gameplay.UIObjects;

namespace MeesGame
{
    public class FileExplorer : UIList
    {
        private int buttonDistance = 10;

        private String fileExtension;

        //returns the selected variable
        private int selected = 0;

        //for now this is one directory, but it should be well doable to allow for a folder structure
        private String currentDirectory;


        public FileExplorer(Vector2 location, Vector2 dimensions, UIContainer parent, String fileExtension, string path) : base(location, dimensions, parent)
        {
            currentDirectory = path;
            this.fileExtension = fileExtension;

            generateFileList();
            scrollBar.ChangeTotalElementsSize();
        }

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

        public void ItemSelect(Button button)
        {
            ((Button)children[selected]).Selected = false;
            button.Selected = true;
            selected = children.IndexOf(button);
        }
    }
}
