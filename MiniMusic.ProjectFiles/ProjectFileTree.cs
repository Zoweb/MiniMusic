using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace MiniMusic.ProjectFiles
{
    public class ProjectFileTree
    {
        public static ProjectFileTree Empty => new ProjectFileTree();

        private ProjectFileTree()
        {
            
        }

        public Dictionary<string, List<NoteData>> Notes;
        public SongInformation SongInfo;

        public ProjectFileTree(XDocument fileXmlDocument)
        {
            var schemaSet = new XmlSchemaSet();
            schemaSet.Add("", XmlReader.Create(Properties.Resources.ProjectFileSchema.AsStream()));

            string validationError = null;
            fileXmlDocument.Validate(schemaSet, (sender, args) =>
            {
                validationError = args.Message;
            });

            if (validationError != null) throw new XmlException(validationError);
        }

        public List<NoteData> AddInstrument(string instrumentName)
        {
            if (!Notes.ContainsKey(instrumentName))
            {
                Notes.Add(instrumentName, new List<NoteData>());
            }

            return Notes[instrumentName];
        }

        public void AddNote(NoteData note, string instrumentName)
        {
            var instrument = AddInstrument(instrumentName);
            instrument.Add(note);
        }

        public List<NoteData> GetInstrument(string instrumentName)
        {
            return Notes.ContainsKey(instrumentName) ? Notes[instrumentName] : null;
        }
    }
}