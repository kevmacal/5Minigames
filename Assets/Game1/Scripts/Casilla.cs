using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Casilla : MonoBehaviour
{
    [Header("Componentes UI")]
    [SerializeField] private Button boton;
    [SerializeField] private TextMeshProUGUI textoNumero;
    [SerializeField] Sprite landmineIcon;
    [SerializeField] AudioSource explosionSound;

    // Coordenadas en la matriz
    public int Fila { get; private set; }
    public int Columna { get; private set; }
    public bool IsLandMine { get; set; }
    public bool IsRevealed { get; private set; }
    public int CountMines { get; set; }

    private TableroManager _tableroManager;

    public void Configurar(int fila, int columna, TableroManager manager)
    {
        Fila = fila;
        Columna = columna;
        IsRevealed = false;
        IsLandMine= false;
        _tableroManager = manager;

        // Limpiamos la UI de la casilla
        textoNumero.text = "";

        // Asignamos la acción del toque por código
        boton.onClick.RemoveAllListeners();
        boton.onClick.AddListener(OnCasillaPresionada);
    }
    private void OnCasillaPresionada()
    {
        if (IsRevealed) return;
        _tableroManager.CasillaTocada(Fila, Columna);
    }
    public void DesactivarCasilla()
    {
        boton.interactable = false;
    }

    public void MostrarCasilla()
    {
        IsRevealed = true; //Para evitar el reproceso de revelar
        boton.interactable = false; //Se desactiva el boton para que no siga dando clic indefinidamente
        if (IsLandMine)
        {
            boton.image.sprite = landmineIcon;
            explosionSound.Play();
            //textoNumero.text = "";
            //textoNumero.color = Color.pink;
        }
        else
        {
            textoNumero.text = CountMines > 0 ? CountMines.ToString() : "";
            textoNumero.color = ObtenerColorMineAround(CountMines);
        }
    }
    public Color ObtenerColorMineAround(int cantidad)
    {
        switch (cantidad)
        {
            case 1: return Color.blue;
            case 2: return new Color(0f, 0.5f, 0f); // Verde
            case 3: return Color.red;
            case 4: return new Color(0.5f, 0f, 0.5f); // Morado
            default: return Color.black;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
