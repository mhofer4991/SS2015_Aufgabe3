//-----------------------------------------------------------------------
// <copyright file="StudentsCertificateScreen.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a screen, where the user can view certificates from students.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a screen, where the user can view certificates from students.
    /// </summary>
    public class StudentsCertificateScreen
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
        /// List of all available students.
        /// </summary>
        private List<Student> availableStudents;

        /// <summary>
        /// List of all available evaluations.
        /// </summary>
        private List<Evaluation> availableEvaluations;

        /// <summary>
        /// The currently selected year group.
        /// </summary>
        private YearGroup selectedYearGroup;

        /// <summary>
        /// The currently selected student.
        /// </summary>
        private Student selectedStudent;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentsCertificateScreen"/> class.
        /// </summary>
        public StudentsCertificateScreen()
        {
            this.firstSelectionPosition = new int[] { 5, 5 };

            this.inputValues = new string[StudentsCertificateScreen.MaxSelection + 1];
            
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
                Console.WriteLine("\n [ESC] Close  [F1] Help  [F2] Analyse  [F3] Choose student [Up / Down] Navigate");
            }

            Console.WriteLine("\n - Show certificate from students\n");
            Console.WriteLine("    [ ] Year group: {0}\n", this.inputValues[0]);
            Console.WriteLine("    [ ] Student:    {0}\n", this.inputValues[1]);

            if (this.invalidInput)
            {
                Console.WriteLine("    ERROR: The year group or the student could not be found!");
            }

            Console.SetCursorPosition(this.firstSelectionPosition[0], this.firstSelectionPosition[1] + (this.currentSelection * 2));
            Console.Write('*');
        }

        /// <summary>
        /// Waits for the user to complete all input fields and then renders the certificate of the selected referent for a certain year group.
        /// </summary>
        /// <param name="loggedInReferent">A referent, who is logged in.</param>
        public void ShowCertificateScreen(Referent loggedInReferent)
        {
            this.loggedInReferent = loggedInReferent;

            this.availableYearGroups = this.loggedInReferent.YearGroups;
            this.availableStudents = this.loggedInReferent.Students;
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
                    // Waits for the user to press F3 so it can open a sub menu where all available students from a year group are displayed.
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
                    if (this.currentSelection < StudentsCertificateScreen.MaxSelection)
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
                        this.currentSelection = StudentsCertificateScreen.MaxSelection;
                    }

                    break;
                case ConsoleKey.F1:
                    this.ShowHelp();
                    break;
                case ConsoleKey.F2:
                    // Before the students can be evaluated, check if all input fields are correct.
                    if (this.selectedStudent != null && this.selectedYearGroup != null)
                    {
                        this.ShowCertificate();
                        this.invalidInput = false;
                    }
                    else
                    {
                        this.invalidInput = true;
                    }

                    break;
                case ConsoleKey.F3:
                    // Show a sub menu depending on the current selection.
                    if (this.currentSelection == 0)
                    {
                        this.inputValues[this.currentSelection] = YearGroupSelectionScreen.ShowAvailableYearGroups(this.availableYearGroups);

                        this.selectedYearGroup = StaticQueries.GetYearGroupFromList(this.inputValues[0], this.availableYearGroups);

                        // Reset the student, because it can't be guaranteed that it still matches with the new year group.
                        this.inputValues[1] = string.Empty;
                    }
                    else if (this.currentSelection == 1)
                    {
                        List<Student> tempAvailableStudents = new List<Student>();

                        // If the user selected a year group, the students can be filtered by it.
                        if (this.selectedYearGroup != null)
                        {
                            tempAvailableStudents = StaticQueries.FilterStudentsByYearGroup(this.selectedYearGroup, this.availableStudents);
                        }

                        this.inputValues[this.currentSelection] = StudentsSelectionScreen.ShowAvailableStudents(tempAvailableStudents);

                        this.selectedStudent = StaticQueries.GetStudentFromList(this.inputValues[1], this.availableStudents);
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
            Console.WriteLine("     To view certificates of students, fill in all input fields.");
            Console.WriteLine("     The program lists all visited courses (abbreviation, description)");
            Console.WriteLine("     and their average grade.");
            Console.WriteLine("     To switch between the fields, just press Up or Down, Enter or Tab.\n");
            Console.WriteLine("     Consider following rules:\n");
            Console.WriteLine("       - Year group and student have to be selected from a list (F3)\n");
            Console.ReadLine();
        }

        /// <summary>
        /// Displays all courses and the average grade, which belong to the student for a certain group year.
        /// </summary>
        private void ShowCertificate()
        {
            List<Evaluation> filteredEvaluations;
            double averageGrade;

            Console.Clear();
            Console.WriteLine("\n [Enter] Close\n");
            Console.WriteLine("Student - ID: {0}", this.selectedStudent.MatriculationNumber);
            Console.WriteLine("Name        : {0} {1}", this.selectedStudent.FirstName, this.selectedStudent.LastName);
            Console.WriteLine("Year group  : {0} - {1}", this.selectedYearGroup.Description, this.selectedYearGroup.Year);

            Console.WriteLine("-------------------------------------");
            Console.WriteLine("Course    Description           Grade");
            Console.WriteLine("-------------------------------------");

            // Enumerate all courses from a referent, which are used for filtering the evaluations.
            for (int i = 0; i < this.loggedInReferent.Courses.Count; i++)
            {
                // Filter evaluation
                filteredEvaluations = StaticQueries.FilterEvaluationList(this.selectedYearGroup, this.loggedInReferent.Courses[i], this.selectedStudent, this.loggedInReferent.Evaluations);

                if (filteredEvaluations.Count > 0)
                {
                    averageGrade = 0;

                    for (int j = 0; j < filteredEvaluations.Count; j++)
                    {
                        averageGrade += filteredEvaluations[j].ExamGrade;
                    }

                    // Calculate the average grade for a course.
                    averageGrade = Math.Round(averageGrade / filteredEvaluations.Count, 2);

                    // Display a row
                    Console.Write("{0, -10}", filteredEvaluations[0].Course.Abbreviation);
                    Console.Write("{0, -22}", filteredEvaluations[0].Course.Description);
                    Console.WriteLine(" {0, 4}", averageGrade);
                }
            }

            Console.ReadLine();
        }
    }
}