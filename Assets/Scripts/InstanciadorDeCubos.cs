using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanciadorDeCubos : MonoBehaviour
{
    #region Singleton



    public static InstanciadorDeCubos instance;
    private void Awake()
    {


        if (instance != null)
        {
            Debug.LogWarning("Hay más de un Instanciador!");
        }

        instance = this;
    }



    #endregion

    #region Variables Instanciador
        [SerializeField]
        int numeroMaxCubosEnEscena;
        [SerializeField]
        int cubosAEliminar;
        [SerializeField]
        int rangoInstanciarEnemigos;
        int cubosEliminados;
        float xAleatoria, zAleatoria;
        public int numeroCubosEscena;
    #endregion

    #region Objetos Referenciados
        public List<GameObject> listaCubos;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        numeroCubosEscena = 0;
        StartCoroutine(GeneracionCubos());
    }

    // Update is called once per frame

    IEnumerator GeneracionCubos()
    {
        if (numeroCubosEscena < numeroMaxCubosEnEscena && cubosEliminados<cubosAEliminar)
        {
            GenerarCuboNormal();
        }
        yield return new WaitForSeconds(1.5f);
        if (cubosEliminados >= cubosAEliminar && numeroCubosEscena==0)
        {
            yield return new WaitForSeconds(2);
            GenerarTitan();
        }
        else
        {
            StartCoroutine(GeneracionCubos());
        }
        
    }
    void GenerarCuboNormal()
    {
        int cuboRandom = Random.Range(0, 3);
        xAleatoria = Random.Range(-rangoInstanciarEnemigos, rangoInstanciarEnemigos);
        zAleatoria = Random.Range(-rangoInstanciarEnemigos, rangoInstanciarEnemigos);
        Vector3 posicionInstancia = new Vector3(xAleatoria, listaCubos[cuboRandom].transform.localScale.y / 2, zAleatoria);
        Instantiate(listaCubos[cuboRandom], posicionInstancia, Quaternion.identity);
        numeroCubosEscena++;
    }

    void GenerarTitan()
    {
        xAleatoria = Random.Range(-rangoInstanciarEnemigos, rangoInstanciarEnemigos);
        if (xAleatoria < 3 && xAleatoria > -3)
        {
            xAleatoria = 30;
        }

        zAleatoria = Random.Range(-60, 61);
        if (zAleatoria < 2 && zAleatoria > -2)
        {
            zAleatoria = -30;
        }
        Vector3 posicionInstancia = new Vector3(xAleatoria, listaCubos[3].transform.localScale.y / 2, zAleatoria);
        Instantiate(listaCubos[3], posicionInstancia, Quaternion.identity);
    }

    public void RestarCubosEnEscena()
    {
        numeroCubosEscena--;
        cubosEliminados++;
    }

    
}
