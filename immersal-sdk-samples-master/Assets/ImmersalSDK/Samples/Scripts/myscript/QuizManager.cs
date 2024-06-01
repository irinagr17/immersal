using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class QuizManager : MonoBehaviour
{

    public List<QnA_Block> Tests;
    public GameObject[] options; //лист для хранения кнопок варианты ответа
    public int currentTest; // номер текущего теста
    public int currentQuestion;  // номер текущего вопроса

    public List<Statuses> Statuses; //список статусов
    public List<GameObject> Ar_Models; //список моделек, отдельно, чтобы удобно разом выключить

    public GameObject QuizPanel;  // панель вопросы и ответы
    public GameObject TestEndPanel;  // панель конец теста
    public GameObject EndPanel;  // панель конец квиза
    public GameObject StartPanel; // панель начать игру

    public TextMeshProUGUI QuestionNameTxt;    // текст с заголовкам вопроса
    public TextMeshProUGUI QuestionTxt; //место где хранится вопрос
    public TextMeshProUGUI TestNameTxt;    // место где хранится Название теста
    public TextMeshProUGUI ResultTxt;    // текст с результатом теста
    public TextMeshProUGUI ScoreTxt;    // текст кол-во очков (общее)
    public TextMeshProUGUI ScoreTestTxt;    // текст кол-во очков за тест

    public int score;       // кол-во правильных ответов на тест
    public int common_score;       // кол-во правильных ответов
    public int countAllQ;       // кол-во всего вопросов
    List<int> indexes_Test = new List<int> { };  //список индексов тестов
    List<int> indexes = new List<int> { };  //список индексов вопросов


    private void Start() // срабатывает только 1 раз -> при запуске приложения
    {
        EndPanel.SetActive(false); // скрываем лишние панели
        QuizPanel.SetActive(false);
        TestEndPanel.SetActive(false);
    }

    public void OpenQuiz()  //начало квиза (при нажатии кнопки начали или заново)
    {
        countAllQ = 0;
        for (int i = 0; i < Tests.Count; i++) //по кол-ву созданных тестов (QnA_Block)
        {
            indexes_Test.Add(i);  // заполняем лист индексов тестов
            countAllQ += Tests[i].QnA.Count; //добавляем кол-во вопросов в тесте к общему кол-ву

        }
        for (int i = 0; i < Ar_Models.Count; i++) //по кол-ву созданных моделек
        {
            Ar_Models[i].SetActive(false); // скрываем все AR

        }
        common_score = 0; // кол-во набранных очков на начало игры 0
        StartPanel.SetActive(false); // скрываем кнопку начать игру
        GenerateTest();         // выбираем 1й тест рандомно - см функцию ниже
    }

    public void NewTest()  //начало нового теста
    {

        for (int i = 0; i < Tests[currentTest].QnA.Count; i++) //по кол-ву созданных QnA
        {
            indexes.Add(i);  // заполняем лист индексов вопросов из текущего теста
        }



        score = 0; // кол-во набранных очков на начало игры 0
        TestEndPanel.SetActive(false); // закрыть панель с результатами от пред. теста
        QuizPanel.SetActive(true);  // открываем панель игры
        GenerateQuestion();         // выбираем 1й вопрос - см функцию ниже
    }


    public void Exit() // запуск по кнопке выход в конце игры
    {
        EndPanel.SetActive(false); // закрывает панель конец игры
        StartPanel.SetActive(true); // открывает панель начать игру
    }

    public void Retry() // запуск по кнопке заново в конце игры
    {
        EndPanel.SetActive(false); // закрывает панель конец игры
        OpenQuiz(); //f начало квиза - выше
    }

    public void GameOver() // конец игры
    {
        TestEndPanel.SetActive(false); // закрываем панель конца теста
        EndPanel.SetActive(true);   // открываем панель конец 
        GetStatus();
        ScoreTxt.text = "Набрано баллов: " + common_score + "/" + countAllQ; // выводим кол-во набранных очков / всего вопросов
    }

    public void EndTest() // следующий тест
    {
        indexes_Test.Remove(currentTest); // удаляем из листа индексов номер текущего теста
        QuizPanel.SetActive(false); // закрываем панель самой игры
        TestEndPanel.SetActive(true); // открыть конец теста
        ScoreTestTxt.text = score + "/" + Tests[currentTest].QnA.Count; // выводим кол-во набранных очков / всего вопросов
        TestNameTxt.text = Tests[currentTest].Name;

    }

    public void NextQ() // следующий вопрос
    {
        indexes.Remove(currentQuestion); // удаляем из листа индексов номер текущего вопроса
        Tests[currentTest].QnA[currentQuestion].AR_Model.SetActive(false); // видимое Ar делаем снова невидимым
        StartCoroutine(WaitForNext());   // запускаем ожидание на 1сек (f (WaitForNext() ниже). Задержка нужна чтобы мы успели увидеть, как кнопка перекрасилась
    }

    public void AddPoint() // добавляем очко
    {
        score += 1; // просто добавляем 1 к счетчику очков
        common_score += 1;
    }

    IEnumerator WaitForNext() // спец штука для работы со временем
    {
        yield return new WaitForSeconds(1); //ждем секунду
        GenerateQuestion(); // выбираем следующий рандомный вопрос
    }
    void GetStatus()
    {
        int percent = common_score * 100 / countAllQ;
        Debug.Log("-------------->");
        Debug.Log(percent);
        Debug.Log("-/*******/->");
        Debug.Log(countAllQ);
        Debug.Log("AAAAAAAAA");
        Debug.Log(common_score);
        for (int i = 0; i < Statuses.Count; i++) //по кол-ву созданных статусов
        {

            if (percent <= Statuses[i].beforePercent)
            {
                ResultTxt.text = Statuses[i].Status;
                break;
            }
        }
    }

    void SetAR() // отображает нужное ar
    {
        Tests[currentTest].QnA[currentQuestion].AR_Model.SetActive(true); // обращаемся к модели у QnA
    }

    void SetAnswers() //заполняем кнопки ответов
    {
        for (int i = 0; i < options.Length; i++) //для каждой кнопки ответа
        {

            options[i].GetComponent<AnswerScript>().isCorrect = false; // скрипт кнопки - делаем ответ неверными 
            options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = Tests[currentTest].QnA[currentQuestion].Answers[i]; // заполняем ответ текстом из текущего QnA, список ответов аналог. индекс 
            options[i].GetComponent<Image>().color = options[i].GetComponent<AnswerScript>().startColor; // скрипт кнопки - ставим стартовый цвет
            if (Tests[currentTest].QnA[currentQuestion].CorrectAnswer == i + 1) // если это кнопка с верным ответом
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true; // скрипт кнопки - делаем ответ верными 
            }
        }
    }

    void GenerateQuestion() //выбирает рандомный вопрос
    {
        int qDone = Tests[currentTest].QnA.Count - indexes.Count;
        if (indexes.Count > 0)  // если еще есть индексы вопросов
        {
            currentQuestion = indexes[Random.Range(0, indexes.Count)]; // получает рандомный из листа


            QuestionNameTxt.text = Tests[currentTest].Name + ": " + qDone + "/" + Tests[currentTest].QnA.Count; //пишем заголовок *имя теста*: 1/5
            QuestionTxt.text = Tests[currentTest].QnA[currentQuestion].Question; // в поле вопроса пишем значение св-ва Question из QnA по рандомному индексу

            SetAnswers(); // заполняем кнопки ответов - см выше f
            SetAR(); // отображаем нужное ar - см выше f
        }
        else // если вопросы кончились
        {
            Debug.Log("out_questions");
            EndTest(); //следующий тест
        }
    }

    public void GenerateTest() //выбирает рандомный тест
    {
        if (indexes_Test.Count > 0)  // если еще есть индексы тестов
        {
            //currentTest = indexes_Test[Random.Range(0, indexes_Test.Count)]; // получает рандомный из листа
            currentTest = indexes_Test[0];
            NewTest();
        }
        else // если тесты кончились
        {
            Debug.Log("out_tests");
            GameOver();  // выходим в панель конец игры - см выше f
        }
    }
}
