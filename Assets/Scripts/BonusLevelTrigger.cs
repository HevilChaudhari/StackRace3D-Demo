using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusLevelTrigger : MonoBehaviour
{
    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Body"))
        {

            //GameManager.Instance.UpdateGameState(GameState.bonus);

        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

}
