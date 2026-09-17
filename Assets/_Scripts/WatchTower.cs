using TMPro;
using UnityEngine;

public class WatchTower : MonoBehaviour
{
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private LayerMask playerLayer;

    private void Update()
    {
        Vector3 center = boxCollider.bounds.center;
        Vector3 halfExtens = Vector3.Scale(transform.lossyScale * 0.5f, transform.lossyScale);

        Collider[] result = Physics.OverlapBox(center, halfExtens, transform.rotation, playerLayer);

        if(result.Length <= 0 )
        {
            text.text = "Not Detected";
            text.color = Color.white;
            return;
        }

        foreach(Collider c in result)
        {
            if(c.TryGetComponent(out Player player))
            {
                text.text = "Detected";
                text.color = Color.red;
            }
        }
    }
}
