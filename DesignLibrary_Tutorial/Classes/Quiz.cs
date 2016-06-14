

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace DesignLibrary_Tutorial.Classes
{
    [Serializable]
    class Quiz
    {
        private List<Question> _Questions;
        public QuizHistory _QuizHistory;

        public List<Question> Questions
        {
            get { return _Questions; }
        }

        //Default constructor. Creates an empty Quiz object.
        public Quiz()
        {
            _Questions = new List<Question>(); //The list of questions has to be filled later !
            _QuizHistory = new QuizHistory(); //The history will be an empty stack
        }

        //Returns the current question, which the user has to answer in order to go forward
        public Question CurrentQuestion
        {
            get
            {
                int NextQuestionId;
                if (_QuizHistory.IsEmpty) //If the history is empty, return the first question of the list
                {
                    NextQuestionId = 0;
                }
                else
                {
                    NextQuestionId = _Questions[_QuizHistory.LastEntry.QuestionId].Choices[_QuizHistory.LastEntry.ChoiceId].NextQuestionId;
                }

                if (NextQuestionId != -1) //-1 Should be set for all choices leading to the end of the quiz
                {
                    return (_Questions[NextQuestionId]);
                }
                else
                    return (null); //When there is no more questions, it's the end of the quiz ! Display of the result should be handled externally.

            }
        }

        //Answers the current question by adding an entry in the history. Returns the next question, or null if it's the end of the quiz.
        public void AnswerQuestion(int choiceId)
        {
            _QuizHistory.AddEntry(_Questions.IndexOf(CurrentQuestion), choiceId);
            return ;
        }

        //Returns a QuizResult object with the final score of the quiz, and all related information.
        public QuizResult QuizResult
        {
            get { return CalculateResult(); }
        }


        public void SaveToXml(String FilePath)
        {
            XmlSerializer xsSubmit = new XmlSerializer(typeof(Quiz));
            using (StringWriter sww = new StringWriter())
            using (XmlWriter writer = XmlWriter.Create(sww))
            {
                xsSubmit.Serialize(writer, this);
                var xml = sww.ToString(); // Your XML
            }
            return;
        }


        //Private
        private QuizResult CalculateResult()
        {
            int score = 0;
            foreach (QuizHistoryItem i in _QuizHistory.FullHistory)
            {
                score += _Questions[i.QuestionId].Choices[i.ChoiceId].Score;
            }
            return (new QuizResult(score, "Dynamic result text is not yet implemented."));
        }


    }

    class Question
    {
        private String _Issue; //Text displayed to the user. This is basically the question the user has to answer.
        private List<Choice> _Choices; //List of possible answers, each containg information like "what question goes next" or the score associated with this choice.

        public String Issue
        {
            get { return _Issue; }
            set { _Issue = value; } //TODO : read only!
        }

        public List<Choice> Choices
        {
            get { return _Choices; }
            set { Choices = value; } //TODO : read only!
        }

        //Default constructor - Creates an empty Question object
        public Question()
        {
            _Issue = "DefaultIssue";
            _Choices = new List<Choice>();
        }


        public Question(String issue, List<Choice> choices)
        {
            _Issue = issue;
            _Choices = new List<Choice>(choices);
        }

        public List<String> ChoicesTexts
        {
            get
            {
                List<String> ChoicesTexts = new List<String>();
                foreach (Choice c in _Choices)
                {
                    ChoicesTexts.Add(c.Answer);
                }
                return (ChoicesTexts);
            }
        }

    }


    class Choice
    {
        public String Answer; //Text displayed to the user
        public int Score; //Score associated to this answer
        public int NextQuestionId; //ID for the next question to fetch after selecting this answer

        //Default constructor - Creates an empty Choice object
        public Choice()
        {
            Answer = "DefaultAnswer";
            Score = 0;
            NextQuestionId = 0;
        }

        public Choice(String answer, int score, int nextquestionId)
        {
            Answer = answer;
            Score = score;
            NextQuestionId = nextquestionId;
        }

    }

    class QuizResult
    {
        public int _score;
        public String _detail;

        //Default constructor - Creates an empty QuizResult object
        public QuizResult()
        {
            _score = 0;
            _detail = "DefaultDetail";
        }

        public QuizResult(int score, String detail)
        {
            _score = score;
            _detail = detail;
        }

    }


    #region History managment
    class QuizHistory
    {
        private Stack<QuizHistoryItem> _QuizHistoryItems;

        public Stack<QuizHistoryItem> FullHistory
        {
            get { return (_QuizHistoryItems); }
        }

        public QuizHistoryItem LastEntry
        {
            get { return _QuizHistoryItems.Peek(); }
        }

        public bool IsEmpty
        {
            get { return (_QuizHistoryItems.Count == 0); }
        }

        //Default constructor - Creates an empty QuizHistory object
        public QuizHistory()
        {
            _QuizHistoryItems = new Stack<QuizHistoryItem>();
        }

        public void AddEntry(int questionId, int choiceId)
        {
            _QuizHistoryItems.Push(new QuizHistoryItem(questionId, choiceId));
        }

        public QuizHistoryItem RemoveLastEntry()
        {
            if (_QuizHistoryItems.Count > 0)
                return (_QuizHistoryItems.Pop());
            else
                return null;
        }


    }
    class QuizHistoryItem
    {
        private int _questionId;
        private int _choiceId;

        public int QuestionId
        {
            get { return _questionId; }
            private set { _questionId = value; }
        }

        public int ChoiceId
        {
            get { return _choiceId; }
            private set { _choiceId = value; }
        }

        //Default constructor - Creates an empty QuizHistory object
        public QuizHistoryItem()
        {
            QuestionId = 0;
            ChoiceId = 0;
        }

        public QuizHistoryItem(int questionId, int choiceId)
        {
            QuestionId = questionId;
            ChoiceId = choiceId;
        }

    }
    #endregion

}

