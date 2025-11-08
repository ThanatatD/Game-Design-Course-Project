using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Combat; // For Energy component

public class StartpointController : MonoBehaviour
{
    [SerializeField] private float _energyAmount = 100f; // energy given at start
    Animator _anim;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _anim.SetTrigger("IsMoving");

        // Check if collided object has Energy component
        Energy playerEnergy = collision.GetComponent<Energy>();
        if (playerEnergy != null)
        {
            // Only add energy if below 100
            if (playerEnergy.CurrentEnergy < 100f)
            {
                float amountToAdd = Mathf.Min(_energyAmount, 100f - playerEnergy.CurrentEnergy);
                playerEnergy.ModifyEnergy(amountToAdd);
                Debug.Log($"{collision.name} received {amountToAdd} energy at start point.");
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _anim.SetTrigger("IsMoving");
    }
}
