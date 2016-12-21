using System;
using System.IO;
using System.Windows.Forms;
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
        public static void Save(TileField tilefield)
        {
            string fileName = null;
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "txt Files (*.txt) | *.txt ";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialog.FileName;
                Stream stream = new FileStream(fileName, FileMode.Create);
                LevelData data = new LevelData(tilefield);
                XmlWriter writer = XmlWriter.Create(stream);
                new XmlSerializer(typeof(LevelData)).Serialize(writer, data);
                writer.Close();
            }
        }

        /// <summary>
        /// Loads a level from a file with the specified name.
        /// <param name = "filename"> The name of the file the level is being loaded from. </param>
        /// </summary>
        public static LevelData Load(string filename)
        {
            LevelData data = new LevelData();
            Stream stream = new FileStream(filename, FileMode.Open);
            XmlReader reader = XmlReader.Create(stream);
            return data = (LevelData)new XmlSerializer(typeof(LevelData)).Deserialize(reader);
        }
    }
}
