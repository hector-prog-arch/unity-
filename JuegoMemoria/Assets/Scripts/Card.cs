using UnityEngine;
using System.Collections;

public class Card : MonoBehaviour
{
    public int cardID { get; private set; } // Identificador del planeta (par)
    public bool isFlipped = false; // ¿Está revelada actualmente?
    public bool isMatched = false; // ¿Ya se encontró su par?

    // Referencias a los objetos (Asigna en el Prefab)
    public GameObject cardFront; // El objeto Sprite (Front)
    public GameObject cardBack;  // El objeto Sprite (Back)

    private SpriteRenderer frontSpriteRenderer;

    void Awake()
    {
        // Obtener la referencia al SpriteRenderer del frente
        frontSpriteRenderer = cardFront.GetComponent<SpriteRenderer>();
    }

    // Llamada por GameManager para asignar el planeta y ID
    public void Initialize(int id, Sprite planetSprite)
    {
        cardID = id;
        frontSpriteRenderer.sprite = planetSprite;
    }

    // Detecta el clic del ratón en el Collider 2D
    void OnMouseDown()
    {
        // Pide al GameManager que maneje la selección
        GameManager.Instance.CardSelected(this);
    }

    // Voltea la carta para mostrar el frente
    public void FlipCard()
    {
        if (isFlipped || isMatched) return;

        isFlipped = true;

        // Usaremos una Corrutina para animar la rotación
        StartCoroutine(FlipAnimation(true));
    }

    // Voltea la carta para mostrar la parte trasera
    public void FlipCardBack()
    {
        if (!isFlipped || isMatched) return;

        isFlipped = false;

        // Usaremos una Corrutina para animar la rotación de vuelta
        StartCoroutine(FlipAnimation(false));
    }

    // Corrutina para animar el giro
    public IEnumerator FlipAnimation(bool showFront)
    {
        float duration = 0.5f; // Duración del giro
        float time = 0f;

        // Rotación inicial/final
        Quaternion startRotation = transform.rotation;
        Quaternion endRotation = Quaternion.identity;

        // Si showFront es true, giramos 180 grados en Y (ocultando el Back en medio)
        // Si es false, volvemos a 0 grados (ocultando el Front en medio)
        if (showFront)
        {
            endRotation = Quaternion.Euler(0, 180, 0);
        }

        while (time < duration)
        {
            // Interpolación de la rotación para un giro suave
            transform.rotation = Quaternion.Lerp(startRotation, endRotation, time / duration);
            time += Time.deltaTime;

            // Lógica para alternar visibilidad en la mitad del giro (efecto 3D)
            if (time > duration / 2f)
            {
                if (showFront)
                {
                    cardBack.SetActive(false); // Oculta el reverso
                    cardFront.SetActive(true); // Muestra el frente
                }
                else
                {
                    cardBack.SetActive(true);  // Muestra el reverso
                    cardFront.SetActive(false); // Oculta el frente
                }
            }
            yield return null;
        }

        // Asegurar que la rotación sea exacta al final
        transform.rotation = endRotation;

        // Ajustar el objeto para que esté activo/inactivo correctamente al final de la animación
        cardBack.SetActive(!showFront);
        cardFront.SetActive(showFront);
    }
}
