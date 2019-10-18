using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovimientoCubo : MonoBehaviour
{
    [Header("Propiedades")]
    [SerializeField]
    public float fuerzaEmpuje;
    public float vidaQueQuita =20;
    [SerializeField]
    public float retardoEntreMovimiento;
    bool llegoEjeX = false;
    bool llegoEjeZ = false;
    int movimientoAleatorio;

    //Objetos referenciados
    GameController gameController;
    Vector3 puntoDondeAtacar;
    int ladoAMover;
    Rigidbody rigid;

    
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        gameController = GameController.instance;
        puntoDondeAtacar = gameController.transform.position;
        StartCoroutine(mueveCubo());

    }

    IEnumerator mueveCubo()
    {
        yield return new WaitForSeconds(retardoEntreMovimiento);
        movimientoAleatorio = Random.Range(0, 2);
        if (movimientoAleatorio==0 && !llegoEjeX)
        {
            MueveEjeX();
        }else if(!llegoEjeZ)
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
