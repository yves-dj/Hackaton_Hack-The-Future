using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hackaton_Hack_The_Future
{
    public static class NumeralConvertor
    {
        public static int RomanToInt(string s)
        {
            int sum = 0;
            Dictionary<char, int> romanNumbersDictionary = new()
            {
                { 'I', 1 },
                { 'V', 5 },
                { 'X', 10 },
                { 'L', 50 },
                { 'C', 100 },
                { 'D', 500 },
                { 'M', 1000 }
            };

            for (int i = 0; i < s.Length; i++)
            {
                char currentRomanChar = s[i];
                romanNumbersDictionary.TryGetValue(currentRomanChar, out int num);
                if (i + 1 < s.Length && romanNumbersDictionary[s[i + 1]] > romanNumbersDictionary[currentRomanChar])
                {
                    sum -= num;
                }
                else
                {
                    sum += num;
                }
            }
            return sum;
        }

        public static string IntToRoman(int num)
        {
            string romanResult = string.Empty;
            string[] romanLetters = {
                "M",
                "CM",
                "D",
                "CD",
                "C",
                "XC",
                "L",
                "XL",
                "X",
                "IX",
                "V",
                "IV",
                "I"
            };
            int[] numbers = {
                1000,
                900,
                500,
                400,
                100,
                90,
                50,
                40,
                10,
                9,
                5,
                4,
                1
            };

            int i = 0;
            while (num != 0)
            {
                if (num >= numbers[i])
                {
                    num -= numbers[i];
                    romanResult += romanLetters[i];
                }
                else
                {
                    i++;
                }
            }
            return romanResult;
        }
    }
}
