using MeesGame.UI;
using System.Collections.Generic;
using System.IO;

namespace MeesGame
{
    class FileExplorer : SortedList
    {
        public delegate void OnFileSelectedEventHandler(FileExplorer explorer);

        public event OnFileSelectedEventHandler FileSelected;

        /// <summary>
        /// Distance between each of the buttons in the file explorer.
        /// </summary>
        private const int DistanceBetweenChildren = 20;

        /// <summary>
        /// The files with the extension "EG. .txt" the fileExplorer shows.
        /// </summary>
        private string fileExtension;

        /// <summary>
        /// Directory the file explorer is displaying.
        /// </summary>
        private string currentDirectory;

        /// <summary>
        /// Index of the selected button.
        /// </summary>
        private Button selectedChild = null;

        /// <summary>
        /// Maps children to their paths.
        /// </summary>
        private Dictionary<UIComponent, string> childToPath;

        /// <summary>
        /// Creates a fileExplorer.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="dimensions"></param>
        /// <param name="fileExtension">Extension the file explorer looks for, for example .txt</param>
        /// <param name="path">path of the folder the files are located in</param>
        public FileExplorer(Location location = null, Dimensions dimensions = null, string fileExtension = "", string path = "", Background background = null) : base(location, dimensions, DistanceBetweenChildren: DistanceBetweenChildren)
        {
            Background uiBackground = background ?? new Background(Utility.SolidWhiteTexture, Utility.DrawingColorToXNAColor(DefaultUIValues.Default.FileExplorerBackground));

            currentDirectory = path;
            this.fileExtension = fileExtension;

            childToPath = new Dictionary<UIComponent, string>();

            GenerateFileList();
        }

        public override void AddChild(UIComponent child)
        {
            AddChild(child, "");
        }

        public void AddChild(UIComponent child, string path = "")
        {
            base.AddChild(child);
            childToPath.Add(child, path);
            child.Click += ChildClickHandler;
        }

        /// <summary>
        /// lists the file in the directory and puts them into buttons for the file explorer
        /// </summary>
        public void GenerateFileList()
        {
            string[] foundFiles = Directory.GetFiles(currentDirectory);
            Location nextButtonLocation = SimpleLocation.Zero;
            for (int i = 0; i < foundFiles.Length; i++)
            {
                if (foundFiles[i].EndsWith("." + fileExtension))
                {
                    AddChild(new SpriteSheetButton(SimpleLocation.Zero, null, foundFiles[i].Substring(currentDirectory.Length + 1, foundFiles[i].Length - currentDirectory.Length - fileExtension.Length - 2), ChildClickHandler), foundFiles[i]);
                }
            }
        }

        /// <summary>
        /// method called when a button is pressed. Changes the selected item in the list and
        /// updates the selected variable
        /// </summary>
        /// <param name="button">Button that was just pressed</param>
        public void ChildClickHandler(UIComponent clickedChild)
        {
            if (selectedChild is Button)
                selectedChild.Selected = false;
            if(clickedChild is Button)
            {
                selectedChild = (Button)clickedChild;
                selectedChild.Selected = true;
            }
            FileSelected?.Invoke(this);
        }

        /// <summary>
        /// Index of the selected button.
        /// </summary>
        public Button SelectedFile
        {
            get
            {
                return selectedChild;
            }
        }

        public string GetPathFromChild(UIComponent button)
        {
            if(childToPath.ContainsKey(button))
                return childToPath[button];
            return "";
        }

        public override void Reset()
        {
            base.Reset();
            childToPath.Clear();
        }
    }
}
