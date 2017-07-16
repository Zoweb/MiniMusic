using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using MME.Console;
using MME.Data;
using MME.Exceptions;
using MME.Sound;
using MME.Sound.Waves;

namespace MME.Project
{
    /*public static class ProjectFile
    {
        public static string CurrentFileLocation;
        public static readonly Version CurrentVersion = new Version(1, 1);

        private static ExceptionWindow _loadProject(string stream)
        {
            var saveAtEnd = false;

            var projectFile = XElement.Parse(stream);

            var fileVersionAttr = projectFile.Attribute("version");
            if (fileVersionAttr == null)
                //throw new XmlException("The file has no version");
                return ExceptionWindow.ThrowError(ExceptionWindow.Exceptions.ProjectNoVersion);
            var fileVersionStr = fileVersionAttr.Value;
            var fileVersion = Version.Parse(fileVersionStr);
            var compared = fileVersion.CompareTo(CurrentVersion);
            if (compared < 0)
            {
                saveAtEnd = true;
                ExceptionWindow.ThrowError(ExceptionWindow.Exceptions.ProjectOldVersion);
            }
            if (compared > 0)
            {
                saveAtEnd = true;
                ExceptionWindow.ThrowError(ExceptionWindow.Exceptions.ProjectNewVersion);
            }

            System.Console.WriteLine($"Project version is: {fileVersion}");

            if (projectFile.Descendants("instruments").Count() != 1)
                return ExceptionWindow.ThrowError(ExceptionWindow.Exceptions.ProjectInstrumentsNodeInvalidCount);
            //throw new XmlException("There must be one and only one `instruments` node");

            if (projectFile.Descendants("song").Count() != 1)
                return ExceptionWindow.ThrowError(ExceptionWindow.Exceptions.ProjectSongNodeInvalidCount);
            //throw new XmlException("There must be one and only one `song` node");

            var songData = projectFile.Descendants("song");
            foreach (var song in songData)
            {
                var bpmAttr = song.Attribute("bpm");

                if (bpmAttr == null) continue;

                SongData.Bpm = float.Parse(bpmAttr.Value);
                System.Console.WriteLine($"Song BPM is set to {bpmAttr.Value}");
            }


            SongData.RemoveAllInstruments();
            MainWindow.StartButton.IsEnabled = true;
            MainWindow.EndButton.IsEnabled = false;


            var instruments = projectFile.Descendants("instruments").Elements();

            foreach (var instrument in instruments)
            {
                if (instrument.Name.LocalName != "instrument")
                    //throw new XmlException("Node `instruments` must only contain the node `instrument`");
                    return ExceptionWindow.ThrowError(ExceptionWindow.Exceptions.ProjectInstrumentsNodeInvalidChild);

                var instrumentTypeAttr = instrument.Attribute("type");
                var instrumentNameAttr = instrument.Attribute("name");
                if (instrumentTypeAttr == null || instrumentNameAttr == null)
                    //throw new XmlException("Node `instrument` must have attributes `type` and `name`");
                    return ExceptionWindow.ThrowError(ExceptionWindow.Exceptions
                        .ProjectInstrumentNodeInvalidAttributes);

                var instrumentType = instrumentTypeAttr.Value;
                var instrumentName = instrumentNameAttr.Value;

                WaveFunction instrumentFunction;

                switch (instrumentType)
                {
                    case "SineWave":
                        instrumentFunction = new SineWave();
                        break;
                    case "SquareWave":
                        instrumentFunction = new SquareWave();
                        break;
                    case "TriangleWave":
                        instrumentFunction = new TriangleWave();
                        break;
                    default:
                        //throw new XmlException($"Instrument {instrumentType} does not exist");
                        return ExceptionWindow.ThrowError(ExceptionWindow.Exceptions.ProjectInstrumentTypeNotExist);
                }

                SongData.AddInstrument(instrumentName, instrumentFunction);
                System.Console.WriteLine($"Loaded instrument {instrumentType}");

                if (instrument.Descendants("pattern").Count() != 1)
                    //throw new XmlException("There must be one and only one `pattern` node");
                    return ExceptionWindow.ThrowError(ExceptionWindow.Exceptions.ProjectPatternNodeInvalidCount);

                var notes = instrument.Descendants("pattern").Elements();
                foreach (var note in notes)
                {
                    var volumeAttr = note.Attribute("volume");
                    var indexAttr = note.Attribute("index");
                    var startAttr = note.Attribute("start");
                    var lengthAttr = note.Attribute("length");

                    if (volumeAttr == null || indexAttr == null || startAttr == null || lengthAttr == null)
                        //throw new XmlException(
                        //    "Node `note` must have attributes `index`, `volume`, `start` and `length`");
                        return ExceptionWindow.ThrowError(ExceptionWindow.Exceptions.ProjectNoteNodeInvalidAttributes);

                    var volume = volumeAttr.Value;
                    var index = indexAttr.Value;
                    var start = startAttr.Value;
                    var length = lengthAttr.Value;

                    SongData.AddNote(instrumentName, int.Parse(index), float.Parse(volume), float.Parse(start),
                        float.Parse(length));

                    System.Console.WriteLine($"Added note {index} to {instrumentName}");
                }
            }

            Logger.Info("Finished loading project file");

            System.Console.WriteLine("Finished parsing project file");

            if (saveAtEnd)
            {
                MessageBox.Show("Please save the file with another name so that you don't loose any data.");
                MainWindow.MainWindowAsStatic.MenuFileSaveAs_OnClick(null, null);
            }

            return new ExceptionWindow(ExceptionWindow.Exceptions.Empty);
        }

        public static void LoadProject(string location)
        {
            Logger.Debug($"Loading project file from {location}");


            ZipFile zf = null;
            try
            {
                var fs = File.OpenRead(location);
                zf = new ZipFile(fs);
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                        continue;

                    var fileName = zipEntry.Name;
                    if (fileName != ".minimusic")
                        //throw new ZipException("Invalid project file");
                        ExceptionWindow.ThrowError(ExceptionWindow.Exceptions.ProjectZipFileNameNotCorrect);

                    var zipStream = zf.GetInputStream(zipEntry);

                    var reader = new StreamReader(zipStream);
                    var xmlFile = reader.ReadToEnd();

                    CurrentFileLocation = location;
                    _loadProject(xmlFile);
                    MainWindow.ChangeTitle(location + " - MiniMusic Editor");

                    break;
                }
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true;
                    zf.Close();
                }
            }
        }

        public static void SaveProject(string location)
        {
            var xmlFile = new XDocument(new XDeclaration("1.0", "utf-8", "false"),
                new XComment(" MiniMusic (c) zoweb 2017 "), new XElement("project"));

            var root = xmlFile.Root;

            if (root == null) return;

            Logger.Debug($"Saving project file to {location}");

            root.SetAttributeValue("version", CurrentVersion.ToString());

            var songData = new XElement("song");
            songData.SetAttributeValue("bpm", 140);
            root.Add(songData);

            var instruments = new XElement("instruments");

            foreach (var instrument in SongData.GetInstruments())
            {
                var instrumentName = instrument.Key;
                var instrumentType = instrument.Value.Type;

                var instrumentElem = new XElement("instrument");
                instrumentElem.SetAttributeValue("type", instrumentType);
                instrumentElem.SetAttributeValue("name", instrumentName);

                var patternElem = new XElement("pattern");

                foreach (var note in SongData.GetInstrumentNotes(instrumentName))
                {
                    var noteElem = new XElement("note");
                    noteElem.SetAttributeValue("index", note.Index);
                    noteElem.SetAttributeValue("volume", note.Volume);
                    noteElem.SetAttributeValue("start", note.StartBeat);
                    noteElem.SetAttributeValue("length", note.LengthBeats);
                    patternElem.Add(noteElem);
                }

                instrumentElem.Add(patternElem);
                instruments.Add(instrumentElem);
            }

            root.Add(instruments);

            var fsOut = File.Create(location);
            var zipStream = new ZipOutputStream(fsOut);
            zipStream.SetLevel(9);

            var newEntry = new ZipEntry(".minimusic");

            zipStream.PutNextEntry(newEntry);

            var buffer = new byte[4096];
            using (var writer = new StringWriter(new StringBuilder()))
            {
                xmlFile.Save(writer);
                using (var streamReader = _genStreamFromString(writer.ToString()))
                {
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                }
            }
            zipStream.CloseEntry();

            zipStream.IsStreamOwner = true;
            zipStream.Close();
        }

        private static Stream _genStreamFromString(string s)
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);

            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }*/
}