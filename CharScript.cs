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
    public class CharScript
    {
        [Serializable]
        public struct CharScriptData
        {
            public struct TheScript
            {
                public string Line1;
                public string Line2;
                public string SFX;
                public int SayingType;
            }

            public TheScript[] theScript;
            public int Count;

            public CharScriptData(int count)
            {

                theScript = new TheScript[count];
                Count = count;

            }
        }


        public static void SaveScripts(CharScriptData data, string filename)
        {
            // Get the path of the save game
            string fullpath = Path.Combine(StorageContainer.TitleLocation, filename);

            // Open the file, creating it if necessary
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate);
            try
            {
                // Convert the object to XML data and put it in the stream
                XmlSerializer serializer = new XmlSerializer(typeof(CharScriptData));
                serializer.Serialize(stream, data);
            }
            finally
            {
                // Close the file
                stream.Close();
            }
        }


        public static CharScriptData LoadScripts(string filename)
        {
            CharScriptData data;
            


            // Get the path of the save game
            string fullpath = Path.Combine(StorageContainer.TitleLocation, filename);
      
         


            // Open the file
            FileStream stream = File.Open(fullpath, FileMode.OpenOrCreate,
            FileAccess.Read);
            try
            {

                // Read the data from the file
                XmlSerializer serializer = new XmlSerializer(typeof(CharScriptData));
                data = (CharScriptData)serializer.Deserialize(stream);
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
            CharScriptData data;
            data = new CharScriptData();

            data.Count = 2;
            data.theScript = new CharScriptData.TheScript[data.Count];
            data.theScript[0].Line1 = "test1";
            data.theScript[0].Line2 = "test2";
            data.theScript[0].SFX = "vfx_test";
            data.theScript[0].SayingType = 1;

            data.theScript[1].Line1 = "test1a";
            data.theScript[1].Line2 = "";
            data.theScript[1].SFX = "vfx_testb";
            data.theScript[1].SayingType = 2;

            
            SaveScripts(data, "Data/ScriptData.xml");

           
        }

    }
}

