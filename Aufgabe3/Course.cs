//-----------------------------------------------------------------------
// <copyright file="Course.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a course.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a course.
    /// </summary>
    public class Course
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Course"/> class.
        /// </summary>
        public Course()
            : this(string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Course"/> class.
        /// </summary>
        /// <param name="abbreviation">The abbreviation of the course.</param>
        /// <param name="description">The description of the course.</param>
        public Course(string abbreviation, string description)
        {
            this.Abbreviation = abbreviation;
            this.Description = description;
        }

        /// <summary>
        /// Gets the abbreviation of the course.
        /// </summary>
        /// <value> The abbreviation of the course. </value>
        public string Abbreviation { get; private set; }

        /// <summary>
        /// Gets the description of the course.
        /// </summary>
        /// <value> The description of the course. </value>
        public string Description { get; private set; }

        /// <summary>
        /// Sets the abbreviation of the course.
        /// </summary>
        /// <param name="abbreviation">The new abbreviation of the course.</param>
        public void SetAbbreviation(string abbreviation)
        {
            if (abbreviation.Length >= 2 && abbreviation.Length <= 4)
            {
                this.Abbreviation = abbreviation;
            }
            else
            {
                throw new ArgumentException("The abbreviation must contain at least 2 letters!");
            }
        }

        /// <summary>
        /// Sets the description of the course.
        /// </summary>
        /// <param name="description">The new description of the course.</param>
        public void SetDescription(string description)
        {
            if (description.Length >= 2)
            {
                this.Description = description;
            }
            else
            {
                throw new ArgumentException("The description must have at least two characters!");
            }
        }

        /// <summary>
        /// Decides if two instances of this class are the same.
        /// </summary>
        /// <param name="obj">The object, which will be compared.</param>
        /// <returns>A boolean, indicating whether the two objects are the same or not.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Course)
            {
                Course comparingCourse = (Course)obj;

                return comparingCourse.Abbreviation.Equals(this.Abbreviation);
            }

            return false;
        }

        /// <summary>
        /// Gets the hash code.
        /// </summary>
        /// <returns>The hash code of the course.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
