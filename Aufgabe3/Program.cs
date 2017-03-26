//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="Markus Hofer">
//     Copyright (c) Markus Hofer
// </copyright>
// <summary>The user can use this program to view and edit evaluations of students, courses and year groups.</summary>
//-----------------------------------------------------------------------
namespace Aufgabe3
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// The user can use this program to view and edit evaluations of students, courses and year groups.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Constant value for the program state at the begin.
        /// </summary>
        private const int MenuStateDefault = 1;

        /// <summary>
        /// Constant value for the program state, when the user is logged in.
        /// </summary>
        private const int MenuStateLoggedIn = 2;

        /// <summary>
        /// Constant value for the program state, when the user wants to see some evaluation.
        /// </summary>
        private const int MenuStateAnalysis = 3;

        /// <summary>
        /// Screen, which is used for registration.
        /// </summary>
        private static RegistrationScreen registrationScreen;

        /// <summary>
        /// Screen, which is used for logging in.
        /// </summary>
        private static LoginScreen loginScreen;

        /// <summary>
        /// Screen, which is used for creating year groups.
        /// </summary>
        private static YearGroupCreatorScreen yearGroupCreatorScreen;

        /// <summary>
        /// Screen, which is used for creating courses.
        /// </summary>
        private static CourseCreatorScreen courseCreatorScreen;

        /// <summary>
        /// Screen, which is used for creating students.
        /// </summary>
        private static StudentCreatorScreen studentCreatorScreen;

        /// <summary>
        /// Screen, which is used for submitting evaluations.
        /// </summary>
        private static EvaluationCreatorScreen evaluationCreatorScreen;

        /// <summary>
        /// Screen, which is used for analyzing students by year group and course.
        /// </summary>
        private static StudentsAnalysisScreen studentsAnalysisScreen;

        /// <summary>
        /// Screen, which is used for viewing certificates of students.
        /// </summary>
        private static StudentsCertificateScreen studentsCertificateScreen;

        /// <summary>
        /// Screen, which is used for viewing evaluations within a certain time period.
        /// </summary>
        private static EvaluationsPeriodScreen evaluationsPeriodScreen;

        /// <summary>
        /// List of all referents.
        /// </summary>
        private static List<Referent> allReferents;

        /// <summary>
        /// List of all year groups.
        /// </summary>
        private static List<YearGroup> allYearGroups;

        /// <summary>
        /// List of all students.
        /// </summary>
        private static List<Student> allStudents;

        /// <summary>
        /// List of all courses.
        /// </summary>
        private static List<Course> allCourses;

        /// <summary>
        /// List of all evaluations.
        /// </summary>
        private static List<Evaluation> allEvaluations;

        /// <summary>
        /// The referent, who is currently logged in to the system.
        /// </summary>
        private static Referent loggedInReferent;

        /// <summary>
        /// Used for handling user input at start.
        /// </summary>
        private static InputHandler defaultInputHandler;

        /// <summary>
        /// Used for handling user input, when a referent is logged in.
        /// </summary>
        private static InputHandler loggedInInputHandler;

        /// <summary>
        /// Current state of the program.
        /// </summary>
        private static int currentMenuState;

        /// <summary>
        /// This method represents the entry point of the program.
        /// </summary>
        /// <param name="args">Array of command line arguments.</param>
        private static void Main(string[] args)
        {
            allReferents = new List<Referent>();

            allYearGroups = new List<YearGroup>();
            allStudents = new List<Student>();
            allCourses = new List<Course>();
            allEvaluations = new List<Evaluation>();

            registrationScreen = new RegistrationScreen(allReferents);
            loginScreen = new LoginScreen(allReferents);
            yearGroupCreatorScreen = new YearGroupCreatorScreen();
            courseCreatorScreen = new CourseCreatorScreen();
            studentCreatorScreen = new StudentCreatorScreen();
            evaluationCreatorScreen = new EvaluationCreatorScreen(allReferents);
            studentsAnalysisScreen = new StudentsAnalysisScreen();
            studentsCertificateScreen = new StudentsCertificateScreen();
            evaluationsPeriodScreen = new EvaluationsPeriodScreen();

            // Subscribe for keys to handle user input at different program states.
            defaultInputHandler = new InputHandler();
            defaultInputHandler.SubscribeForKey(ConsoleKey.R);
            defaultInputHandler.SubscribeForKey(ConsoleKey.L);
            defaultInputHandler.SubscribeForKey(ConsoleKey.X);

            loggedInInputHandler = new InputHandler();
            loggedInInputHandler.SubscribeForKey(ConsoleKey.Y);
            loggedInInputHandler.SubscribeForKey(ConsoleKey.C);
            loggedInInputHandler.SubscribeForKey(ConsoleKey.S);
            loggedInInputHandler.SubscribeForKey(ConsoleKey.E);
            loggedInInputHandler.SubscribeForKey(ConsoleKey.A);
            loggedInInputHandler.SubscribeForKey(ConsoleKey.O);
            loggedInInputHandler.SubscribeForKey(ConsoleKey.X);
            loggedInInputHandler.SubscribeForKey(ConsoleKey.D1);
            loggedInInputHandler.SubscribeForKey(ConsoleKey.D2);
            loggedInInputHandler.SubscribeForKey(ConsoleKey.D3);

            // Begin program
            currentMenuState = MenuStateDefault;

            loggedInReferent = null;

            ConsoleKey key;
            bool running = true;

            while (running)
            {
                DrawMenu();

                if (currentMenuState == MenuStateDefault)
                {
                    key = defaultInputHandler.WaitForSubscribedKey();
                }
                else
                {
                    key = loggedInInputHandler.WaitForSubscribedKey();
                }

                Console.Clear();

                switch (key)
                {
                    case ConsoleKey.R:
                        registrationScreen.Draw();
                        loggedInReferent = registrationScreen.GetNewReferentFromInput(new Random().Next(10000, 100000).ToString());

                        if (loggedInReferent != null)
                        {
                            allReferents.Add(loggedInReferent);
                            currentMenuState = MenuStateLoggedIn;
                        }

                        break;
                    case ConsoleKey.L:
                        loginScreen.Draw();
                        loggedInReferent = loginScreen.GetLoggedInReferent();

                        if (loggedInReferent != null)
                        {
                            currentMenuState = MenuStateLoggedIn;
                        }

                        break;
                    case ConsoleKey.Y:
                        yearGroupCreatorScreen.Draw();
                        allYearGroups.Add(yearGroupCreatorScreen.GetNewAgeGroupFromInput(loggedInReferent));

                        break;
                    case ConsoleKey.C:
                        courseCreatorScreen.Draw();
                        allCourses.Add(courseCreatorScreen.GetNewCourseFromInput(loggedInReferent));

                        break;
                    case ConsoleKey.S:
                        if (loggedInReferent.Courses.Count > 0 && loggedInReferent.YearGroups.Count > 0)
                        {
                            studentCreatorScreen.Draw();
                            allStudents.Add(studentCreatorScreen.GetNewStudentFromInput(loggedInReferent));
                        }

                        break;
                    case ConsoleKey.E:
                        if (loggedInReferent.Courses.Count > 0 && loggedInReferent.YearGroups.Count > 0 && loggedInReferent.Students.Count > 0)
                        {
                            evaluationCreatorScreen.Draw();
                            allEvaluations.Add(evaluationCreatorScreen.GetNewEvaluationFromInput(loggedInReferent));
                        }

                        break;
                    case ConsoleKey.A:
                        if (currentMenuState == MenuStateLoggedIn && ConfigurationCompleted())
                        {
                            currentMenuState = MenuStateAnalysis;
                        }
                        else if (currentMenuState == MenuStateAnalysis)
                        {
                            currentMenuState = MenuStateLoggedIn;
                        }

                        break;
                    case ConsoleKey.D1:
                        if (currentMenuState == MenuStateAnalysis)
                        {
                            studentsAnalysisScreen.Draw();
                            studentsAnalysisScreen.ShowAnalysisScreen(loggedInReferent);
                        }

                        break;
                    case ConsoleKey.D2:
                        if (currentMenuState == MenuStateAnalysis)
                        {
                            studentsCertificateScreen.Draw();
                            studentsCertificateScreen.ShowCertificateScreen(loggedInReferent);
                        }

                        break;
                    case ConsoleKey.D3:
                        if (currentMenuState == MenuStateAnalysis)
                        {
                            evaluationsPeriodScreen.Draw();
                            evaluationsPeriodScreen.ShowEvaluationsPerPeriodScreen(loggedInReferent);
                        }

                        break;
                    case ConsoleKey.O:
                        loggedInReferent = null;
                        currentMenuState = MenuStateDefault;

                        break;
                    case ConsoleKey.X:
                        running = false;
                        break;
                }

                Console.Clear();
            }
        }

        /// <summary>
        /// Checks if the configuration of system is completed.
        /// Completed means the referent created at least one year group, one course and one student.
        /// </summary>
        /// <returns>A boolean indicating whether the configuration is completed or not.</returns>
        private static bool ConfigurationCompleted()
        {
            if (loggedInReferent != null)
            {
                return loggedInReferent.YearGroups.Count > 0 && loggedInReferent.Courses.Count > 0 && loggedInReferent.Students.Count > 0 && loggedInReferent.Evaluations.Count > 0;
            }

            return false;
        }

        /// <summary>
        /// Draws the menu with the currently available options on the console.
        /// </summary>
        private static void DrawMenu()
        {
            if (currentMenuState == MenuStateDefault)
            {
                Console.WriteLine("\n [R] Register");
                Console.WriteLine("\n [L] Login");
            }
            else if (currentMenuState == MenuStateLoggedIn || currentMenuState == MenuStateAnalysis)
            {
                Console.WriteLine("\n [Y] Create year group");
                Console.WriteLine("\n [C] Create course");

                if (loggedInReferent.Courses.Count > 0 && loggedInReferent.YearGroups.Count > 0)
                {
                    Console.WriteLine("\n [S] Create student");

                    if (loggedInReferent.Students.Count > 0)
                    {
                        Console.WriteLine("\n [E] Submit an evaluation");
                    }
                }
                
                if (ConfigurationCompleted())
                {
                    Console.WriteLine("\n [A] Perform analysis");
                }
                
                if (currentMenuState == MenuStateAnalysis)
                {
                    Console.WriteLine("\n     [1] Analyse students by year group and course");
                    Console.WriteLine("\n     [2] Show certificates from students");
                    Console.WriteLine("\n     [3] Show evaluations within a time frame");
                }

                Console.WriteLine("\n [O] Log off");
            }

            Console.WriteLine("\n [X] Exit");
        }
    }
}
