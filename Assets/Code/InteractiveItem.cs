using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class InteractiveItem : MonoBehaviour
{
    [SerializeField]
    private string actionLabel1;
    public string ActionLabel1 => actionLabel1;
    [SerializeField]
    private string actionLabel2;
    public string ActionLabel2 => actionLabel2;

    public abstract bool CanInteract();

    public abstract void Interact(); 
}
