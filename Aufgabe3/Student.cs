//-----------------------------------------------------------------------
// <copyright file="Student.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a student.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a student.
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Student"/> class.
        /// </summary>
        public Student() : this(string.Empty, string.Empty, string.Empty, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Student"/> class.
        /// </summary>
        /// <param name="matriculationNumber">Matriculation number of the student.</param>
        /// <param name="firstName">First name of the student.</param>
        /// <param name="lastName">Last name of the student.</param>
        /// <param name="year">Year group of the student.</param>
        public Student(string matriculationNumber, string firstName, string lastName, YearGroup year)
        {
            this.MatriculationNumber = matriculationNumber;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Year = year;
        }

        /// <summary>
        /// Gets the matriculation number of the student.
        /// </summary>
        /// <value>The matriculation number of the student.</value>
        public string MatriculationNumber { get; private set; }

        /// <summary>
        /// Gets the first name of the student.
        /// </summary>
        /// <value>The first name of the student.</value>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets the last name of the student.
        /// </summary>
        /// <value>The last name of the student.</value>
        public string LastName { get; private set; }

        /// <summary>
        /// Gets the year group of the student.
        /// </summary>
        /// <value>The year group of the student.</value>
        public YearGroup Year { get; private set; }

        /// <summary>
        /// Gets the full name of the student.
        /// </summary>
        /// <returns>A string, which contains the full name of the student.</returns>
        public string GetFullName()
        {
            return this.FirstName + " " + this.LastName;
        }

        /// <summary>
        /// Sets the matriculation number of the student.
        /// </summary>
        /// <param name="matriculationNumber">The new matriculation number of the student.</param>
        public void SetMatriculationNumber(string matriculationNumber)
        {
            if (matriculationNumber.Length == 10)
            {
                long temp = 0;

                if (long.TryParse(matriculationNumber, out temp))
                {
                    this.MatriculationNumber = matriculationNumber;
                }
                else
                {
                    throw new ArgumentException("Matriculation number must contain only digits!");
                }
            }
            else
            {
                throw new ArgumentException("Length of the matriculation number must be 10!");
            }
        }

        /// <summary>
        /// Sets the first name of the student.
        /// </summary>
        /// <param name="firstName">The new first name of the student.</param>
        public void SetFirstName(string firstName)
        {
            if (firstName.Length > 0)
            {
                this.FirstName = firstName;
            }
            else
            {
                throw new ArgumentException("The first name cannot be empty!");
            }
        }

        /// <summary>
        /// Sets the last name of the student.
        /// </summary>
        /// <param name="lastName">The new last name of the student.</param>
        public void SetLastName(string lastName)
        {
            if (lastName.Length > 0)
            {
                this.LastName = lastName;
            }
            else
            {
                throw new ArgumentException("The last name cannot be empty!");
            }
        }

        /// <summary>
        /// Sets the year group of the student by the identifier.
        /// </summary>
        /// <param name="yearIdentifier">Identifier of the year group.</param>
        /// <param name="yearGroups">List of available year groups.</param>
        public void SetYear(string yearIdentifier, List<YearGroup> yearGroups)
        {
            YearGroup yearGroup = StaticQueries.GetYearGroupFromList(yearIdentifier, yearGroups);

            if (yearGroup != null)
            {
                this.SetYear(yearGroup);
            }
            else
            {
                throw new ArgumentException("The year group could not be found!");
            }
        }

        /// <summary>
        /// Sets the year group of the student.
        /// </summary>
        /// <param name="yearGroup">The new year group of the student.</param>
        public void SetYear(YearGroup yearGroup)
        {
            this.Year = yearGroup;
        }

        /// <summary>
        /// Decides if two instances of this class are the same.
        /// </summary>
        /// <param name="obj">The object, which will be compared.</param>
        /// <returns>A boolean, indicating whether the two objects are the same or not.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Student)
            {
                Student comparingStudent = (Student)obj;

                return comparingStudent.MatriculationNumber.Equals(this.MatriculationNumber);
            }

            return false;
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The hash code of the referent.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
