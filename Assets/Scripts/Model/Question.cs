using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts.Controller;

namespace Assets.Scripts.Model
{
    public class Question
    {
        private QuestionData rawData;

        public QuestionType Type => rawData.Type;
        public string Quest => rawData.Quest;
        public event CorrectChoice QuestionResult;

        public Question(QuestionData data)
        {
            rawData = new QuestionData();
            rawData.Type = data.Type;
            rawData.Quest = data.Quest;
            rawData.Answers = new List<string>(data.Answers);
            rawData.CorrectIndex = data.CorrectIndex;

            QuestionResult += isTrue;
            /*Swap(0, 1);
            Debug.Log(rawData.Answers[0] + " old:" + data.Answers[0]);
            Debug.Log(rawData.Answers[1] + " old:" + data.Answers[1]);*/
        }

        private bool isTrue(List<int> listAnswers)
        {
            // remove same index in list
            var truthList = new List<int>();
            for (int i = 0; i < listAnswers.Count; i++)
            {
                if (!truthList.Contains(listAnswers[i])) truthList.Add(listAnswers[i]);
            }

            // check count
            if (truthList.Count != rawData.CorrectIndex.Count) return false;

            // Fill handle
            if (Type == QuestionType.Fill)
            {
                for (int j = 0; j < truthList.Count; j++)
                {
                    if (truthList[j] != rawData.CorrectIndex[j]) return false;
                }

                return true;
            }

            // match answer
            foreach (var item in truthList)
            {
                if (!rawData.CorrectIndex.Contains(item)) return false; // in Multiple need more 1 event to send signal to UI
            }

            return true;
        }

        public List<string> GetAnswers()
        {
            MixAnswers();
            return rawData.Answers;
        }

        private void MixAnswers()
        {
            switch (Type)
            {
                case QuestionType.OneTrue:
                    {
                        int i = Random.Range(1, 6); // loop count 1 to 5
                        for (; i > 0; i--)
                        {
                            int index1 = Random.Range(0, rawData.Answers.Count); // Index will be swap 0 to Count-1
                            int index2 = Random.Range(0, rawData.Answers.Count); // Index will be swap
                            if (index1 == rawData.CorrectIndex[0]) rawData.CorrectIndex[0] = index2; // Change CorrectIndex
                            else if (index2 == rawData.CorrectIndex[0]) rawData.CorrectIndex[0] = index1; // Change CorrectIndex
                            //Debug.Log($"Swap {index1} and  {index2} - {rawData.CorrectIndex}");
                            Swap(index1, index2);
                        }
                        break;
                    }
                case QuestionType.MultipleTrue:
                    {
                        int i = Random.Range(3, 10); // loop count 3 to 9
                        for (; i > 0; i--)
                        {
                            int index1 = Random.Range(0, rawData.Answers.Count); // Index will be swap 0 to Count-1
                            int index2 = Random.Range(0, rawData.Answers.Count); // Index will be swap

                            var checkIndex1 = rawData.CorrectIndex.Contains(index1);
                            var checkIndex2 = rawData.CorrectIndex.Contains(index2);
                            if (checkIndex1 && checkIndex2)
                            {
                                int indexA = rawData.CorrectIndex.IndexOf(index1);
                                int indexB = rawData.CorrectIndex.IndexOf(index2);
                                rawData.CorrectIndex[indexA] = index2; // Change CorrectIndex
                                rawData.CorrectIndex[indexB] = index1; // Change CorrectIndex
                            }
                            else
                            {
                                if (checkIndex1)
                                {
                                    int indexA = rawData.CorrectIndex.IndexOf(index1);
                                    rawData.CorrectIndex[indexA] = index2; // Change CorrectIndex
                                }

                                if (checkIndex2)
                                {
                                    int indexB = rawData.CorrectIndex.IndexOf(index2);
                                    rawData.CorrectIndex[indexB] = index1; // Change CorrectIndex
                                }
                            }
                            //Debug.Log($"Swap {index1} and  {index2} - {rawData.CorrectIndex}");
                            Swap(index1, index2);
                        }
                        break;
                    }
                case QuestionType.Fill:
                    {
                        int i = Random.Range(5, 8); // loop count 5 to 7
                        for (; i > 0; i--)
                        {
                            int index1 = Random.Range(0, rawData.Answers.Count); // Index will be swap 0 to Count-1
                            int index2 = Random.Range(0, rawData.Answers.Count); // Index will be swap

                            var checkIndex1 = rawData.CorrectIndex.Contains(index1);
                            var checkIndex2 = rawData.CorrectIndex.Contains(index2);
                            if (checkIndex1 && checkIndex2)
                            {
                                int indexA = rawData.CorrectIndex.IndexOf(index1);
                                int indexB = rawData.CorrectIndex.IndexOf(index2);
                                rawData.CorrectIndex[indexA] = index2; // Change CorrectIndex
                                rawData.CorrectIndex[indexB] = index1; // Change CorrectIndex
                            }
                            else
                            {
                                if (checkIndex1)
                                {
                                    int indexA = rawData.CorrectIndex.IndexOf(index1);
                                    rawData.CorrectIndex[indexA] = index2; // Change CorrectIndex
                                }

                                if (checkIndex2)
                                {
                                    int indexB = rawData.CorrectIndex.IndexOf(index2);
                                    rawData.CorrectIndex[indexB] = index1; // Change CorrectIndex
                                }
                            }
                            //Debug.Log($"Swap {index1} and  {index2} - {rawData.CorrectIndex}");
                            Swap(index1, index2);
                        }
                        break;
                    }
                case QuestionType.TwoChoice:
                    {
                        int isEven = Random.Range(0, 100);
                        if (isEven % 2 == 0)
                        {
                            rawData.CorrectIndex[0] = rawData.CorrectIndex[0] == 0 ? 1 : 0;
                            Swap(0, 1);
                        }
                        break;
                    }
            }
        }

        private void Swap(int i1, int i2)
        {
            string c = rawData.Answers[i1];
            rawData.Answers[i1] = rawData.Answers[i2];
            rawData.Answers[i2] = c;
        }

        public void UnSubQuestionResult() => QuestionResult -= isTrue;

        public bool CheckingResult(List<int> choice) => QuestionResult.Invoke(choice);

        public delegate bool CorrectChoice(List<int> choice);
    }
}