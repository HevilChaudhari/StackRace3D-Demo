using System;
using UnityEngine;

public class BodyBehaviour : MonoBehaviour
{
    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    public static BodyBehaviour instance;

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    public static event Action OnBodyDeteched;

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    public float jumpHeight = 0.5f;
    [SerializeField] private float upperRayDistance = 0.6f;

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    [SerializeField] private LayerMask bodyLayer;
    [SerializeField] private LayerMask waterLayer;
    [SerializeField] private LayerMask multiplierLayer;

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private bool isBodyDetected = false;
    private bool isWaterDetected = false;
    private bool isBounsLevelStarted = false;
    private bool ismultiplierDetected = false;

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private int detachedLayerIndex;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        detachedLayerIndex = LayerMask.NameToLayer("DetachedBody");
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void GameManager_OnGameStateChanged(GameState state)
    {
        isBounsLevelStarted = state == GameState.bonus;
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void Update()
    {
        CheckUpperBody();
    
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void CheckUpperBody()
    {
        RaycastHit hit;

        if (!isBodyDetected)
        {
            if(Physics.Raycast(transform.position,transform.up,out hit, upperRayDistance,bodyLayer))
            {
                GameObject hitObject = hit.collider.gameObject;

                if (hit.collider != null)
                {
                    if (!AttechBodyWithPlayer.instance.hitObjectsThisFrame.Contains(hitObject))
                    {
                        isBodyDetected = true;
             
                        jumpHeight += 0.5f;

                    }
                }

            }

        }


        if (!isWaterDetected)
        {
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.31f, waterLayer))
            {
                OnBodyDeteched?.Invoke();
                GameManager.Instance.numberOfBlocksAttech--;
                isWaterDetected = true;
                // Set the parent to the water object's transform
                transform.SetParent(null);
                this.gameObject.layer = detachedLayerIndex;
            }
        }
     

        if (isBounsLevelStarted)
        {
            if (!ismultiplierDetected)
            {
                if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.31f, multiplierLayer))
                {
                    ismultiplierDetected = true;
                    GameManager.Instance.numberOfBlocksAttech--;
                    GameManager.Instance.multiplier++;
                    // Set the parent to the water object's transform
                    this.gameObject.layer = detachedLayerIndex;
                    transform.SetParent(null);
                    OnBodyDeteched?.Invoke();
                }
            }
        }
        

    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    
    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//



}