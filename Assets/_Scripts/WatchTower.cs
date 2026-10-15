using TMPro;
using UnityEngine;

public class WatchTower : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private LayerMask playerLayer;

    private Player cachePlayer;

    private void Update()
    {
        Vector3 center = boxCollider.bounds.center;
        Vector3 halfExtens = Vector3.Scale(transform.lossyScale * 0.5f, transform.lossyScale);

        Collider[] result = Physics.OverlapBox(center, halfExtens, transform.rotation, playerLayer);

        if(result.Length <= 0 )
        {
            return;
        }

        foreach(Collider c in result)
        {
            if(c.TryGetComponent(out Player player) && !player.isDetected)
            {
                Debug.Log(transform + "playerDetected");
                player.isDetected = true;
            }

        }
    }
}
