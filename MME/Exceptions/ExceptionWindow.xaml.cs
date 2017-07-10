using System;
using System.Text;
using System.Windows;
using MME.Console;

namespace MME.Exceptions
{
    /// <summary>
    ///     Interaction logic for ExceptionWindow.xaml
    /// </summary>
    public partial class ExceptionWindow
    {
        public enum Exceptions
        {
            Empty,

            /* PROJECT EXCEPTIONS */

            // File Errors
            ProjectZipFileNameNotCorrect,

            // Version Errors
            ProjectNoVersion,
            ProjectOldVersion,
            ProjectNewVersion,

            // Node Count Errors
            ProjectInstrumentsNodeInvalidCount,
            ProjectSongNodeInvalidCount,
            ProjectPatternNodeInvalidCount,

            // Node Attribute Errors
            ProjectInstrumentNodeInvalidAttributes,
            ProjectNoteNodeInvalidAttributes,

            // Node Invalid Child Errors
            ProjectInstrumentsNodeInvalidChild,

            // Other
            ProjectInstrumentTypeNotExist,

            /* LICENSE EXCEPTIONS */
            LicenseNotAgreed
        }

        public ExceptionWindow(Exceptions type)
        {
            if (type == Exceptions.Empty) return;

            InitializeComponent();

            string errorType = "", errorMessage = "", errorId = "";

            switch (type)
            {
                case Exceptions.ProjectNoVersion:
                    errorType = "Invalid Project File";
                    errorMessage = "Project file comes from a time that doesn't exist";
                    errorId = "Error.Project.NoVer";
                    break;
                case Exceptions.ProjectOldVersion:
                    errorType = "Invalid Project File";
                    errorMessage = "Project file comes from long, long ago. We'll try to open it, but no promises.";
                    errorId = "Error.Project.OldVer";
                    break;
                case Exceptions.ProjectNewVersion:
                    errorType = "Invalid Project File";
                    errorMessage = "Project file comes from the future. We'll try to open it, but no promises.";
                    errorId = "Error.Project.NewVer";
                    break;
                case Exceptions.ProjectInstrumentsNodeInvalidCount:
                    errorType = "Invalid Project File";
                    errorMessage = "Project file doesn't have any instruments";
                    errorId = "Error.Project.Node.Instruments.InvalidCount";
                    break;
                case Exceptions.ProjectSongNodeInvalidCount:
                    errorType = "Invalid Project File";
                    errorMessage = "Project file forgot who it was";
                    errorId = "Error.Project.Node.Song.InvalidCount";
                    break;
                case Exceptions.ProjectPatternNodeInvalidCount:
                    errorType = "Invalid Project File";
                    errorMessage = "Project file forgot how to play itself";
                    errorId = "Error.Project.Node.Pattern.InvalidCount";
                    break;
                case Exceptions.ProjectInstrumentNodeInvalidAttributes:
                    errorType = "Invalid Project File";
                    errorMessage = "Project file's instruments forgot what they were called";
                    errorId = "Error.Project.Node.Instrument.InvalidAttributes";
                    break;
                case Exceptions.ProjectNoteNodeInvalidAttributes:
                    errorType = "Invalid Project File";
                    errorMessage = "Project file's notes forgot how to play themselves";
                    errorId = "Error.Project.Node.Note.InvalidAttributes";
                    break;
                case Exceptions.ProjectInstrumentsNodeInvalidChild:
                    errorType = "Invalid Project File";
                    errorMessage = "Project file's instruments must only be instruments";
                    errorId = "Error.Project.Node.Instruments.InvalidChild";
                    break;
                case Exceptions.ProjectInstrumentTypeNotExist:
                    errorType = "Invalid Project File";
                    errorMessage = "Project file looked for a non-existant instrument";
                    errorId = "Error.Project.Node.Instrument.TypeNotExist";
                    break;
                case Exceptions.LicenseNotAgreed:
                    errorType = "License";
                    errorMessage = "You must agree to the license to continue";
                    errorId = "Error.License.NotAgreed";
                    break;
                case Exceptions.Empty:
                    break;
                case Exceptions.ProjectZipFileNameNotCorrect:
                    errorType = "Invalid Project File";
                    errorMessage = "Project file contained alien specimens and was rejected";
                    errorId = "Error.Project.File.NameNotCorrect";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            // parse error ids
            var splitErrorId = errorId.Split('.');
            var newErrorId = new StringBuilder();
            foreach (var current in splitErrorId)
            {
                var message = (current.Length ^ (current[0].GetHashCode() / 10)).ToString();
                newErrorId.Append(message.Substring(0, 2));
            }
            newErrorId.Append(splitErrorId.Length);

            Content.Text = $"{errorType}:\n{errorMessage}\nError Code: {newErrorId}";
            Logger.Console(LogLevel.Severe, $"An error occured: {errorId}");
        }

        public static ExceptionWindow ThrowError(Exceptions type)
        {
            var window = new ExceptionWindow(type) {ShowInTaskbar = false};
            window.ShowDialog();
            return window;
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}