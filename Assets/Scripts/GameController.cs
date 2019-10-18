using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameController : MonoBehaviour
{

    #region Singleton

    

    public static GameController instance;
    private void Awake()
    {
        Time.timeScale = 1;


        if (instance != null)
        {
            Debug.LogWarning("Hay más de un GameController!");
        }

        instance = this;
        
    }



    #endregion

    #region UI
        [Header("UI")]
        public Text winText;
        public GameObject menuFin;
        public GameObject menuPausa;
        public Image vidaActual;
    #endregion

    #region Variables del Controller

        [SerializeField]
        float VidaJugador;
        int estadoCamara;  //0 primera persona y 1 tercera persona
        bool zoom3Persona = false;
        Vector3 offsetArma;
        int armaActual = 0; //0 ametralladora, 1 escopeta, 2 francotirador
        bool estasApuntando;
        bool juegoEnPausa = false;
    #endregion

    #region Objetos Referenciados
        [Header("Objetos Referenciados")]
        public Camera[] camaras;
        public Transform personaje;
        public GameObject[] armas;
        public GameObject mirilla;
        public AudioClip shootSound;
        AudioSource audioSource;

    #endregion
    #region Eventos Para suscribir
        public delegate void CambioZoom();
        public CambioZoom zoomHaCambiado;

        public delegate void SeHaPausado();
        public SeHaPausado elJuegoSeHaPausado;
    #endregion


    void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
        estasApuntando = false;
        camaras[0].enabled = true;
        camaras[1].enabled = false;
        camaras[2].enabled = false;
        
        offsetArma = new Vector3(.6f, 0, 0.2f);
        estadoCamara = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!estasApuntando)
        {
            CambiarCamara();
            CambiarArma();
        }
        Pausa();
        HacerZoom();
       
    }

    void HacerZoom()
    {
        if (Input.GetButtonDown("Fire2") && armaActual == 2)
        {
            if (estadoCamara == 1 && !estasApuntando)
            {
                camaras[1].enabled = false;

                camaras[0].enabled = true;
                zoom3Persona = true;
                estadoCamara = 0;
                zoomHaCambiado.Invoke();
            }
            else if (estadoCamara == 0 && estasApuntando && zoom3Persona)
            {
                zoomHaCambiado.Invoke();
                estadoCamara = 1;
                zoom3Persona = false;

                camaras[0].enabled = false;

                camaras[1].enabled = true;

            }
            else if (estadoCamara == 0)
            {
                zoomHaCambiado.Invoke();

            }

            estasApuntando = !estasApuntando;

        }
    }
    void CambiarCamara()
    {
        if (Input.GetKey(KeyCode.A) && !camaras[2].enabled)
        {
            camaras[0].enabled = false;
            camaras[1].enabled = false;
            camaras[2].enabled = true;
            mirilla.SetActive(false);
        }else if (Input.GetKeyUp(KeyCode.A))
        {
            mirilla.SetActive(true);
            camaras[2].enabled = false;
            camaras[estadoCamara].enabled = true;
        }
        else
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (estadoCamara == 0)
            {
                
                camaras[0].enabled = false;
                camaras[1].enabled = true;
                estadoCamara = 1;
            }
            else
            {
                camaras[1].enabled = false;
                camaras[0].enabled = true;
               
                estadoCamara = 0;
            }

        }
    }
    void CambiarArma()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            if (armaActual == 2)
            {
                armaActual = 0;
            }
            else
            {
                armaActual++;
            }
            
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            if (armaActual == 0)
            {
                armaActual = 2;
            }
            else
            {
                armaActual--;
            }
            
        }

        if (Input.GetKeyDown("1"))
        {
            armaActual = 0;
        }
        if (Input.GetKeyDown("2"))
        {
            armaActual = 1;

        }
        if (Input.GetKeyDown("3"))
        {
            armaActual = 2;

        }

        EquiparArma(armaActual);

    }

    void EquiparArma(int armaAEquipar)
    {
        for(int i = 0; i < armas.Length; i++)
        {
            if (i == armaAEquipar)
            {
                armas[i].SetActive(true);
            }
            else
            {
                armas[i].SetActive(false);
            }
            
        }
    }

    public int GetEstadoCam()
    {
        return estadoCamara;
    }

    public void PlayDisparo()
    {
        audioSource.PlayOneShot(shootSound);
    }

    public void RestarVida(float vidaRestada)
    {
        InstanciadorDeCubos.instance.RestarCubosEnEscena();
        VidaJugador -= vidaRestada;
        vidaActual.fillAmount =Mathf.Max(0, (VidaJugador / 100));
        if (VidaJugador <= 0)
        {
            
            menuFin.gameObject.SetActive(true);
            winText.text = "HAS PERDIDO";
            Cursor.lockState = CursorLockMode.None;
            elJuegoSeHaPausado.Invoke();
            Time.timeScale = 0;

        }
    }

    public void Win()
    {
        
        menuFin.gameObject.SetActive(true);
        elJuegoSeHaPausado.Invoke();
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0;

    }

    public void Reiniciar()
    {
        SceneManager.LoadScene("CampoBatalla"); 
    }

    public void VolverAInicio()
    {
        SceneManager.LoadScene("Inicio");
    }

    public void Pausa()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !menuFin.activeInHierarchy)
        {
            if (!juegoEnPausa)
            {
                Time.timeScale = 0;
                menuPausa.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                juegoEnPausa = true;
            }
            else
            {
                menuPausa.SetActive(false);

                Time.timeScale = 1;
                juegoEnPausa = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
            elJuegoSeHaPausado.Invoke();
        }
    }
}
