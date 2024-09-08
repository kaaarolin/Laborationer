// Laboration 1

using System;
using System.Data.SqlTypes;
using System.Diagnostics.CodeAnalysis;

Console.WriteLine("Mata in din sträng: ");
string inputString = Console.ReadLine();
string subString;
long totalSum = 0; 

for (int i = 0; i < inputString.Length; i++)
{
    char firstChar = inputString[i];
    subString = "";

    if (char.IsLetter(firstChar))
    {
        continue;
    }
    else if (char.IsDigit(firstChar))
    {
        subString += firstChar;

        for (int j = i + 1; j < inputString.Length; j++)
        {
            char nextChar = inputString[j];

            if (char.IsLetter(nextChar)) 
            {
                break;
            }
            subString += nextChar;

            if (firstChar == nextChar)
            {
                Console.Write(inputString[0..i]);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(subString);
                Console.ResetColor();
                Console.WriteLine(inputString[(j+1)..]);
                
                long.TryParse(subString, out long result);
                totalSum += result;

                break;

            }
             
        }
    } 
}
Console.WriteLine("Delsträngarnas totala summa är: " + totalSum);

