using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscopetaScript : MonoBehaviour
{
    [Header("Propiedades")]
    public float dmg = 10f;
    public float fuerzaArma = 40f;
    public float rango = 40f;
    public float cadencia = .6f;
    public float dispersionBalas = 0.4f;
    float tiempoCadencia;

    [Header("Objetos referenciados")]

    Animator animator;
    public ParticleSystem flash;

    GameController gameController;
    public Camera[] fpsCam;
    public GameObject flare;



    // Update is called once per frame

    private void Start()
    {
        gameController = GameController.instance;
        animator = GetComponent<Animator>();

        tiempoCadencia = Time.time;
    }

    void Update()
    {
        if (Input.GetButton("Fire1") && (tiempoCadencia + cadencia <= Time.time))
        {
            int fragmentosEscopeta = Random.Range(5, 8);
            gameController.PlayDisparo();
            animator.SetBool("shoot", true);
            for (int i = 0; i < fragmentosEscopeta; i++)
            {
                Shoot();
            }
            
            tiempoCadencia = Time.time;
        }
        else
        {
            animator.SetBool("shoot", false);
        }
    }

    void Shoot()
    {
        flash.Play();
        
        RaycastHit hit;
        Transform puntoColision = fpsCam[gameController.GetEstadoCam()].transform;
        Vector3 direccionDisparo = puntoColision.transform.forward;
        direccionDisparo.x += Random.Range(-dispersionBalas, dispersionBalas);
        direccionDisparo.y += Random.Range(-dispersionBalas, dispersionBalas);

        if (Physics.Raycast(puntoColision.position, direccionDisparo, out hit, rango))
        {
            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDamage(dmg);
            }
            if (hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * fuerzaArma);
            }
        }
        GameObject efectoDisparo = Instantiate(flare, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(efectoDisparo, 0.25f);
    }
}
