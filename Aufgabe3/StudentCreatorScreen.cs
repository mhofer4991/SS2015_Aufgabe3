//-----------------------------------------------------------------------
// <copyright file="StudentCreatorScreen.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a screen, where the user can create new students.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a screen, where the user can create new students.
    /// </summary>
    public class StudentCreatorScreen
    {
        /// <summary>
        /// The last index of the input fields.
        /// </summary>
        private const int MaxSelection = 3;

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
        /// New student, which will be returned after successful user input.
        /// </summary>
        private Student newStudent;

        /// <summary>
        /// Referent, who is the creator of the student.
        /// </summary>
        private Referent creator;

        /// <summary>
        /// If this boolean is true, the user created a student who already exists.
        /// </summary>
        private bool sameStudent;

        /// <summary>
        /// As long as this boolean is false, the screen waits for user input.
        /// </summary>
        private bool savePressed;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentCreatorScreen"/> class.
        /// </summary>
        public StudentCreatorScreen()
        {
            this.firstSelectionPosition = new int[] { 5, 5 };

            this.errorMessages = new string[StudentCreatorScreen.MaxSelection + 1];
            this.inputValues = new string[StudentCreatorScreen.MaxSelection + 1];

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
            if (this.currentSelection == 3)
            {
                Console.WriteLine("\n [ESC] Close  [F1] Help  [F2] Save  [F3] Choose year  [Up / Down] Navigate");
            }
            else
            {
                Console.WriteLine("\n [ESC] Close  [F1] Help  [F2] Save  [Up / Down] Navigate");
            }

            Console.WriteLine("\n - Create new student\n");
            Console.WriteLine("    [ ] Matriculation number: {0}\n", this.inputValues[0]);
            Console.WriteLine("    [ ] First name:           {0}\n", this.inputValues[1]);
            Console.WriteLine("    [ ] Last name:            {0}\n", this.inputValues[2]);
            Console.WriteLine("    [ ] Year group:           {0}\n", this.inputValues[3]);

            for (int i = 0; i < this.errorMessages.Length; i++)
            {
                if (!this.errorMessages[i].Equals(string.Empty))
                {
                    Console.WriteLine("    ERROR: {0}", this.errorMessages[i]);

                    if (i == 3)
                    {
                        Console.WriteLine("           Press F3 to view all available year groups!");
                    }
                }
            }

            if (this.sameStudent)
            {
                Console.WriteLine("    ERROR: This referent already created a student with the same\n           matriculation number!");
            }

            Console.SetCursorPosition(this.firstSelectionPosition[0], this.firstSelectionPosition[1] + (this.currentSelection * 2));
            Console.Write('*');
        }

        /// <summary>
        /// Waits for the user to complete all input fields and returns a new student.
        /// </summary>
        /// <param name="creator">A referent, who is logged in.</param>
        /// <returns>A new student created by the referent.</returns>
        public Student GetNewStudentFromInput(Referent creator)
        {
            this.newStudent = new Student();

            this.creator = creator;

            while (!this.savePressed)
            {
                this.HandleUserInput();
                Console.Clear();
                this.Draw();
            }

            if (this.newStudent != null)
            {
                this.creator.AddStudent(this.newStudent);
            }

            this.Reset();

            return this.newStudent;
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
            this.sameStudent = false;
        }

        /// <summary>
        /// Handles the user input to all input fields.
        /// </summary>
        public void HandleUserInput()
        {
            Console.SetCursorPosition(this.firstSelectionPosition[0] + 25, this.firstSelectionPosition[1] + (this.currentSelection * 2));

            this.errorMessages[this.currentSelection] = string.Empty;

            switch (this.currentSelection)
            {
                case 0:
                    this.inputHandler.ReadChars(ref this.inputValues[0], 10);

                    break;
                case 1:
                    this.inputHandler.ReadChars(ref this.inputValues[1], 20);

                    break;
                case 2:
                    this.inputHandler.ReadChars(ref this.inputValues[2], 20);

                    break;
                case 3:
                    // Waits for the user to press F3 so it can open a sub menu where all available year groups are displayed.
                    this.inputHandler.WaitForSubscribedKey();

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
                    if (this.currentSelection < StudentCreatorScreen.MaxSelection)
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
                        this.currentSelection = StudentCreatorScreen.MaxSelection;
                    }

                    break;
                case ConsoleKey.F1:
                    this.ShowHelp();
                    break;
                case ConsoleKey.F2:
                    if (this.ApplySettings())
                    {
                        if (!this.creator.Students.Contains(this.newStudent))
                        {
                            this.savePressed = true;
                            this.sameStudent = false;
                        }
                        else
                        {
                            this.sameStudent = true;
                        }
                    }

                    break;
                case ConsoleKey.F3:
                    if (this.currentSelection == 3)
                    {
                        // The user sets the input field by choosing from a range of available year groups.
                        this.inputValues[3] = YearGroupSelectionScreen.ShowAvailableYearGroups(this.creator.YearGroups);
                    }

                    break;
                case ConsoleKey.Escape:
                    this.savePressed = true;
                    this.newStudent = null;
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
            Console.WriteLine("     To create a new student, fill in all input fields.");
            Console.WriteLine("     Make sure the entered student doesn't exist yet!");
            Console.WriteLine("     To switch between the fields, just press Up or Down, Enter or Tab.\n");
            Console.WriteLine("     Consider following rules:\n");
            Console.WriteLine("       - Matriculation number (UNIQUE!): Must contain 10 digits.\n");
            Console.WriteLine("       - First name: Must contain at least 2 characters.\n");
            Console.WriteLine("       - Last name: Must contain at least 2 characters.\n");
            Console.WriteLine("       - Year group: You have to choose from a list of year groups. (F3)\n");
            Console.ReadLine();
        }

        /// <summary>
        /// Applies the filled input fields to the new student.
        /// </summary>
        /// <returns>A boolean, indicating whether the input values are valid or not.</returns>
        private bool ApplySettings()
        {
            if (this.newStudent != null)
            {
                int index = 0;

                try
                {
                    this.newStudent.SetMatriculationNumber(this.inputValues[index]);
                    this.newStudent.SetFirstName(this.inputValues[index = 1]);
                    this.newStudent.SetLastName(this.inputValues[index = 2]);
                    this.newStudent.SetYear(this.inputValues[index = 3], this.creator.YearGroups);

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