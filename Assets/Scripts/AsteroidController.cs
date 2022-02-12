using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidController : MonoBehaviour
{
    public Sprite[] sprite_options;//Array com os diferentes sprites que o asteroide pode ter
    public float min_size = 0.5f;//Tamanho mínimo de um asteroide
    public float max_size = 1.5f;//Tamanho máximo de um asteroide
    public float size = 1.0f;//Tamanho padrão de um asteroide
    public float speed = 6;//Velocidade base do asteroide

    private SpriteRenderer sprite;//Componente SpriteRenderer
    private Vector3 move_dir;//Direção do movimento do asteroide
    private Camera cam;//Câmera
    private Renderer render;//Componente Renderer
    private Vector3 new_pos;//Variável para a nova posição calculada durante o wrapping
    private float cam_height;//Altura da visão da câmera
    private float cam_width;//Largura da visão da câmera
    private bool wrapX;//Define se o asteroide está sofrendo wrapping em X
    private bool wrapY;//Define se o asteroide está sofrendo wrapping em Y
    private bool been_seen = false;//Define se o asteroide já entrou na tela alguma vez

    private void Awake()
    {
        //Obtém os componentes necessários
        sprite = GetComponent<SpriteRenderer>();  
        render = GetComponent<Renderer>();
        cam = Camera.main;    
    }

    // Start is called before the first frame update
    private void Start()
    {
        //Define a alutra e largura de visão da câmera
        cam_height = 2f * cam.orthographicSize;
        cam_width = cam_height * cam.aspect;

        //Define a direção de movimento subtraindo a posição atual de um ponto dentro da tela
        move_dir = (transform.position - new Vector3(Random.Range(0, cam_width), Random.Range(0, cam_height), 0)).normalized;

        //Define randomicamente o tamanho dentro dos limites mínimo e máximo
        size = Random.Range(min_size, max_size);

        //Define randomicamente o sprite do asteroide dentro do vetor de sprites
        sprite.sprite = sprite_options[Random.Range(0, sprite_options.Length)];

        //Define randomicamente a rotação do asteroide
        transform.eulerAngles = new Vector3(0, 0, Random.Range(0,360));

        //Transforma o tamanho do asteroide para o tamanho calculado anteriormente
        transform.localScale = new Vector3(size, size, size);

        //Define a velocidade de maneira inversamente proporcional ao tamanho
        speed /= 1.5f * size;
    }

    // Update is called once per frame
    void Update()
    {
        //Move o asteroide na direção e velocidade especificados
        transform.position -= move_dir * speed * Time.deltaTime;

        //Quando o objeto se torna visível para o jogador define-se que ele já foi visto
        if(render.isVisible)
            been_seen = true;

        //A partir do momento em que o asteroide é visto pela primeira vez, ele está rápido a sofrer o wrapping
        if(been_seen)
        {
            //Se o asteroide saiu da tela
            if(!render.isVisible)
            {
                //Copia a posição atual para a nova posição
                new_pos = transform.position;

                //Define o viewport da camera
                var viewportPosition = cam.WorldToViewportPoint(transform.position);

                //Se não estiver sofrendo wrapping em X e estiver fora do viewport (em X)
                if(!wrapX && (viewportPosition.x > 1 || viewportPosition.x < 0))
                {
                    //Inverte a posição em X para o outro lado da tela e ativa o booleano de wrapping em X
                    new_pos.x = -new_pos.x;
                    wrapX = true;
                }
                //Se não estiver sofrendo wrapping em Y e estiver fora do viewport (em Y)
                if(!wrapY && (viewportPosition.y > 1 || viewportPosition.y < 0))
                {
                    //Inverte a posição em X para o outro lado da tela e ativa o booleano de wrapping em X
                    new_pos.y = -new_pos.y;
                    wrapY = true;
                }
                //Atualiza a posição do asteroide
                transform.position = new_pos;
            }
            //Se o asteroide estiver sendo visto, desativa os booleanos de wrapping
            else
            {
                wrapX = false;
                wrapY = false;
            }
        }
    }

    //Destroi o asteroide ao colidir com um projétil
    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "Bullet")
            Destroy(this.gameObject);
    }

}
