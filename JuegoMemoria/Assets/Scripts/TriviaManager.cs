using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TriviaManage : MonoBehaviour
{
	public TMP_Text preguntaText;
	public TMP_Text[] respuestasText;

	private int preguntaActual = 0;
	private int puntuacion = 0;

	string[] preguntas =
	{
		"¿Cuál es el planeta más cercano al Sol?",
		"¿Cuál es el planeta rojo?",
		"¿Cuál es el planeta más grande del Sistema Solar?"
	};

	string[,] respuestas =
	{
		{"Venus", "Mercurio", "Saturno", "Marte"},
		{"Júpiter", "Marte", "Saturno", "Urano"},
		{"Júpiter", "Saturno", "Neptuno", "Tierra"}
	};

	int[] correctas =
	{
		1, // Mercurio
        1, // Marte
        0  // Júpiter
    };

	void Start()
	{
		MostrarPregunta();
	}

	void MostrarPregunta()
	{
		preguntaText.text = preguntas[preguntaActual];

		for (int i = 0; i < 4; i++)
		{
			respuestasText[i].text = respuestas[preguntaActual, i];
		}
	}

	public void Responder(int opcion)
	{
		if (opcion == correctas[preguntaActual])
		{
			puntuacion++;
			Debug.Log("Respuesta Correcta");
		}
		else
		{
			Debug.Log("Respuesta Incorrecta");
		}

		preguntaActual++;

		if (preguntaActual < preguntas.Length)
		{
			MostrarPregunta();
		}
		else
		{
			preguntaText.text = "Juego Terminado\nPuntos: " + puntuacion;

			for (int i = 0; i < 4; i++)
			{
				respuestasText[i].text = "";
			}
		}
	}
}