using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AnswerScript : MonoBehaviour
{
    public bool isCorrect = false; //верная кнопка или нет
    public QuizManager quizManager; // чтобы могли обратиться в скрипт  QuizManager и выполнить нужные f

    public Color startColor;  // базовый цвет

    private void Start()
    {
    
        startColor = GetComponent<Image>().color; // получаем из св-в базовый цвет
    }

    public void Answer() // срабатывает при нажатии кнопки ответа
    {
        if (isCorrect) // если был верный
        {
            GetComponent<Image>().color = Color.green; // зеленый цвет
            Debug.Log("Correct");
            quizManager.AddPoint(); // в скрипт  QuizManager выполняем f добавить очко
            quizManager.NextQ(); // в скрипт  QuizManager выполняем f следующий вопрос
        }
        else
        {
            GetComponent<Image>().color = Color.red; // красный цвет
            Debug.Log("Wrong");
            quizManager.NextQ();  // в скрипт  QuizManager выполняем f следующий вопрос
        }
    }
}
