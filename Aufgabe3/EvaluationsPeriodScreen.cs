//-----------------------------------------------------------------------
// <copyright file="EvaluationsPeriodScreen.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a screen, where the user can view all evaluations from a specific period.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a screen, where the user can view all evaluations from a specific period.
    /// </summary>
    public class EvaluationsPeriodScreen
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
        /// If this string is not empty, an error occurred.
        /// </summary>
        private string errorMessage;

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
        /// List of all available evaluations.
        /// </summary>
        private List<Evaluation> availableEvaluations;

        /// <summary>
        /// Earliest date for a list of evaluations.
        /// </summary>
        private DateTime minimalDate;

        /// <summary>
        /// Latest date for a list of evaluations.
        /// </summary>
        private DateTime maximumDate;

        /// <summary>
        /// Initializes a new instance of the <see cref="EvaluationsPeriodScreen"/> class.
        /// </summary>
        public EvaluationsPeriodScreen()
        {
            this.firstSelectionPosition = new int[] { 5, 5 };

            this.inputValues = new string[EvaluationsPeriodScreen.MaxSelection + 1];
            
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
            Console.WriteLine("\n [ESC] Close  [F1] Help  [F2] Analyse  [Up / Down] Navigate");
            Console.WriteLine("\n - Show evaluations per period\n");
            Console.WriteLine("    [ ] First date: {0}\n", this.inputValues[0]);
            Console.WriteLine("    [ ] Last date:  {0}\n", this.inputValues[1]);

            if (!this.errorMessage.Equals(string.Empty))
            {
                Console.WriteLine("    ERROR: {0}", this.errorMessage);
            }

            Console.SetCursorPosition(this.firstSelectionPosition[0], this.firstSelectionPosition[1] + (this.currentSelection * 2));
            Console.Write('*');
        }

        /// <summary>
        /// Waits for the user to complete all input fields and then renders a list of all evaluations within a minimal and maximal date.
        /// </summary>
        /// <param name="loggedInReferent">A referent, who is logged in.</param>
        public void ShowEvaluationsPerPeriodScreen(Referent loggedInReferent)
        {
            this.loggedInReferent = loggedInReferent;

            this.availableEvaluations = this.loggedInReferent.Evaluations;

            this.minimalDate = StaticQueries.GetFirstDate(this.availableEvaluations);
            this.maximumDate = StaticQueries.GetLastDate(this.availableEvaluations);

            this.inputValues[0] = this.minimalDate.ToString("dd.MM.yyyy");
            this.inputValues[1] = this.maximumDate.ToString("dd.MM.yyyy");

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

            for (int i = 0; i < this.inputValues.Length; i++)
            {
                this.inputValues[i] = string.Empty;
            }

            this.canceled = false;
            this.errorMessage = string.Empty;
        }

        /// <summary>
        /// Handles the user input to all input fields.
        /// </summary>
        public void HandleUserInput()
        {
            Console.SetCursorPosition(this.firstSelectionPosition[0] + 15, this.firstSelectionPosition[1] + (this.currentSelection * 2));

            // Waits for the user to enter a valid date.
            switch (this.currentSelection)
            {
                case 0:
                    this.inputHandler.ReadChars(ref this.inputValues[0], 10);

                    break;
                case 1:
                    this.inputHandler.ReadChars(ref this.inputValues[1], 10);

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
                    if (this.currentSelection < EvaluationsPeriodScreen.MaxSelection)
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
                        this.currentSelection = EvaluationsPeriodScreen.MaxSelection;
                    }

                    break;
                case ConsoleKey.F1:
                    this.ShowHelp();
                    break;
                case ConsoleKey.F2:
                    // Before the user can view the evaluations, the input values must be checked.
                    if (!Evaluation.IsValidDateFormat(this.inputValues[0]) || !Evaluation.IsValidDateFormat(this.inputValues[1]))
                    {
                        this.errorMessage = "The date must have the format DD.MM.YYYY!";
                    }
                    else if (DateTime.Parse(this.inputValues[0]) < minimalDate || DateTime.Parse(this.inputValues[1]) > maximumDate)
                    {
                        this.errorMessage = "The date must be between " + minimalDate.ToString("dd.MM.yyyy") + " and " + maximumDate.ToString("dd.MM.yyyy") + "!";
                    }
                    else if (DateTime.Parse(this.inputValues[0]) > DateTime.Parse(this.inputValues[1]))
                    {
                        this.errorMessage = "The first date cannot be later than the last date!";
                    }
                    else
                    {
                        // All input fields are valid.
                        this.AnalyseEvaluationsPerPeriod();
                        this.errorMessage = string.Empty;
                    }

                    break;
                case ConsoleKey.F3:

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
            Console.WriteLine("     To view all evaluations within a time period, fill in all input fields.");
            Console.WriteLine("     The program lists all matching evaluations (date, description, grade)");
            Console.WriteLine("     and their matching students (matriculation number, full name).");
            Console.WriteLine("     To switch between the fields, just press Up or Down, Enter or Tab.\n");
            Console.WriteLine("     Consider following rules:\n");
            Console.WriteLine("       - First date: Must have the format DD.MM.YYYY.\n");
            Console.WriteLine("       - Last date: Must have the format DD.MM.YYYY.\n");
            Console.WriteLine("       - The first date cannot be earlier than the earliest possible date.\n");
            Console.WriteLine("       - The last date cannot be later than the latest possible date.\n");
            Console.WriteLine("       - The first date cannot be later than the last date.\n");
            Console.ReadLine();
        }

        /// <summary>
        /// Displays a list of all evaluations within a minimal and maximal date.
        /// </summary>
        private void AnalyseEvaluationsPerPeriod()
        {
            // List of evaluations gets sorted by date and then restricted by the first and last date.
            List<Evaluation> sortedEvaluations = StaticQueries.FilterByFirstAndLastDate(
                DateTime.Parse(this.inputValues[0]), 
                DateTime.Parse(this.inputValues[1]), 
                StaticQueries.SortListByDate(this.availableEvaluations));

            Console.Clear();
            Console.WriteLine("\n [Enter] Close\n");
                        
            Console.Write("--------------------------------------------------------------------------------");
            Console.Write("Exam date     Description           Student - ID  Name                     Grade");
            Console.Write("--------------------------------------------------------------------------------");

            for (int i = 0; i < sortedEvaluations.Count; i++)
            {
                // Display a row
                Console.Write("{0, -14}", sortedEvaluations[i].ExamDate);

                Console.Write("{0, -22}", sortedEvaluations[i].ExamDescription);

                Console.Write("{0}", sortedEvaluations[i].Student.MatriculationNumber);

                if (sortedEvaluations[i].Student.GetFullName().Length > 23)
                {
                    Console.Write("    {0}", sortedEvaluations[i].Student.GetFullName().Substring(0, 23));
                }
                else
                {
                    Console.Write("    {0}", sortedEvaluations[i].Student.GetFullName());
                }

                Console.SetCursorPosition(77, Console.CursorTop);
                Console.Write("  {0}", sortedEvaluations[i].ExamGrade);
            }

            Console.ReadLine();
        }
    }
}