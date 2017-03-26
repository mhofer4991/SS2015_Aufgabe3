//-----------------------------------------------------------------------
// <copyright file="RegistrationScreen.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>This class represents a screen, where the user can create new referents.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// This class represents a screen, where the user can create new referents.
    /// </summary>
    public class RegistrationScreen
    {
        /// <summary>
        /// The last index of the input fields.
        /// </summary>
        private const int MaxSelection = 5;

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
        /// New referent, which will be returned after successful user input.
        /// </summary>
        private Referent newRegisteredReferent;

        /// <summary>
        /// List of existing referents to check and avoid duplicate IDs.
        /// </summary>
        private List<Referent> referents;

        /// <summary>
        /// As long as this boolean is false, the screen will be displayed.
        /// </summary>
        private bool savePressed;

        /// <summary>
        /// If this boolean is true, the user created a referent which already exists.
        /// </summary>
        private bool sameReferent;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationScreen"/> class.
        /// </summary>
        /// <param name="referents">List of referents, which already exist.</param>
        public RegistrationScreen(List<Referent> referents)
        {
            this.referents = referents;

            this.firstSelectionPosition = new int[] { 5, 5 };

            this.errorMessages = new string[RegistrationScreen.MaxSelection + 1];
            this.inputValues = new string[RegistrationScreen.MaxSelection + 1];

            this.Reset();

            this.inputHandler = new InputHandler();

            // If the user presses one of the subscribed keys, an event will be called.
            this.inputHandler.SubscribeForKey(ConsoleKey.UpArrow);
            this.inputHandler.SubscribeForKey(ConsoleKey.DownArrow);
            this.inputHandler.SubscribeForKey(ConsoleKey.Enter);
            this.inputHandler.SubscribeForKey(ConsoleKey.Tab);
            this.inputHandler.SubscribeForKey(ConsoleKey.F1);
            this.inputHandler.SubscribeForKey(ConsoleKey.F2);
            this.inputHandler.SubscribeForKey(ConsoleKey.Escape);

            this.inputHandler.OnSubscribedKeyCalled += this.InputHandler_OnSubscribedKeyCalled;
        }

        /// <summary>
        /// Draws the screen on the console.
        /// </summary>
        public void Draw()
        {
            Console.WriteLine("\n [ESC] Close  [F1] Help  [F2] Save  [Up / Down] Navigate");
            Console.WriteLine("\n - Registration\n");
            Console.WriteLine("    [ ] ID - number: {0}\n", this.inputValues[0]);
            Console.WriteLine("    [ ] First name:  {0}\n", this.inputValues[1]);
            Console.WriteLine("    [ ] Last name:   {0}\n", this.inputValues[2]);
            Console.WriteLine("    [ ] Password:    {0}\n", this.inputValues[3]);
            Console.WriteLine("    [ ] E - Mail:    {0}\n", this.inputValues[4]);
            Console.WriteLine("    [ ] Phone:       {0}\n", this.inputValues[5]);

            for (int i = 0; i < this.errorMessages.Length; i++)
            {
                if (!this.errorMessages[i].Equals(string.Empty))
                {
                    Console.WriteLine("    ERROR: {0}", this.errorMessages[i]);
                }
            }

            if (this.sameReferent)
            {
                Console.WriteLine("    ERROR: A referent with this ID already exists!");
            }

            Console.SetCursorPosition(this.firstSelectionPosition[0], this.firstSelectionPosition[1] + (this.currentSelection * 2));
            Console.Write('*');
        }

        /// <summary>
        /// Waits for the user to complete all input fields and returns a new referent.
        /// </summary>
        /// <param name="suggestedID">A default referent - ID.</param>
        /// <returns>A new instance of the referent.</returns>
        public Referent GetNewReferentFromInput(string suggestedID)
        {
            this.newRegisteredReferent = new Referent();

            this.inputValues[0] = suggestedID;

            while (!this.savePressed)
            {
                this.HandleUserInput();
                Console.Clear();
                this.Draw();
            }

            this.Reset();

            return this.newRegisteredReferent;
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
            this.sameReferent = false;
        }

        /// <summary>
        /// Handles the user input to all input fields.
        /// </summary>
        public void HandleUserInput()
        {
            // Move the cursor to the input field.
            Console.SetCursorPosition(this.firstSelectionPosition[0] + 16, this.firstSelectionPosition[1] + (this.currentSelection * 2));

            this.errorMessages[this.currentSelection] = string.Empty;

            // Depending on the input field, the handler reads a certain amount of letters.
            switch (this.currentSelection)
            {
                case 0:
                    this.inputHandler.ReadChars(ref this.inputValues[0], 5);

                    break;
                case 1:
                    this.inputHandler.ReadChars(ref this.inputValues[1], 20);

                    break;
                case 2:
                    this.inputHandler.ReadChars(ref this.inputValues[2], 20);

                    break;
                case 3:
                    this.inputHandler.ReadChars(ref this.inputValues[3], 20);

                    break;
                case 4:
                    this.inputHandler.ReadChars(ref this.inputValues[4], 20);

                    break;
                case 5:
                    this.inputHandler.ReadChars(ref this.inputValues[5], 20);

                    break;
            }

            // Try to apply the input values to the new referent.
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
                    // The user wants to move the selection cursor to the bottom.
                    if (this.currentSelection < RegistrationScreen.MaxSelection)
                    {
                        this.currentSelection++;
                    }
                    else
                    {
                        this.currentSelection = 0;
                    }

                    break;
                case ConsoleKey.UpArrow:
                    // The user wants to move the selection cursor to the top.
                    if (this.currentSelection > 0)
                    {
                        this.currentSelection--;
                    }
                    else
                    {
                        this.currentSelection = RegistrationScreen.MaxSelection;
                    }

                    break;
                case ConsoleKey.F1:
                    this.ShowHelp();

                    break;
                case ConsoleKey.F2:
                    // The user wants to save the new referent.
                    if (this.ApplySettings())
                    {
                        // Duplicate IDs are not possible.
                        if (!this.referents.Contains(this.newRegisteredReferent))
                        {
                            this.savePressed = true;
                            this.sameReferent = false;
                        }
                        else
                        {
                            this.sameReferent = true;   
                        }
                    }

                    break;
                case ConsoleKey.Escape:
                    // The user wants to leave the screen without saving.
                    this.savePressed = true;
                    this.newRegisteredReferent = null;
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
            Console.WriteLine("     To register and therefore create a new referent, fill in all input fields.");
            Console.WriteLine("     To switch between the fields, just press Up or Down, Enter or Tab.\n");
            Console.WriteLine("     Consider following rules:\n");
            Console.WriteLine("       - ID: Number, which must be between 10000 and 99999.\n");
            Console.WriteLine("       - First name: Must contain at least 2 characters.\n");
            Console.WriteLine("       - Last name: Must contain at least 2 characters.\n");
            Console.WriteLine("       - Password: Must contain at least 4 characters.\n");
            Console.WriteLine("       - E-Mail: Must have the format name@provider.com.\n");
            Console.WriteLine("       - Phone: Must contain only digits.\n");
            Console.ReadLine();
        }

        /// <summary>
        /// Applies the filled input fields to the new referent.
        /// </summary>
        /// <returns>A boolean, indicating whether the input values are valid or not.</returns>
        private bool ApplySettings()
        {
            if (this.newRegisteredReferent != null)
            {
                int index = 0;

                try
                {
                    this.newRegisteredReferent.SetID(this.inputValues[index]);
                    this.newRegisteredReferent.SetFirstName(this.inputValues[index = 1]);
                    this.newRegisteredReferent.SetLastName(this.inputValues[index = 2]);
                    this.newRegisteredReferent.SetPassword(this.inputValues[index = 3]);
                    this.newRegisteredReferent.SetEmail(this.inputValues[index = 4]);
                    this.newRegisteredReferent.SetPhone(this.inputValues[index = 5]);

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
