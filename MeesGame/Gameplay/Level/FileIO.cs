using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace MeesGame
{
    static class FileIO
    {
        /// <summary>
        /// Shows a Dialog to select a file location for saving
        /// </summary>
        /// <returns>The selected file name. Null if the dialog is cancelled by the user.</returns>
        public static string ShowSaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            string directory = GameEnvironment.AssetManager.Content.RootDirectory + "/levels";
            DirectoryInfo info = Directory.CreateDirectory(directory);
            saveFileDialog.InitialDirectory = info.FullName;
            saveFileDialog.Filter = Strings.file_dialog_filter_lvl;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                return saveFileDialog.FileName;
            else
                return null;
        }

        /// <summary>
        /// Saves the level to a file with the specified name.
        /// </summary>
        /// <param name="level"> The level being saved. </param>
        public static void Save(TileField tilefield)
        {
            string fileName = ShowSaveFileDialog();
            if (fileName != null)
            {
                try
                {
                    using (Stream stream = new FileStream(fileName, FileMode.Create))
                    using (XmlWriter writer = XmlWriter.Create(stream))
                        new XmlSerializer(typeof(LevelData)).Serialize(writer, new LevelData(tilefield));
                }
                catch (IOException e)
                {
                    MessageBox.Show(Strings.save_file_error_with_comment + e.Message);
                }
            }
        }

        /// <summary>
        /// Loads a level from a file with the specified name.
        /// <param name = "filename"> The name of the file the level is being loaded from. </param>
        /// </summary>
        public static LevelData Load(string filename)
        {
            try
            {
                using (Stream stream = new FileStream(filename, FileMode.Open))
                using (XmlReader reader = XmlReader.Create(stream))
                    return (LevelData)new XmlSerializer(typeof(LevelData)).Deserialize(reader);
            }
            catch (IOException e)
            {
                MessageBox.Show(Strings.load_file_error_with_comment + e.Message);
                throw;
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show(Strings.load_file_error_parse);
                throw;
            }
        }
    }
}
