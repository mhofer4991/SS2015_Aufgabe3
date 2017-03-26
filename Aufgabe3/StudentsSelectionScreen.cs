//-----------------------------------------------------------------------
// <copyright file="StudentsSelectionScreen.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a screen, where the user can choose from a list of students.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a screen, where the user can choose from a list of students.
    /// </summary>
    public static class StudentsSelectionScreen
    {
        /// <summary>
        /// Draws the list of selectable students in a screen. It's possible to search for a student.
        /// </summary>
        /// <param name="selectableStudents">List of students, from which the user can select one.</param>
        /// <returns>A string identifying the student.</returns>
        public static string ShowAvailableStudents(List<Student> selectableStudents)
        {
            Console.Clear();
            Console.WriteLine("\n [Enter] Proceed\n");
            Console.WriteLine(" - Options\n");
            Console.WriteLine("    [0] Search for a student\n");
            Console.WriteLine("    [1] Select a student from the list\n");

            Console.Write("   Your choice: ", selectableStudents.Count - 1);

            string option = Console.ReadLine();
            List<Student> tempSelectableStudents;

            if (option.Equals("0"))
            {
                Console.Write("\n    First name: ");

                string name = Console.ReadLine();

                Console.Write("\n     Last name: ");

                string lastname = Console.ReadLine();

                tempSelectableStudents = StaticQueries.FilterStudentsByName(name, lastname, selectableStudents);
            }
            else if (option.Equals("1"))
            {
                tempSelectableStudents = selectableStudents;
            }
            else
            {
                return string.Empty;
            }

            Console.Clear();
            Console.WriteLine("\n [Enter] Close\n");
            Console.WriteLine(" - Select a student\n");

            if (tempSelectableStudents.Count < 1)
            {
                Console.WriteLine("    The list is empty with the current options!\n    Try changing the year group and the search filter!");
            }
            else
            {
                for (int i = 0; i < tempSelectableStudents.Count; i++)
                {
                    Console.WriteLine("    [{0, 2}] {1} - {2} {3}\n", i, tempSelectableStudents[i].MatriculationNumber, tempSelectableStudents[i].FirstName, tempSelectableStudents[i].LastName);
                }

                Console.Write("   Your choice [0 - {0}]: ", tempSelectableStudents.Count - 1);
            }

            int index = 0;

            int.TryParse(Console.ReadLine(), out index);

            if (index >= 0 && index < tempSelectableStudents.Count)
            {
                return tempSelectableStudents[index].MatriculationNumber;
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
