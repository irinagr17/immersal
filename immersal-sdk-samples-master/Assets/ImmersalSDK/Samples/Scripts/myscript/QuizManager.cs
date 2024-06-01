using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class QuizManager : MonoBehaviour
{

    public List<QnA_Block> Tests;
    public GameObject[] options; //���� ��� �������� ������ �������� ������
    public int currentTest; // ����� �������� �����
    public int currentQuestion;  // ����� �������� �������

    public List<Statuses> Statuses; //������ ��������
    public List<GameObject> Ar_Models; //������ �������, ��������, ����� ������ ����� ���������

    public GameObject QuizPanel;  // ������ ������� � ������
    public GameObject TestEndPanel;  // ������ ����� �����
    public GameObject EndPanel;  // ������ ����� �����
    public GameObject StartPanel; // ������ ������ ����

    public TextMeshProUGUI QuestionNameTxt;    // ����� � ���������� �������
    public TextMeshProUGUI QuestionTxt; //����� ��� �������� ������
    public TextMeshProUGUI TestNameTxt;    // ����� ��� �������� �������� �����
    public TextMeshProUGUI ResultTxt;    // ����� � ����������� �����
    public TextMeshProUGUI ScoreTxt;    // ����� ���-�� ����� (�����)
    public TextMeshProUGUI ScoreTestTxt;    // ����� ���-�� ����� �� ����

    public int score;       // ���-�� ���������� ������� �� ����
    public int common_score;       // ���-�� ���������� �������
    public int countAllQ;       // ���-�� ����� ��������
    List<int> indexes_Test = new List<int> { };  //������ �������� ������
    List<int> indexes = new List<int> { };  //������ �������� ��������


    private void Start() // ����������� ������ 1 ��� -> ��� ������� ����������
    {
        EndPanel.SetActive(false); // �������� ������ ������
        QuizPanel.SetActive(false);
        TestEndPanel.SetActive(false);
    }

    public void OpenQuiz()  //������ ����� (��� ������� ������ ������ ��� ������)
    {
        countAllQ = 0;
        for (int i = 0; i < Tests.Count; i++) //�� ���-�� ��������� ������ (QnA_Block)
        {
            indexes_Test.Add(i);  // ��������� ���� �������� ������
            countAllQ += Tests[i].QnA.Count; //��������� ���-�� �������� � ����� � ������ ���-��

        }
        for (int i = 0; i < Ar_Models.Count; i++) //�� ���-�� ��������� �������
        {
            Ar_Models[i].SetActive(false); // �������� ��� AR

        }
        common_score = 0; // ���-�� ��������� ����� �� ������ ���� 0
        StartPanel.SetActive(false); // �������� ������ ������ ����
        GenerateTest();         // �������� 1� ���� �������� - �� ������� ����
    }

    public void NewTest()  //������ ������ �����
    {

        for (int i = 0; i < Tests[currentTest].QnA.Count; i++) //�� ���-�� ��������� QnA
        {
            indexes.Add(i);  // ��������� ���� �������� �������� �� �������� �����
        }



        score = 0; // ���-�� ��������� ����� �� ������ ���� 0
        TestEndPanel.SetActive(false); // ������� ������ � ������������ �� ����. �����
        QuizPanel.SetActive(true);  // ��������� ������ ����
        GenerateQuestion();         // �������� 1� ������ - �� ������� ����
    }


    public void Exit() // ������ �� ������ ����� � ����� ����
    {
        EndPanel.SetActive(false); // ��������� ������ ����� ����
        StartPanel.SetActive(true); // ��������� ������ ������ ����
    }

    public void Retry() // ������ �� ������ ������ � ����� ����
    {
        EndPanel.SetActive(false); // ��������� ������ ����� ����
        OpenQuiz(); //f ������ ����� - ����
    }

    public void GameOver() // ����� ����
    {
        TestEndPanel.SetActive(false); // ��������� ������ ����� �����
        EndPanel.SetActive(true);   // ��������� ������ ����� 
        GetStatus();
        ScoreTxt.text = "������� ������: " + common_score + "/" + countAllQ; // ������� ���-�� ��������� ����� / ����� ��������
    }

    public void EndTest() // ��������� ����
    {
        indexes_Test.Remove(currentTest); // ������� �� ����� �������� ����� �������� �����
        QuizPanel.SetActive(false); // ��������� ������ ����� ����
        TestEndPanel.SetActive(true); // ������� ����� �����
        ScoreTestTxt.text = score + "/" + Tests[currentTest].QnA.Count; // ������� ���-�� ��������� ����� / ����� ��������
        TestNameTxt.text = Tests[currentTest].Name;

    }

    public void NextQ() // ��������� ������
    {
        indexes.Remove(currentQuestion); // ������� �� ����� �������� ����� �������� �������
        Tests[currentTest].QnA[currentQuestion].AR_Model.SetActive(false); // ������� Ar ������ ����� ���������
        StartCoroutine(WaitForNext());   // ��������� �������� �� 1��� (f (WaitForNext() ����). �������� ����� ����� �� ������ �������, ��� ������ �������������
    }

    public void AddPoint() // ��������� ����
    {
        score += 1; // ������ ��������� 1 � �������� �����
        common_score += 1;
    }

    IEnumerator WaitForNext() // ���� ����� ��� ������ �� ��������
    {
        yield return new WaitForSeconds(1); //���� �������
        GenerateQuestion(); // �������� ��������� ��������� ������
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
        for (int i = 0; i < Statuses.Count; i++) //�� ���-�� ��������� ��������
        {

            if (percent <= Statuses[i].beforePercent)
            {
                ResultTxt.text = Statuses[i].Status;
                break;
            }
        }
    }

    void SetAR() // ���������� ������ ar
    {
        Tests[currentTest].QnA[currentQuestion].AR_Model.SetActive(true); // ���������� � ������ � QnA
    }

    void SetAnswers() //��������� ������ �������
    {
        for (int i = 0; i < options.Length; i++) //��� ������ ������ ������
        {

            options[i].GetComponent<AnswerScript>().isCorrect = false; // ������ ������ - ������ ����� ��������� 
            options[i].transform.GetChild(0).GetComponent<TMP_Text>().text = Tests[currentTest].QnA[currentQuestion].Answers[i]; // ��������� ����� ������� �� �������� QnA, ������ ������� ������. ������ 
            options[i].GetComponent<Image>().color = options[i].GetComponent<AnswerScript>().startColor; // ������ ������ - ������ ��������� ����
            if (Tests[currentTest].QnA[currentQuestion].CorrectAnswer == i + 1) // ���� ��� ������ � ������ �������
            {
                options[i].GetComponent<AnswerScript>().isCorrect = true; // ������ ������ - ������ ����� ������� 
            }
        }
    }

    void GenerateQuestion() //�������� ��������� ������
    {
        int qDone = Tests[currentTest].QnA.Count - indexes.Count;
        if (indexes.Count > 0)  // ���� ��� ���� ������� ��������
        {
            currentQuestion = indexes[Random.Range(0, indexes.Count)]; // �������� ��������� �� �����


            QuestionNameTxt.text = Tests[currentTest].Name + ": " + qDone + "/" + Tests[currentTest].QnA.Count; //����� ��������� *��� �����*: 1/5
            QuestionTxt.text = Tests[currentTest].QnA[currentQuestion].Question; // � ���� ������� ����� �������� ��-�� Question �� QnA �� ���������� �������

            SetAnswers(); // ��������� ������ ������� - �� ���� f
            SetAR(); // ���������� ������ ar - �� ���� f
        }
        else // ���� ������� ���������
        {
            Debug.Log("out_questions");
            EndTest(); //��������� ����
        }
    }

    public void GenerateTest() //�������� ��������� ����
    {
        if (indexes_Test.Count > 0)  // ���� ��� ���� ������� ������
        {
            //currentTest = indexes_Test[Random.Range(0, indexes_Test.Count)]; // �������� ��������� �� �����
            currentTest = indexes_Test[0];
            NewTest();
        }
        else // ���� ����� ���������
        {
            Debug.Log("out_tests");
            GameOver();  // ������� � ������ ����� ���� - �� ���� f
        }
    }
}
