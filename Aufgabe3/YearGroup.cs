//-----------------------------------------------------------------------
// <copyright file="YearGroup.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a year group.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a year group.
    /// </summary>
    public class YearGroup
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="YearGroup"/> class.
        /// </summary>
        public YearGroup() : this(2000, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="YearGroup"/> class.
        /// </summary>
        /// <param name="year">Year of the year group.</param>
        /// <param name="description">Description of the year group.</param>
        public YearGroup(int year, string description)
        {
            this.Year = year;
            this.Description = description;
        }

        /// <summary>
        /// Gets the year of the year group.
        /// </summary>
        /// <value>The year of the year group.</value>
        public int Year { get; private set; }

        /// <summary>
        /// Gets the description of the year group.
        /// </summary>
        /// <value>The description of the year group.</value>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the identifier of the year group.
        /// </summary>
        /// <returns>The identifier of the year group.</returns>
        public string GetIdentifier()
        {
            return this.Description;
        }

        /// <summary>
        /// Sets the year of the year group.
        /// </summary>
        /// <param name="year">The new year of the year group.</param>
        public void SetYear(string year)
        {
            int temp = 0;

            if (int.TryParse(year, out temp))
            {
                this.SetYear(temp);
            }
            else
            {
                throw new ArgumentException("The year must be a valid number!");
            }
        }

        /// <summary>
        /// Sets the year of the year group.
        /// </summary>
        /// <param name="year">The new year of the year group.</param>
        public void SetYear(int year)
        {
            string date = DateTime.Now.ToString();
            DateTime datevalue = Convert.ToDateTime(date.ToString());

            string yy = datevalue.Year.ToString();

            if (year >= 1950 && year <= int.Parse(yy) + 1)
            {
                this.Year = year;
            }
            else
            {
                throw new ArgumentException("The year must exist!");
            }
        }

        /// <summary>
        /// Sets the description of the year group.
        /// </summary>
        /// <param name="description">The new description of the year group.</param>
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
            if (obj is YearGroup)
            {
                YearGroup comparingAgeGroup = (YearGroup)obj;

                return comparingAgeGroup.GetIdentifier().Equals(this.GetIdentifier());
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
