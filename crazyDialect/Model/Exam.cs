using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class Exam
    {
        public Exam() { }

        #region Model
        private int _exam_id;
        private int _question_id;
        private string _question;
        private string _answer;
        private string _option;

        public int ExamID
        {
            set { _exam_id = value; }
            get { return _exam_id; }
        }

        public int QuestionID
        {
            set { _question_id = value; }
            get { return _question_id; }
        }

        public string Question
        {
            set { _question = value; }
            get { return _question; }
        }

        public string Answer
        {
            set { _answer = value; }
            get { return _answer; }
        }

        public string Option
        {
            set { _option = value; }
            get { return _option; }
        }
        #endregion Model
    }
}
