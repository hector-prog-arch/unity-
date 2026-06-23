using System.Security.Authentication.ExtendedProtection;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class jugador : MonoBehaviour
{

    public Rigidbody2D rigidBody;
    public int saltoDoble = 2;
    public TextMeshProUGUI puntosUI;
    public int puntos =0;
    public GameObject finUI;
    public AudioSource audiopieza;


    void Start()
    {
       
    }


    void Update()
    {

        // rigiBody.linearVelocity = new Vector2(1,1);
        if (Input.GetKeyDown(KeyCode.Space) && saltoDoble > 0)
        {
            rigidBody.linearVelocity = Vector2.zero;
            rigidBody.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
            saltoDoble--;
        }
        rigidBody.linearVelocity = new Vector2(Input.GetAxis("Horizontal") * 5, rigidBody.linearVelocity.y);

        if (transform.position.y < -16)

        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Pieza>())
        {
            //puntos += 10;
            //puntosUI.text = "Puntos :" + puntos;
            //audiopieza.Play();
            Destroy(collision.gameObject);
            puntos += 1;
        }

        if (puntos == 5)

        {
            finUI.SetActive(true);
            //SceneManager.LoadScene("Menu");
        }

    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        saltoDoble = 2;

        if (collision.gameObject.GetComponent<enemigo>())

        {

            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
    }
}
