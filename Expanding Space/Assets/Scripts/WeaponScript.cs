using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;

    public GameObject[] ammo;
    private int ammoAmount;


    private void Start()
    {
        for (int i = 0; i <= 3; i++)
        {
            ammo[i].gameObject.SetActive(false);
        }

        ammoAmount = 0;
    }

    private void Update()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float rotateY = 0f;

        if (mousePos.x < transform.position.x)
        {
            rotateY = 180f;
        }

        transform.eulerAngles = new Vector3(transform.rotation.x, rotateY, transform.rotation.z);


        if (Input.GetButtonDown("Fire1") && ammoAmount > 0)
        {
            Shoot();
            ammoAmount -= 1;
            ammo[ammoAmount].gameObject.SetActive(false);
        }

        if (Input.GetKey(KeyCode.R))
        {
            ammoAmount = 4;
            for (int i = 0; i <= 3; i++)
            {
                ammo[i].gameObject.SetActive(true);            }
        }
    }

    private void Shoot ()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.transform.position, firePoint.transform.rotation);
        Destroy(bullet, 5f);
    }

}
