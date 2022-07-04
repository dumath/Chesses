using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ChessWizard : MonoBehaviour
{
    #region Fields.
    private static Dictionary<string, System.Action> storedMethods;
    #endregion

    #region Awake, Start, Update, LateUpdate, OnDestroy.
    void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
       
    }
    #endregion

    #region Methods of operation with event.
    /// <summary>
    /// ���������� ��������, ����� ���������� ����� ���� �������.
    /// </summary>
    /// <param name="keyEvent"> ���� �������. </param>
    public static void Broadcast(string keyEvent)
    {
        if (storedMethods.ContainsKey(keyEvent))
        {
            storedMethods[keyEvent]();
            return;
        }
        else
           Debug.Log("ChessWizard: ����� �����������. ��������� ����� ������ AddListener, ����� ������� Broadcast.");
    }

    /// <summary>
    /// ���������� ��� ���������� ������ �������.
    /// </summary>
    /// <param name="keyEvent"> ���� �������. </param>
    /// <param name="action"> ���������� �������. </param>
    public static void AddListener(string keyEvent, System.Action action)
    {
        if (storedMethods == null)
            storedMethods = new Dictionary<string, System.Action>();
        if (storedMethods.ContainsKey(keyEvent))
            return;
        else
            storedMethods.Add(keyEvent, action);
        return;
    }

    /// <summary>
    /// ���������� ��� �������� �������.
    /// </summary>
    /// <param name="keyEvent"> ���� �������. </param>
    public static void RemoveListener(string keyEvent)
    {
        if (storedMethods.ContainsKey(keyEvent))
        {
            storedMethods.Remove(keyEvent);
            return;
        }
        Debug.Log(string.Format("ChessWizard: ����������� {0}", keyEvent));
    }
    #endregion
}
