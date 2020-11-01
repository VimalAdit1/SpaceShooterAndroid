using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="NewWeapon",menuName ="Gun")]
public class Gun : ScriptableObject
{
    public string gunName;
    public int noOfBullets;
    public float timeBetweenShots;
    public GameObject bulletPrefab;
    public GameObject gunPoint;
    public float spreadAngle;
    // Start is called before the first frame update
    public void shoot()
    {
        if (noOfBullets == 1)
        {
            Instantiate(bulletPrefab, gunPoint.transform.position, gunPoint.transform.rotation);
        }
        else
        {
            float offset = spreadAngle / noOfBullets;
            float gunAngle = gunPoint.transform.rotation.eulerAngles.z;
            float startAngle = gunAngle - ((noOfBullets / 2) * offset);
            for (int i=0;i<noOfBullets;i++)
            {
                Instantiate(bulletPrefab, gunPoint.transform.position, Quaternion.Euler(new Vector3(0f ,0f ,startAngle)));
                startAngle += offset;
            }
        }
    }
}
