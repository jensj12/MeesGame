using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Content;
using System.IO;

namespace MeesGame
{
    public class FileExplorer : UIList
    {
        private ContentManager content;

        private int buttonDistance = 10;

        private String fileExtension;

        //returns the selected variable
        private int selected = 0;

        //for now this is one directory, but it should be well doable to allow for a folder structure
        private String currentDirectory;


        public FileExplorer(Vector2 location, Vector2 dimensions, UIObject parent, ContentManager content, String fileExtension, string path) : base(location, dimensions, parent)
        {
            currentDirectory = path;
            this.content = content;
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
                        children.Add(new ListButton(new Vector2(0, 0), Dimensions, this, content, tmpFileList[i].Substring(currentDirectory.Length + 1, tmpFileList[i].Length - currentDirectory.Length - fileExtension.Length - 2), index, ItemSelect));
                    }
                    else
                    {
                        children.Add(new ListButton(new Vector2(0, buttonDistance + ((ListButton)children[index - 1]).Rectangle.Height + (int)((ListButton)children[index - 1]).RelativeLocation.Y), Dimensions, this, content, tmpFileList[i].Substring(currentDirectory.Length + 1, tmpFileList[i].Length - currentDirectory.Length - fileExtension.Length - 2), index, ItemSelect));
                    }
                    index++;
                }
            }
        }

        public void ItemSelect(Object o)
        {
            ListButton listbutton = (ListButton)o;
            ((ListButton)children[selected]).Selected = false;
            listbutton.Selected = true;
            selected = listbutton.Index;
        }
    }
}
