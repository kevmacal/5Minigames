using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerController : MonoBehaviour
{
    [Header("Puntos de Aparición (5 Spawn Points)")]
    [SerializeField] Transform[] puntosSpawn; // Arrastra aquí los 5 GameObjects

    [Header("Prefabs y Probabilidades")]
    [SerializeField] List<GameObject> listaPrefabs;

    [Header("Configuración de Oleada")]
    [SerializeField]float tiempoEntreSpawns;
    private bool _spawneoActivo = true;
    private int _totalSpawns = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Spawnear());
    }
    private IEnumerator Spawnear()
    {
        while (_spawneoActivo)
        {
            yield return new WaitForSeconds(tiempoEntreSpawns);

            if (puntosSpawn.Length > 0 && listaPrefabs.Count > 0)
            {
                SpawnearObjetoAleatorio();
            }
        
        }
    }
    private void SpawnearObjetoAleatorio()
    {
        // 1. Selecciona uno de los 5 puntos al azar
        int indicePuntoAleatorio = Random.Range(0, puntosSpawn.Length);
        Transform puntoElegido = puntosSpawn[indicePuntoAleatorio];

        // 2. Selecciona un prefab basándose en el peso de probabilidad
        GameObject prefabElegido = SeleccionarPrefabPorPeso();

        // 3. Instancia el prefab en la posición del punto seleccionado
        if (prefabElegido != null)
        {
            Instantiate(prefabElegido, puntoElegido.position, Quaternion.identity);
            _totalSpawns++;
            if (tiempoEntreSpawns>0.3)
            {
                //Spawnea mas rapido cada 3 spawns
                if (_totalSpawns%3==0)
                {
                    tiempoEntreSpawns = tiempoEntreSpawns - 0.1f;
                }
            }
        }
    }
    private GameObject SeleccionarPrefabPorPeso()
    {
        int numeroRandom = Random.Range(0, 20);
        //int pesoAcumulado = 0;

        if (numeroRandom>1)
        {
            return listaPrefabs[1];
        }

        return listaPrefabs[0]; // Por respaldo
    }
    public void DetenerSpawneo()
    {
        _spawneoActivo = false;
        StopAllCoroutines();
        Time.timeScale = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
