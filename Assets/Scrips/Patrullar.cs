using UnityEngine;

public class Patrullar : MonoBehaviour
{
    public Transform puntoA;
    public Transform puntoB;
    public float velocidad = 4f;

    private Vector3 destino;
    private bool vivo = true;

    void Start()
    {
        destino = puntoB.position;
    }

    void Update()
    {
        // Mover hacie el destino
        if (vivo)
        {
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidad * Time.deltaTime);

            // Si llega al destino, cambia de dirección
            if (Vector3.Distance(transform.position, destino) < 0.1f)
            {
                destino = (destino == puntoA.position) ? puntoB.position : puntoA.position;
                Flip(); //Voltear
            }
        }

    }

    void Flip()
    {
        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }

    public void parar()
    {
        this.enabled = false;
    }

}