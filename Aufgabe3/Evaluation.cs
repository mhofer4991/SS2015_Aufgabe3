//-----------------------------------------------------------------------
// <copyright file="Evaluation.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents an evaluation.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents an evaluation.
    /// </summary>
    public class Evaluation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Evaluation"/> class.
        /// </summary>
        public Evaluation()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Evaluation"/> class.
        /// </summary>
        /// <param name="referent">The referent of the evaluation.</param>
        /// <param name="yearGroup">The year group of the evaluation.</param>
        /// <param name="student">The student of the evaluation.</param>
        /// <param name="course">The course of the evaluation.</param>
        /// <param name="examDescription">The exam description of the evaluation.</param>
        /// <param name="examDate">The exam date of the evaluation.</param>
        /// <param name="examGrade">The exam grade of the evaluation.</param>
        public Evaluation(Referent referent, YearGroup yearGroup, Student student, Course course, string examDescription, string examDate, int examGrade)
        {
            this.Referent = referent;
            this.YearGroup = yearGroup;
            this.Student = student;
            this.Course = course;
            this.ExamDescription = examDescription;
            this.ExamDate = examDate;
            this.ExamGrade = examGrade;
        }

        /// <summary>
        /// Gets the referent of the evaluation.
        /// </summary>
        /// <value> The referent of the evaluation. </value>
        public Referent Referent { get; private set; }

        /// <summary>
        /// Gets the year group of the evaluation.
        /// </summary>
        /// <value> The year group of the evaluation. </value>
        public YearGroup YearGroup { get; private set; }

        /// <summary>
        /// Gets the student of the evaluation.
        /// </summary>
        /// <value> The student of the evaluation. </value>
        public Student Student { get; private set; }

        /// <summary>
        /// Gets the course of the evaluation.
        /// </summary>
        /// <value> The course of the evaluation. </value>
        public Course Course { get; private set; }

        /// <summary>
        /// Gets the exam description of the evaluation.
        /// </summary>
        /// <value> The exam description of the evaluation. </value>
        public string ExamDescription { get; private set; }

        /// <summary>
        /// Gets the exam date of the evaluation.
        /// </summary>
        /// <value> The exam date of the evaluation. </value>
        public string ExamDate { get; private set; }

        /// <summary>
        /// Gets the exam grade of the evaluation.
        /// </summary>
        /// <value> The exam grade of the evaluation. </value>
        public int ExamGrade { get; private set; }

        /// <summary>
        /// Checks if a given string is a valid date.
        /// </summary>
        /// <param name="date">The string, which will be checked.</param>
        /// <returns>A boolean indicating whether the given string is a valid date or not.</returns>
        public static bool IsValidDateFormat(string date)
        {
            string[] temp = date.Split('.');

            if (temp[0].Length == 2 && temp[1].Length == 2 && temp[2].Length == 4)
            {
                try
                {
                    DateTime dt = DateTime.Parse(date);

                    return true;
                }
                catch (FormatException)
                {
                    return false;
                }
            }

            return false;
        }

        /// <summary>
        /// Sets the referent of the evaluation.
        /// </summary>
        /// <param name="referent">The new referent of the evaluation.</param>
        public void SetReferent(Referent referent)
        {
            this.Referent = referent;
        }

        /// <summary>
        /// Sets the year group of the evaluation.
        /// </summary>
        /// <param name="yearGroup">The new year group of the evaluation.</param>
        public void SetYearGroup(YearGroup yearGroup)
        {
            this.YearGroup = yearGroup;
        }

        /// <summary>
        /// Sets the student of the evaluation.
        /// </summary>
        /// <param name="student">The new student of the evaluation.</param>
        public void SetStudent(Student student)
        {
            this.Student = student;
        }

        /// <summary>
        /// Sets the course of the evaluation.
        /// </summary>
        /// <param name="course">The new course of the evaluation.</param>
        public void SetCourse(Course course)
        {
            this.Course = course;
        }

        /// <summary>
        /// Sets the exam description of the evaluation.
        /// </summary>
        /// <param name="examDescription">The new exam description of the evaluation.</param>
        public void SetExamDescription(string examDescription)
        {
            if (examDescription.Length >= 2)
            {
                this.ExamDescription = examDescription;
            }
            else
            {
                throw new ArgumentException("The exam description must contain at least 2 letters!");
            }
        }

        /// <summary>
        /// Sets the exam date of the evaluation.
        /// </summary>
        /// <param name="examDate">The new exam date of the evaluation.</param>
        public void SetExamDate(string examDate)
        {
            if (Evaluation.IsValidDateFormat(examDate))
            {
                this.ExamDate = examDate;
            }
            else
            {
                throw new ArgumentException("The date must have the format DD.MM.YYYY!");
            }
        }

        /// <summary>
        /// Sets the exam grade of the evaluation.
        /// </summary>
        /// <param name="examGrade">The new exam grade of the evaluation.</param>
        public void SetExamGrade(string examGrade)
        {
            int temp = 0;

            if (int.TryParse(examGrade, out temp))
            {
                this.SetExamGrade(temp);
            }
            else
            {
                throw new ArgumentException("The exam grade must be a number between 1 and 5!");
            }
        }

        /// <summary>
        /// Sets the exam grade of the evaluation.
        /// </summary>
        /// <param name="examGrade">The new exam grade of the evaluation.</param>
        public void SetExamGrade(int examGrade)
        {
            if (examGrade >= 1 && examGrade <= 5)
            {
                this.ExamGrade = examGrade;
            }
            else
            {
                throw new ArgumentException("The exam grade must be a number between 1 and 5!");
            }
        }
    }
}
