using UnityEngine;

public class plataforma : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public bool Arriba;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 posicionDeseada = Vector3.zero;
        if (Arriba)
            posicionDeseada = puntoA.position;
        else
            posicionDeseada = puntoB.position;

        Vector3 Direccion = (posicionDeseada - transform.position);
        transform.position += Direccion.normalized * Time.deltaTime;
        if (Direccion.magnitude < 0.5f)
        {
            if (Arriba)
                Arriba = false;
            else
                Arriba |= true;

        }


    }
}
