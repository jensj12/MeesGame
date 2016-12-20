using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace MeesGame
{
    class FileIO
    {
        /// <summary>
        /// Saves the level to a file with the specified name.
        /// </summary>
        /// <param name="obj"> Dummy variable. </param>
        /// <param name="ea"> Dummy variable. </param>
        public void Save(object obj, EventArgs ea)
        {
            string fileName = null;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt Files (*.txt) | *.txt | All Files (*.*) | *.*";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                Stream stream = new FileStream(fileName, FileMode.Create);
                XmlWriter.Create(stream);
                //doe iets
            }
        }

        /// <summary>
        /// Loads a level from a file with the specified name.
        /// </summary>
        /// <param name="obj"> Dummy variable. </param>
        /// <param name="ea"> Dummy variable. </param>
        public void Load(object obj, EventArgs ea)
        {
            //iets
        }
    }
}