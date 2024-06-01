using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class QandA 

{
    public string Question; // строка вопрос
    public string[] Answers; // массив ответов
    public int CorrectAnswer;  // номер верного ответа
    public GameObject AR_Model; //моделька для вопроса

}
