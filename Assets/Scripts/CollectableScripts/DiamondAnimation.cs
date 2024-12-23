        using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DiamondAnimation : MonoBehaviour
{
    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    public static DiamondAnimation Instance;

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    [SerializeField]private GameObject animatedDiomandPrefab;

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    [SerializeField] private RectTransform target;
    [SerializeField] private RectTransform startPoint;
    Vector3 targetPos;
    Vector3 startPos;

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    [Space]
    [Header("Available Diamonds : (Diamond to Pool)")]
    [SerializeField] private int maxDiamonds;
    Queue<GameObject> diamondQueue = new Queue<GameObject>();


    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    [Space]
    [Header("Animation Settings")]
    [SerializeField][Range(0.2f, 0.9f)] float minAnimationDuration;
    [SerializeField][Range(0.9f, 2f)] float maxAnimationDuration;
    [SerializeField] Ease easeType;

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void Awake()
    {
        Instance = this;

        targetPos = Camera.main.ScreenToWorldPoint(target.position);
        startPos = Camera.main.ScreenToWorldPoint(startPos);

        //Making pool
        prepareDiamonds();

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
         if(state == GameState.finish)
        {

            Animate(startPoint.position);
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    private void prepareDiamonds()
    {

            GameObject diamond;
        for(int i = 0;i<=maxDiamonds;i++)
        {
            diamond = Instantiate(animatedDiomandPrefab);
            diamond.transform.parent = transform;
            diamond.SetActive(false);
            diamondQueue.Enqueue(diamond);

        }
      
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

    public void Animate(Vector3 collectedDiamondPos)
    {
        for(int i = 0; i < GameManager.Instance.totalScore; i++)
        {

            if(diamondQueue.Count > 0)
            {
                GameObject diamond;
                diamond = diamondQueue.Dequeue();
                diamond.SetActive(true);
                diamond.transform.position = collectedDiamondPos;
                float duration  = Random.Range(minAnimationDuration, maxAnimationDuration);
                diamond.transform.DOMove(targetPos, duration)
                .SetEase(easeType)
                .OnComplete(() => {
                diamond.SetActive(false);
                diamondQueue.Enqueue(diamond);
                });

            }
        }
    }

    //--------------------------------------------------------------------------------------------------------------------------------------------//
    //--------------------------------------------------------------------------------------------------------------------------------------------//

}
