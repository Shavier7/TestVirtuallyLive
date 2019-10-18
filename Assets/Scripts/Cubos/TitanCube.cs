using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitanCube : MonoBehaviour
{
    [Header("Propiedades")]
    [SerializeField]
    public float fuerzaEmpuje;
    public float vidaQueQuita = 20;
    [SerializeField]
    public float retardoEntreMovimiento;
    bool llegoEjeX = false;
    bool llegoEjeZ = false;
    int movimientoAleatorio;
    int ladoAMover;

    //Objetos referenciados
    GameController gameController;
    Vector3 puntoDondeAtacar;
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

        Vector3 vector = transform.position - puntoDondeAtacar;     //Con esto calculamos el angulo de donde está el objeto
        vector.Normalize();                                         //Dividiendo así el plano en 4 partes triangulares para
        float angle = Mathf.Atan2(vector.x, vector.z) * Mathf.Rad2Deg;    //optimizar el movimiento del cuadrado hacia la plataforma 


        if (angle > -45 && angle < 45)
        {
            
            ladoAMover = 2;    //-z
        }
        else if (angle > -135 && angle < -45)
        {
            
            ladoAMover = 3;   //+x
        }
        else if (angle > -180 && angle < -135)
        {

            ladoAMover = 1;   //+z
        }
        else
        {

            ladoAMover = 4;    //-x
        }

        MueveHaciaDelante();

        StartCoroutine(mueveCubo());

    }

    void MueveHaciaDelante()
    {
        if (ladoAMover == 1)
        {
            rigid.AddForce(Vector3.forward * fuerzaEmpuje);
        }
        else if (ladoAMover == 2)
        {
            rigid.AddForce(Vector3.back * fuerzaEmpuje);
        }
        else if (ladoAMover == 3)
        {
            rigid.AddForce(Vector3.right * fuerzaEmpuje);
        }
        else
        {

            rigid.AddForce(Vector3.left * fuerzaEmpuje);
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
