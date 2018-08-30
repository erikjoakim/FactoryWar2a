using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;
    private static GameObject _selObj = null;
    public static GameObject selectedObject
    {
        get { return _selObj; }
        set
        {
            if (_selObj == null)
            {
                _selObj = value;
            }
            else
            {
                SelectableGameObject selObj = _selObj.GetComponent<SelectableGameObject>();
                if (selObj != null)
                {
                    selObj.OnDeSelected();
                }
                _selObj = value;
            }
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
            DontDestroyOnLoad(gameObject);
            
            //DO INITIALIZATION
        }
        
    }
}
