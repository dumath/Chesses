using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    #region Components of Event.
    public const string CHESSER_STEP = "CHESSER STEP"; // ��������� �������. ���������� Wizard'�.
    private static bool isChesserStep; // ����������� ��������� ���� Chessera.
    private static void chesserCanMove() // ����������, ����� "Enemy" - �������� ���.
    {
        isChesserStep = !isChesserStep;
    }
    #endregion

    #region Position fields
    private const float SCALE_POWER = 1.3f; // ����������� ����������/���������� "Chesser", ��� ������.
    private const float CHESSER_PLANE = -1.0f; // ���������, � ������� ��������� ��� �������, ������������ �� ��������������.
    private const float SEARCH_DISTANCE = 1.0f; // ��������� ������ ����� ������.
    private const float FORBIDDEN_DISTANCE = 4.0f; // ����������� ���������, ��� ������� "Chesser" ������ �������� �������.
    private GameObject currentCell; // "Point" ������, � ������� ������ ��������� "Chesser".
    #endregion

    #region Awake, Start, Update, LateUpdate, OnDestroy
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Awake()
    {
        ChessWizard.AddListener(CHESSER_STEP, chesserCanMove);
    }
    #endregion

    #region Methods of Mouse event.
    public void OnMouseDown()
    {
        if (isChesserStep)
        {
            currentCell = getCurrentCell();
            transform.localScale *= SCALE_POWER;
        }
            
    }

    public void OnMouseDrag()
    {
        if (isChesserStep)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = CHESSER_PLANE;
            transform.position = newPosition;

        }
    }

    public void OnMouseUp()
    {
        if (isChesserStep)
        {
            transform.localScale /= SCALE_POWER;
            bool endMove;
            seeCellObjects(out endMove);
            if (endMove)
                ChessWizard.Broadcast(SceneController.CHANGE_STEP);
            else
                transform.position = currentCell.transform.position;
        }
    }
    #endregion

    #region Methods of Coordination.
    /// <summary>
    /// �����, ������� "Cell", ������� ������ Chesser.
    /// </summary>
    /// <param name="endMove"> ��� ������� - true. </param>
    private void seeCellObjects(out bool endMove)
    {
        endMove = false;
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, SEARCH_DISTANCE);
        foreach (Collider2D target in targets)
        {
            if(Vector3.Distance(target.gameObject.transform.position, currentCell.transform.position) < FORBIDDEN_DISTANCE)
            {
                if (target.gameObject.GetComponent<Enemy>() != null)
                {
                    GameObject point = null;
                    if (canKillEnemy(target.gameObject, out point))
                    {
                        nextCell(point);
                        target.gameObject.GetComponent<Enemy>().GetPoint.GetComponent<Point>().IsEmpty = true;
                        DestroyImmediate(target.gameObject);
                        endMove = true;
                        break;
                    }
                }
                if (target.gameObject.GetComponent<Point>() != null)
                {
                    if(canMove(target.gameObject))
                    {
                        nextCell(target.gameObject);
                        endMove = true;
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// ����� ��������� ������, �� "Enemy", �� ��������� � "Chesserom".
    /// </summary>
    /// <param name="target"> ����, �� ������� ��������� "Cell". </param>
    /// <param name="pointBehindEnemy"> ���� "Cell" ������ = null. </param>
    /// <returns></returns>
    private bool canKillEnemy(GameObject target, out GameObject pointBehindEnemy)
    {
        Vector3 direction = target.transform.position - currentCell.transform.position;
        Vector3 cellBehind = target.transform.position + direction;
        Collider2D[] points = Physics2D.OverlapCircleAll(cellBehind, SEARCH_DISTANCE);
        foreach(Collider2D point in points)
        {
            if(point.GetComponent<Point>() != null)
            {
                if (canMove(point.gameObject))
                {
                    pointBehindEnemy = point.gameObject;
                    return true;
                }
            }
        }
        pointBehindEnemy = null;
        return false;
    }

    /// <summary>
    /// �����, ��������� "Cell".
    /// </summary>
    /// <param name="point"> ����������� "Cell" ������. </param>
    /// <returns></returns>
    private bool canMove(GameObject point)
    {
        return point.GetComponent<Point>().IsEmpty;
    }

    /// <summary>
    /// ����� ������ ������� "Chesser".
    /// </summary>
    /// <param name="point"> "Cell" ������, �� ������� ��������� "Chesser". </param>
    private void nextCell(GameObject point)
    {
        currentCell.GetComponent<Point>().IsEmpty = true;
        currentCell = point;
        currentCell.GetComponent<Point>().IsEmpty = false;
        this.transform.position = currentCell.transform.position;
    }

    #endregion

    #region Other
    /// <summary>
    /// ����� �������� ������ �� "Point" ������, �� ������� ����� Chesser.
    /// PS: ��� ���������� ������.
    /// </summary>
    /// <returns> "Point" ������. </returns>
    private GameObject getCurrentCell()
    {
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, GetComponent<CircleCollider2D>().radius);
        foreach (Collider2D coll in collider2Ds)
        {
            if (coll.gameObject.GetComponent<Point>() != null)
            {
                if (Vector3.Distance(transform.position, coll.gameObject.transform.position) == 0)
                {
                    return coll.gameObject;
                }
            }
        }
        Debug.LogError("Move.class: getCurrentCell.method:  ��������� ������ ������ �� �����, �� ������� ����� Chesser");
        return null;
    }

    //��������� �����, ��� ������ �������.
    public Point CurrentCell
    {
        get => currentCell.GetComponent<Point>();
    }
    #endregion
}
