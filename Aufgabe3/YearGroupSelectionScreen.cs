//-----------------------------------------------------------------------
// <copyright file="YearGroupSelectionScreen.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a screen, where the user can choose from a list of year groups.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a screen, where the user can choose from a list of year groups.
    /// </summary>
    public static class YearGroupSelectionScreen
    {
        /// <summary>
        /// Draws the list of selectable year groups in a screen.
        /// </summary>
        /// <param name="selectableYearGroups">List of year groups, from which the user can select one.</param>
        /// <returns>A string identifying the year group.</returns>
        public static string ShowAvailableYearGroups(List<YearGroup> selectableYearGroups)
        {
            Console.Clear();
            Console.WriteLine("\n [Enter] Close\n");
            Console.WriteLine(" - Select a year group\n");

            if (selectableYearGroups.Count < 1)
            {
                Console.WriteLine("    The program couldn't find any year group!");
            }
            else
            {
                for (int i = 0; i < selectableYearGroups.Count; i++)
                {
                    Console.WriteLine("    [{0, 2}] {1}\n", i, selectableYearGroups[i].GetIdentifier());
                }

                Console.Write("   Your choice [0 - {0}]: ", selectableYearGroups.Count - 1);
            }

            int index = 0;

            int.TryParse(Console.ReadLine(), out index);

            if (index >= 0 && index < selectableYearGroups.Count)
            {
                return selectableYearGroups[index].GetIdentifier();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
