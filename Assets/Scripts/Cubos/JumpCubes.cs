using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCubes : MonoBehaviour
{
    [Header("Propiedades")]
    [SerializeField]
    float fuerzaSalto;
    [SerializeField]
    float fuerzaEmpuje;
    public float vidaQueQuita = 20;
    bool llegoEjeX = false;
    bool llegoEjeZ = false;
    int movimientoAleatorio;
    int probabilidadSalto = 40;     //40% de probabilidad
    int ladoAMover;

    //Objetos referenciados
    GameController gameController;
    Vector3 puntoDondeAtacar;
    Rigidbody rigid;

    
    
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        gameController = GameController.instance;
        puntoDondeAtacar = gameController.transform.position;
        StartCoroutine(mueveCubo());

    }

    IEnumerator mueveCubo()
    {
        
        int random = Random.Range(0, 101);
        if (random <= probabilidadSalto)
        {
            rigid.AddForce(Vector3.up * fuerzaSalto);
            
            Debug.Log("SALTA");
            yield return new WaitForSeconds(1.6f);
        }
        else
        {
            yield return new WaitForSeconds(1.1f);
            
        }
        movimientoAleatorio = Random.Range(0, 2);
        if (movimientoAleatorio == 0 && !llegoEjeX)
        {
            MueveEjeX();
        }
        else if (!llegoEjeZ)
        {
            MueveEjeZ();
        }


        
        StartCoroutine(mueveCubo());

    }

    void MueveEjeX()
    {
        if (transform.position.x < puntoDondeAtacar.x - 0.6f)
        {
            rigid.AddForce(Vector3.right * fuerzaEmpuje);
        }
        else if (transform.position.x > puntoDondeAtacar.x + 0.6f)
        {
            rigid.AddForce(Vector3.left * fuerzaEmpuje);
        }
        else
        {
            llegoEjeX = true;
            MueveEjeZ();
        }
    }
    void MueveEjeZ()
    {
        if (transform.position.z < puntoDondeAtacar.z - 0.6f)
        {
            rigid.AddForce(Vector3.forward * fuerzaEmpuje);
        }
        else if (transform.position.z > puntoDondeAtacar.z + 0.6f)
        {
            rigid.AddForce(Vector3.back * fuerzaEmpuje);
        }
        else
        {
            llegoEjeZ = true;
            MueveEjeX();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Centro")
        {
            gameController.RestarVida(vidaQueQuita);
            Destroy(this.gameObject);

        }
    }
}
