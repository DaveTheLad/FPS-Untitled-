using UnityEngine;

public class pistolScript : MonoBehaviour
{
    public float damage = 10f;
    public float range = 100f;

    public Camera fpsCam;
    public ParticleSystem muzzleFlash;

    AudioSource sound;
    public AudioClip shootSounds;

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
        if (Input.GetButtonDown("Fire1"))
        {
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
        }
    }
}
