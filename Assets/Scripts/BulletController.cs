using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float speed;//Velocidade do tiro

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Move projétil na direção disparada e com a velocidade definida
        transform.position += transform.up * speed * Time.deltaTime;

        //Destrói projétil após 3 segundos
        Destroy(gameObject, 3);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        //Ao colidir com asteroide o projétil é destruído
        if(other.gameObject.tag == "Asteroid")
        {
            Destroy(this.gameObject);
        }
    }
}
