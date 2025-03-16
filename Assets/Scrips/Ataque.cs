using UnityEngine;

public class Ataque : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnCollisionEnter2D(Collision2D enemigo)
    {
        if (enemigo.gameObject.CompareTag("Enemy"))
        {
            Destroy(enemigo.gameObject);
        }
    }
}
