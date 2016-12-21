using Microsoft.Xna.Framework;
using System;
using System.IO;

namespace MeesGame
{
    public class FileExplorer : UIList
    {
        public delegate void FileSelectedEventHandler(FileExplorer explorer);
        public event FileSelectedEventHandler OnFileSelected;

        private readonly Color DefaultBackgroundColor = Color.Wheat;

        /// <summary>
        /// distance between each of the buttons in the file explorer
        /// </summary>
        private int buttonDistance = 10;

        private string fileExtension;

        private const int NOTHING_SELECTED = -1;

        ///returns the index of the selected button
        private int selected = NOTHING_SELECTED;

        /// <summary>
        /// directory the file explorer is displaying
        /// </summary>
        private string currentDirectory;

        /// <summary>
        /// The files that are currently shown
        /// </summary>
        private string[] fileList;

        /// <summary>
        ///
        /// </summary>
        /// <param name="location"></param>
        /// <param name="dimensions"></param>
        /// <param name="parent"></param>
        /// <param name="content"></param>
        /// <param name="fileExtension">extensions the file explorer looks for, for example .lvl</param>
        /// <param name="path">path of the folder the files are located in</param>
        public FileExplorer(Vector2? location = null, Vector2? dimensions = null, string fileExtension = "", string path = "", Color? defaultBackgroundColor = null) : base(location, dimensions, backgroundColor: defaultBackgroundColor)
        {
            backgroundColor = backgroundColor ?? DefaultBackgroundColor;

            currentDirectory = path;
            this.fileExtension = fileExtension;

            generateFileList();
            scrollBar.UpdateParentHeightWhenShowingAllChildren();
        }

        /// <summary>
        /// lists the file in the directory and puts them into buttons for the file explorer
        /// </summary>
        public void generateFileList()
        {
            children.Reset();
            fileList = Directory.GetFiles(currentDirectory);
            Vector2 nextButtonLocation = Vector2.Zero;
            for (int i = 0; i < fileList.Length; i++)
            {
                if (fileList[i].EndsWith("." + fileExtension))
                {
                    AddChild(new Button(nextButtonLocation, null, fileList[i].Substring(currentDirectory.Length + 1, fileList[i].Length - currentDirectory.Length - fileExtension.Length - 2), OnItemClicked));
                    nextButtonLocation = new Vector2(0, buttonDistance);
                }
            }
        }

        /// <summary>
        /// method called when a button is pressed. Changes the selected item in the list and
        /// updates the selected variable
        /// </summary>
        /// <param name="button">Button that was just pressed</param>
        public void OnItemClicked(UIObject button)
        {
            Button btn = (Button)button;
            if(selected != NOTHING_SELECTED)
                ((Button)children[selected]).Selected = false;
            btn.Selected = true;
            selected = children.IndexOf(btn);
            OnFileSelected?.Invoke(this);
        }

        public string SelectedFile
        {
            get
            {
                if (selected == NOTHING_SELECTED) return null;
                return fileList[selected];
            }
        }
    }
}