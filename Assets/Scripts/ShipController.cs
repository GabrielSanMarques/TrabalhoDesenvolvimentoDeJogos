using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipController : MonoBehaviour
{
    private Rigidbody2D rigidbody;//Componente Rigidbody
    private Renderer render;//Componente Renderer
    private bool moving;//Define se a nave está se movendo
    private bool wrapX;//Define se a nave está sofrendo wrapping em X
    private bool wrapY;//Define se a nave está sofrendo wrapping em Y
    private float rot_dir;//Direção de rotação
    private Vector3 new_pos;//Variável para a nova posição calculada durante o wrapping

    public GameObject bullet;//Prefab do projétil
    public float rot_speed = 0.1f;//Velocidade de rotação da nave
    public float speed = 1.0f;//Velocidade de movimento da nave
    public Camera cam;//Câmera

    //Cria um projétil (atira)
    private void Shoot()
    {
        Instantiate(bullet, transform.position, transform.rotation);
    }

    private void Awake()
    {
        //Obtém os componentes necessários
        rigidbody = GetComponent<Rigidbody2D>();
        render = GetComponent<Renderer>();
        cam = Camera.main;
    }

    // Update is called once per frame
    private void Update()
    {
        //Define se a nave está se movendo com base na entrada do usuário
        if (Input.GetKey(KeyCode.UpArrow))
            moving = true;
        else
            moving = false;

        //Define a rotação para a esquerda ou para a direita, ou se não há rotação
        if (Input.GetKey(KeyCode.RightArrow))
            rot_dir = -1.0f;
        else if (Input.GetKey(KeyCode.LeftArrow))
            rot_dir = 1.0f;
        else
            rot_dir = 0.0f;

        //Atira ao pressionar Espaço
        if (Input.GetKeyDown(KeyCode.Space))
            Shoot();

        //Se a nave saiu da tela
        if(!render.isVisible)
        {
            //Copia a posição atual para a nova posição
            new_pos = transform.position;

            //Define o viewport da câmera
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
                //Inverte a posição em Y para o outro lado da tela e ativa o booleano de wrapping em Y
                new_pos.y = -new_pos.y;
                wrapY = true;
            }
            //Atualiza a posição da nave
            transform.position = new_pos;
        }
        //Se o asteroide estiver sendo visto, desativa os booleanos de wrapping
        else
        {
            wrapX = false;
            wrapY = false;
        }
    }

    private void FixedUpdate()
    {
        //Se a nave estiver se movendo, adiciona uma força baseada na velocidade
        if (moving)
            rigidbody.AddForce(transform.up * speed);
        
        //Se a nave estiver rotacionando, adiciona um torque baseado na direção e velocidade da rotação
        if (rot_dir != 0.0f)
            rigidbody.AddTorque(rot_speed * rot_dir);
    }
    
    //Ao colidir com um asteroid fica inativo e recarrega a cena após 3 segundos
    private void OnCollisionEnter2D(Collision2D col) {
        if(col.gameObject.tag == "Asteroid")
            gameObject.SetActive(false);
            Invoke("ReloadScene", 3f);
    }

    //Recarrega a cena
    private void ReloadScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
