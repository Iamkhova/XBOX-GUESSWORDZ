#region File Description
//-----------------------------------------------------------------------------
// Sound.cs
//
// Microsoft XNA Community Game Platform
// Copyright (C) Microsoft Corporation. All rights reserved.
//-----------------------------------------------------------------------------
#endregion

#region Using Statements
using System;
using System.IO;
using Microsoft.Xna.Framework.Audio;
#endregion

namespace GuessWurdz
{
    /// <summary>
    /// An enum for all of the Marblets sounds
    /// </summary>
    public enum SoundEntry
    {
        /// <summary>
        /// Background Music
        /// </summary>
        IntroMusic,

        SFXLogo,

        Voice_yeah_lucky_guess,

        SFX_Click,

        SFX_MenuSelect,

        SFX_NewGoldenBall,

        SFX_WrongLetter,

        SFX_CorrectLetter,

        SFX_PerfectRound,

        VFX_ReallyOnARoll,

        VFX_SoWhat,

        VFX_BigDeal,

        VFX_AllTheAnswers,

        SFX_BombAlert,

        SFX_Explosion,

 
    }

    /// <summary>
    /// Abstracts away the sounds for a simple interface using the Sounds enum
    /// </summary>
    public static class Sound
    {
        private static AudioEngine audioEngine;
        private static WaveBank waveBank;
        private static SoundBank soundBank;

        private static string[] cueNames = new string[]
        {
            "intro_music", //Title Screen
            "fx_logo", // Logo Sound Effect
            "yeah_lucky_guess",
            "fx_click",
            "fx_menuselect",
            "fx_newgoldenball",
            "fx_wrong_letter",
            "fx_correct_letter",
            "fx_perfectround",
            "vfx_001",
            "vfx_002",
            "vfx_003",
            "vfx_004",
            "fx_bomb_alert",
            "fx_explosion",
            
      
        };

        /// <summary>
        /// Plays a sound
        /// </summary>
        /// <param name="cueName">Which sound to play</param>
        /// <returns>XACT cue to be used if you want to stop this particular looped 
        /// sound. Can be ignored for one shot sounds</returns>
        public static Cue Play(string cueName)
        {
            if (cueName == "")
            {
                cueName = "fx_click"; //XBOX bug workaround
            }
            Cue returnValue = soundBank.GetCue(cueName);
            returnValue.Play();
            
            return returnValue;
        }

        /// <summary>
        /// Plays a sound
        /// </summary>
        /// <param name="sound">Which sound to play</param>
        /// <returns>XACT cue to be used if you want to stop this particular looped 
        /// sound. Can be ignored for one shot sounds</returns>
        public static Cue Play(SoundEntry sound)
        {
            return Play(cueNames[(int)sound]);
        }

        /// <summary>
        /// Stops a previously playing cue
        /// </summary>
        /// <param name="cue">The cue to stop that you got returned from Play(sound)
        /// </param>
        public static void Stop(Cue cue)
        {
            if (cue != null)
            {
                cue.Stop(AudioStopOptions.Immediate);
            }
        }

        /// <summary>
        /// Starts up the sound code
        /// </summary>
        public static void Initialize()
        {
            // Audio
            audioEngine = new AudioEngine("Content\\Audio\\gwsound.xgs");
            waveBank = new WaveBank(audioEngine, "Content\\Audio\\WaveBank.xwb");
            soundBank = new SoundBank(audioEngine, "Content\\Audio\\SoundBank.xsb");


        }

        /// <summary>
        /// Shuts down the sound code tidily
        /// </summary>
        public static void Shutdown()
        {
            if (soundBank != null) soundBank.Dispose();
            if (waveBank != null) waveBank.Dispose();
            if (audioEngine != null) audioEngine.Dispose();
        }
    }
}
