using UnityEngine;

public class AK47Script : MonoBehaviour
{
    public float damage = 15f;
    public float range = 150f;
    public float fireRate = 5; // Adjust this value for a slower firing rate

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;
    public GameObject impactEffect;

    AudioSource sound;
    public AudioClip shootSounds;

    private float nextTimeToFire = 0f;

    // Start is called before the first frame update
    void Start()
    {
        // Get the AudioSource component attached to the same game object
        sound = GetComponent<AudioSource>();

        // If AudioSource component is not present, add it
        if (sound == null)
        {
            sound = gameObject.AddComponent<AudioSource>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Fire1") && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Shoot();
            sound.PlayOneShot(shootSounds);
            muzzleFlash.Play();
        }
    }

    void Shoot()
    {
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
