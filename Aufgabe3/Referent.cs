//-----------------------------------------------------------------------
// <copyright file="Referent.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a referent.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a referent.
    /// </summary>
    public class Referent
    {
        /// <summary>
        /// The password of the referent.
        /// </summary>
        private string password;

        /// <summary>
        /// Initializes a new instance of the <see cref="Referent"/> class.
        /// </summary>
        public Referent() : this(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Referent"/> class.
        /// </summary>
        /// <param name="id">ID of the referent.</param>
        /// <param name="firstName">First name of the referent.</param>
        /// <param name="lastName">Last name of the referent.</param>
        /// <param name="password">Password of the referent.</param>
        /// <param name="email">E-Mail of the referent.</param>
        /// <param name="phone">Phone of the referent.</param>
        public Referent(string id, string firstName, string lastName, string password, string email, string phone)
        {
            this.ID = id;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.password = password;
            this.Email = email;
            this.Phone = phone;

            this.YearGroups = new List<YearGroup>();
            this.Courses = new List<Course>();
            this.Students = new List<Student>();
            this.Evaluations = new List<Evaluation>();
        }

        /// <summary>
        /// Gets the ID of the referent.
        /// </summary>
        /// <value>The ID of the referent.</value>
        public string ID { get; private set; }

        /// <summary>
        /// Gets the first name of the referent.
        /// </summary>
        /// <value>The first name of the referent.</value>
        public string FirstName { get; private set; }

        /// <summary>
        /// Gets the last name of the referent.
        /// </summary>
        /// <value>The last name of the referent.</value>
        public string LastName { get; private set; }

        /// <summary>
        /// Gets the email of the referent.
        /// </summary>
        /// <value>The email of the referent.</value>
        public string Email { get; private set; }

        /// <summary>
        /// Gets the phone of the referent.
        /// </summary>
        /// <value>The phone of the referent.</value>
        public string Phone { get; private set; }

        /// <summary>
        /// Gets all year groups, which have been created by this referent.
        /// </summary>
        /// <value>A list of all year groups, which have been created by this referent.</value>
        public List<YearGroup> YearGroups { get; private set; }

        /// <summary>
        /// Gets all courses, which have been created by this referent.
        /// </summary>
        /// <value>A list of all courses, which have been created by this referent.</value>
        public List<Course> Courses { get; private set; }

        /// <summary>
        /// Gets all students, which have been created by this referent.
        /// </summary>
        /// <value>A list of all students, which have been created by this referent.</value>
        public List<Student> Students { get; private set; }

        /// <summary>
        /// Gets all evaluations, which have been created by this referent.
        /// </summary>
        /// <value>A list of all evaluations, which have been created by this referent.</value>
        public List<Evaluation> Evaluations { get; private set; }

        /// <summary>
        /// Checks, if a given text matches with the referent's password.
        /// </summary>
        /// <param name="pwd">The given password, which will be checked.</param>
        /// <returns>A boolean indicating whether the given password matches with the referent's password or not.</returns>
        public bool IsMatchingPassword(string pwd)
        {
            return pwd.Equals(this.password);
        }

        /// <summary>
        /// Sets the ID of the referent.
        /// </summary>
        /// <param name="id">The new ID of the referent.</param>
        public void SetID(string id)
        {
            if (id.Length == 5)
            {
                int temp = 0;

                if (int.TryParse(id, out temp))
                {
                    if (temp >= 10000 && temp < 100000)
                    {
                        this.ID = id;
                    }
                    else
                    {
                        throw new ArgumentException("ID must contain 5 digits!");
                    }
                }
                else
                {
                    throw new ArgumentException("ID must contain only digits!");
                }
            }
            else
            {
                throw new ArgumentException("Length of the ID must be 5!");
            }
        }

        /// <summary>
        /// Sets the first name of the referent.
        /// </summary>
        /// <param name="firstName">The new first name of the referent.</param>
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
        /// Sets the last name of the referent.
        /// </summary>
        /// <param name="lastName">The new last name of the referent.</param>
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
        /// Sets the password of the referent.
        /// </summary>
        /// <param name="password">The new password of the referent.</param>
        public void SetPassword(string password)
        {
            if (password.Length >= 4)
            {
                this.password = password;
            }
            else
            {
                throw new ArgumentException("Password must contain at least 4 characters!");
            }
        }

        /// <summary>
        /// Sets the email of the referent.
        /// </summary>
        /// <param name="email">The new email of the referent.</param>
        public void SetEmail(string email)
        {
            if (this.IsValidEmail(email))
            {
                this.Email = email;
            }
            else
            {
                throw new ArgumentException("E-Mail doesn't have a valid format!");
            }
        }

        /// <summary>
        /// Sets the phone of the referent.
        /// </summary>
        /// <param name="phone">The new phone of the referent.</param>
        public void SetPhone(string phone)
        {
            long temp = 0;

            if (long.TryParse(phone, out temp))
            {
                this.Phone = phone;
            }
            else
            {
                throw new ArgumentException("Phone number can only contain digits!");
            }
        }

        /// <summary>
        /// Adds a year group, which has been created by this referent.
        /// </summary>
        /// <param name="yearGroup">A year group, which has been created by this referent.</param>
        /// <returns>A boolean, indicating whether the insertion was successful or not.</returns>
        public bool AddYearGroup(YearGroup yearGroup)
        {
            if (!this.YearGroups.Contains(yearGroup))
            {
                this.YearGroups.Add(yearGroup);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a course, which has been created by this referent.
        /// </summary>
        /// <param name="course">A course, which has been created by this referent.</param>
        /// <returns>A boolean, indicating whether the insertion was successful or not.</returns>
        public bool AddCourse(Course course)
        {
            if (!this.Courses.Contains(course))
            {
                this.Courses.Add(course);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a student, which has been created by this referent.
        /// </summary>
        /// <param name="student">A student, which has been created by this referent.</param>
        /// <returns>A boolean, indicating whether the insertion was successful or not.</returns>
        public bool AddStudent(Student student)
        {
            if (!this.Students.Contains(student))
            {
                this.Students.Add(student);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Adds a evaluation, which has been created by this referent.
        /// </summary>
        /// <param name="evaluation">A evaluation, which has been created by this referent.</param>
        /// <returns>A boolean, indicating whether the insertion was successful or not.</returns>
        public bool AddEvaluation(Evaluation evaluation)
        {
            this.Evaluations.Add(evaluation);

            return true;
        }

        /// <summary>
        /// Decides if two instances of this class are the same.
        /// </summary>
        /// <param name="obj">The object, which will be compared.</param>
        /// <returns>A boolean, indicating whether the two objects are the same or not.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Referent)
            {
                return this.ID.Equals(((Referent)obj).ID);
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

        /// <summary>
        /// Checks if a given text is a valid email.
        /// </summary>
        /// <param name="s">Text which will be checked.</param>
        /// <returns>A boolean, indicating whether the given text is a valid email or not.</returns>
        private bool IsValidEmail(string s)
        {
            if (s.Contains("@") && s.Split('@')[1].Contains('.'))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
