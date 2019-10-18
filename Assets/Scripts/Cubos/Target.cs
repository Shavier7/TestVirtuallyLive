using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{

    MeshRenderer colorEnemigo;
    [SerializeField]
    public float vidaMaxima;
    float vidaActual;
    bool muerto=false;

    

    private void Start()
    {
        
        vidaActual = vidaMaxima;
        
        colorEnemigo = GetComponent<MeshRenderer>();
    }


    public void TakeDamage(float dmgTaken)
    {
        if (vidaActual >= 0)
        {
            vidaActual -= dmgTaken;

            colorEnemigo.materials[0].color = new Color(colorEnemigo.materials[0].color.r, colorEnemigo.materials[0].color.g, colorEnemigo.materials[0].color.b, Mathf.Min(((vidaActual / vidaMaxima) + .25f), 1));
            if (!muerto && vidaActual<=0)
            {
                muerto = true;
                Die();
            }
        }
        
    }
    void Die()
    {
        if (gameObject.tag == "Titan")
        {
            GameController.instance.Win();
        }
        InstanciadorDeCubos.instance.RestarCubosEnEscena();
        Destroy(gameObject);
        
    }
}
