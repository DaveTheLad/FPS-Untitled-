using UnityEngine;
using TMPro; 
using System.Collections;

public class pistolScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;
    public float fireRate = 5;

    public int maxAmmo = 10;
    private int currentAmmo;
    private float reloadTime = 2f;
    private bool isReloading = false;

    [SerializeField]
    public TextMeshProUGUI reloadText;
    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    public AudioSource sound;
    public AudioClip shootSounds;
    public AudioClip reloadSound;

    private float nextTimeToFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        currentAmmo = maxAmmo;

        sound = GetComponent<AudioSource>();

        if (sound == null)
        {
            sound = gameObject.AddComponent<AudioSource>();
        }

        reloadText.text = "";
    }

    private void OnEnable()
    {
        isReloading = false;
        nextTimeToFire = 0f;

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

        if (Input.GetButtonDown("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            sound.PlayOneShot(shootSounds);
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;
        reloadText.text = "Reloading...";

        if (reloadSound != null && sound != null)
        {
            sound.PlayOneShot(reloadSound);
        }

        yield return new WaitForSeconds(reloadTime - .25f);

        currentAmmo = maxAmmo;
        isReloading = false;
        reloadText.text = "Reloaded!";

        yield return new WaitForSeconds(1.5f);
        reloadText.text = "";
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
