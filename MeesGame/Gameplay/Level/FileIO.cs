using System;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MeesGame
{
    class FileIO
    {
        /// <summary>
        /// Saves the level to a file with the specified name.
        /// </summary>
        /// <param name="level"> The level being saved. </param>
        /// <param name="filename"> The name of the file the level is being saved to. </param>
        public static void Save(Level level)
        {
            string fileName = null;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt Files (*.txt) | *.txt ";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog.FileName;
                Stream stream = new FileStream(fileName, FileMode.Create);
                XmlWriter writer = XmlWriter.Create(stream);
                new XmlSerializer(typeof(TileField)).Serialize(writer, level.Tiles);
                writer.Close();
            }
        }

        /// <summary>
        /// Loads a level from a file with the specified name.
        /// </summary>
        public static void Load(string filename)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "txt Files|*.txt ";
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //Lees het bestand uit en construeer een TileField hieruit.
            }
        }
    }
}
