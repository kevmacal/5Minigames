using UnityEngine;

public class TableroManager : MonoBehaviour
{
    [Header("Configuración del Tablero")]
    [SerializeField] int filas = 8;
    [SerializeField] int columnas = 7;
    [SerializeField] int nMinas;
    [SerializeField] GameObject casillaPrefab;
    [SerializeField] Transform panelTablero;

    [Header("Canvas de Control")]
    [SerializeField] TopUI controlCanvasTop;

    private Casilla[,] _cuadroCasillas;
    private bool _isWinGame = false;

    public void OnEnable()
    {
        TopUI.OnTimeOut += HandleTimeOut;
    }
    private void OnDisable()
    {
        TopUI.OnTimeOut -= HandleTimeOut;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerarTablero();
    }
    private void GenerarTablero()
    {
        _cuadroCasillas = new Casilla[filas, columnas];

        //Eliminar todas las casillas (Por siaca hubiera)
        foreach (Transform child in panelTablero)
        {
            Destroy(child.gameObject);
        }

        //Bucle anidado para instanciar las 7x8 casillas
        for (int f = 0; f < filas; f++)
        {
            for (int c = 0; c < columnas; c++)
            {
                // Instanciamos el prefab como hijo del Grid Layout Group
                GameObject nuevaCasillaGO = Instantiate(casillaPrefab, panelTablero);

                Casilla casillaScript = nuevaCasillaGO.GetComponent<Casilla>();

                if (casillaScript != null)
                {
                    casillaScript.Configurar(f, c, this);
                    _cuadroCasillas[f, c] = casillaScript;
                }
            }
        }

        //Debug.Log($"Tablero de {filas}x{columnas} generado exitosamente con {filas * columnas} casillas.");
        //Completo el tablero se hacen las minas aleatorias. Siempre y cuando el numero sea mayor a cero y menor al 50%
        if (nMinas>0&&nMinas<((filas*columnas)/2))
        {
            PutRandomLandMines(nMinas);
        }
        else
        {
            PutRandomLandMines(filas);
        }
        CalcularMinasVecinas();
    }
    private void PutRandomLandMines(int cantidadMinas)
    {
        int minasColocadas = 0;

        while (minasColocadas < cantidadMinas)
        {
            int filaRandom = Random.Range(0, filas);
            int columnaRandom = Random.Range(0, columnas);

            //Si la casilla no tiene mina se coloca la mina
            if (!_cuadroCasillas[filaRandom, columnaRandom].IsLandMine)
            {
                _cuadroCasillas[filaRandom, columnaRandom].IsLandMine = true;
                minasColocadas++;
            }
        }
    }
    private void CalcularMinasVecinas()
    {
        for (int f = 0; f < filas; f++)
        {
            for (int c = 0; c < columnas; c++)
            {
                if (_cuadroCasillas[f, c].IsLandMine) continue;

                int minasAlrededor = 0;

                // Revisar los 8 vecinos de la casilla actual [f, c]
                for (int df = -1; df <= 1; df++)
                {
                    for (int dc = -1; dc <= 1; dc++)
                    {
                        if (df == 0 && dc == 0) continue; // Salta la casilla misma

                        int vecinoFila = f + df;
                        int vecinoColumna = c + dc;

                        // Verificar que el vecino esté dentro de los límites de la matriz
                        if (vecinoFila >= 0 && vecinoFila < filas && vecinoColumna >= 0 && vecinoColumna < columnas)
                        {
                            if (_cuadroCasillas[vecinoFila, vecinoColumna].IsLandMine)
                            {
                                minasAlrededor++;
                            }
                        }
                    }
                }

                _cuadroCasillas[f, c].CountMines = minasAlrededor;
            }
        }
    }

    public void CasillaTocada(int fila, int columna)
    {
        //Debug.Log($"Jugador tocó la casilla en la posición: [{fila}, {columna}]");
        Casilla casilla = _cuadroCasillas[fila,columna];
        if (casilla.IsRevealed) return;

        // Ejemplo de revelar casilla
        // _cuadroCasillas[fila, columna].MostrarCasilla("1", Color.blue);
        _cuadroCasillas[fila, columna].MostrarCasilla();
        if (casilla.IsLandMine)
        {
            controlCanvasTop.MineTouched();
            if (!controlCanvasTop.IsAlife())
            {
                ShowAllMines();
                GameOver();
            }            
        }
        else
        {
            if (casilla.CountMines==0)
            {
                ShowNeighbour(fila,columna);
            }
            CheckGame();
        }
    }
    private void CheckGame()
    {
        int totalCount = 0;
        int revealedCount = 0;
        int minesCount = 0;
        
        for (int f = 0; f < filas; f++)
        {
            for (int c = 0; c < columnas; c++)
            {
                totalCount++;
                if(_cuadroCasillas[f, c].IsRevealed&&!_cuadroCasillas[f, c].IsLandMine)
                {
                    revealedCount++;
                }
                if (_cuadroCasillas[f,c].IsLandMine)
                {
                    minesCount++;
                }
            }
        }
        if (totalCount==(revealedCount+minesCount))
        {
            _isWinGame = true;
            //Debug.Log("Win Game");
            controlCanvasTop.WinGame();
        }
    }
    private void ShowNeighbour(int f, int c)
    {
        for (int df = -1; df <= 1; df++)
        {
            for (int dc = -1; dc <= 1; dc++)
            {
                int vf = f + df;
                int vc = c + dc;

                if (vf >= 0 && vf < filas && vc >= 0 && vc < columnas)
                {
                    Casilla vecina = _cuadroCasillas[vf, vc];
                    if (!vecina.IsRevealed && !vecina.IsLandMine)
                    {
                        vecina.MostrarCasilla();
                        /*Activar si se desea mostrar mas casillas*/
                        //if (vecina.CountMines == 0)
                        //{
                            //RevelarCasillasVaciasVecinas(vf, vc); // Algoritmo de inundación (Flood Fill)
                        //}
                    }
                }
            }
        }
    }
    private void ShowAllMines()
    {
        for (int f = 0; f < filas; f++)
        {
            for (int c = 0; c < columnas; c++)
            {
                if (_cuadroCasillas[f, c].IsLandMine)
                {
                    _cuadroCasillas[f, c].MostrarCasilla();
                }
            }
        }
    }
    private void HandleTimeOut()
    {
        GameOver();
    }
    private void GameOver()
    {
        ShowAllMines();
        for (int f = 0; f < filas; f++)
        {
            for (int c = 0; c < columnas; c++)
            {
                _cuadroCasillas[f, c].DesactivarCasilla();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
