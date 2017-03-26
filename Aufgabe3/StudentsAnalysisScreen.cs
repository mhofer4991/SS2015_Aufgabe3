//-----------------------------------------------------------------------
// <copyright file="StudentsAnalysisScreen.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a screen, where the user can analyze students by year group and course.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a screen, where the user can analyze students by year group and course.
    /// </summary>
    public class StudentsAnalysisScreen
    {
        /// <summary>
        /// The last index of the input fields.
        /// </summary>
        private const int MaxSelection = 1;

        /// <summary>
        /// The position of the first selected input field.
        /// </summary>
        private int[] firstSelectionPosition;

        /// <summary>
        /// An array of entered values for each input field.
        /// </summary>
        private string[] inputValues;

        /// <summary>
        /// The current index of the input fields.
        /// </summary>
        private int currentSelection;

        /// <summary>
        /// Used for handling user input.
        /// </summary>
        private InputHandler inputHandler;

        /// <summary>
        /// Referent, who is logged in.
        /// </summary>
        private Referent loggedInReferent;

        /// <summary>
        /// As long as this boolean is false, the screen waits for user input.
        /// </summary>
        private bool canceled;

        /// <summary>
        /// If this boolean is true, the user entered some invalid values.
        /// </summary>
        private bool invalidInput;

        /// <summary>
        /// List of all available year groups.
        /// </summary>
        private List<YearGroup> availableYearGroups;

        /// <summary>
        /// List of all available courses.
        /// </summary>
        private List<Course> availableCourses;

        /// <summary>
        /// List of all available evaluations.
        /// </summary>
        private List<Evaluation> availableEvaluations;

        /// <summary>
        /// The currently selected year group.
        /// </summary>
        private YearGroup selectedYearGroup;

        /// <summary>
        /// The currently selected course.
        /// </summary>
        private Course selectedCourse;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentsAnalysisScreen"/> class.
        /// </summary>
        public StudentsAnalysisScreen()
        {
            this.firstSelectionPosition = new int[] { 5, 5 };

            this.inputValues = new string[StudentsAnalysisScreen.MaxSelection + 1];
            
            this.Reset();

            this.inputHandler = new InputHandler();

            this.inputHandler.SubscribeForKey(ConsoleKey.UpArrow);
            this.inputHandler.SubscribeForKey(ConsoleKey.DownArrow);
            this.inputHandler.SubscribeForKey(ConsoleKey.Enter);
            this.inputHandler.SubscribeForKey(ConsoleKey.Tab);
            this.inputHandler.SubscribeForKey(ConsoleKey.F1);
            this.inputHandler.SubscribeForKey(ConsoleKey.F2);
            this.inputHandler.SubscribeForKey(ConsoleKey.F3);
            this.inputHandler.SubscribeForKey(ConsoleKey.Escape);

            this.inputHandler.OnSubscribedKeyCalled += this.InputHandler_OnSubscribedKeyCalled;
        }

        /// <summary>
        /// Draws the screen on the console.
        /// </summary>
        public void Draw()
        {
            if (this.currentSelection == 0)
            {
                Console.WriteLine("\n [ESC] Close  [F1] Help  [F2] Analyse  [F3] Choose year  [Up / Down] Navigate");
            }
            else if (this.currentSelection == 1)
            {
                Console.WriteLine("\n [ESC] Close  [F1] Help  [F2] Analyse  [F3] Choose course  [Up / Down] Navigate");
            }

            Console.WriteLine("\n - Analyse students by year group and course\n");
            Console.WriteLine("    [ ] Year group: {0}\n", this.inputValues[0]);
            Console.WriteLine("    [ ] Course:     {0}\n", this.inputValues[1]);

            if (this.invalidInput)
            {
                Console.WriteLine("    ERROR: The year group or the course could not be found!");
            }

            Console.SetCursorPosition(this.firstSelectionPosition[0], this.firstSelectionPosition[1] + (this.currentSelection * 2));
            Console.Write('*');
        }

        /// <summary>
        /// Waits for the user to complete all input fields and then renders an evaluation of all students with a certain year group and course.
        /// </summary>
        /// <param name="loggedInReferent">A referent, who is logged in.</param>
        public void ShowAnalysisScreen(Referent loggedInReferent)
        {
            this.loggedInReferent = loggedInReferent;

            this.availableYearGroups = this.loggedInReferent.YearGroups;
            this.availableCourses = this.loggedInReferent.Courses;
            this.availableEvaluations = this.loggedInReferent.Evaluations;

            while (!this.canceled)
            {
                this.HandleUserInput();
                Console.Clear();
                this.Draw();
            }

            this.Reset();
        }

        /// <summary>
        /// Resets the selection, all input fields and error messages.
        /// </summary>
        public void Reset()
        {
            this.currentSelection = 0;

            this.canceled = false;
            this.invalidInput = false;
        }

        /// <summary>
        /// Handles the user input to all input fields.
        /// </summary>
        public void HandleUserInput()
        {
            Console.SetCursorPosition(this.firstSelectionPosition[0] + 15, this.firstSelectionPosition[1] + (this.currentSelection * 2));

            switch (this.currentSelection)
            {
                case 0:
                    // Waits for the user to press F3 so it can open a sub menu where all available year groups are displayed.
                    this.inputHandler.WaitForSubscribedKey();

                    break;
                case 1:
                    // Waits for the user to press F3 so it can open a sub menu where all available courses are displayed.
                    this.inputHandler.WaitForSubscribedKey();

                    break;
            }
        }

        /// <summary>
        /// Is called when the user presses one of the subscribed keys.
        /// </summary>
        /// <param name="key">Subscribed key.</param>
        private void InputHandler_OnSubscribedKeyCalled(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.DownArrow:
                case ConsoleKey.Enter:
                case ConsoleKey.Tab:
                    if (this.currentSelection < StudentsAnalysisScreen.MaxSelection)
                    {
                        this.currentSelection++;
                    }
                    else
                    {
                        this.currentSelection = 0;
                    }

                    break;
                case ConsoleKey.UpArrow:
                    if (this.currentSelection > 0)
                    {
                        this.currentSelection--;
                    }
                    else
                    {
                        this.currentSelection = StudentsAnalysisScreen.MaxSelection;
                    }

                    break;
                case ConsoleKey.F1:
                    this.ShowHelp();
                    break;
                case ConsoleKey.F2:
                    // Before the students can be evaluated, check if all input fields are correct.
                    if (this.selectedCourse != null && this.selectedYearGroup != null)
                    {
                        this.AnalyseStudents();
                        this.invalidInput = false;
                    }
                    else
                    {
                        this.invalidInput = true;
                    }

                    break;
                case ConsoleKey.F3:
                    if (this.currentSelection == 0)
                    {
                        this.inputValues[this.currentSelection] = YearGroupSelectionScreen.ShowAvailableYearGroups(this.availableYearGroups);

                        this.selectedYearGroup = StaticQueries.GetYearGroupFromList(this.inputValues[0], this.availableYearGroups);
                    }
                    else if (this.currentSelection == 1)
                    {
                        this.inputValues[this.currentSelection] = CourseSelectionScreen.ShowAvailableCourses(this.availableCourses);

                        this.selectedCourse = StaticQueries.GetCourseFromList(this.inputValues[1], this.availableCourses);
                    }

                    break;
                case ConsoleKey.Escape:
                    this.canceled = true;
                    break;
            }
        }

        /// <summary>
        /// Shows the help to this screen.
        /// </summary>
        private void ShowHelp()
        {
            Console.Clear();
            Console.WriteLine("\n [Enter] Close\n");
            Console.WriteLine(" - Help\n");
            Console.WriteLine("     To analyze students, fill in all input fields.");
            Console.WriteLine("     The program shows information to the evaluation (description, date, grade) ");
            Console.WriteLine("     and the student (full name and matriculation number). ");
            Console.WriteLine("     To switch between the fields, just press Up or Down, Enter or Tab.\n");
            Console.WriteLine("     Consider following rules:\n");
            Console.WriteLine("       - Year group and course have to be selected from a list (F3)\n");
            Console.ReadLine();
        }

        /// <summary>
        /// Displays the students, who belongs to a certain course and year group.
        /// This method shows information to the student and description, date and grade of all relative evaluations.
        /// </summary>
        private void AnalyseStudents()
        {
            double averageGrade = 0;
            List<Evaluation> filteredEvaluations;

            // Calculate the average grade for a course.
            for (int i = 0; i < this.availableEvaluations.Count; i++)
            {
                averageGrade += this.availableEvaluations[i].ExamGrade;
            }

            Console.Clear();
            Console.WriteLine("\n [Enter] Close\n");
            Console.WriteLine("Course    : {0} - {1}", this.selectedCourse.Abbreviation, this.selectedCourse.Description);
            Console.WriteLine("Year group: {0} - {1}", this.selectedYearGroup.Description, this.selectedYearGroup.Year);            
            Console.WriteLine("Avg. grade: {0}", averageGrade / (double)this.availableEvaluations.Count);

            Console.Write("--------------------------------------------------------------------------------");
            Console.Write("Student - ID  Name                    Exam description         Exam date   Grade");
            Console.Write("--------------------------------------------------------------------------------");
            
            // Enumerate all students from a referent and show their relative evaluations.
            for (int i = 0; i < this.loggedInReferent.Students.Count; i++)
            {
                // Filter evaluation
                filteredEvaluations = StaticQueries.FilterEvaluationList(this.selectedYearGroup, this.selectedCourse, this.loggedInReferent.Students[i], this.availableEvaluations);

                if (filteredEvaluations.Count > 0)
                {
                    averageGrade = 0;

                    for (int j = 0; j < filteredEvaluations.Count; j++)
                    {
                        // Display a row
                        Console.Write("{0}", filteredEvaluations[j].Student.MatriculationNumber);

                        if (filteredEvaluations[j].Student.GetFullName().Length > 22)
                        {
                            Console.Write("    {0}", filteredEvaluations[j].Student.GetFullName().Substring(0, 22));
                        }
                        else
                        {
                            Console.Write("    {0}", filteredEvaluations[j].Student.GetFullName());
                        }

                        Console.SetCursorPosition(36, Console.CursorTop);

                        Console.Write("  {0}", filteredEvaluations[j].ExamDescription);

                        Console.SetCursorPosition(61, Console.CursorTop);

                        Console.Write("  {0}      {1}", filteredEvaluations[j].ExamDate, filteredEvaluations[j].ExamGrade);

                        averageGrade += filteredEvaluations[j].ExamGrade;
                    }

                    // Calculate the average grade for a student.
                    averageGrade = Math.Round(averageGrade / filteredEvaluations.Count, 2);

                    Console.Write("                                                                            ----");
                    Console.WriteLine("                                                                Avg. grade: {0, 4}", averageGrade);
                }
            }

            Console.ReadLine();
        }
    }
}