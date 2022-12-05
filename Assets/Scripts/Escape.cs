using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Escape : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Player Has Escaped!!!");
        SceneManager.LoadScene(2);
    }
}


