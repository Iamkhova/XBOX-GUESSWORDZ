using System;
using System.Collections.Generic;
using System.Threading;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;

namespace GuessWurdz
{
    public class GuessWurdzGame : SpriteHandling
    {
        /// <summary>
        /// Define Variables
        /// </summary>
        PhraseData thePhrase;
        CharScript.CharScriptData scriptGuessRight;
        CharScript.CharScriptData scriptGuessWrong;
        HighScores highScore = new HighScores();
        CharScript charScript = new CharScript();
        LetterBlock[] letterBlock = new LetterBlock[26];
        LetterBlock[] boardPiece = new LetterBlock[56];
        GameBoard[] gameBoard = new GameBoard[56];
        Scoring theScore;
        public int gameType;
        LetterPosition currentLetterPosition;
        LetterPosition nextLetterPosition;
        Letter theLetter = new Letter();
        //bool newLevelStart = false;
        public Vector2 bombJitter;
        string gamePhrase;
        int lastNum = 0; // Last random Number Used
        
      
        //public static SoundEntry backgroundMusic;
        public static bool isMusicPlaying;


        // SpriteHandling spriteHandling = new SpriteHandling();

        /// <summary>
        /// Define Structures
        /// </summary>
        public struct PhraseData
        {
            public string title;
            public string line1;
            public string line2;
            public string line3;
            public string line4;
            public string hint;

        }

        public struct Scoring
        {
            public const int ADDSCORE = 1;
            public const int GUESSRIGHT = 2;
            public const int GUESSWRONG = 3;
            public int current_points;
            public int current_level;
            public int number_right;
            public int number_wrong;
            public int streak_number_right;
            public int streak_number_wrong;
            public int bombs_diffused;
            public int golden_balls_used;
            public int num_perfect_rounds;
            public bool scoringState;
            public int lives_left;
            public int gold_balls;
            public int number_wrong_level;
            public int last_perfect_round;
            public int last_reward_level; // give gold ball ever 5000 points
            public int next_reward_level;

        }

        public struct LetterPosition
        {
            public Vector2 position;
            public int letterIs;
        }

        public struct Coord2d
        {
            public int X;
            public int Y;
        }


        public struct LetterBlock
        {
            public Rectangle sprite;
            public Coord2d position;
            public bool active;
            public bool alreadyChosen;

        }

        public struct GameBoard
        {
            public struct Bomb
            {
                public int bombType;
                public int turnsToExplode;

            }

            public char letterValue;
            public bool letterVisible;
            public bool blockVisible;
            public Bomb bomb;
        }

        public class Letter
        {
            public char[] value; // Holds the Value of the Letter
            public int[] position; // Defines the Position of the Letter
            public int[] visible; // Determies if the Letter is Visible
        }


        public void initGame()
        {
            int randomNumber = 0;
            bool loadBomb = false;
            LoadPhrases.LoadPhrasesData allPhrases;
            char[] letterArray;
            string filePath;
            bool findphrase = false;
            Functions function = new Functions();

            filePath = newBackground();

            GameplayScreen.gfxGameBackground = GuessWurdz.content.Load<Texture2D>(filePath);

            // Load the Phrases;
            allPhrases = LoadPhrases.LoadThePhrases("Data/LoadPhrases.xml");

            // Configure Game Options if new level
            if (GuessWurdz.newLevelStart == false)
            {
                if (GuessWurdz.gameType == GuessWurdz.GAMETYPE_NORMAL)
                {
                    theScore.scoringState = true;
                    theScore.current_points = 0;
                    theScore.lives_left = 10;
                    theScore.current_level = 1;
                    theScore.gold_balls = 0;
                    theScore.last_reward_level = 0;
                    theScore.next_reward_level = 5000;
                    theScore.number_right = 0;
                    theScore.number_wrong = 0;
                    theScore.streak_number_right = 0;
                    theScore.streak_number_wrong = 0;
                    theScore.num_perfect_rounds = 0;
                    theScore.bombs_diffused = 0;
                    theScore.golden_balls_used = 0;
                    theScore.number_wrong_level = 0;
                    theScore.last_perfect_round = 0;

                }
                loadScripts(); // Load Talking Scripts
            }

            // Pick Random Phrase and ensure no duplicates
            if (!phraseAvail())
            {

                for (int i = 0; i < 2000; i++)
                {
                    GuessWurdz.gamePhrase[i] = false;
                }
            }

            // random number activate bomb
            {
                randomNumber = function.RandomNumber(1, 4);
                if (randomNumber <=2 )
                {
                    loadBomb = true;
                }
            }

            while (findphrase == false)
            {
                randomNumber = function.RandomNumber(1, (allPhrases.Count - 1));
                if (GuessWurdz.gamePhrase[randomNumber] == false)
                {
                    findphrase = true;
                    GuessWurdz.gamePhrase[randomNumber] = true;
                }
            }
            // Converting Phrase into Game format.
            thePhrase = convertPhrase(allPhrases.ThePhrase[randomNumber].PhraseText,
                allPhrases.ThePhrase[randomNumber].PhraseHint,
                allPhrases.ThePhrase[randomNumber].Title);

            // Center the Phrase
            thePhrase.line1 = CenterPhrase(thePhrase.line1);
            thePhrase.line2 = CenterPhrase(thePhrase.line2);
            thePhrase.line3 = CenterPhrase(thePhrase.line3);
            thePhrase.line4 = CenterPhrase(thePhrase.line4);

            // Compile Formated Phrase
            gamePhrase = thePhrase.line1 + thePhrase.line2 + thePhrase.line3 +
                thePhrase.line4;

            //Load Formated Phrase
            letterArray = gamePhrase.ToCharArray();

            // Initalize Vars
            theLetter.value = new char[letterArray.Length];
            theLetter.position = new int[letterArray.Length];
            theLetter.visible = new int[letterArray.Length];

            // Build The Board

            for (int i = 0; i < 55; i++)
            {
                gameBoard[i].blockVisible = false;
                gameBoard[i].letterVisible = false;
                gameBoard[i].bomb.bombType = 0;
                gameBoard[i].bomb.turnsToExplode = 0;
            }

            //Plant Random Bomn.. Bad Code.. pickes # 1-55 and only displays if block
            //has active letter - need to improve
            do
            {
                randomNumber = function.RandomNumber(1, gamePhrase.Length);
            }
            while (letterArray[randomNumber] == ' ');


            // Load Phrase into Board
            for (int i = 0; i < gamePhrase.Length; i++)
            {
                gameBoard[i].letterValue = letterArray[i];
                gameBoard[i].blockVisible = true;
                if (gameBoard[i].letterValue == '\'') { gameBoard[i].letterVisible = true; }
                if (gameBoard[i].letterValue == '/') { gameBoard[i].letterVisible = true; }
                if (gameBoard[i].letterValue == '-') { gameBoard[i].letterVisible = true; }
                if (gameBoard[i].letterValue == ',') { gameBoard[i].letterVisible = true; }

            }

            if (loadBomb)
            {
                setBomb(randomNumber);
            }


            //Initalize Letter Chosen .. make all blocks active
            for (int z = 0; z < 26; z++)
            {
                letterBlock[z].alreadyChosen = false;
                letterBlock[z].active = false;
            }
            currentLetterPosition.position.X = 0;
            currentLetterPosition.position.Y = 0;
            currentLetterPosition.letterIs = 0;
            nextLetterPosition.position.X = 0;
            nextLetterPosition.position.Y = 0;
            nextLetterPosition.letterIs = 0;
            letterBlock[0].active = true;


            GuessWurdz.oskb.Initialize();
            // GuessWurdz.gameState = GuessWurdz.GameState.DrawGameMenu;


        }

        private void setBomb(int i)
        {
            int randnum;
            Functions function = new Functions();

            if (theScore.current_level >= 5 && theScore.current_level <= 9)
            {
                gameBoard[i].bomb.turnsToExplode = 5;
                gameBoard[i].bomb.bombType = 1; // <-- Fix Bomb Types
                Sound.Play(SoundEntry.SFX_BombAlert);
            }

            if (theScore.current_level >= 10 && theScore.current_level <= 14)
            {
                randnum = function.RandomNumber(4, 5);
                gameBoard[i].bomb.turnsToExplode = randnum;
                gameBoard[i].bomb.bombType = 1; // <-- Fix Bomb Types
                Sound.Play(SoundEntry.SFX_BombAlert);
            }

            if (theScore.current_level >= 15 && theScore.current_level <= 50)
            {
                randnum = function.RandomNumber(3, 5);
                gameBoard[i].bomb.turnsToExplode = randnum;
                gameBoard[i].bomb.bombType = 1; // <-- Fix Bomb Types
                Sound.Play(SoundEntry.SFX_BombAlert);
            }
            if (theScore.current_level > 50)
            {
                randnum = function.RandomNumber(2, 5);
                gameBoard[i].bomb.turnsToExplode = randnum;
                gameBoard[i].bomb.bombType = 1; // <-- Fix Bomb Types
                Sound.Play(SoundEntry.SFX_BombAlert);
            }

            

        }

        public void DisplayLetterBoard(int x, int y)
        {

            int width = 800;
            int height = 400;
            SpriteBatch spriteBatch = GuessWurdz.screenManager.SpriteBatch;
            spriteBatch.Begin();


            spriteBatch.Draw(GameplayScreen.gfxLetterBoard,
                new Rectangle(x, y, width, height), new Color(255, 255, 255));
            spriteBatch.End();
            DrawLetterBoard(x + 200, y + 75);
            DrawSprites();


        }

        public void DisplayTitle(int x, int y)
        {
            int width = 851;
            int height = 54;
            Vector2 pixel_length;
            int start_pixel;
            SpriteBatch spriteBatch = GuessWurdz.screenManager.SpriteBatch;

            pixel_length = GameplayScreen.snap_itc.MeasureString(thePhrase.title.ToString());
            start_pixel = (width / 2) - ((int)pixel_length.X / 2);
            spriteBatch.Begin();


            spriteBatch.Draw(GameplayScreen.gfxTitleBoard,
                new Rectangle(x, y, width, height), new Color(255, 255, 255));
            spriteBatch.DrawString(GameplayScreen.snap_itc, thePhrase.title.ToString(), new Vector2(x + start_pixel, y), Color.Black);
            spriteBatch.End();

        }

        private void DrawLetterBoard(int x, int y)
        {
            int xpos = 0;
            int ypos = 0;
            int i = 0;

            for (int row = 0; row < 6; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    if (i < 26)
                    {
                        letterBlock[i].sprite = new Rectangle(xpos, ypos,
                            GuessWurdz.LETTER_BLOCK_SIZEX, GuessWurdz.LETTER_BLOCK_SIZEY);

                        xpos += GuessWurdz.LETTER_BLOCK_SIZEX;
                        i++;
                    }
                }
                xpos = 0;
                ypos += GuessWurdz.LETTER_BLOCK_SIZEY;

            }

            // Draw Alphabit
            i = 0;
            xpos = x;
            ypos = y;

            for (int row = 1; row <= 4; row++)
            {
                for (int col = 1; col <= 8; col++)
                {
                    if (i < 26)
                    {
                        letterBlock[i].position.X = xpos;
                        letterBlock[i].position.Y = ypos;

                        xpos += GuessWurdz.LETTER_BLOCK_SIZEX;
                        i++;
                    }
                }
                xpos = x;
                ypos += GuessWurdz.LETTER_BLOCK_SIZEY;
            }

            if (GuessWurdz.gameState == GuessWurdz.GameState.DrawGameMenu)
            {
                if (GuessWurdz.startOfGame == true)
                {
                    letterBlock[0].active = true;


                }
                GuessWurdz.gameState = GuessWurdz.GameState.GameStarted;

            }

            for (int z = 0; z < 26; z++)
            {

                if (letterBlock[z].active == true)
                {
                    RenderSprite(GameplayScreen.gfxLetterTileActive,
                        letterBlock[z].position.X, letterBlock[z].position.Y,
                        letterBlock[z].sprite);

                }
                else if (letterBlock[z].alreadyChosen == true)
                {
                    RenderSprite(GameplayScreen.gfxLetterTileInactive,
                                            letterBlock[z].position.X, letterBlock[z].position.Y,
                                            letterBlock[z].sprite);

                }
                else
                {

                    RenderSprite(GameplayScreen.gfxLetterTileNormal,
                        letterBlock[z].position.X, letterBlock[z].position.Y,
                        letterBlock[z].sprite);
                }

            }




        }

        public void DisplayGoldenBalls(int xpos, int ypos)
        {
            SpriteBatch spriteBatch = GuessWurdz.screenManager.SpriteBatch;
            if (theScore.gold_balls > 0)
            {
                spriteBatch.Begin();
                for (int i = 0; i < theScore.gold_balls; i++)
                {

                    spriteBatch.Draw(GameplayScreen.gfxGoldenBall, new Vector2(xpos + (i * 30), ypos), new Color(255, 255, 255));
                }
                spriteBatch.Draw(GameplayScreen.gfxGoldenBall, new Vector2(xpos, ypos), new Color(255, 255, 255));
                spriteBatch.End();
            }


        }

        public void DrawHighScoreScreen()
        {
            SpriteBatch spriteBatch = GuessWurdz.screenManager.SpriteBatch;
            int count = 0;
            int position;

            spriteBatch.Begin();

            spriteBatch.Draw(GameplayScreen.gfx_newhighscore_text,
                new Rectangle(445, 150, 711, 68), new Color(255, 255, 255));
            spriteBatch.Draw(GameplayScreen.gfx_trophy,
                new Rectangle(65, 100, 353, 521), new Color(255, 255, 255));

            // Draw Scores
            for (int i = 0; i <= (GuessWurdz.theHighScore.Count - 1); i++)
            {
                position = (count * 70);
                if ( theScore.current_points == GuessWurdz.theHighScore.Score[i])
                {
                    spriteBatch.DrawString(GameplayScreen.highScoreFont, GuessWurdz.theHighScore.PlayerName[i].ToString(), new Vector2(455, 250 + position), Color.Red);
                    spriteBatch.DrawString(GameplayScreen.highScoreFont, GuessWurdz.theHighScore.Score[i].ToString(), new Vector2(980, 250 + position), Color.Red);
                }
                else
                {
                    spriteBatch.DrawString(GameplayScreen.highScoreFont, GuessWurdz.theHighScore.PlayerName[i].ToString(), new Vector2(455, 250 + position), Color.Black);
                    spriteBatch.DrawString(GameplayScreen.highScoreFont, GuessWurdz.theHighScore.Score[i].ToString(), new Vector2(980, 250 + position), Color.Black);

                }
                count++;
            }

            spriteBatch.End();
            displayAbutton(40, 650);



        }

        public void DisplayScoreBoard(int xposition, int yposition)
        {
            SpriteBatch spriteBatch = GuessWurdz.screenManager.SpriteBatch;
            int width = 398;
            int height = 263;


            spriteBatch.Begin();


            spriteBatch.Draw(GameplayScreen.gfxScoreBoard,
                 new Rectangle(xposition, yposition, width, height), new Color(255, 255, 255));
            if (!Guide.IsTrialMode)
            {
                spriteBatch.Draw(GameplayScreen.gfxScoreText, new Vector2(xposition + 50, yposition + 20), new Color(255, 255, 255));
                spriteBatch.Draw(GameplayScreen.gfxRoundText, new Vector2(xposition + 50, yposition + 150), new Color(255, 255, 255));
                spriteBatch.DrawString(GameplayScreen.scoreFont, theScore.current_points.ToString(), new Vector2(xposition + 205, yposition + 8), new Color(202, 14, 14));
                spriteBatch.DrawString(GameplayScreen.scoreFont, theScore.current_level.ToString(), new Vector2(xposition + 210, yposition + 138), new Color(202, 14, 14));

            }
            spriteBatch.Draw(GameplayScreen.gfxChancesText, new Vector2(xposition + 50, yposition + 80), new Color(255, 255, 255));
            spriteBatch.DrawString(GameplayScreen.scoreFont, theScore.lives_left.ToString(), new Vector2(xposition + 270, yposition + 68), new Color(202, 14, 14));

            if (Guide.IsTrialMode)
            {
                spriteBatch.Draw(GameplayScreen.gfxTrialModeText, new Vector2(xposition + 85, yposition + 150), new Color(255, 255, 255));

            }
            spriteBatch.End();



        }

        public void RevealBoard()
        {
            for (int i = 0; i < 56; i++)
            {
                if (gameBoard[i].blockVisible == true && gameBoard[i].letterValue != ' ')
                {
                    gameBoard[i].letterVisible = true;
                }
            }
           // GuessWurdz.gameState = GuessWurdz.GameState.GameLost;
        }

        public void CheckLife()
        {
            if (theScore.lives_left <= 0)
            {
                if (GuessWurdz.gameState != GuessWurdz.GameState.GameLost)
                {
                    GuessWurdz.gameState = GuessWurdz.GameState.GameLost;
                    RevealBoard();
                }
            }

        }

        public bool reloadBGMusic()
        {
            int x;
            bool check = false;

            x = theScore.current_level % 3;
            if (x == 0) {check = true;}

            return check;

        }


        public void DisplayGameBoard(int xpos, int ypos)
        {

            int count = 0;

            int i = 0;
            int shakefactor = 0;

            for (int row = 0; row < 14; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (gameBoard[count].blockVisible == true)
                    {
                        if (gameBoard[count].letterVisible == true)
                        {
                            // Display Board Piece with Letter
                            boardPiece[count].sprite = new Rectangle(findLetterGFX('x', gameBoard[count].letterValue, 60, 54), findLetterGFX('y', gameBoard[count].letterValue, 60, 54), 60, 54);

                        }
                        else if (gameBoard[count].bomb.turnsToExplode > 0)
                        {
                            //Display Block 5 Bomb
                            int varx = (5 - gameBoard[count].bomb.turnsToExplode) * 54;
                            boardPiece[count].sprite = new Rectangle(varx, 0, 54, 52);

                        }
                        else
                        {
                            // Display Board Piece No Letter
                            boardPiece[count].sprite = new Rectangle(60, 0, 60, 54);
                        }



                    }
                    else
                    {

                        // Display Empty Piece
                        boardPiece[count].sprite = new Rectangle(0, 0, 60, 54);

                    }

                    if (gameBoard[count].letterValue == ' ')
                    {
                        boardPiece[count].sprite = new Rectangle(0, 0, 60, 54);
                    }
                    count++;

                }

            }

            // Setup Board
            // Draw Alphabit
            i = 0;
            //xpos = 100;
            //ypos = 450;
            for (int row = 1; row <= 4; row++)
            {
                for (int col = 1; col <= 14; col++)
                {
                    if (i < 56)
                    {
                        // RenderSprite(letterBlocksGFX, xpos, ypos, letterRect[i]);
                        boardPiece[i].position.X = xpos;
                        boardPiece[i].position.Y = ypos;

                        xpos += 60;
                        i++;

                    }
                }
                xpos = 100;
                ypos += 54;
            }


            // Draw Board

            for (int x = 0; x < 56; x++)
            {
                if (gameBoard[x].blockVisible == true)
                {
                    if (gameBoard[x].letterVisible == true)
                    {

                        RenderSprite(GameplayScreen.gfxGameLetter, boardPiece[x].position.X, boardPiece[x].position.Y, boardPiece[x].sprite);

                    }
                    else
                    {
                        if (gameBoard[x].bomb.turnsToExplode > 0)
                        {
                            if (gameBoard[x].bomb.turnsToExplode == 3) { shakefactor = 1; }
                            if (gameBoard[x].bomb.turnsToExplode == 2) { shakefactor = 5; }
                            if (gameBoard[x].bomb.turnsToExplode == 1) { shakefactor = 15; }
                            bombJitter.X = ((bombJitter.X * shakefactor) + boardPiece[x].position.X);
                            bombJitter.Y = ((bombJitter.Y * shakefactor) + boardPiece[x].position.Y);
                            RenderSprite(GameplayScreen.gfxBombTiles, (int)bombJitter.X, (int)bombJitter.Y, boardPiece[x].sprite);
                            RenderSprite(GameplayScreen.gfxBombTiles, boardPiece[x].position.X, boardPiece[x].position.Y, boardPiece[x].sprite);
                        }
                        else
                        {

                            RenderSprite(GameplayScreen.gfxGameTile, boardPiece[x].position.X, boardPiece[x].position.Y, boardPiece[x].sprite);
                        }
                    }
                }
                else
                {
                    RenderSprite(GameplayScreen.gfxGameTile, boardPiece[x].position.X, boardPiece[x].position.Y, boardPiece[x].sprite);
                }

            }
            DrawSprites();



        }

        public void handleHighScore()
        {
            string playerName;
            playerName = GuessWurdz.oskb.Text;
        }

        /// <summary>
        /// Centers the Phrase for the Board
        /// </summary>
        /// <param name="phraseLine"></param>
        /// <returns></returns>
        private string CenterPhrase(string phraseLine)
        {
            int center, value;
            string phrase = "";
            int test;

            value = (GuessWurdz.BOARD_ROW_LENGTH - phraseLine.Length);
            phrase = phrase.PadRight(value);
            center = (GuessWurdz.BOARD_ROW_LENGTH - phraseLine.Length) / 2;
            phrase = phrase.Insert(center, phraseLine);
            test = phrase.Length;

            return phrase;


        }

        /// <summary>
        /// Find the Letter of the Graphics Space
        /// </summary>
        /// <param name="axis"></param>
        /// <param name="theletter"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private int findLetterGFX(char axis, char theletter, int width, int height)
        {
            int xpos, ypos, value;
            Converter convert = new Converter();


            theletter = convert.tolowerchar(theletter);


            switch (theletter)
            {
                case 'a':
                    xpos = 0; ypos = 0;
                    break;
                case 'b':
                    xpos = width; ypos = 0;
                    break;
                case 'c':
                    xpos = width * 2; ypos = 0;
                    break;
                case 'd':
                    xpos = width * 3; ypos = 0;
                    break;
                case 'e':
                    xpos = width * 4; ypos = 0;
                    break;
                case 'f':
                    xpos = 0; ypos = height;
                    break;
                case 'g':
                    xpos = width; ypos = height;
                    break;
                case 'h':
                    xpos = width * 2; ypos = height;
                    break;
                case 'i':
                    xpos = width * 3; ypos = height;
                    break;
                case 'j':
                    xpos = width * 4; ypos = height;
                    break;
                case 'k':
                    xpos = 0; ypos = height * 2;
                    break;
                case 'l':
                    xpos = width; ypos = height * 2;
                    break;
                case 'm':
                    xpos = width * 2; ypos = height * 2;
                    break;
                case 'n':
                    xpos = width * 3; ypos = height * 2;
                    break;
                case 'o':
                    xpos = width * 4; ypos = height * 2;
                    break;
                case 'p':
                    xpos = 0; ypos = height * 3;
                    break;
                case 'q':
                    xpos = width; ypos = height * 3;
                    break;
                case 'r':
                    xpos = width * 2; ypos = height * 3;
                    break;
                case 's':
                    xpos = width * 3; ypos = height * 3;
                    break;
                case 't':
                    xpos = width * 4; ypos = height * 3;
                    break;
                case 'u':
                    xpos = 0; ypos = height * 4;
                    break;
                case 'v':
                    xpos = width; ypos = height * 4;
                    break;
                case 'w':
                    xpos = width * 2; ypos = height * 4;
                    break;
                case 'x':
                    xpos = width * 3; ypos = height * 4;
                    break;
                case 'y':
                    xpos = width * 4; ypos = height * 4;
                    break;
                case 'z':
                    xpos = 0; ypos = height * 5;
                    break;
                case '-':
                    xpos = width * 1; ypos = height * 5;
                    break;
                case '/':
                    xpos = width * 2; ypos = height * 5;
                    break;
                case '\'':
                    xpos = width * 3; ypos = height * 5;
                    break;
                case ',':
                    xpos = width * 4; ypos = height * 5;
                    break;



                default:
                    xpos = 400; ypos = 400;

                    break;

            }
            if (axis == 'x')
            {
                value = xpos;
            }
            else
            {
                value = ypos;
            }
            return value;


        }
        /// <summary>
        /// Move Cursor Down
        /// </summary>
        public void moveActiveDown()
        {
            nextLetterPosition.position.Y += 1;
        }

        /// <summary>
        /// Move Cursor Down
        /// </summary>
        public void moveActiveUp()
        {
            nextLetterPosition.position.Y -= 1;
        }

        public void moveActiveRight()
        {
            nextLetterPosition.position.X += 1;
        }

        public void moveActiveLeft()
        {
            nextLetterPosition.position.X -= 1;
        }

        public void setLetterActivePosition()
        {
            int nextLetterValue, currentLetterValue;

            if (nextLetterPosition.position.Y < 4 && nextLetterPosition.position.Y >= 0
                && nextLetterPosition.position.X < 8 && nextLetterPosition.position.X >= 0)
            {
                nextLetterValue = (int)convertToLetter((int)nextLetterPosition.position.X,
                    (int)nextLetterPosition.position.Y);

                if ((int)convertToLetter((int)nextLetterPosition.position.X, (int)nextLetterPosition.position.Y) > 25)
                {
                    // out of bounds.. no more letters
                    nextLetterPosition = currentLetterPosition;
                }
                else
                {

                    currentLetterValue = (int)convertToLetter((int)currentLetterPosition.position.X, (int)currentLetterPosition.position.Y);

                    letterBlock[currentLetterValue].active = false;
                    letterBlock[nextLetterValue].active = true;
                    currentLetterPosition = nextLetterPosition;
                }

            }
            else
            {
                nextLetterPosition = currentLetterPosition;
            }


        }


        public void chooseLetter(int keyboard_letter)
        {
            Converter convert = new Converter();
            bool findLetter = false;
            int letterChosen;

            if (GuessWurdz.gameState == GuessWurdz.GameState.GameStarted)
            {
                if (keyboard_letter == 99)
                {
                    letterChosen = (int)convertToLetter((int)currentLetterPosition.position.X, (int)currentLetterPosition.position.Y);
                }
                else
                {
                    letterChosen = keyboard_letter;
                }
                if (letterBlock[letterChosen].alreadyChosen == false)
                {
                    for (int i = 0; i < 56; i++)
                    {
                        if (convert.tolowerchar(gameBoard[i].letterValue) == convert.intToLetter(letterChosen))
                        {
                            if (gameBoard[i].letterVisible != true)
                            {
                                //Right Letter! add points !! good job
                                Sound.Play(SoundEntry.SFX_CorrectLetter);
                                if (!Guide.IsTrialMode)
                                {
                                    score(Scoring.GUESSRIGHT);
                                    score(Scoring.ADDSCORE);
                                    checkReward();
                                }
                                display_thinkblock(0, 0, 2, 1);
                                if (gameBoard[i].bomb.bombType == 1)
                                {
                                    // Bomb Defused
                                    theScore.bombs_diffused += 1;
                                }



                            }
                            gameBoard[i].letterVisible = true;

                            findLetter = true;

                        }

                    }
                    if (findLetter == false)
                    {
                        display_thinkblock(0, 0, 2, 2);
                        Sound.Play(SoundEntry.SFX_WrongLetter);
                        score(Scoring.GUESSWRONG);
                    }
                    CheckBomb();
                    letterBlock[letterChosen].alreadyChosen = true;
                }

            }
        }

        public void wonLevel()
        {

            if (Guide.IsTrialMode && theScore.current_level >= 3)
            {
                GuessWurdz.gameState = GuessWurdz.GameState.GameLost;
            }
            else
            {
                GuessWurdz.gameState = GuessWurdz.GameState.GameWon;
            }
            if (theScore.number_wrong_level == 0)
            {
                checkReward();
            }
        }
         
        public void newLevel()
        {
            GuessWurdz.newLevelStart = true;
            GuessWurdz.startOfGame = false;
            GameplayScreen.show_thinkblock = false;
            GuessWurdz.gameState = GuessWurdz.GameState.GameStarted;
            theScore.current_level += 1;
            theScore.number_wrong_level = 0;
            

            initGame();
            //Initalize Letter Chosen .. make all blocks active
            for (int z = 0; z < 26; z++)
            {
                letterBlock[z].alreadyChosen = false;
            }

        }

        public void gameLost()
        {

            //GuessWurdz.gscore = theScore.current_points;
            //GuessWurdz.glevel = theScore.current_level;
            if (GameplayScreen.highScoreFound)
            {
                GuessWurdz.gameState = GuessWurdz.GameState.HandelHighScore;

            }
            else
            {
                GuessWurdz.gameState = GuessWurdz.GameState.GameEnd;
                GuessWurdz.newLevelStart = false;
                GuessWurdz.screenManager.AddScreen(new EndGameMenuScreen());
                //LoadingScreen.Load(GuessWurdz.screenManager, false, new BackgroundScreen());
            }

           
           
        }

        public void displayGameOverScreen(int x, int y)
        {
            SpriteBatch spriteBatch = GuessWurdz.screenManager.SpriteBatch;

            spriteBatch.Begin();
            spriteBatch.Draw(GameplayScreen.gfxGameLost, new Vector2(x, y), new Color(255, 255, 255));
            //  spriteBatch.DrawString(GameplayScreen.gameFont, "GOOD JOB!", new Vector2(200, 200), Color.Blue);
            spriteBatch.End();

        }

        private void showKeyboard()
        {
            GuessWurdz.oskb.Visible = true;
            GuessWurdz.oskb.Enabled = true;
            GuessWurdz.oskb.Prompt = "Enter Name for High Score";

            // GuessWurdz.gameState = GuessWurdz.GameState.HandelHighScore;

        }

        public bool isGameWon()
        {
            bool gameCheck = true;
            if (GuessWurdz.gameState == GuessWurdz.GameState.GameLost)
            {
                gameCheck = false;
            }
            else
            {
                for (int i = 0; i < 56; i++)
                {
                    if (gameBoard[i].blockVisible == true && gameBoard[i].letterValue != ' ')
                    {
                        if (gameBoard[i].letterVisible == false)
                        {
                            gameCheck = false;
                        }
                    }

                }
                if (gameCheck)
                {
                    GuessWurdz.gameState = GuessWurdz.GameState.GameWon;

                }
            }


            return gameCheck;
        }

        public void score(int option)
        {
            if (theScore.scoringState == true)

                switch (option)
                {
                    case Scoring.ADDSCORE:
                        theScore.current_points += 10 * theScore.number_right;
                        break;
                    case Scoring.GUESSRIGHT:
                        theScore.number_wrong = 0;
                        theScore.number_right += 1;
                        if (theScore.number_right > theScore.streak_number_right)
                        { theScore.streak_number_right = theScore.number_right; }
                        break;
                    case Scoring.GUESSWRONG:
                        theScore.number_right = 0;
                        theScore.number_wrong += 1;
                        theScore.lives_left -= 1;
                        theScore.number_wrong_level += 1;
                        if (theScore.number_wrong > theScore.streak_number_wrong)
                        { theScore.streak_number_wrong = theScore.number_wrong; }
                        break;
                    default:
                        break;
                }

        }

        public void saveHighScore()
        {
            highScore.SaveHighScore(theScore.current_points, theScore.current_level, GuessWurdz.oskb.Text);
        }

        public void CheckBomb()
        {
            for (int i = 0; i < 56; i++)
            {
                if (gameBoard[i].blockVisible == true && gameBoard[i].letterVisible == false && gameBoard[i].bomb.turnsToExplode >= 1)
                {
                    gameBoard[i].bomb.turnsToExplode = gameBoard[i].bomb.turnsToExplode - 1;
                    Sound.Play(SoundEntry.SFX_BombAlert);
                    if (gameBoard[i].bomb.turnsToExplode == 0)
                    {
                        GuessWurdz.gameState = GuessWurdz.GameState.GameLost;
                        RevealBoard();
                        Sound.Play(SoundEntry.SFX_Explosion);
                        //gameBomb = true;
                    }
                }
            }

        }

        private string newBackground()
        {
            int randomNumber;
            string filePath, gfxNum;
            Functions function = new Functions();

            randomNumber = function.RandomNumber(1, GameplayScreen.NUM_GFX_BACKGROUND);
            filePath = "Backgrounds/Game/bg";
            gfxNum = randomNumber.ToString();

            filePath = filePath + gfxNum;

            return filePath;

        }

        public void displayAbutton(int x, int y)
        {
            SpriteBatch spriteBatch = GuessWurdz.screenManager.SpriteBatch;
            spriteBatch.Begin();
            spriteBatch.Draw(GameplayScreen.gfxAContinue, new Vector2(x, y), new Color(255, 255, 255));
            spriteBatch.End();





        }




        private void displayNumber(int value, int xpos, int ypos)
        {
            const int width = 34;
            const int height = 34;
            string temp_value;
            SpriteBatch spriteBatch = GuessWurdz.screenManager.SpriteBatch;

            temp_value = value.ToString();
            char[] intvalue = temp_value.ToCharArray();

            spriteBatch.Begin();
            for (int i = 0; i < intvalue.Length; i++)
            {
                if (intvalue[i].ToString() == "0")
                {
                    spriteBatch.Draw(GameplayScreen.gfxNumberTile001, new Rectangle(xpos + (i * width), ypos, width, height), new Rectangle(width * 0, 0, width, height), new Color(255, 255, 255));


                }
                if (intvalue[i].ToString() == "1")
                {
                    spriteBatch.Draw(GameplayScreen.gfxNumberTile001, new Rectangle(xpos + (i * width), ypos, width, height), new Rectangle(width * 4, height, width, height), new Color(255, 255, 255));

                }
                if (intvalue[i].ToString() == "2")
                {
                    spriteBatch.Draw(GameplayScreen.gfxNumberTile001, new Rectangle(xpos + (i * width), ypos, width, height), new Rectangle(width * 3, height, width, height), new Color(255, 255, 255));

                }
                if (intvalue[i].ToString() == "3")
                {
                    spriteBatch.Draw(GameplayScreen.gfxNumberTile001, new Rectangle(xpos + (i * width), ypos, width, height), new Rectangle(width * 2, height, width, height), new Color(255, 255, 255));

                }
                if (intvalue[i].ToString() == "4")
                {
                    spriteBatch.Draw(GameplayScreen.gfxNumberTile001, new Rectangle(xpos + (i * width), ypos, width, height), new Rectangle(width * 1, height, width, height), new Color(255, 255, 255));

                }
                if (intvalue[i].ToString() == "5")
                {
                    spriteBatch.Draw(GameplayScreen.gfxNumberTile001, new Rectangle(xpos + (i * width), ypos, width, height), new Rectangle(width * 0, height, width, height), new Color(255, 255, 255));


                }
                if (intvalue[i].ToString() == "6")
                {

                    spriteBatch.Draw(GameplayScreen.gfxNumberTile001, new Rectangle(xpos + (i * width), ypos, width, height), new Rectangle(width * 4, 0, width, height), new Color(255, 255, 255));


                }
                if (intvalue[i].ToString() == "7")
                {
                    spriteBatch.Draw(GameplayScreen.gfxNumberTile001, new Rectangle(xpos + (i * width), ypos, width, height), new Rectangle(width * 3, 0, width, height), new Color(255, 255, 255));

                }
                if (intvalue[i].ToString() == "8")
                {
                    spriteBatch.Draw(GameplayScreen.gfxNumberTile001, new Rectangle(xpos + (i * width), ypos, width, height), new Rectangle(width * 2, 0, width, height), new Color(255, 255, 255));

                }
                if (intvalue[i].ToString() == "9")
                {
                    spriteBatch.Draw(GameplayScreen.gfxNumberTile001, new Rectangle(xpos + (i * width), ypos, width, height), new Rectangle(width, 0, width, height), new Color(255, 255, 255));

                }

            }
            spriteBatch.End();


        }

        private float convertToLetter(int x, int y)
        {
            int conversion;

            conversion = ((x + y) + (y * 7)); // convert Matrix Cord into Letter Array Space
            return conversion;
        }

        private bool phraseAvail()
        {
            bool phraseOpen = false;

            for (int i = 0; i < 2000; i++)
            {
                if (GuessWurdz.gamePhrase[i] == false)
                {
                    phraseOpen = true;

                }
            }

            return phraseOpen;

        }



        /// <summary>
        /// Converts Phrase into five seperate rows for the game.
        /// </summary>
        /// <param name="phrase"></param>
        /// <param name="hint"></param>
        /// <param name="title"></param>
        /// <returns></returns>
        private PhraseData convertPhrase(string phrase, string hint, string title)
        {
            PhraseData tempPhrase;
            string line1, line2, line3, line4, space, tempword;

            int wordcount = 0;
            string[] words = phrase.Split(' ');
            line1 = "";
            line2 = "";
            line3 = "";
            line4 = "";
            space = "";
            int linecount = 1;


            foreach (string word in words)
            {
                // if (wordcount > 0) { space = " "; }

                if (linecount == 1)
                {
                    if (line1.Length <= GuessWurdz.BOARD_ROW_LENGTH)
                    {
                        if (line1.Length > 0) { space = " "; }
                        tempword = line1;
                        tempword += space + string.Join(" ", words, wordcount, 1);
                        if (tempword.Length < GuessWurdz.BOARD_ROW_LENGTH)
                        {
                            line1 += space + string.Join(" ", words, wordcount, 1);
                        }
                        else
                        {
                            linecount = 2;
                            space = "";
                        }
                    }

                }

                if (linecount == 2)
                {
                    if (line1.Length <= GuessWurdz.BOARD_ROW_LENGTH)
                    {
                        if (line2.Length > 0) { space = " "; }
                        tempword = line2;
                        tempword += space + string.Join(" ", words, wordcount, 1);
                        if (tempword.Length < GuessWurdz.BOARD_ROW_LENGTH)
                        {
                            line2 += space + string.Join(" ", words, wordcount, 1);
                        }
                        else
                        {
                            linecount = 3;
                            space = "";

                        }
                    }

                }
                if (linecount == 3)
                {
                    if (line3.Length <= GuessWurdz.BOARD_ROW_LENGTH)
                    {
                        if (line1.Length > 0) { space = " "; }
                        tempword = line3;
                        tempword += space + string.Join(" ", words, wordcount, 1);
                        if (tempword.Length < GuessWurdz.BOARD_ROW_LENGTH)
                        {
                            line3 += space + string.Join(" ", words, wordcount, 1);
                        }
                        else
                        {
                            linecount = 4;
                            space = "";
                        }
                    }

                }
                if (linecount == 4)
                {
                    if (line1.Length <= GuessWurdz.BOARD_ROW_LENGTH)
                    {
                        if (line4.Length > 0) { space = " "; }
                        tempword = line4;
                        tempword += space + string.Join(" ", words, wordcount, 1);
                        if (tempword.Length < GuessWurdz.BOARD_ROW_LENGTH)
                        {
                            line4 += space + string.Join(" ", words, wordcount, 1);
                        }
                        else
                        {
                            linecount = 5;
                        }
                    }

                }

                wordcount += 1;
            }

            tempPhrase.hint = hint;
            tempPhrase.title = title;
            tempPhrase.line1 = line1;
            tempPhrase.line2 = line2;
            tempPhrase.line3 = line3;
            tempPhrase.line4 = line4;

            return tempPhrase;

        }
        public void checkReward()
        {
            if (!Guide.IsTrialMode)
            {
                if (theScore.current_points > theScore.last_reward_level &&
                    theScore.current_points > theScore.next_reward_level)
                {
                    theScore.gold_balls += 1;
                    Sound.Play(SoundEntry.SFX_NewGoldenBall);
                    theScore.last_reward_level = theScore.next_reward_level;
                    theScore.next_reward_level += 5000;
                }

                //Perfect Round
                if (GuessWurdz.gameState == GuessWurdz.GameState.GameWon)
                {
                    if (theScore.number_wrong_level == 0 &&
                        theScore.current_level > theScore.last_perfect_round)
                    {
                        theScore.gold_balls += 1;
                        Sound.Play(SoundEntry.SFX_NewGoldenBall);
                        Sound.Play(SoundEntry.SFX_PerfectRound);
                        theScore.last_perfect_round = theScore.current_level;
                        theScore.num_perfect_rounds += 1;
                    }
                }

                if (theScore.gold_balls > 10) { theScore.gold_balls = 10; }

            }
        }

        

        public void display_thinkblock(int xpos, int ypos, int mode, int type)
        {
            Functions function = new Functions();
            int randomNumber;
            int randomNumber2;

            if (!GameplayScreen.show_thinkblock)
            {
                if (mode == 2) // Set Think Block
                {
                    randomNumber2 = function.RandomNumber(1, 15);
                    if (randomNumber2 == 3)
                    {
                        // Cleart Thinkblock
                        GameplayScreen.thinkBlocktext.line1 = "";
                        GameplayScreen.thinkBlocktext.line2 = "";
                        if (type == 1) // Good Guess
                        {
                            do
                            {
                                randomNumber = function.RandomNumber(0, scriptGuessRight.Count);
                            }
                            while (randomNumber == lastNum);

                            lastNum = randomNumber;
                            GameplayScreen.thinkBlocktext.line1 = scriptGuessRight.theScript[randomNumber].Line1;
                            GameplayScreen.thinkBlocktext.line2 = scriptGuessRight.theScript[randomNumber].Line2;
                            GameplayScreen.thinkBlocktext.SoundFx = scriptGuessRight.theScript[randomNumber].SFX;
                            GameplayScreen.thinkBlocktext.playSFX = true;   
                        }

                        if (type == 2) // Wrogn Guess
                        {
                            do
                            {
                                randomNumber = function.RandomNumber(0, scriptGuessWrong.Count);
                            }
                            while (randomNumber == lastNum);

                            lastNum = randomNumber;
                            GameplayScreen.thinkBlocktext.line1 = scriptGuessWrong.theScript[randomNumber].Line1;
                            GameplayScreen.thinkBlocktext.line2 = scriptGuessWrong.theScript[randomNumber].Line2;
                            GameplayScreen.thinkBlocktext.SoundFx = scriptGuessWrong.theScript[randomNumber].SFX;
                            if (GameplayScreen.thinkBlocktext.SoundFx.Length > 0) { GameplayScreen.thinkBlocktext.playSFX = true; }
                            else { GameplayScreen.thinkBlocktext.playSFX = false; }

                        }

                        GameplayScreen.show_thinkblock = true;
                    }
                }
            }

            if (mode == 1)
            {
       
                SpriteBatch spriteBatch = GuessWurdz.screenManager.SpriteBatch;
                GameplayScreen.show_thinkblock = true;
                Vector2 pixel_length = new Vector2(0, 0);
                int start_pixel, width;

                spriteBatch.Begin();
                spriteBatch.Draw(GameplayScreen.gfxThinkBlock, new Vector2(xpos, ypos), new Color(255, 255, 255));
                {
                    width = 285;
                    pixel_length = GameplayScreen.thinkblockFont.MeasureString(GameplayScreen.thinkBlocktext.line1.ToString());
                    start_pixel = (width / 2) - ((int)pixel_length.X / 2);
                    spriteBatch.DrawString(GameplayScreen.thinkblockFont, GameplayScreen.thinkBlocktext.line1, new Vector2(xpos + 16 + start_pixel, ypos + 40), Color.White);
                }
                if (GameplayScreen.thinkBlocktext.line2.Length > 0)
                {
                    width = 265;
                    pixel_length = GameplayScreen.thinkblockFont.MeasureString(GameplayScreen.thinkBlocktext.line2.ToString());
                    start_pixel = (width / 2) - ((int)pixel_length.X / 2);
                    spriteBatch.DrawString(GameplayScreen.thinkblockFont, GameplayScreen.thinkBlocktext.line2, new Vector2(xpos + 26 + start_pixel, ypos + 60), Color.White);
                }
                spriteBatch.End();
                if (GameplayScreen.thinkBlocktext.playSFX)
                {
                    Sound.Play(GameplayScreen.thinkBlocktext.SoundFx);
                    GameplayScreen.thinkBlocktext.playSFX = false;
                }
            
                
            }
        }

        public void display_stats(int xpos, int ypos)
        {

            SpriteBatch spriteBatch = GuessWurdz.screenManager.SpriteBatch;

            spriteBatch.Begin();
            // Right in a Row
            spriteBatch.DrawString(GameplayScreen.gameFontSmall, "Incorrect Guesses in a Row:", new Vector2(xpos, ypos), Color.Blue);
            spriteBatch.DrawString(GameplayScreen.gameFontSmall, theScore.number_wrong.ToString(), new Vector2(xpos + 300, ypos), Color.Black);
            // Wrong in a Row
            spriteBatch.DrawString(GameplayScreen.gameFontSmall, "Correct Guesses in a Row:", new Vector2(xpos, ypos + 20), Color.Blue);
            spriteBatch.DrawString(GameplayScreen.gameFontSmall, theScore.number_right.ToString(), new Vector2(xpos + 300, ypos + 20), Color.Black);
            // Streak Right
            spriteBatch.DrawString(GameplayScreen.gameFontSmall, "Longest Correct Streak:", new Vector2(xpos, ypos + 40), Color.Blue);
            spriteBatch.DrawString(GameplayScreen.gameFontSmall, theScore.streak_number_right.ToString(), new Vector2(xpos + 300, ypos + 40), Color.Black);
            // Streak Wrong
            spriteBatch.DrawString(GameplayScreen.gameFontSmall, "Longest Incorrect Streak:", new Vector2(xpos, ypos + 60), Color.Blue);
            spriteBatch.DrawString(GameplayScreen.gameFontSmall, theScore.streak_number_wrong.ToString(), new Vector2(xpos + 300, ypos + 60), Color.Black);
            // Bombs Diffused
            spriteBatch.DrawString(GameplayScreen.gameFontSmall, "Bombs Diffused:", new Vector2(xpos, ypos + 80), Color.Blue);
            spriteBatch.DrawString(GameplayScreen.gameFontSmall, theScore.bombs_diffused.ToString(), new Vector2(xpos + 300, ypos + 80), Color.Black);
            // Golden Ball Used
            spriteBatch.DrawString(GameplayScreen.gameFontSmall, "Golden Balls Used:", new Vector2(xpos, ypos + 100), Color.Blue);
            spriteBatch.DrawString(GameplayScreen.gameFontSmall, theScore.golden_balls_used.ToString(), new Vector2(xpos + 300, ypos + 100), Color.Black);
            // Perfect Rounds
            spriteBatch.DrawString(GameplayScreen.gameFontSmall, "Perfect Rounds:", new Vector2(xpos, ypos + 120), Color.Blue);
            spriteBatch.DrawString(GameplayScreen.gameFontSmall, theScore.num_perfect_rounds.ToString(), new Vector2(xpos + 300, ypos + 120), Color.Black);
            // Score to Next Golden Ball
            spriteBatch.DrawString(GameplayScreen.gameFontSmall, "Score for Next Golden Ball:", new Vector2(xpos, ypos + 140), Color.Blue);
            spriteBatch.DrawString(GameplayScreen.gameFontSmall, theScore.next_reward_level.ToString(), new Vector2(xpos + 300, ypos + 140), Color.Black);

            spriteBatch.End();


        }

        public void cheat_addpoints()
        {
            theScore.current_points += 100;
            checkReward();
        }

        public void cheat_takelife()
        {
            theScore.lives_left -= 1;
            if (theScore.lives_left < 0) { theScore.lives_left = 0; }
        }

        public void start_bgmusic()
        {

            GuessWurdz.cue = Sound.Play("Underground_Journey");
    

              

        }

        public void stop_bgmusic()
        {
            GuessWurdz.cue.Stop(AudioStopOptions.AsAuthored);
           // bool test = cue.IsStopping;
          //  bool test2 = cue.IsStopped;

        }

        public bool isHighScore()
        {
           
                HighScores.LoadHighScores();
            
            int scoreIndex = -1;
            bool response;
            for (int i = 0; i < (GuessWurdz.theHighScore.Count); i++)
            {
                if (theScore.current_points > GuessWurdz.theHighScore.Score[i])
                {
                    scoreIndex = i;
                    break;
                }
            }

            if (scoreIndex == -1) { response = false; } else { response = true; }

            return response;

        }

        private void loadScripts()
        {
            scriptGuessRight = CharScript.LoadScripts("Data/GuessRightData.xml");
            scriptGuessWrong = CharScript.LoadScripts("Data/GuessWrongData.xml");
        }

        public void ballActivated(int code)
        {
            bool usedBall = false;
            Converter convert = new Converter();
            Functions function = new Functions();
            bool findLetter = false;
            if (code == 999) { theScore.gold_balls = 10; Sound.Play(SoundEntry.SFX_NewGoldenBall); }
            if (theScore.gold_balls > 0 || code == 10)
            {
                // Check for Bombs
                for (int i = 0; i < 56; i++)
                {
                    if (gameBoard[i].bomb.bombType == 1)
                    {
                        gameBoard[i].bomb.bombType = 0;
                        gameBoard[i].bomb.turnsToExplode = 0;
                        theScore.bombs_diffused += 1;
                        usedBall = true;
                    }
                }

                if (usedBall == false)
                {
                    while (findLetter == false)
                    {
                        int theLetter = function.RandomNumber(0, 26);

                        if (letterBlock[theLetter].alreadyChosen == false)
                        {
                            for (int i = 0; i < 56; i++)
                            {
                                if (convert.tolowerchar(gameBoard[i].letterValue) == convert.intToLetter(theLetter))
                                {
                                    if (gameBoard[i].letterVisible != true)
                                    {
                                        //Right Letter! add points !! good job
                                        usedBall = true;

                                    }
                                    gameBoard[i].letterVisible = true;

                                    findLetter = true;
                                    letterBlock[theLetter].alreadyChosen = true;
                                }

                            }


                        }
                    }

                }


            }

            if (usedBall == true && code != 10)
            {
                theScore.gold_balls = theScore.gold_balls - 1;
                theScore.golden_balls_used += 1;

            }
        }

    }
}
