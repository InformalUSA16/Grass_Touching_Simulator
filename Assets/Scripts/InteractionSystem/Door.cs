using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;

    public string InteractionPromt => _prompt;
    // ReSharper disable Unity.PerformanceAnalysis
    public bool Interact(Interactor interactor)
    {
        var inventorty = interactor.GetComponent<Inventory>();

        if (inventorty == null) return false;
        if (inventorty.HasKey)
        {
            Debug.Log("Opening door!");
                    return true;
        }

        Debug.Log("No key found!");
        return false;

    }
}

