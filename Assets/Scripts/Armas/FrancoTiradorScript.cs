using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrancoTiradorScript : MonoBehaviour
{
    [Header("Propiedades")]
    public float dmg = 40f;
    public float fuerzaArma = 100f;
    public float rango = 100f;
    public float cadencia = .2f;
    float dispersionBalasActual;
    public float dispersionZoomBalas = 0;
    public float dispersionSinZoomBalas = 0.4f;
    float tiempoCadencia;
    public int zoomApuntar = 10;

    [Header("Objetos referenciados")]

    Animator animator;
    public ParticleSystem flash;

    GameController gameController;
    public Camera[] fpsCam;
    public GameObject flare;



    // Update is called once per frame

    private void Start()
    {
        animator = GetComponent<Animator>();
        dispersionBalasActual = dispersionSinZoomBalas;
        gameController = GameController.instance;
        gameController.zoomHaCambiado += CambiarDispersion;
        
        tiempoCadencia = Time.time;
    }

    void Update()
    {
        Shoot();
        
        
    }

    void Shoot()
    {
        if (Input.GetButton("Fire1") && (tiempoCadencia + cadencia <= Time.time))
        {
            animator.SetBool("shoot", true);
            flash.Play();
            gameController.PlayDisparo();
            RaycastHit hit;
            Transform puntoColision = fpsCam[gameController.GetEstadoCam()].transform;
            Vector3 direccionDisparo = puntoColision.transform.forward;
            direccionDisparo.x += Random.Range(-dispersionBalasActual, dispersionBalasActual);
            direccionDisparo.y += Random.Range(-dispersionBalasActual, dispersionBalasActual);

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
            tiempoCadencia = Time.time;
        }
        else
        {
            animator.SetBool("shoot", false);
        }
    }

    public void CambiarDispersion()
    {
        if(dispersionBalasActual == dispersionSinZoomBalas)
        {
            dispersionBalasActual = dispersionZoomBalas;
            fpsCam[0].fieldOfView = zoomApuntar;

        }
        else
        {
            dispersionBalasActual = dispersionSinZoomBalas;
            fpsCam[0].fieldOfView = 60;
        }
        
    }
}
