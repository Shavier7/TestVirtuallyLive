using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZigZagCubes : MonoBehaviour
{
    [Header("Propiedades")]
    [SerializeField]
    public float fuerzaEmpuje;
    public float vidaQueQuita = 20;
    int ladoAMover;
    int numeroPasosAdelante;
    int numeroPasosLado;
    int direccionATomar;    //1 --> forward, 2-->back, 3--> right, 4--> left
    bool llegoEjeX = false;
    bool llegoEjeZ = false;
    int movimientoAleatorio;

    //Obejtos referenciados
    GameController gameController;
    Vector3 puntoDondeAtacar;
    Rigidbody rigid;
    

    

    // Start is called before the first frame update
    void Start()
    {
        numeroPasosAdelante = 0;
        numeroPasosLado = 0;
        rigid = GetComponent<Rigidbody>();
        gameController = GameController.instance;
        puntoDondeAtacar = gameController.transform.position;
        StartCoroutine(mueveCubo());

    }

    IEnumerator mueveCubo()
    {
        yield return new WaitForSeconds(1.2f);
        
        if (numeroPasosLado == 0 && numeroPasosAdelante == 0)
        {
            numeroPasosAdelante = Random.Range(2, 5);
            numeroPasosLado = Random.Range(1, 3);
            ladoAMover = Random.Range(-1, 1);
            if (ladoAMover == 0) { ladoAMover = 1; }


            //----------------------------División del plano en 4 triangulos-----------------

            Vector3 vector = transform.position - puntoDondeAtacar;     //Con esto calculamos el angulo de donde está el objeto
            vector.Normalize();                                         //Dividiendo así el plano en 4 partes triangulares para
            float angle = Mathf.Atan2(vector.x, vector.z)*Mathf.Rad2Deg;    //optimizar el movimiento del cuadrado hacia la plataforma 


            if (angle>-45 && angle<45)
            {
                Debug.Log("Vas a moverte en -z " + angle);
                direccionATomar = 2;    //-z
            }else if(angle>-135 && angle < -45)
            {
                Debug.Log("Vas a moverte en +x " +angle );
                direccionATomar = 3;   //+x
            }
            else if (angle>-180&& angle<-135)
            {
                Debug.Log("Vas a moverte en +z " + angle);
                direccionATomar = 1;   //+z
            }
            else
            {
                Debug.Log("Vas a moverte en -x " + angle);
                direccionATomar = 4;    //-x
            }
            //----------------------------------------------------------------------------
        }

        if (numeroPasosAdelante > 0)
        {
            MueveHaciaEscenario();
        }
        else
        {
            if (numeroPasosLado > 0)
            {
                MueveAUnLado();
            }

        }
        

        StartCoroutine(mueveCubo());

    }


    void MueveAUnLado()
    {
        if (direccionATomar == 1 || direccionATomar == 2)
        {
            rigid.AddForce(Vector3.right * ladoAMover * fuerzaEmpuje);
        }

        else if (direccionATomar == 3 || direccionATomar == 4)
        {
            rigid.AddForce(Vector3.forward * ladoAMover * fuerzaEmpuje);
        }
        numeroPasosLado--;
    }


    void MueveHaciaEscenario()
    {
        if (direccionATomar == 1)
        {
            rigid.AddForce(Vector3.forward * fuerzaEmpuje);
        }
        else if (direccionATomar == 2)
        {
            rigid.AddForce(Vector3.back * fuerzaEmpuje);
        }
        else if (direccionATomar == 3)
        {
            rigid.AddForce(Vector3.right * fuerzaEmpuje);
        }
        else
        {

            rigid.AddForce(Vector3.left * fuerzaEmpuje);
        }

        numeroPasosAdelante--;
        
        
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
