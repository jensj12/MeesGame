using MeesGame.UI;
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
        private int buttonDistance = 10;

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
        private int selectedIndex = -1;

        /// <summary>
        /// A list of strings the fileExplorer is currently showing.
        /// </summary>
        private string[] fileList;

        /// <summary>
        /// Creates a fileExplorer.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="dimensions"></param>
        /// <param name="fileExtension">Extension the file explorer looks for, for example .txt</param>
        /// <param name="path">path of the folder the files are located in</param>
        public FileExplorer(Location location = null, Dimensions dimensions = null, string fileExtension = "", string path = "", Background background = null) : base(location, dimensions)
        {
            Background uiBackground = background ?? new Background(Utility.SolidWhiteTexture, Utility.DrawingColorToXNAColor(DefaultUIValues.Default.FileExplorerBackground));

            currentDirectory = path;
            this.fileExtension = fileExtension;

            generateFileList();
            UpdateHeightWhenExpanded();
        }

        /// <summary>
        /// lists the file in the directory and puts them into buttons for the file explorer
        /// </summary>
        public void generateFileList()
        {
            children.Clear();
            fileList = Directory.GetFiles(currentDirectory);
            Location nextButtonLocation = SimpleLocation.Zero;
            for (int i = 0; i < fileList.Length; i++)
            {
                if (fileList[i].EndsWith("." + fileExtension))
                {
                    AddChild(new SpriteSheetButton(nextButtonLocation, null, fileList[i].Substring(currentDirectory.Length + 1, fileList[i].Length - currentDirectory.Length - fileExtension.Length - 2), OnItemClicked));
                    nextButtonLocation = new SimpleLocation(0, buttonDistance);
                }
            }
        }

        /// <summary>
        /// method called when a button is pressed. Changes the selected item in the list and
        /// updates the selected variable
        /// </summary>
        /// <param name="button">Button that was just pressed</param>
        public void OnItemClicked(UIComponent button)
        {
            Button btn = (Button)button;
            if (selectedIndex != -1)
                ((Button)children[selectedIndex]).Selected = false;
            btn.Selected = true;
            selectedIndex = children.IndexOf(btn);
            FileSelected?.Invoke(this);
        }

        /// <summary>
        /// Index of the selected button.
        /// </summary>
        public string SelectedFile
        {
            get
            {
                if (selectedIndex == -1) return null;
                return fileList[selectedIndex];
            }
        }
    }
}
