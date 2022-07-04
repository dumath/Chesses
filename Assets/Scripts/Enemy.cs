using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Event of Enemy step
    public const string ENEMY_STEP = "ENEMY STEP";
    private static bool isEnemyStep;
    private static void enemyCanMove()
    {
        isEnemyStep = !isEnemyStep;
        if (isEnemyStep)
            SelectEnemyForStep();

    }
    private void StepSelectedEnemy()
    {
        findNextMove();
    }
    private static void SelectEnemyForStep()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if(enemies == null || enemies.Length == 0)
        {
            Debug.Log("GAME OVER");
            return;
        }
        int index = Random.Range(0, enemies.Length);
        enemies[index].StepSelectedEnemy();
    }
    #endregion

    #region Fields of position
    private const float FORBIDDEN_DISTANCE = 4.0f;
    private const float SEARCH_DISTANCE = 1.0f;
    private float searchDistance; // Дистанция онаружения "Cell", для хода.
    private GameObject currentCell; // "Cell", на которой сейчас стоит "Enemy".
    #endregion

    #region Awake, Start, Update, LateUpdate, OnDestroy
    void Awake()
    {
        ChessWizard.AddListener(ENEMY_STEP, enemyCanMove);
    }
    // Start is called before the first frame update
    void Start()
    {
        searchDistance = SceneController.HeigthCell * 2.0f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    #region Methods of Coordination
    private GameObject getCurrentCell()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, SEARCH_DISTANCE);
        foreach (Collider2D point in collider2Ds)
        {
            if (point.gameObject.GetComponent<Point>() != null)
                return point.gameObject;
        }
        return null;
    }
    public GameObject GetPoint
    {
        get => currentCell;
    }
    #endregion

    #region Eye
    private void findNextMove()
    {
        if (currentCell == null)
            currentCell = getCurrentCell();
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, searchDistance);
        if (colliders != null)
        {
            List<GameObject> points = seeCellObjects(colliders);
            if (points == null)
                ChessWizard.Broadcast(SceneController.CHANGE_STEP);
            else
            {
                GameObject point = selectCell(points);
                if (point != null)
                {
                    NextCell(point);
                    ChessWizard.Broadcast(SceneController.CHANGE_STEP);
                }
                else
                    SelectEnemyForStep();
            }
        }
    }

    private List<GameObject> seeCellObjects(Collider2D[] colliders)
    {
        List<GameObject> points = new List<GameObject>();
        foreach (Collider2D target in colliders)
        {
            if (target.gameObject.GetComponent<Move>() != null)
            {
                GameObject point;
                if (canKill(target, out point))
                {
                    ChesserFinded(target, point);
                    points = null;
                    break;
                }
            }
            else if (target.gameObject.GetComponent<Point>() != null)
                points.Add(target.gameObject);
        }
        return points;
    }
    #endregion

    #region Legs
    private GameObject selectCell(List<GameObject> points)
    {
        List<GameObject> newPoints = points.FindAll(p => Vector3.Distance(p.transform.position, currentCell.transform.position) < FORBIDDEN_DISTANCE);
        if (newPoints.Count == 0 || newPoints == null)
            return null;
        newPoints = newPoints.FindAll(p => p.GetComponent<Point>().IsEmpty == true);
        if (newPoints.Count == 0 || newPoints == null)
            return null;
        int index = Random.Range(0, newPoints.Count);
        return newPoints[index];
    }

    private void NextCell(GameObject point)
    {
        currentCell.GetComponent<Point>().IsEmpty = true;
        currentCell = point;
        currentCell.GetComponent<Point>().IsEmpty = false;
        transform.position = currentCell.transform.position;
    }
    #endregion

    #region Hand
    private bool canKill(Collider2D target, out GameObject point)
    {
        if(Vector3.Distance(currentCell.transform.position, target.gameObject.transform.position) > FORBIDDEN_DISTANCE)
        {
            point = null;
            return false;
        }
        Vector3 direction = target.gameObject.transform.position - currentCell.transform.position;
        Vector3 positionBehind = target.gameObject.transform.position + direction;
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(positionBehind, SEARCH_DISTANCE);
        foreach (Collider2D nextPoint in collider2Ds)
        {
            if (nextPoint.gameObject.GetComponent<Point>() != null)
                if (nextPoint.gameObject.GetComponent<Point>().IsEmpty)
                {
                    point = nextPoint.gameObject;
                    return true;
                }
        }
        point = null;
        return false;
    }

    private void ChesserFinded(Collider2D target, GameObject point)
    {
        NextCell(point);
        target.gameObject.GetComponent<Move>().CurrentCell.IsEmpty = true;
        DestroyImmediate(target.gameObject);
    }
    #endregion

}
