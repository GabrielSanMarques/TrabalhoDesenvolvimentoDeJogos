using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    public float rate = 2.0f;//Frequência de spawn
    public float start_dist = 10.0f;//Distância base dos pontos de spawn em relação à posição do spawner
    public GameObject asteroid_prefab;//Prefab do asteroide

    // Start is called before the first frame update
    void Start()
    {
        //Spawna um asteroide na frequência definida
        InvokeRepeating(nameof(Spawn), rate, rate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Spawna um asteroide
    void Spawn()
    {
        //Define a diposição inicial baseado em um ponto escolhido aleatoriamente na borda de um círculo de 10 unidades de raio
        Vector3 start_point = Random.insideUnitCircle.normalized * start_dist;
        Instantiate(asteroid_prefab, start_point, transform.rotation);
    }
}
