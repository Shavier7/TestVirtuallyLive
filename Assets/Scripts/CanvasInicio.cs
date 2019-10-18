using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasInicio : MonoBehaviour
{
    public GameObject canvasPrincipal;
    public GameObject canvasTutorial;

    // Start is called before the first frame update
 
    public void cambiaATutorial()
    {
        canvasPrincipal.SetActive(false);
        canvasTutorial.SetActive(true);
    }

    public void IniciarJuego()
    {
        SceneManager.LoadScene("CampoBatalla");
    }
}
