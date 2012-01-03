using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using System.Xml;
using System.Xml.Serialization;


namespace GuessWurdz
{
    public class LoadPhrases
    {
        [Serializable]
        public struct LoadPhrasesData
        {
            public struct ThePhraseData
            {
                public string Title;
                public string PhraseText;
                public string PhraseHint;
            }

            public ThePhraseData[] ThePhrase;
            public int Count;

            public LoadPhrasesData(int count)
            {

                ThePhrase = new ThePhraseData[count];
                Count = count;

            }
        }


        public static void SavePhrases(LoadPhrasesData data, string filename)
        {
            // Get the path of the save game
            string fullpath = Path.Combine(StorageContainer.TitleLocation, filename);

            // Open the file, creating it if necessary
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate);
            try
            {
                // Convert the object to XML data and put it in the stream
                XmlSerializer serializer = new XmlSerializer(typeof(LoadPhrasesData));
                serializer.Serialize(stream, data);
            }
            finally
            {
                // Close the file
                stream.Close();
            }
        }


        public static LoadPhrasesData LoadThePhrases(string filename)
        {
            LoadPhrasesData data;
            


            // Get the path of the save game
            string fullpath = Path.Combine(StorageContainer.TitleLocation, filename);
      
         


            // Open the file
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate,
            FileAccess.Read);
            try
            {

                // Read the data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(LoadPhrasesData));
                data = (LoadPhrasesData)serializer.Deserialize(stream);
            }
            finally
            {
                // Close the file
                stream.Close();
            }

            return (data);

        }

        public void SaveThePhrase()
        {
            // Create the data to save
            LoadPhrasesData data;
            data = new LoadPhrasesData();

            data.Count = 2;
            data.ThePhrase = new LoadPhrasesData.ThePhraseData[data.Count];
            data.ThePhrase[0].Title = "Test Phrase Title";
            data.ThePhrase[0].PhraseText = "In God We Trust";
            data.ThePhrase[0].PhraseHint = "Comment Found on Dollar Bill";

            data.ThePhrase[1].Title = "Movie";
            data.ThePhrase[1].PhraseText = "James Bond";
            data.ThePhrase[1].PhraseHint = "Action Movie";

            SavePhrases(data, "Data/LoadPhrases.xml");

        }

    }
}

