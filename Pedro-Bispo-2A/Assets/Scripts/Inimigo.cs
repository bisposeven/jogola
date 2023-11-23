using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inimigo : MonoBehaviour
{

    public Rigidbody2D corpoInimigo;
    public float velocidade;
    public float tempoParaVirar;
    public Animator animator;
    private string newState;
    private string CurrentState;

    const string BANDIT_HURT = "HeavyBandit_Hurt";
    const string BANDIT_ATTACK = "HeavyBandit_Attack";
    const string BANDIT_RECOVER = "HeavyBandit_Recover";
    const string BANDIT_IDLE = "HeavyBandit_Idle";
    const string BANDIT_RUN = "HeavyBandit_Run";
    const string BANDIT_DEATH = "HeavyBandit_Death";




    void Start()
    {
        StartCoroutine("MudarDirecao");
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        corpoInimigo.velocity = new Vector2(velocidade, 0);
        
    }
    public  IEnumerator MudarDirecao() {

        velocidade *= -1;

        yield return new WaitForSeconds(tempoParaVirar);

        StartCoroutine("MudarDirecao");
        
        if (velocidade > 0) {
            
            transform.localScale = new Vector3(-1.553217f,1.730903f,1f);
        }
        else 
        {
            transform.localScale = new Vector3(1.553217f,1.730903f,1f);
        }


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

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.gameObject.CompareTag("ataque"))
        {
            Destroy(gameObject);
        }
    }
}
