using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using SpaceShooter.Extensions;
using UnityEngine;

namespace SpaceShooter
{
    /// <summary>
    /// The highscore controller
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class HighscoreController : Singleton<HighscoreController>
    {
        /// <summary>
        /// Highscore value
        /// </summary>
        public struct Highscore : IComparable<Highscore>, IEquatable<Highscore>
        {
            #region Constants
            /// <summary>
            /// The amount of tokens in the saved format
            /// </summary>
            private const int formatLength = 3;

            /// <summary>
            /// The timestamp format
            /// </summary>
            private const string timeFormat = "dd/MM/yyyy-HH:mm:ss";

            /// <summary>
            /// The timestamp formatter
            /// </summary>
            private static readonly CultureInfo formatter = CultureInfo.InvariantCulture;

            /// <summary>
            /// Save format separators
            /// </summary>
            private static readonly char[] separators = { '=', '|' };
            #endregion

            #region Properties
            /// <summary>
            /// Name of whoever set the highscore
            /// </summary>
            public string Name { get; }

            /// <summary>
            /// Highest score
            /// </summary>
            public int Score { get; }

            /// <summary>
            /// Timestamp of the highscore
            /// </summary>
            public DateTime Time { get; }
            #endregion

            #region Constructors
            /// <summary>
            /// Creates a new Highscore with the given name and score
            /// </summary>
            /// <param name="name">Name of the person setting the highscore</param>
            /// <param name="score">The score itself</param>
            /// <exception cref="ArgumentNullException">If <paramref name="name"/> is null</exception>
            /// <exception cref="ArgumentException">If <paramref name="name"/> is empty or only whitespace</exception>
            /// <exception cref="ArgumentOutOfRangeException">If <paramref name="score"/> is negative</exception>
            public Highscore(string name, int score)
            {
                //Check arguments
                if (name == null) { throw new ArgumentNullException(nameof(name), "Name cannot be null"); }
                if (string.IsNullOrWhiteSpace(name)) { throw new ArgumentException("Name cannot empty or whitespace", nameof(name)); }
                if (score < 0) { throw new ArgumentOutOfRangeException(nameof(score), score, "Score must be greater than zero"); }

                //Set values
                this.Name = name.Trim();
                this.Score = score;
                this.Time = DateTime.Now;
            }

            /// <summary>
            /// Creates a new Highscore from a saved highscore string
            /// </summary>
            /// <param name="saved">Saved score string</param>
            /// <exception cref="ArgumentNullException">If <paramref name="saved"/> is null</exception>
            /// <exception cref="ArgumentException">If <paramref name="saved"/> is empty or only whitespace</exception>
            /// <exception cref="FormatException">If <paramref name="saved"/> is not correctly formatted, or if the name is empty or whitespace, the score an invalid int or negative, or if the timestamp is of an invalid format</exception>
            public Highscore(string saved)
            {
                //Check argument
                if (saved == null) { throw new ArgumentNullException(nameof(saved), "Saved string cannot be null"); }
                if (string.IsNullOrWhiteSpace(saved)) { throw new ArgumentException("Saved string cannot empty or whitespace", nameof(saved)); }

                //Check save format
                string[] splits = saved.Split(separators);
                if (splits.Length != formatLength) { throw new FormatException("Saved string is incorrectly formatted"); }

                //Check name
                string name = splits[0].Trim();
                if (string.IsNullOrEmpty(name)) { throw new FormatException("Saved name cannot empty or whitespace"); }

                //Check score
                int score;
                if (!int.TryParse(splits[1].Trim(), out score) || score < 0) { throw new FormatException("Saved score is not a valid non negative integer"); }

                //Check timestamp
                DateTime time;
                if (!DateTime.TryParseExact(splits[2].Trim(), timeFormat, formatter, DateTimeStyles.AssumeLocal, out time)) { throw new FormatException("The timestamp is invalid"); }

                //Set the values
                this.Name = name;
                this.Score = score;
                this.Time = time;
            }
            #endregion

            #region Methods
            /// <summary>
            /// Creates a save string for this Highscore
            /// </summary>
            /// <returns>The appropriate save string</returns>
            public string Save() => $"{this.Name}={this.Score}|{this.Time.ToString(timeFormat, formatter)}";

            /// <summary>
            /// Orders the Highscores, equal scores are sorted as greater
            /// </summary>
            /// <param name="other">Other score to compare to</param>
            /// <returns>-1 if the score is lower, </returns>
            /// <inheritdoc cref="IComparable{T}"/>
            public int CompareTo(Highscore other)
            {
                //Compare scores
                int comp = other.Score.CompareTo(this.Score);
                //If scores are equal, compare timestamp
                if (comp == 0) { comp = this.Time < other.Time ? -1 : 1; }
                return comp;
            }

            /// <summary>
            /// Tests equality between this Highscore and another highscore
            /// </summary>
            /// <param name="other">Other Highscore to test again</param>
            /// <returns>True if both highscores are equal, false otherwise</returns>
            public bool Equals(Highscore other) => this == other;

            /// <summary>
            /// Tests equality between this Highscore and another object
            /// </summary>
            /// <param name="o">Other object to test again</param>
            /// <returns>True if the other object is an equal Highscore instance, false otherwise</returns>
            public override bool Equals(object o) => o is Highscore && Equals((Highscore)o);

            /// <summary>
            /// Gets the hashcode of this instance
            /// </summary>
            /// <returns>The hashcode</returns>
            public override int GetHashCode() => unchecked((((this.Name != null ? this.Name.GetHashCode() : 0) * 397) ^ this.Score * 397) ^ this.Time.GetHashCode());

            /// <summary>
            /// String representation of this Highscore
            /// </summary>
            /// <returns>String representation of the highscore for UI</returns>
            public override string ToString() => $"{this.Name} - {this.Score}";
            #endregion

            #region Operators
            /// <summary>
            /// Tests equality between two highscores
            /// </summary>
            /// <param name="a">First highscore</param>
            /// <param name="b">Second highscore</param>
            /// <returns>True if both highscores are equal, false otherwise</returns>
            public static bool operator ==(Highscore a, Highscore b) => a.Name == b.Name && a.Score == b.Score && a.Time == b.Time;

            /// <summary>
            /// Tests equality between two highscores
            /// </summary>
            /// <param name="a">First highscore</param>
            /// <param name="b">Second highscore</param>
            /// <returns>True if both highscores are unequal, false otherwise</returns>
            public static bool operator !=(Highscore a, Highscore b) => a.Name != b.Name || a.Score != b.Score || a.Time != b.Time;
            #endregion
        }

        #region Constants
        /// <summary>
        /// The Highscore save file name
        /// </summary>
        public const string FILE_NAME = "Highscore.txt";

        /// <summary>
        /// File tag char indicator
        /// </summary>
        private const char tagChar = '#';

        /// <summary>
        /// Normal scores section
        /// </summary>
        private const string normalTag = "# NORMAL";

        /// <summary>
        /// Hard scores section tag
        /// </summary>
        private const string hardTag = "# HARD";

        /// <summary>
        /// The save file base template
        /// </summary>
        private const string template = normalTag + "\n\n" + hardTag;
        #endregion

        #region Static properties
        /// <summary>
        /// The current highscores
        /// </summary>
        public static SortedSet<Highscore> CurrentScores => GameLogic.IsHard ? Instance.HardScores : Instance.NormalScores;

        /// <summary>
        /// The full path location of the Highscore save file
        /// </summary>
        public static string FilePath { get; private set; }
        #endregion

        #region Properties
        /// <summary>
        /// Sorted list of Normal mode highscores
        /// </summary>
        public SortedSet<Highscore> NormalScores { get; } = new SortedSet<Highscore>();

        /// <summary>
        /// Sorted list of Hard mode highscores
        /// </summary>
        public SortedSet<Highscore> HardScores { get; } = new SortedSet<Highscore>();
        #endregion

        #region Methods
        /// <summary>
        /// Creates the save file for Highscores
        /// </summary>
        /// <param name="message">Optional message to log</param>
        private void CreateSaveFile(string message)
        {
            //Log message then create file
            this.LogError(message);
            File.WriteAllText(FilePath, template);
        }

        /// <summary>
        /// Load the Highscore text file from the disk
        /// </summary>
        private void LoadFile()
        {
            //Log start message
            this.Log($"Loading Highscores file @ {FilePath}");

            //Check if file exists, if not create it
            if (!File.Exists(FilePath)) { CreateSaveFile("Highscore file not found, creating file"); }
            else
            {
                //Error flag
                bool mustResave = false;

                this.Log($"Highscore file:\n{File.ReadAllText(FilePath)}");

                //Open file for reading
                using (StreamReader reader = File.OpenText(FilePath))
                {
                    //Skipping leading white lines
                    string line = reader.SkipBlanks();

                    //Making sure there is actually data in the file, else create a new one
                    if (line == null)
                    {
                        CreateSaveFile("No data found in file, creating new one");
                        return;
                    }

                    //Reading normal scores
                    if (line == normalTag)
                    {
                        this.Log("Loading normal scores");
                        line = LoadScores(reader, this.NormalScores);
                    }
                    else
                    {
                        this.LogError("Could not find normal tag, proceeding");
                        mustResave = true;
                    }

                    //Loading hard scores
                    if (line == hardTag)
                    {
                        this.Log("Loading hard scores");
                        line = LoadScores(reader, this.HardScores);
                    }
                    else
                    {
                        this.LogError("Could not find hard tag, proceeding");
                        mustResave = true;
                    }

                    //Check for trailing data
                    if (line != null)
                    {
                        this.LogError("Save file has trailing information");
                        mustResave = true;
                    }
                }

                //Save file again if errors happened
                if (mustResave) { SaveFile(); }
            }
        }

        /// <summary>
        /// Save the high score text file to the disk
        /// </summary>
        private void SaveFile()
        {
            //Log message
            this.Log($"Saving Highscore file @ {FilePath}");

            //Open file for writing
            using (StreamWriter writer = File.CreateText(FilePath))
            {
                //Add normal scores to file
                writer.WriteLine(normalTag);
                foreach (Highscore score in this.NormalScores)
                {
                    writer.WriteLine(score.Save());
                }

                //Write blank line
                writer.WriteLine();

                //Add hard scores to file
                writer.WriteLine(hardTag);
                foreach (Highscore score in this.HardScores)
                {
                    writer.WriteLine(score.Save());
                }
            }
        }

        /// <summary>
        /// Loads all possible scores in a tag to a given score list, until the end of the file is reached or a new tag
        /// </summary>
        /// <param name="reader">StreamReader reading the highscore file</param>
        /// <param name="list">Highscore list to add loaded scores to</param>
        /// <returns>The last line read before exiting</returns>
        private string LoadScores(StreamReader reader, SortedSet<Highscore> list)
        {
            //Read lines until end of file or a tag is found
            string line;
            for (line = reader.SkipBlanks(); line != null && line[0] != tagChar; line = reader.SkipBlanks())
            {
                //Try loading scores from text
                try
                {
                    Highscore score = new Highscore(line);
                    this.NormalScores.Add(score);
                }
                //Catch parse exceptions
                catch (Exception e)
                {
                    this.LogException(e, "Error encountered while loading Highscore");
                }
            }

            //Print amount loaded
            this.Log($"Loaded {list.Count} scores");

            //Return last line read
            return line;
        }

        /// <summary>
        /// Adds the highscore to the save file, and saves it to the disk
        /// </summary>
        /// <param name="highscore">Highscore to add</param>
        public void AddHighscore(Highscore highscore)
        {
            CurrentScores.Add(highscore);
            SaveFile();
        }
        #endregion

        #region Functions
        protected override void OnAwake()
        {
            //Load file
            FilePath = Path.Combine(Application.dataPath, FILE_NAME);
            LoadFile();
        }
        #endregion
    }
}
