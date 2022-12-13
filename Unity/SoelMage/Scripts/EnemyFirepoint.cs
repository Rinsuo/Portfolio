using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFirepoint : MonoBehaviour
{
    public Transform player;
    public Transform enemy;
    public GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float bulletSize;
    [SerializeField] private Color bulletColor;
    private Vector3 v3pos;
    private float angle;
    [SerializeField] private float distance = 0.3f;
    [SerializeField] private float height = 0.3f;
    private bool hasdmg = true;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 diff = player.transform.position - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
    }

    private void FireBullet(float dmg)
    {
        if (hasdmg)
        {
            Instantiate(bulletPrefab, transform.position, transform.rotation);
            BulletMove.instance.BulletDetails(dmg, bulletSpeed, bulletSize, bulletColor, false);
        }
    }
}
