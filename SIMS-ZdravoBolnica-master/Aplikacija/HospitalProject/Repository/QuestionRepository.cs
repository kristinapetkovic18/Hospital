﻿using HospitalProject.FileHandler;
using HospitalProject.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HospitalProject.DataUtility;

namespace HospitalProject.Repository
{
    public class QuestionRepository
    {

        private List<Question> questions;
        private IHandleData<Question> questionsFileHandler;
        private int maxId;

        public QuestionRepository()
        {
            questionsFileHandler = new QuestionsFileHandler(FilePathStorage.QUESTIONS_FILE);
            questions = questionsFileHandler.ReadAll().ToList();
            maxId = GetMaxId();
        }

        public List<Question> GetQuestionsByCategory(string category)
        {
            Category _category = EnumConverter.ConvertStringToCategory(category);

            return questionsFileHandler.ReadAll().Where(question => question.Category == _category).ToList();

        }

        public Question GetQuestionById(int _id)
        {
            

            return questionsFileHandler.ReadAll().FirstOrDefault(question => question.Id == _id);

        }

        public List<Question> ReadAll()
        {
            return questionsFileHandler.ReadAll().ToList();
        }

        public int GetMaxId()
        {
            return questions.Count() == 0 ? 0 : questions.Max(question => question.Id);
        }

        public List<Question> GetQuestionsByType(Category type)
        {
            List<Question> wantedQuestions = new List<Question>();
            foreach (Question question in questions)
            {
                if (question.Category == type)
                {
                    wantedQuestions.Add(question);
                }
            }

            return wantedQuestions;
        }

        public bool CheckQuestionType(Category category, int id)
        {
            Question question = GetQuestionById(id);
            if (question.Category == category)
            {
                return true;
            }

            return false;
        }

    }
}
