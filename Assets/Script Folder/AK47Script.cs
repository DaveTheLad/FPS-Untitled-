using UnityEngine;
using System.Collections;

public class AK47Script : MonoBehaviour
{
    public float damage = 15f;
    public float range = 150f;
    public float fireRate = 5; // Adjust this value for a slower firing rate

    public int maxAmmo = 25;
    private int currentAmmo;
    private float reloadTime = 5f;
    private bool isReloading = false;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    AudioSource sound;
    public AudioClip shootSounds;

    private float nextTimeToFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;

        // Get the AudioSource component attached to the same game object
        sound = GetComponent<AudioSource>();

        // If AudioSource component is not present, add it
        if (sound == null)
        {
            sound = gameObject.AddComponent<AudioSource>();
        }
    }

    private void OnEnable()
    {
        // Reset variables when enabling the script
        isReloading = false;
        nextTimeToFire = 0f;

        // Check if the script is enabled before starting the reload coroutine
        if (currentAmmo <= 0 && enabled)
        {
            StartCoroutine(Reload());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isReloading)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.R) && currentAmmo < maxAmmo)
        {
            StartCoroutine(Reload());
            return;
        }

        if (currentAmmo <= 0f)
        {
            StartCoroutine(Reload());
            return;
        }

        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            sound.PlayOneShot(shootSounds);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        Debug.Log("Reloading....");
        yield return new WaitForSeconds(reloadTime - .25f);
        currentAmmo = maxAmmo;
        isReloading = false;
        Debug.Log("Reloaded!");
    }

    void Shoot()
    {
        muzzleFlash.Play();

        currentAmmo--;

        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Debug.Log(hit.transform.name);

            Target target = hit.transform.GetComponent<Target>();
            if (target != null)
            {
                target.TakeDame(damage);
            }
            GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(impactGO, 2f);
        }
    }
}
