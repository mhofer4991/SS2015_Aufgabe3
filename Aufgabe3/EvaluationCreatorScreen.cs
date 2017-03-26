//-----------------------------------------------------------------------
// <copyright file="EvaluationCreatorScreen.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a screen, where the user can create new evaluations.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a screen, where the user can create new evaluations.
    /// </summary>
    public class EvaluationCreatorScreen
    {
        /// <summary>
        /// The last index of the input fields.
        /// </summary>
        private const int MaxSelection = 6;

        /// <summary>
        /// The position of the first selected input field.
        /// </summary>
        private int[] firstSelectionPosition;

        /// <summary>
        /// An array of error messages for each input field.
        /// </summary>
        private string[] errorMessages;

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
        /// New evaluation, which will be returned after successful user input.
        /// </summary>
        private Evaluation newEvaluation;

        /// <summary>
        /// Referent, who is the creator of the evaluation.
        /// </summary>
        private Referent creator;

        /// <summary>
        /// As long as this boolean is false, the screen waits for user input.
        /// </summary>
        private bool savePressed;

        /// <summary>
        /// List of all available referents.
        /// </summary>
        private List<Referent> availableReferents;

        /// <summary>
        /// List of all available year groups.
        /// </summary>
        private List<YearGroup> availableYearGroups;

        /// <summary>
        /// List of all available students.
        /// </summary>
        private List<Student> availableStudents;

        /// <summary>
        /// List of all available courses.
        /// </summary>
        private List<Course> availableCourses;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationCreatorScreen"/> class.
        /// </summary>
        /// <param name="availableReferents">The list of all available referents.</param>
        public EvaluationCreatorScreen(List<Referent> availableReferents)
        {
            this.firstSelectionPosition = new int[] { 5, 5 };

            this.errorMessages = new string[EvaluationCreatorScreen.MaxSelection + 1];
            this.inputValues = new string[EvaluationCreatorScreen.MaxSelection + 1];

            this.availableReferents = availableReferents;
            
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
            if (this.currentSelection == 1)
            {
                Console.WriteLine("\n [ESC] Close  [F1] Help  [F2] Save  [F3] Choose year  [Up / Down] Navigate");
            }
            else if (this.currentSelection == 2)
            {
                Console.WriteLine("\n [ESC] Close  [F1] Help  [F2] Save  [F3] Choose student  [Up / Down] Navigate");
            }
            else if (this.currentSelection == 3)
            {
                Console.WriteLine("\n [ESC] Close  [F1] Help  [F2] Save  [F3] Choose course  [Up / Down] Navigate");
            }
            else
            {
                Console.WriteLine("\n [ESC] Close  [F1] Help  [F2] Save  [Up / Down] Navigate");
            }

            Console.WriteLine("\n - Create new evaluation\n");
            Console.WriteLine("    [ ] Referent - ID:    {0}\n", this.inputValues[0]);
            Console.WriteLine("    [ ] Year group:       {0}\n", this.inputValues[1]);
            Console.WriteLine("    [ ] Student:          {0}\n", this.inputValues[2]);
            Console.WriteLine("    [ ] Course:           {0}\n", this.inputValues[3]);
            Console.WriteLine("    [ ] Exam description: {0}\n", this.inputValues[4]);
            Console.WriteLine("    [ ] Exam date:        {0}\n", this.inputValues[5]);
            Console.WriteLine("    [ ] Exam grade:       {0}\n", this.inputValues[6]);

            for (int i = 0; i < this.errorMessages.Length; i++)
            {
                if (!this.errorMessages[i].Equals(string.Empty))
                {
                    Console.WriteLine("    ERROR: {0}", this.errorMessages[i]);
                }
            }

            Console.SetCursorPosition(this.firstSelectionPosition[0], this.firstSelectionPosition[1] + (this.currentSelection * 2));
            Console.Write('*');
        }

        /// <summary>
        /// Waits for the user to complete all input fields and returns a new evaluation.
        /// </summary>
        /// <param name="creator">A referent, who is logged in.</param>
        /// <returns>A new student created by the referent.</returns>
        public Evaluation GetNewEvaluationFromInput(Referent creator)
        {
            this.newEvaluation = new Evaluation();

            this.creator = creator;

            this.availableStudents = this.creator.Students;
            this.availableYearGroups = this.creator.YearGroups;
            this.availableCourses = this.creator.Courses;

            this.inputValues[0] = this.creator.ID;

            while (!this.savePressed)
            {
                this.HandleUserInput();
                Console.Clear();
                this.Draw();
            }

            if (this.newEvaluation != null)
            {
                this.creator.AddEvaluation(this.newEvaluation);
            }

            this.Reset();

            return this.newEvaluation;
        }

        /// <summary>
        /// Resets the selection, all input fields and error messages.
        /// </summary>
        public void Reset()
        {
            this.currentSelection = 0;

            for (int i = 0; i < this.inputValues.Length; i++)
            {
                this.inputValues[i] = this.errorMessages[i] = string.Empty;
            }

            this.savePressed = false;
        }

        /// <summary>
        /// Handles the user input to all input fields.
        /// </summary>
        public void HandleUserInput()
        {
            Console.SetCursorPosition(this.firstSelectionPosition[0] + 21, this.firstSelectionPosition[1] + (this.currentSelection * 2));

            this.errorMessages[this.currentSelection] = string.Empty;

            switch (this.currentSelection)
            {
                case 0:
                    // The user cannot change this value.
                    Console.Write(this.inputValues[0]);
                    this.inputHandler.WaitForSubscribedKey();

                    break;
                case 1:
                    // Waits for the user to press F3 so it can open a sub menu where all available year groups are displayed.
                    this.inputHandler.WaitForSubscribedKey();

                    break;
                case 2:
                    // Waits for the user to press F3 so it can open a sub menu where all available students from a year group are displayed.
                    this.inputHandler.WaitForSubscribedKey();

                    break;
                case 3:
                    // Waits for the user to press F3 so it can open a sub menu where all available courses are displayed.
                    this.inputHandler.WaitForSubscribedKey();

                    break;
                case 4:
                    this.inputHandler.ReadChars(ref this.inputValues[4], 20);

                    break;
                case 5:
                    this.inputHandler.ReadChars(ref this.inputValues[5], 10);

                    break;
                case 6:
                    this.inputHandler.ReadChars(ref this.inputValues[6], 1);

                    break;
            }

            this.ApplySettings();
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
                    if (this.currentSelection < EvaluationCreatorScreen.MaxSelection)
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
                        this.currentSelection = EvaluationCreatorScreen.MaxSelection;
                    }

                    break;
                case ConsoleKey.F1:
                    this.ShowHelp();
                    break;
                case ConsoleKey.F2:
                    if (this.ApplySettings())
                    {
                        this.savePressed = true;
                    }

                    break;
                case ConsoleKey.F3:
                    // Show a sub menu depending on the current selection.
                    if (this.currentSelection == 1)
                    {
                        this.inputValues[this.currentSelection] = YearGroupSelectionScreen.ShowAvailableYearGroups(this.availableYearGroups);

                        // Reset the student, because it can't be guaranteed that it still matches with the new year group.
                        this.inputValues[2] = string.Empty;
                    }
                    else if (this.currentSelection == 2)
                    {
                        List<Student> tempAvailableStudents = new List<Student>();

                        // If the user selected a year group, the students can be filtered by it.
                        if (this.newEvaluation.YearGroup != null)
                        {
                            tempAvailableStudents = StaticQueries.FilterStudentsByYearGroup(this.newEvaluation.YearGroup, this.availableStudents);
                        }

                        this.inputValues[this.currentSelection] = StudentsSelectionScreen.ShowAvailableStudents(tempAvailableStudents);
                    }
                    else if (this.currentSelection == 3)
                    {
                        this.inputValues[this.currentSelection] = CourseSelectionScreen.ShowAvailableCourses(this.availableCourses);
                    }

                    break;
                case ConsoleKey.Escape:
                    this.savePressed = true;
                    this.newEvaluation = null;
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
            Console.WriteLine("     To create a new evaluation, fill in all input fields.");
            Console.WriteLine("     To switch between the fields, just press Up or Down, Enter or Tab.\n");
            Console.WriteLine("     Consider following rules:\n");
            Console.WriteLine("       - Year group, student and course have to be selected from a list (F3)\n");
            Console.WriteLine("       - Description: Must contain at least 2 characters.\n");
            Console.WriteLine("       - Date: Must have the format DD.MM.YYYY.\n");
            Console.WriteLine("       - Grade: Range from 1 to 5.\n");
            Console.ReadLine();
        }

        /// <summary>
        /// Applies the filled input fields to the new evaluation.
        /// </summary>
        /// <returns>A boolean, indicating whether the input values are valid or not.</returns>
        private bool ApplySettings()
        {
            if (this.newEvaluation != null)
            {
                int index = 0;

                try
                {
                    // Check if the entered referent ID exists.
                    if (StaticQueries.GetReferentFromList(this.inputValues[0], this.availableReferents) != null)
                    {
                        this.newEvaluation.SetReferent(StaticQueries.GetReferentFromList(this.inputValues[0], this.availableReferents));
                    }
                    else
                    {
                        this.errorMessages[0] = "A referent with this ID could not be found!";
                    }

                    // Check if the entered year group exists.
                    if (StaticQueries.GetYearGroupFromList(this.inputValues[1], this.availableYearGroups) != null)
                    {
                        this.newEvaluation.SetYearGroup(StaticQueries.GetYearGroupFromList(this.inputValues[1], this.availableYearGroups));
                    }
                    else
                    {
                        this.errorMessages[1] = "This year group could not be found!";
                    }

                    // Check if the entered student exists.
                    if (StaticQueries.GetStudentFromList(this.inputValues[2], this.availableStudents) != null)
                    {
                        this.newEvaluation.SetStudent(StaticQueries.GetStudentFromList(this.inputValues[2], this.availableStudents));
                    }
                    else
                    {
                        this.errorMessages[2] = "This student could not be found!";
                    }

                    // Check if the entered course exists.
                    if (StaticQueries.GetCourseFromList(this.inputValues[3], this.availableCourses) != null)
                    {
                        this.newEvaluation.SetCourse(StaticQueries.GetCourseFromList(this.inputValues[3], this.availableCourses));
                    }
                    else
                    {
                        this.errorMessages[3] = "This course could not be found!";
                    }

                    this.newEvaluation.SetExamDescription(this.inputValues[index = 4]);
                    this.newEvaluation.SetExamDate(this.inputValues[index = 5]);
                    this.newEvaluation.SetExamGrade(this.inputValues[index = 6]);

                    return true;
                }
                catch (ArgumentException ex)
                {
                    this.errorMessages[index] = ex.Message;

                    return false;
                }
            }

            return false;
        }
    }
}