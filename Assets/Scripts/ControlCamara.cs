using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlCamara : MonoBehaviour
{


    #region Variables de Control de Cámara
    
    Vector2 posicionRaton;
    Vector2 smoothCamara;
    public float sesibilidad = 5f;
    public float smoothing = 2f;
    Vector2 movimientoRaton;
    public bool esCamaraAtras;
    bool permitirMovimiento=true;

    #endregion

    
    GameObject personaje;
    GameController gameCont;
    
    

    

    private void Start()
    {
        gameCont = GameController.instance;
        gameCont.elJuegoSeHaPausado += juegoPausado;
        personaje = this.transform.parent.gameObject;
        Cursor.lockState = CursorLockMode.Locked;
    }


    void juegoPausado()
    {
        permitirMovimiento = !permitirMovimiento;
    }

    private void Update()
    {
        if (permitirMovimiento)
        {


            movimientoRaton = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            smoothCamara.x = Mathf.Lerp(smoothCamara.x, movimientoRaton.x, 1f / smoothing);
            smoothCamara.y = Mathf.Lerp(smoothCamara.y, movimientoRaton.y, 1f / smoothing);
            posicionRaton += smoothCamara;
            transform.localRotation = Quaternion.AngleAxis(-posicionRaton.y, Vector3.right);
            personaje.transform.localRotation = Quaternion.AngleAxis(posicionRaton.x, personaje.transform.up);
        }

        

        
    }

}
