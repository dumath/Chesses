using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour
{

    #region Fiels
    public bool _isEmpty = true;
    #endregion

    #region Awake, Start, Update, LateUpdate, OnDestroy
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    #endregion

    /// <summary>
    /// Свойство. Занято/Свободно.
    /// </summary>
    public bool IsEmpty
    {
        get => _isEmpty;
        set => _isEmpty = value;
    }

}
