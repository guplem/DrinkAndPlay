using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0649
public abstract class SectionManager : MonoBehaviour
{
    public GameManager gm { get; private set; }
    [SerializeField] private GameObject GameManagerPrefab;
    public static SectionManager Current { get; private set; }

    private void Awake()
    {
        if (GameManagerPrefab == null)
            Debug.LogError("'GameManagerPrefab is null in the object' " + name, gameObject);

        Instantiate(GameManagerPrefab).GetComponent<GameManager>().Initialize();
        gm = GameManager.Instance;
        Current = this;
        Debug.Log("Awaked section manager");
    }


}