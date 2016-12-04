using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Content;
using System.IO;
using MeesGame.Gameplay.UIObjects;

namespace MeesGame
{
    /// <summary>
    /// UI Object that allows you to select a file
    /// </summary>
    public class FileExplorer : UIList
    {
        /// <summary>
        /// ContentManager for the textures
        /// </summary>
        private ContentManager content;

        /// <summary>
        /// The distance between buttons
        /// </summary>
        private float BUTTON_DISTANCE = 10;

        /// <summary>
        /// The extension of the files that should be shown
        /// </summary>
        private string fileExtension;

        /// <summary>
        /// The index of the selected selected button
        /// </summary>
        private int selected = -1;

        /// <summary>
        /// Path to the directory of which the files should be shown
        /// </summary>
        private string currentDirectory;

        /// <summary>
        /// Creates a new FileExplorer
        /// </summary>
        /// <param name="position">The position</param>
        /// <param name="dimensions">The dimensions</param>
        /// <param name="parent">The parent</param>
        /// <param name="content">The contentmanager to be used for textures</param>
        /// <param name="fileExtension">The extension to filter files by</param>
        /// <param name="directory">Path to the directory of which files should be shown</param>
        public FileExplorer(Vector2 position, Vector2 dimensions, UIContainer parent, ContentManager content, string fileExtension, string directory) : base(position, dimensions, parent)
        {
            currentDirectory = directory;
            this.content = content;
            this.fileExtension = fileExtension;

            generateButtons();
            scrollBar.ChangeTotalElementsSize();
        }

        /// <summary>
        /// Generates buttons for the files that should be shown in this FileExplorer
        /// </summary>
        private void generateButtons()
        {
            children.Reset();
            string[] filePaths = Directory.GetFiles(currentDirectory);
            Vector2 buttonPosition = Vector2.Zero;
            foreach (string filePath in filePaths)
            {
                if (filePath.EndsWith("." + fileExtension))
                {
                    string name = filePath.Substring(currentDirectory.Length + 1, filePath.Length - currentDirectory.Length - fileExtension.Length - 2);
                    children.Add(new Button(buttonPosition, Dimensions, this, content, name, SelectButton));
                    buttonPosition = new Vector2(buttonPosition.X, buttonPosition.Y + BUTTON_DISTANCE);
                }
            }
        }

        /// <summary>
        /// If a file in this explorer was selected
        /// </summary>
        public bool HasSelected
        {
            get
            {
                return selected >= 0;
            }
        }

        /// <summary>
        /// Mark the button as selected
        /// </summary>
        /// <param name="button"></param>
        public void SelectButton(Button button)
        {
            if(HasSelected)
                ((Button)children[selected]).Selected = false;
            button.Selected = true;
            selected = children.IndexOf(button);
        }
    }
}
