using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{

    public float initialRevealTime = 4f; // Duración en segundos para la revelación inicial
    public int initialFlipCount = 3; // <--- NUEVA VARIABLE: Cantidad de veces que girarán
    private bool gameStarted = false;    // Para prevenir clics durante la intro

    //contador
    //public TMP_Text counterText;
    int blockLeft;
    public int counter = 0;
    //public int puntos = 0;


    //public Text contador;


    // Arreglo de los sprites de los planetas (Asigna esto en el Inspector)
    public Sprite[] planetSprites;
    // Prefab de la carta que creaste (Arrastra Card Prefab aquí)
    public GameObject cardPrefab;
    // Cuántos pares quieres (ej. 6 planetas)
    public int numberOfPairs = 8;
    // Posición donde empezar a generar las cartas (ajusta en el Inspector)
    public Vector3 startPosition = new Vector3(-3f, 1.5f, 0f);
    // Espacio entre cartas (ajusta en el Inspector)
    public float cardSpacing = 1.5f;


    private List<Card> allCards = new List<Card>();
    // Lista para las cartas seleccionadas (máximo 2)
    private List<Card> selectedCards = new List<Card>();
    // Bandera para evitar clics mientras se voltean o esperan
    private bool isChecking = false;

    // Instancia única (Singleton) para acceder fácil
    public static GameManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        InitializeGame();

        // Inicia la corrutina para la animación de inicio
        StartCoroutine(InitialFlipSequence());

        counter = 0;
        //counterText.text = counter.ToString();
    }

    void InitializeGame()
    {
        // 1. Crear la lista de IDs (los pares)
        List<int> cardIDs = new List<int>();
        for (int i = 0; i < numberOfPairs; i++)
        {
            cardIDs.Add(i); // Un planeta
            cardIDs.Add(i); // Su par
        }

        // 2. Mezclar (barajar) la lista
        Shuffle(cardIDs);

        // 3. Generar las cartas en la escena
        for (int i = 0; i < cardIDs.Count; i++)
        {
            int id = cardIDs[i];

            // Calcular posición en una cuadrícula simple
            int col = i % 4; // 4 columnas
            int row = i / 4;
            Vector3 spawnPos = startPosition + new Vector3(col * cardSpacing, -row * cardSpacing, 0f);

            // Crear el objeto carta
            GameObject newCard = Instantiate(cardPrefab, spawnPos, Quaternion.identity);
            Card cardScript = newCard.GetComponent<Card>();

            // Asignar el ID y el Sprite
            cardScript.Initialize(id, planetSprites[id]);


            // ¡LA CLAVE! Añadir la carta a la lista
            allCards.Add(cardScript);
        }
    }

    // Algoritmo de Fisher-Yates (para barajar)
    void Shuffle(List<int> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }


    IEnumerator InitialFlipSequence()
    {
        // Bucle para repetir el giro varias veces
        for (int i = 0; i < initialFlipCount; i++)
        {
            // 1. Gira todas las cartas para mostrar el Frente
            foreach (Card card in allCards)
            {
                StartCoroutine(card.FlipAnimation(true));
            }

            // 2. Espera un breve momento ANTES de que termine el giro (0.5s) y de que se vea revelada
            // Reducimos la pausa después del giro a 0.1s
            yield return new WaitForSeconds(0.1f); // 0.1 segundos de pausa para el Frente

            // 3. Gira todas las cartas de vuelta al Reverso
            foreach (Card card in allCards)
            {
                StartCoroutine(card.FlipAnimation(false));
            }

            // 4. Espera a que termine la animación de giro de ocultación 
            // Reducimos la pausa antes del siguiente giro a 0.1s
            yield return new WaitForSeconds(0.1f); // 0.1 segundos de pausa para el Reverso
        }

        // 5. Espera el tiempo de "inspección" final de 5 segundos
        yield return new WaitForSeconds(initialRevealTime);

        // 6. Permite el juego
        gameStarted = true;
    }


    // Llamada por la carta cuando es clickeada
    public void CardSelected(Card card)
    {
        // AÑADE ESTA CONDICIÓN:
        if (!gameStarted)
        {
            return; // Ignora el click si el juego no ha iniciado
        }

        if (isChecking || selectedCards.Contains(card) || card.isMatched)
        {
            return;
        }




        if (isChecking || selectedCards.Contains(card) || card.isMatched)
        {
            return; // Ignorar si está revisando o si la carta ya fue seleccionada/emparejada
        }

        // 1. Revelar la carta y añadirla a la lista de seleccionadas
        card.FlipCard();
        selectedCards.Add(card);

        // 2. Si ya hay 2 cartas seleccionadas, iniciar la comprobación
        if (selectedCards.Count == 2)
        {
            isChecking = true;
            StartCoroutine(CheckMatchAfterDelay(1f)); // Espera 1 segundo antes de comprobar
        }
    }

    // Corrutina para esperar y luego comprobar el par
    IEnumerator CheckMatchAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Espera el tiempo de revelación/inspección

        Card card1 = selectedCards[0];
        Card card2 = selectedCards[1];

        // Comprobar si son un par (si tienen el mismo ID)
        if (card1.cardID == card2.cardID)
        {
            // 🎉 ¡Coincidencia Correcta!
            card1.isMatched = true;
            card2.isMatched = true;

            counter += 12;

            //counterText.text = counter.ToString();
            //contador.text = counter.ToString();
            //contador.text = "hola";

            //contador.text = "counter";
            Debug.Log("puntos " + counter);

            // Opcional: Destruir o hacer algo visual (ej: bajar opacidad)
            // No hacemos nada más, ya están reveladas (isMatched evita que se oculten).
        }
        else
        {
            // ❌ No son un par - Ocultar cartas
            card1.FlipCardBack();
            card2.FlipCardBack();
        }

        // Restablecer el estado
        selectedCards.Clear();
        isChecking = false;
    }
}