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
        /// The directory from which levels are saved / loaded by default
        /// </summary>
        public static string LEVEL_DIRECTORY = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)+"\\TheAMazeIngEscape\\levels";

        /// <summary>
        /// Shows a Dialog to select a file location for saving a level
        /// </summary>
        /// <returns>The selected file name. Null if the dialog is cancelled by the user.</returns>
        public static string ShowSaveFileDialog()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.InitialDirectory = Directory.CreateDirectory(LEVEL_DIRECTORY).FullName;
            saveFileDialog.Filter = Strings.file_dialog_filter_lvl;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                return saveFileDialog.FileName;
            else
                return null;
        }

        public static LevelData ShowLoadFileDialog()
        {
            OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog();
            DirectoryInfo info = Directory.CreateDirectory(LEVEL_DIRECTORY);
            openFileDialog.InitialDirectory = info.FullName;
            openFileDialog.Filter = Strings.file_dialog_filter_lvl;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return Load(openFileDialog.FileName);
            }
            else return null;
        }

        /// <summary>
        /// Saves the level to a file with the specified name.
        /// </summary>
        /// <param name="tileField">The level being saved.</param>
        /// <param name="fileName">The file to save to.</param>
        public static void Save(TileField tileField, string fileName)
        {
            try
            {
                using (Stream stream = new FileStream(fileName, FileMode.Create))
                using (XmlWriter writer = XmlWriter.Create(stream))
                    new XmlSerializer(typeof(LevelData)).Serialize(writer, new LevelData(tileField));
            }
            catch (IOException e)
            {
                MessageBox.Show(Strings.save_file_error_with_comment + e.Message);
            }
        }

        /// <summary>
        /// Shows a file dialog to choose a file to which the tileField will be saved.
        /// </summary>
        /// <param name="tileField"> The level being saved. </param>
        public static void Save(TileField tileField)
        {
            string fileName = ShowSaveFileDialog();
            if (fileName != null)
                Save(tileField, fileName);
        }

        /// <summary>
        /// Loads a level from a file with the specified name.
        /// </summary>
        /// <param name="filename"> The name of the file the level is being loaded from. </param>
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
