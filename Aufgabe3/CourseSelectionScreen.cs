//-----------------------------------------------------------------------
// <copyright file="CourseSelectionScreen.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a screen, where the user can choose from a list of courses.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a screen, where the user can choose from a list of courses.
    /// </summary>
    public static class CourseSelectionScreen
    {
        /// <summary>
        /// Draws the list of selectable courses in a screen.
        /// </summary>
        /// <param name="selectableCourses">List of courses, from which the user can select one.</param>
        /// <returns>A string identifying the course.</returns>
        public static string ShowAvailableCourses(List<Course> selectableCourses)
        {
            Console.Clear();
            Console.WriteLine("\n [Enter] Close\n");
            Console.WriteLine(" - Select a course\n");

            if (selectableCourses.Count < 1)
            {
                Console.WriteLine("    The program couldn't find any course!");
            }
            else
            {
                for (int i = 0; i < selectableCourses.Count; i++)
                {
                    Console.WriteLine("    [{0}] {1} - {2}\n", i, selectableCourses[i].Abbreviation, selectableCourses[i].Description);
                }

                Console.Write("   Your choice [0 - {0}]: ", selectableCourses.Count - 1);
            }

            int index = 0;

            int.TryParse(Console.ReadLine(), out index);

            if (index >= 0 && index < selectableCourses.Count)
            {
                return selectableCourses[index].Abbreviation;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
