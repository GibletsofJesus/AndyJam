using UnityEngine;
using System.Collections;

public class LaserDamage : MonoBehaviour
{
    [SerializeField] private string[] damageTags = null;
    [SerializeField] private float laserDamage = 5.0f;

    private void OnTriggerStay2D(Collider2D _col)
    {
        for (int i = 0; i < damageTags.Length; ++i)
        {
            if (_col.tag == damageTags[i])
            {
                _col.GetComponent<Actor>().TakeDamage(laserDamage);
                break;
            }
        }
    }
}
