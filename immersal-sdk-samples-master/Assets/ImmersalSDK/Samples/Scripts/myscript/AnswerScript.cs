using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false; //������ ������ ��� ���
    public QuizManager quizManager; // ����� ����� ���������� � ������  QuizManager � ��������� ������ f

    public Color startColor;  // ������� ����

    private void Start()
    {
    
        startColor = GetComponent<Image>().color; // �������� �� ��-� ������� ����
    }

    public void Answer() // ����������� ��� ������� ������ ������
    {
        if (isCorrect) // ���� ��� ������
        {
            GetComponent<Image>().color = Color.green; // ������� ����
            Debug.Log("Correct");
            quizManager.AddPoint(); // � ������  QuizManager ��������� f �������� ����
            quizManager.NextQ(); // � ������  QuizManager ��������� f ��������� ������
        }
        else
        {
            GetComponent<Image>().color = Color.red; // ������� ����
            Debug.Log("Wrong");
            quizManager.NextQ();  // � ������  QuizManager ��������� f ��������� ������
        }
    }
}
