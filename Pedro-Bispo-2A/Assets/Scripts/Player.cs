using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public Rigidbody2D meuCorpo;
    public float velocidade;
    public float direcao;
    public Animator animator;
    public float inputAtaque;

    //animações
    const string PLAYER_DEATH = "death";
    const string PLAYER_RUN = "run";
    const string PLAYER_IDLE = "idle";
    const string PLAYER_JUMP = "jump";
    const string PLAYER_ATTACK = "attack";
    public string newState;
    public string CurrentState;

    //Pulo
    public float inputPulo;
    public float forcaPulo;
    public LayerMask layerChao;
    public float checkRaio;
    public bool noChao;
    public GameObject checkPosicao;


    // ataque
    public bool isAttacking;
    public GameObject checkAtaque;
    public float tempoAtaque;

    //Coletar
    public int coletados;

    //Game Over
    public GameObject painelGameOver;
    public TMP_Text textoColetados;

    // Start is called before the first frame update
    void Start()
    {
        coletados = 0;
        animator = GetComponent<Animator>();
        
    }

   
    // Update is called once per frame
    void Update()
    {
       

        if (isAttacking == false){

            direcao = Input.GetAxis("Horizontal");
            inputPulo = Input.GetAxis("Jump");
            inputAtaque = Input.GetAxis("Fire1");
            noChao = Physics2D.OverlapCircle(checkPosicao.transform.position, checkRaio, layerChao);
            animator = GetComponent<Animator>();

            meuCorpo.velocity = new Vector2(direcao * velocidade, meuCorpo.velocity.y);

            if (inputPulo != 0 && noChao == true)
            {
                meuCorpo.velocity = new Vector2(meuCorpo.velocity.x, forcaPulo);
                ChangeAnimationState("jump");
            }

            if (direcao == 0)
            {
                ChangeAnimationState("idle");
            }
            else
            {
                ChangeAnimationState("run");
            }

            if (direcao < 0) {
                transform.localScale = new Vector3(-1,1,1);
    
            }
            else {
                transform.localScale = new Vector3(1,1,1);
            }  
            if (Input.GetButtonDown("Fire1"))   
        {
            ChangeAnimationState("attack");
            StartCoroutine("atacar");
        }
        } 

        if (animator.GetCurrentAnimatorStateInfo(0).IsName(PLAYER_ATTACK))
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                isAttacking = false;
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("coletavel"))
        {
            coletados++;
            Destroy(collision.gameObject);
        }

        if(collision.CompareTag("inimigo") && isAttacking  == false) {   
            Debug.Log("fudhfudshfu tomei dano");
            ChangeAnimationState("PLAYER_DEATH");
            if (animator.GetCurrentAnimatorStateInfo(0).IsName(PLAYER_DEATH))
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
                    {
                        painelGameOver.SetActive(true);
                        textoColetados.text = "Pontuação: " + coletados;
                    }
                }
    }
    }

    public void ReiniciarJogo()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(checkPosicao.transform.position, checkRaio);
    }

    void ChangeAnimationState(string newState)
    {
        if (CurrentState == newState)
        {
            return;
        }
        animator.Play(newState);
        CurrentState = newState;
    }


    public IEnumerator atacar() 
    {
        checkAtaque.SetActive(true);
        isAttacking = true;
        yield return new WaitForSeconds(tempoAtaque);
        isAttacking = false;
        checkAtaque.SetActive(false);

    }
}