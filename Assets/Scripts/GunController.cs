
using UnityEngine;

public class GunController : MonoBehaviour
{
    public float dmg = 10f;
    public float range = 100f;
    public Camera[] fpsCam;
    public GameObject flare;

    public AudioClip shotSound;
    AudioSource audioSource;

    public ParticleSystem flash;
    // Update is called once per frame

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        flash.Play();
        audioSource.PlayOneShot(shotSound);
        RaycastHit hit;
        /*if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            Target target = hit.transform.GetComponent<Target>();
            if(target != null)
            {
                target.TakeDamage(10);
            }
            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * 100f);
            }
        }
        GameObject efectoDisparo = Instantiate(flare, hit.point, Quaternion.LookRotation(hit.normal));
        Destroy(efectoDisparo, 0.25f);
        */
    }
}
