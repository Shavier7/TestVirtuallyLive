using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetTitan : MonoBehaviour
{
    MeshRenderer colorEnemigo;
    [SerializeField]
    public float vidaMaxima;
    float vidaActual;



    private void Start()
    {

        vidaActual = vidaMaxima;

        colorEnemigo = GetComponent<MeshRenderer>();
    }


    public void TakeDamage(float dmgTaken)
    {
        vidaActual -= dmgTaken;
        Debug.Log(vidaActual);
        colorEnemigo.materials[0].color = new Color(colorEnemigo.materials[0].color.r, colorEnemigo.materials[0].color.g, colorEnemigo.materials[0].color.b, Mathf.Min(((vidaActual / vidaMaxima) + .25f), 1));
       

        if (vidaActual <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        GameController.instance.Win();
        Destroy(gameObject);

    }
}
