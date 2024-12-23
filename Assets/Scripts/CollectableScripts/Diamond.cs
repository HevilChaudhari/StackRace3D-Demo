using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Diamond : MonoBehaviour
{

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    public bool scoremode = false;
    private bool ScoreAdded = false;

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    void Update()
    {
        if (scoremode)
        {
            transform.position = Vector3.MoveTowards(transform.position, GridMovement.instance.diamondNextPos.position, Time.deltaTime * 10f);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * 1.99f);

        }
        if (transform.localScale == Vector3.zero)
        {
            Destroy(gameObject);
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Player") || other.gameObject.layer == LayerMask.NameToLayer("Body"))
        {
            if (!ScoreAdded)
            {
                ScoreAdded = true;
                GameManager.Instance.UpdateScoreState(AddScore.addScore);
            }
            scoremode = true;
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

}
