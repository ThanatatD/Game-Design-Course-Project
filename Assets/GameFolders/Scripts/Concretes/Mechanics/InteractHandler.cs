// using Controllers;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// namespace Mechanics
// {
//     public class InteractHandler : MonoBehaviour
//     {
//         LeverController _normalLever;
//         GameObject _currentInteractableObject;
//         private void OnTriggerStay2D(Collider2D collision)
//         {

//             if (collision.gameObject.CompareTag("InteractableObject"))
//             {
//                 _currentInteractableObject = collision.gameObject;
//             }
//         }
//         private void OnTriggerExit2D(Collider2D collision)
//         {

//             _currentInteractableObject = null;
//         }

//         public void Interact()  //could be better 
//         {
//             if (_currentInteractableObject != null)
//             {
//                 _normalLever = _currentInteractableObject.gameObject.GetComponent<LeverController>();
//                 if(_normalLever != null ) 
//                     _normalLever.LeverInteraction();
//                 else
//                     _currentInteractableObject.gameObject.GetComponent<LeverTeleportController>().LeverInteraction();
//             }
            

//         }
//     }

// }

using Controllers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics
{
    public class InteractHandler : MonoBehaviour
    {
        LeverController _normalLever;
        GameObject _currentInteractableObject;

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("InteractableObject"))
            {
                _currentInteractableObject = collision.gameObject;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject == _currentInteractableObject)
            {
                _currentInteractableObject = null;
            }
        }

        public void Interact()
        {
            if (_currentInteractableObject != null)
            {
                _normalLever = _currentInteractableObject.GetComponent<LeverController>();
                if (_normalLever != null)
                {
                    // ✅ Pass this player's GameObject to the lever
                    _normalLever.LeverInteraction(gameObject);
                }
                else
                {
                    _currentInteractableObject.gameObject.GetComponent<LeverTeleportController>().LeverInteraction();
                }
            }
        }

    }
}
