//-----------------------------------------------------------------------
// <copyright file="StaticQueries.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class delivers methods to filter and search in list's of students and etc.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class delivers methods to filter and search in list's of students and etc.
    /// </summary>
    public static class StaticQueries
    {
        /// <summary>
        /// Gets the referent with a certain id from a list.
        /// </summary>
        /// <param name="id">ID of the wished referent.</param>
        /// <param name="list">List of referents.</param>
        /// <returns>The referent with the certain id or, if not found, a null-object.</returns>
        public static Referent GetReferentFromList(string id, List<Referent> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].ID.Equals(id))
                {
                    return list[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the year group with a certain identifier from a list.
        /// </summary>
        /// <param name="identifier">Identifier of the year group.</param>
        /// <param name="list">List of year groups.</param>
        /// <returns>The year group with the certain identifier or, if not found, a null-object.</returns>
        public static YearGroup GetYearGroupFromList(string identifier, List<YearGroup> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].GetIdentifier().Equals(identifier))
                {
                    return list[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the student with a certain matriculation number from a list.
        /// </summary>
        /// <param name="matriculationNumber">Matriculation number of the student.</param>
        /// <param name="list">List of students.</param>
        /// <returns>The student with the certain matriculation number or, if not found, a null-object.</returns>
        public static Student GetStudentFromList(string matriculationNumber, List<Student> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].MatriculationNumber.Equals(matriculationNumber))
                {
                    return list[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Gets the course with a certain abbreviation from a list.
        /// </summary>
        /// <param name="abbreviation">Abbreviation of the course.</param>
        /// <param name="list">List of courses.</param>
        /// <returns>The course with the certain abbreviation or, if not found, a null-object.</returns>
        public static Course GetCourseFromList(string abbreviation, List<Course> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Abbreviation.Equals(abbreviation))
                {
                    return list[i];
                }
            }

            return null;
        }

        /// <summary>
        /// Filters a list of students by it's first and last name.
        /// </summary>
        /// <param name="firstName">First name of the student.</param>
        /// <param name="lastName">Last name of the student.</param>
        /// <param name="list">List of students.</param>
        /// <returns>A list of the filtered students.</returns>
        public static List<Student> FilterStudentsByName(string firstName, string lastName, List<Student> list)
        {
            List<Student> filteredList = new List<Student>();

            for (int i = 0; i < list.Count; i++)
            {
                if (firstName.Equals(string.Empty))
                {
                    if (list[i].LastName.ToUpper().Contains(lastName.ToUpper()))
                    {
                        filteredList.Add(list[i]);
                    }
                }
                else if (lastName.Equals(string.Empty))
                {
                    if (list[i].FirstName.ToUpper().Contains(firstName.ToUpper()))
                    {
                        filteredList.Add(list[i]);
                    }
                }
                else if (list[i].FirstName.ToUpper().Contains(firstName.ToUpper()) || list[i].LastName.ToUpper().Contains(lastName.ToUpper()))
                {
                    filteredList.Add(list[i]);
                }
            }

            return filteredList;
        }

        /// <summary>
        /// Filters a list of students by it's year group.
        /// </summary>
        /// <param name="yearGroup">The desired year group.</param>
        /// <param name="students">List of students.</param>
        /// <returns>A filtered list of students.</returns>
        public static List<Student> FilterStudentsByYearGroup(YearGroup yearGroup, List<Student> students)
        {
            List<Student> filteredList = new List<Student>();

            for (int i = 0; i < students.Count; i++)
            {
                if (students[i].Year.Equals(yearGroup))
                {
                    filteredList.Add(students[i]);
                }
            }

            return filteredList;
        }

        /// <summary>
        /// Filters a list of evaluations by the year group, the course and the student.
        /// </summary>
        /// <param name="yearGroup">Desired year group.</param>
        /// <param name="course">Desired course.</param>
        /// <param name="student">Desired student.</param>
        /// <param name="list">List of evaluations.</param>
        /// <returns>A filtered list of evaluations.</returns>
        public static List<Evaluation> FilterEvaluationList(YearGroup yearGroup, Course course, Student student, List<Evaluation> list)
        {
            List<Evaluation> filteredEvaluations = new List<Evaluation>();

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Course.Equals(course) && list[i].YearGroup.Equals(yearGroup) && list[i].Student.Equals(student))
                {
                    filteredEvaluations.Add(list[i]);
                }
            }

            return filteredEvaluations;
        }

        /// <summary>
        /// Sorts a list of evaluations by its date, beginning with the latest.
        /// </summary>
        /// <param name="list">A list of evaluations.</param>
        /// <returns>A sorted list of evaluations.</returns>
        public static List<Evaluation> SortListByDate(List<Evaluation> list)
        {
            var query = (from x in list
                         select x).OrderBy(x => x.ExamDate);

            return query.ToList();
        }

        /// <summary>
        /// Gets the earliest date from a list of evaluations.
        /// </summary>
        /// <param name="list">The list of evaluations.</param>
        /// <returns>The earliest date from a list of evaluations.</returns>
        public static DateTime GetFirstDate(List<Evaluation> list)
        {
            if (list.Count > 0)
            {
                return DateTime.Parse(StaticQueries.SortListByDate(list)[0].ExamDate);
            }
            else
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Gets the latest date from a list of evaluations.
        /// </summary>
        /// <param name="list">The list of evaluations.</param>
        /// <returns>The latest date from a list of evaluations.</returns>
        public static DateTime GetLastDate(List<Evaluation> list)
        {
            if (list.Count > 0)
            {
                return DateTime.Parse(StaticQueries.SortListByDate(list)[list.Count - 1].ExamDate);
            }
            else
            {
                return DateTime.Now;
            }
        }

        /// <summary>
        /// Gets all evaluations within a certain time frame.
        /// </summary>
        /// <param name="firstDate">Earliest date.</param>
        /// <param name="lastDate">Latest date.</param>
        /// <param name="list">List of evaluations.</param>
        /// <returns>A list of evaluations within a certain time frame.</returns>
        public static List<Evaluation> FilterByFirstAndLastDate(DateTime firstDate, DateTime lastDate, List<Evaluation> list)
        {
            List<Evaluation> filteredList = new List<Evaluation>();
            DateTime date;

            for (int i = 0; i < list.Count; i++)
            {
                date = DateTime.Parse(list[i].ExamDate);

                if (date >= firstDate && date <= lastDate)
                {
                    filteredList.Add(list[i]);
                }
            }

            return filteredList;
        }
    }
}
