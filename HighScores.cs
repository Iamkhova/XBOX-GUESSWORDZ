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
    public class HighScores
    {
       

        [Serializable]
        public struct HighScoreData
        {
            public string[] PlayerName;
            public int[] Score;
            public int[] Level;

            public int Count;

            public HighScoreData(int count)
            {
                PlayerName = new string[count];
                Score = new int[count];
                Level = new int[count];
                Count = count;
            }

          
        }

        public static bool GameSaveRequest = false;
        public readonly string HighScoresFilename = "Data/HighScores.xml";
        public static bool StorageDeviceSelectorShowed = false;
        public static bool StorageOpen = false;

        public static void SaveHighScores()
        {

            if (GuessWurdz.StorageDevice != null &&
                GuessWurdz.StorageDevice.IsConnected)
            {
                SaveHighScoresCallback(null);
            }
            else
            {
                try
                {
                    if (!Guide.IsVisible)
                    {
                        Guide.BeginShowStorageDeviceSelector(
                            new AsyncCallback(SaveHighScoresCallback), null);
                    }
                }
                catch { }
            }
          
        }

        /// <summary>
        /// Callback method for saving the high scores to the drive.
        /// </summary>
        private static void SaveHighScoresCallback(IAsyncResult result)
        {
            if ((result != null) && result.IsCompleted)
            {
                GuessWurdz.StorageDevice = Guide.EndShowStorageDeviceSelector(result);
            }
            if ((GuessWurdz.StorageDevice != null) &&
                GuessWurdz.StorageDevice.IsConnected)
            {
                
                    using (StorageContainer storageContainer =
                        GuessWurdz.StorageDevice.OpenContainer("GuessWurdz"))
                        
                    {
                       
                        string highscoresPath = Path.Combine(storageContainer.Path,
                                                             "highscores.xml");
                        using (FileStream file = File.Create(highscoresPath))
                        {
                            XmlSerializer serializer = new XmlSerializer(typeof(HighScoreData));
                            serializer.Serialize(file, GuessWurdz.theHighScore);
                        }
                    }
                
            }
        }

        public static void LoadHighScores()
        {
          

            if ((GuessWurdz.StorageDevice != null) &&
               GuessWurdz.StorageDevice.IsConnected)
            {
                LoadHighScoresCallback(null);
            }
            else
            {
                GuessWurdz.setDefaultHighScore();

                try
                {
                    if (!Guide.IsVisible)
                    {
                        Guide.BeginShowStorageDeviceSelector(
                            new AsyncCallback(LoadHighScoresCallback), null);
                         StorageDeviceSelectorShowed = true; 
                    }
                }
                catch { }
                
            }

        }
            
        /// <summary>
        /// Callback method for loading the high scores from the drive.
        /// </summary>
        private static void LoadHighScoresCallback(IAsyncResult result)
        {
            if ((result != null) && result.IsCompleted)
            {
                GuessWurdz.StorageDevice = Guide.EndShowStorageDeviceSelector(result);
            }
            if ((GuessWurdz.StorageDevice != null) &&
                GuessWurdz.StorageDevice.IsConnected)
            {
                if (!StorageOpen)
                {
                    using (StorageContainer storageContainer =
                        GuessWurdz.StorageDevice.OpenContainer("GuessWurdz"))
                    {
                        StorageOpen = true;
                        string highscoresPath = Path.Combine(storageContainer.Path,
                                                             "highscores.xml");
                        if (File.Exists(highscoresPath))
                        {
                            using (FileStream file =
                                File.Open(highscoresPath, FileMode.Open))
                            {
                                XmlSerializer serializer =
                                    new XmlSerializer(typeof(HighScoreData));
                                GuessWurdz.theHighScore = (HighScoreData)serializer.Deserialize(file);
                            }
                        }
                        else
                        {
                            GuessWurdz.setDefaultHighScore();
                        }
                    }
                }
            }
        }

        public void SaveHighScore(int score, int currentLevel, string playername)
        {
            // Create the data to save
            LoadHighScores();

            int scoreIndex = -1;
            for (int i = 0; i <  GuessWurdz.theHighScore.Count; i++)
            {
                if (score > GuessWurdz.theHighScore.Score[i])
                {
                    scoreIndex = i;
                    break;
                }
            }

            if (scoreIndex > -1)
            {

                //New high score found ... do swaps
                for (int i = GuessWurdz.theHighScore.Count - 1; i > scoreIndex; i--)
                {
                    GuessWurdz.theHighScore.PlayerName[i] = GuessWurdz.theHighScore.PlayerName[i];
                    GuessWurdz.theHighScore.Score[i] = GuessWurdz.theHighScore.Score[i];
                    GuessWurdz.theHighScore.Level[i] = GuessWurdz.theHighScore.Level[i];
                }

                GuessWurdz.theHighScore.PlayerName[scoreIndex] = playername; //Retrieve User Name Here
                GuessWurdz.theHighScore.Score[scoreIndex] = score;
                GuessWurdz.theHighScore.Level[scoreIndex] = currentLevel + 1;

                SaveHighScores();
            }
                GuessWurdz.gameState = GuessWurdz.GameState.GameEnd;
           
        }

    }
}
