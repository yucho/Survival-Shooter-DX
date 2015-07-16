using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public int damagePerShot = 20;
    public float timeBetweenBullets = 0.15f;
    public float range = 100f;
	public AudioClip gunClip;
	public float gunVolume = 0.8f;


	private bool canShoot;
    private float timer;
    private Ray shootRay;
    private RaycastHit shootHit;
    private int shootableMask;
    private ParticleSystem gunParticles;
    private LineRenderer gunLine;
    private AudioSource gunAudio;
    private Light gunLight;
    private float effectsDisplayTime = 0.2f;


    void Awake ()
    {
		canShoot = true;
        shootableMask = LayerMask.GetMask ("Shootable", "Enemy");
        gunParticles = GetComponent<ParticleSystem> ();
        gunLine = GetComponent <LineRenderer> ();
        gunAudio = gameObject.AddComponent<AudioSource> ();
		gunAudio.clip = gunClip;
		gunAudio.playOnAwake = false;
        gunLight = GetComponent<Light> ();

		NotificationCentre.AddObserver (this, "OnPlayerDeath");
    }


    void Update ()
    {
        timer += Time.deltaTime;

		if(canShoot && Input.GetButton ("Fire1") && timer >= timeBetweenBullets && Time.timeScale != 0)
        {
            Shoot ();
        }

        if(timer >= timeBetweenBullets * effectsDisplayTime)
        {
            DisableEffects ();
        }
    }


	public void EnableGun (bool enable)
	{
		canShoot = enable;
	}


    void DisableEffects ()
    {
        gunLine.enabled = false;
        gunLight.enabled = false;
    }


    void Shoot ()
    {
        timer = 0f;

		gunAudio.volume = gunVolume;
        gunAudio.Play ();

        gunLight.enabled = true;

        gunParticles.Stop ();
        gunParticles.Play ();

        gunLine.enabled = true;
        gunLine.SetPosition (0, transform.position);

        shootRay.origin = transform.position;
        shootRay.direction = transform.forward;

        if(Physics.Raycast (shootRay, out shootHit, range, shootableMask))
        {
			FlammableBlock block = shootHit.collider.GetComponent <FlammableBlock> ();
            EnemyHealth enemyHealth = shootHit.collider.GetComponent <EnemyHealth> ();

            if(enemyHealth)
            {
                enemyHealth.TakeDamage (damagePerShot, shootHit.point);
            }
			else if (block)
			{
				block.TakeHit (1, shootHit.point);
			}
            gunLine.SetPosition (1, shootHit.point);
        }
        else
        {
            gunLine.SetPosition (1, shootRay.origin + shootRay.direction * range);
        }
    }


	// Disable gun on player death.
	void OnPlayerDeath ()
	{
		EnableGun (false);
	}
}
