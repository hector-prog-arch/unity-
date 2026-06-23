using UnityEngine;
using UnityEngine.UI;

public class TriviaManager : MonoBehaviour
{
    public Text preguntaText;
    public Text resultadoText;

    private int correctas = 0;
    private int incorrectas = 0;

    public void RespuestaCorrecta()
    {
        correctas++;
        resultadoText.text = "¡Correcto!";
        SiguientePregunta();
    }

    public void RespuestaIncorrecta()
    {
        incorrectas++;
        resultadoText.text = "Incorrecto";
        SiguientePregunta();
    }

    void SiguientePregunta()
    {
        // Cambiar a la siguiente pregunta
    }
}