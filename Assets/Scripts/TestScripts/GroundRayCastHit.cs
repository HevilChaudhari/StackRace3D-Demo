using UnityEngine;

public class GroundRayCastHit : MonoBehaviour
{
    public LayerMask layerMask; // Layer mask to filter which objects to check for intersection
    public float distance = 0.001f; // Distance along the Y-axis to perform the box cast
    private Vector3 boxSize; // Size of the box cast
    private float posSetter;
    private bool isBonusLevelTrigger = false;

    void Start()
    {

        posSetter = transform.localPosition.y;
        // Get the size of the plane in the Y-axis
        BoxCollider boxCollider = GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            boxSize = boxCollider.size;
        }
        else
        {
            Debug.LogWarning("Renderer component not found on the object.");
        }
    }

    private void OnEnable()
    {
        AttechBodyWithPlayer.OnAttach += AttechBodyWithPlayer_OnAttach;
        BodyBehaviour.OnBodyDeteched += BodyBehaviour_OnBodyDeteched;
    }

    private void BodyBehaviour_OnBodyDeteched()
    {
        posSetter += 1;
        transform.localPosition = new Vector3(0f, posSetter, 0f);
    }

    private void AttechBodyWithPlayer_OnAttach()
    {
        posSetter += -1;
        transform.localPosition = new Vector3(0f, posSetter, 0f);
    }

    void Update()
    {
        // Perform the box cast

        GroundCheck();
    }

    private void GroundCheck()
    {
        RaycastHit[] hits = Physics.BoxCastAll(transform.position, boxSize / 2f, Vector3.down, Quaternion.identity, distance, layerMask);
        GridMovement.instance.isGrounded = false;
        // Output the results
        if (hits.Length > 0)
        {
            foreach (RaycastHit hit in hits)
            {

                Collider hitCollider = hit.collider;
                GameObject hitObject = hitCollider.gameObject;
                GridMovement.instance.isGrounded = true;
                Debug.Log(GridMovement.instance.isGrounded);

                if (hitObject.layer == LayerMask.NameToLayer("FinishLine") && !isBonusLevelTrigger)
                {
                    isBonusLevelTrigger = true;
                    GameManager.Instance.UpdateGameState(GameState.bonus);
                }



            }
        }
    }

    private void OnDisable()
    {

        AttechBodyWithPlayer.OnAttach -= AttechBodyWithPlayer_OnAttach;
        BodyBehaviour.OnBodyDeteched -= BodyBehaviour_OnBodyDeteched;
    }


}
