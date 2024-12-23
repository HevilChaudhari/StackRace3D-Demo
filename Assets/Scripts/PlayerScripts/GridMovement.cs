using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class GridMovement : MonoBehaviour
{

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    public static GridMovement instance;

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//


    [SerializeField] private float jumpHeight = 1.0f;
    [SerializeField] private float jumpDuration = 0.5f;
    private float offset;


    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    public Vector3 currentGridPosition;
    public Transform diamondNextPos;
    [SerializeField] private Transform groundRaycastDetector;

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private bool isMoving = false;
    private bool canMove = false;
    private bool isMultiplierDetected = false;
    private bool isWaterDetected = false;
    public bool isGrounded = false;

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    [SerializeField] private LayerMask multiplierlayer;
    [SerializeField] private LayerMask waterLayer;

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    [SerializeField] private GameObject diamondCoinNumberPrefab;

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void Awake()
    {
        instance = this;
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void OnEnable()
    {
        GameManager.OnGameStateChanged += BehaviourBaseOnstate;
      
    }

   
    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void BehaviourBaseOnstate(GameState state)
    {
        switch (state)
        {
            case GameState.mainmenu:
                canMove = false;
                break;
            case GameState.playing:
                canMove = true;
                break;
            case GameState.restart:
                canMove = false;
                break;
            case GameState.bonus:
                canMove = false;
                Invoke("StartAutoMoving", 0.1f);
                break;
            case GameState.finish:
                canMove = false;
                StopAllCoroutines();
                break;
        }

    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    void Start()
    {
        currentGridPosition = transform.position;
        UpdatePositionFromGrid();
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    void Update()
    {
        if (!isMoving && isGrounded)
        {
            if (Input.GetMouseButtonDown(0) && canMove && !IsPointerOverItemUI())
            {

                Vector3 forwardPosition = currentGridPosition + new Vector3(0, 0, 1);

               StartCoroutine(MoveToGridPosition(forwardPosition));
            }
         
        }
      
        MultiplierRaycast();

    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//


    IEnumerator MoveToGridPosition(Vector3 targetGridPosition)
    {

        isMoving = true;

        Vector3 startPosition = transform.position;
        Vector3 targetPosition = targetGridPosition;


        targetPosition = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);

        float journey = 0f;
        while (journey <= jumpDuration)
        {
            journey += Time.deltaTime;
            float percent = Mathf.Clamp01(journey / jumpDuration);

            float yOffset = Mathf.Sin(percent * Mathf.PI) * jumpHeight;
            Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, percent) + Vector3.up * yOffset;

            transform.position = newPosition;

            yield return null;
        }

        transform.position = targetPosition;

        currentGridPosition = targetGridPosition;
        yield return new WaitForSeconds(0.02f);

        isMoving = false;

    }

    private void StartAutoMoving()
    {
        StartCoroutine(StartAutoMove(currentGridPosition));
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private IEnumerator StartAutoMove(Vector3 targetGridPosition)
    {
        while (isGrounded)
        {
            yield return new WaitForSeconds(0.02f);
            Debug.Log("Automove");
            isMoving = true;

            Vector3 startPosition = transform.position;
            Vector3 targetPosition = currentGridPosition + Vector3.forward;


            targetPosition = new Vector3(targetPosition.x, targetPosition.y, targetPosition.z);

            float journey = 0f;
            while (journey <= jumpDuration)
            {
                journey += Time.deltaTime;
                float percent = Mathf.Clamp01(journey / jumpDuration);

                float yOffset = Mathf.Sin(percent * Mathf.PI) * jumpHeight;
                Vector3 newPosition = Vector3.Lerp(startPosition, targetPosition, percent) + Vector3.up * yOffset;

                transform.position = newPosition;

                yield return null;
            }

            transform.position = targetPosition;

            currentGridPosition = targetPosition;
            yield return new WaitForSeconds(0.03f);
            isMoving = false;
            while (!isGrounded)
            {
                yield return null;
            }
        }

    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    public static bool IsPointerOverItemUI()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        List<RaycastResult> raysastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, raysastResults);
        foreach (RaycastResult raysastResult in raysastResults)
        {
            //Enable below condidtion if you want to check tag wise
            if (raysastResult.gameObject.layer == 5)
            {
                return true;
            }
        }
        return false;
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void MultiplierRaycast()
    {
        RaycastHit hit;
        if (!isMultiplierDetected)
        {

            if (Physics.Raycast(transform.position, -transform.up, out hit, 0.3f, multiplierlayer))
            {
                Debug.Log("multiplierdetectets");

                isMultiplierDetected = true;
                GameManager.Instance.numberOfBlocksAttech--;
                GameManager.Instance.multiplier++;
                GameManager.Instance.UpdateScoreState(AddScore.multiplierScore);
                GameManager.Instance.UpdateGameState(GameState.finish);

            }
        }

        if (!isWaterDetected)
        {
            if (Physics.Raycast(transform.position, -transform.up, out hit, 0.3f, waterLayer))
            {
                GameManager.Instance.UpdateGameState(GameState.restart);
            }
        }


    }


    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    void UpdatePositionFromGrid()
    {

        transform.position = currentGridPosition;
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void OnTriggerEnter(Collider other)
    {


        if (other.gameObject.layer == LayerMask.NameToLayer("MovingPlatform"))
        {
            // Smoothly move the player to the center of the platform
            offset = other.transform.position.x - transform.position.x;
        }

        if (other.gameObject.layer == LayerMask.NameToLayer("Diamond"))
        {


            if (!other.gameObject.GetComponent<RectTransform>())
            {
                other.gameObject.AddComponent<RectTransform>();
                other.GetComponent<RectTransform>().anchorMin = Vector2.one;
                other.GetComponent<RectTransform>().anchorMax = Vector2.one;

            }
            other.transform.parent = diamondNextPos.parent;

            Destroy(Instantiate(diamondCoinNumberPrefab, transform.position + Vector3.up, Quaternion.identity), 0.5f);

        }


    }



    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("MovingPlatform"))
        {

            currentGridPosition.x = Mathf.Round(other.transform.position.x);


        }
    }


    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("MovingPlatform"))
        {

            currentGridPosition.x = Mathf.Round(other.transform.position.x);
            transform.position = new Vector3(other.transform.position.x - offset, transform.position.y, transform.position.z);
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void OnDisable()
    {
        GameManager.OnGameStateChanged -= BehaviourBaseOnstate;

    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

}

