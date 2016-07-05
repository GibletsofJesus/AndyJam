using UnityEngine;

public class LaserModule : MonoBehaviour
{
    [SerializeField] private TrailRenderer laserPrefab = null;
    private TrailRenderer laserSource;
    [SerializeField] private float laserSpeed = 25.0f;
    private float followTime = 0.0f;

    private void Awake()
    {
        laserSource = Instantiate(laserPrefab);
        laserSource.gameObject.SetActive(false);
        laserSource.time = laserSpeed / 100.0f;
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            //laserSource.transform.position = transform.position + Vector3.up;
            //laserSource.gameObject.SetActive(true);
            //followTime = 0.0f;
        }
        if(laserSource.gameObject.activeSelf)
        {
            followTime += Time.deltaTime;
            Vector3 posToCam = Camera.main.WorldToViewportPoint(laserSource.transform.position);
            if (posToCam.y < 3.0f)
            {
                laserSource.transform.position = new Vector3(followTime >= laserSource.time ? laserSource.transform.position.x : transform.position.x, laserSource.transform.position.y + (Time.deltaTime * laserSpeed));

                RaycastHit2D hit = Physics2D.Raycast(transform.position + (Vector3.up * 1.5f), Vector2.up);
                if (hit)
                {
                    if (hit.collider.tag == "Enemy")
                    {
                        hit.collider.GetComponent<Enemy>().TakeDamage(25.0f);
                    }

                }
            }
            else
            {
                followTime = 0.0f;
                laserSource.gameObject.SetActive(false);
                laserSource.Clear();
            }


        }
    }


}
