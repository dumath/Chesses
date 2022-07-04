using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SceneController : MonoBehaviour
{
    #region Components of Events.
    public const string CHANGE_STEP = "CHANGE STEP";

    private static void changeStep()
    {
        ChessWizard.Broadcast(Move.CHESSER_STEP);
        ChessWizard.Broadcast(Enemy.ENEMY_STEP);
    }
    #endregion

    #region Fields of Plane.
    public const float PLAYING_OBJECTS_Z = -1.0f; // Глубина всех взаимодействующих объектов.
    private static float _heigthCell; // Высота клетки.
    private static float _widthCell; // Ширина клетки.
    public int CountCellsX = 6; // Количество клеток по X оси. Future: Масштабируемость.
    public int CountCellsY = 5; //Количество клекток по Y оси. Future: Масштабируемость.
    private float difficult;
    #endregion

    #region Referrences fields.
    [SerializeField] private Sprite[] sprites; // Коллекция спрайтов.
    [SerializeField] private GameObject chesser; // "Пустая" шашка.
    [SerializeField] private GameObject enemy; // Шашка противника.
    [SerializeField] private GameObject point; // Точка, в которую можно поставить шашку.
    [SerializeField] private GameObject chessDesc; // Шахматная доска.
    #endregion

    #region Additional propertyes.
    public static float HeigthCell { get => _heigthCell; }
    public static float WidthCell { get => _widthCell; }
    #endregion

    #region Awake, Start, Update, LateUpdate, OnDestroy.
    void Awake()
    {
        ChessWizard.AddListener(CHANGE_STEP, changeStep);
    }

    // Start is called before the first frame update
    void Start()
    {
        difficult = PlayerPrefs.GetInt(UIController.DIFFICULT);
        SpriteRenderer sr = chessDesc.GetComponent<SpriteRenderer>();
        _widthCell = sr.sprite.texture.width / sr.sprite.pixelsPerUnit / CountCellsX;
        _heigthCell = sr.sprite.texture.height / sr.sprite.pixelsPerUnit / CountCellsY;
        StartGame();
    }

    // Update is called once per frame
    void Update()
    {

    }
    #endregion

    #region Methods of Game initialize.
    /// <summary>
    /// Вызывается при загрузке игры. Расставляет "Point", "Enemy", "Chesser" объекты.
    /// </summary>
    public void StartGame()
    {
        float x = _widthCell / 2 * CountCellsX - _widthCell / 2;
        float y = _heigthCell / 2 * CountCellsY - _heigthCell / 2;
        float z = PLAYING_OBJECTS_Z;
        Vector3 position = new Vector3(-x, y, z);
        GameObject newPoint = null;
        for (int startY = 0; startY < 5; startY++)
        {
            for (int startX = 0; startX < 3; startX++)
            {
                newPoint = Instantiate(point);
                newPoint.transform.position = position;
                if (startY < difficult)
                    SetEnemies(position, newPoint);
                if (startY > 3)
                    SetChessers(position, newPoint);
                position.x += _widthCell * 2;
            }
            position = newPoint.transform.position;
            position.x = -position.x;
            position.y -= _heigthCell;
        }
        chesser.SetActive(false);
        enemy.SetActive(false);
        point.SetActive(false);
        ChessWizard.Broadcast(Move.CHESSER_STEP);
    }

    /// <summary>
    /// Вызывается методом StartGame().
    /// </summary>
    /// <param name="position"> Позиция, в которой будет находиться "Enemy" объект. </param>
    /// <param name="point"> Связанная с "Enemy" объектом точка. </param>
    /// <param name="firstEnemy"> Для первой инициализации. </param>
    public void SetEnemies(Vector3 position, GameObject point)
    {
        GameObject newEnemy = Instantiate(enemy);
        newEnemy.transform.position = position;
        newEnemy.GetComponent<SpriteRenderer>().sprite = sprites[1];
        point.GetComponent<Point>().IsEmpty = false;
    }

    /// <summary>
    /// Вызывается методом StartGame().
    /// </summary>
    /// <param name="position"> Позиция, в которой будет находиться "Chesser" объект. </param>
    /// <param name="point"> Связанная с "Enemy" объектом точка. </param>
    public void SetChessers(Vector3 position, GameObject point)
    {
        GameObject newChesser = Instantiate(chesser);
        newChesser.transform.position = position;
        newChesser.GetComponent<SpriteRenderer>().sprite = sprites[0];
        point.GetComponent<Point>().IsEmpty = false;
    }
    #endregion

    #region Debug.
    // Отключенный метод. Тестовый. Определение цвета пикселы (Пипетка).
    public Color GetPixel()
    {
        Vector2 colorPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float pixelPerUnit = chessDesc.GetComponent<SpriteRenderer>().sprite.pixelsPerUnit;
        int x = Mathf.RoundToInt(colorPoint.x * pixelPerUnit);
        int y = Mathf.RoundToInt(colorPoint.y * pixelPerUnit);
        x += chessDesc.GetComponent<SpriteRenderer>().sprite.texture.width / 2;
        y += chessDesc.GetComponent<SpriteRenderer>().sprite.texture.height / 2;
        return chessDesc.GetComponent<SpriteRenderer>().sprite.texture.GetPixel(x, y);
    }
    #endregion
}
