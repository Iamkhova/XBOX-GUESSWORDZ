using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GuessWurdz
{
    public class Functions
    {
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
    }

    public class Converter
    {
        
        private int ucode; // The int type data member which is used to 


        public char tolowerchar(char s4) // The tolower method returning a string object
        {
            char ch = s4;
            char s6;

            if (ch >= 'A' && ch <= 'Z')  // Determines if the character is an alphabet and in 
            {    // upper case
                ucode = (int)ch;
                ucode = ucode + 32;
                s6 = (char)ucode;
            }
            else    // else retains the same case into the string object of     
            {    // the class
                s6 = ch;
            }
            return s6;   // Method returns converted string
        }

        public char intToLetter(int theValue)
        {
            char cvrtLetter;
            theValue = theValue + 97; // change value to unicode
            cvrtLetter = (char)theValue; // int is now converted to lowercase letter

            return cvrtLetter;

        }


    }
}
