using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Highscore = SpaceShooter.HighscoreController.Highscore;

namespace SpaceShooter.UI
{
    public class HighscoreWindow : MonoBehaviour
    {
        #region Fields
        //Inspector fields
        [SerializeField]
        private RectTransform content;
        [SerializeField]
        private Text scorePrefab, highscoreLabel, enteredName;
        [SerializeField]
        private CanvasGroup nameField;
        [SerializeField]
        private float spacing;

        //Private fields
        private readonly SortedList<Highscore, Text> labels = new SortedList<Highscore, Text>();
        #endregion

        #region Methods
        /// <summary>
        /// Adding a new highscore event
        /// </summary>
        public void OnAddScore()
        {
            //Make sure the name isn't whitespace
            if (string.IsNullOrWhiteSpace(this.enteredName.text)) { return; }

            //Create highscore
            Highscore highscore = new Highscore(this.enteredName.text.Trim(), GameLogic.CurrentGame.Score);
            this.labels.Add(highscore, Instantiate(this.scorePrefab, this.content, false));
            HighscoreController.Instance.AddHighscore(highscore);

            //Update positions
            int i = 1;
            foreach (KeyValuePair<Highscore, Text> label in this.labels)
            {
                UpdateLabel(label.Key, label.Value, i++);
            }

            //Turn off button
            this.nameField.interactable = false;
        }

        /// <summary>
        /// Update highscore position
        /// </summary>
        /// <param name="highscore">Highscore value</param>
        /// <param name="label">Text label</param>
        /// <param name="i">Position in the list</param>
        private void UpdateLabel(Highscore highscore, Text label, int i)
        {
            //Set position
            Vector2 position = label.rectTransform.anchoredPosition;
            position.y = this.spacing * i;
            label.rectTransform.anchoredPosition = position;
            //Set text
            label.text = $"{i}.\t{highscore}";
        }
        #endregion

        #region Functions
        private void Start()
        {
            //Set highscore label
            this.highscoreLabel.text += GameLogic.IsHard ? " (Hard)" : " (Normal)";

            //Add a highscore for every score
            int i = 1;
            foreach (Highscore highscore in HighscoreController.CurrentScores)
            {
                //Create the label
                Text label = Instantiate(this.scorePrefab, this.content, false);
                //Add them to the list
                this.labels.Add(highscore, label);

                //Update label
                UpdateLabel(highscore, label, i++);
            }
        }
        #endregion
    }
}
