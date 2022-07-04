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
    /// Вызывается объектом, когда происходит какое либо событие.
    /// </summary>
    /// <param name="keyEvent"> Ключ события. </param>
    public static void Broadcast(string keyEvent)
    {
        if (storedMethods.ContainsKey(keyEvent))
        {
            storedMethods[keyEvent]();
            return;
        }
        else
           Debug.Log("ChessWizard: Метод отсутствует. Требуется вызов метода AddListener, перед вызовом Broadcast.");
    }

    /// <summary>
    /// Вызывается при добавлении нового события.
    /// </summary>
    /// <param name="keyEvent"> Ключ события. </param>
    /// <param name="action"> Вызываемое событие. </param>
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
    /// Вызывается при удалении события.
    /// </summary>
    /// <param name="keyEvent"> Ключ события. </param>
    public static void RemoveListener(string keyEvent)
    {
        if (storedMethods.ContainsKey(keyEvent))
        {
            storedMethods.Remove(keyEvent);
            return;
        }
        Debug.Log(string.Format("ChessWizard: Отсутствует {0}", keyEvent));
    }
    #endregion
}
