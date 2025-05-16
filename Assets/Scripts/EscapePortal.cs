using System;
using UnityEngine;

public class EscapePortal : MonoBehaviour
{
    public Action OnCharacterArrivedToPortal;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Character")
        {
            OnCharacterArrivedToPortal?.Invoke();
        }
    }
}
