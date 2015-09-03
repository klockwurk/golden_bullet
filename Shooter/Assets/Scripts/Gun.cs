/*******************************************************************************/
/*!
\file   Gun.cs
\author Swolebraham Lincoln
\par    All content © 2015 DigiPen (USA) Corporation, all rights reserved.
\par    Team That Thumb is Satin

\brief
  File does does things. COOL things.

  
*/
/*******************************************************************************/  

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Transform))]
public class Gun : MonoBehaviour
{
  // Gun Parameters
  [SerializeField] public float Range = 200.0f;

  // Screenshake
  [SerializeField] public GameObject Camera;

  // Muzzle flash
  [SerializeField] public bool MuzzleFlashes = true;
  [SerializeField] public bool MuzzleLights = true;
  GameObject Muzzle;
  float MuzzleLightTime = 0.1f;
  float MuzzleLightTimer = 0.0f;

  // Accuracy
  [SerializeField] public float Accuracy = 0.05f;

  // Shooty ray-casting stuff
  [SerializeField] public bool ShootsRay = true;
  Ray          ShootyRay;
  RaycastHit   ShootyRaycasHit;
  LineRenderer GunLineRenderer;

  // Laser pointer ray-casting stufd
  [SerializeField] public bool HasLaserSights = false;
  Ray LaserRay;
  RaycastHit LaserRaycastHit;
  LineRenderer LaserLineRenderer;
  GameObject LaserSight;

  // Projectile stuff
  [SerializeField] public GameObject Projectile;
  [SerializeField] public float ProjectileSpeed = 10;

  // Sound effects
  [SerializeField] public AudioClip ShootSound;

  // On-Hit
  [SerializeField] public GameObject Explosion;
  [SerializeField] public GameObject[] BulletHoles;
  
  // Casing
  [SerializeField] public GameObject Casing;

  // VFX timer
  float GunLineTime = 0.2f;
  float GunLineTimer = 0.0f;

  // Fire rate
  [SerializeField] public float FireDelay = 0.15f;
  float FireRateTimer = 0.0f;
  

	// Use this for initialization
	void Start ()
  {
    // Grab objects
    Muzzle = transform.FindChild("Muzzle").gameObject;
    LaserSight = transform.FindChild("LaserSight").gameObject;

    // Grab components
    GunLineRenderer = Muzzle.GetComponent<LineRenderer>();
    LaserLineRenderer = LaserSight.GetComponent<LineRenderer>();

    // Set some things up
    if (HasLaserSights)
      LaserLineRenderer.enabled = true;
    else
      LaserLineRenderer.enabled = false;
	}

  /////////////////////////////////////              /////////////////////////////////////
  /////////////////////////////////// Update Functions ///////////////////////////////////
  /////////////////////////////////////              /////////////////////////////////////
  void LaserSightUpdate()
  {
    // Set up the ray
    LaserRay.origin = LaserSight.transform.position;
    LaserRay.direction = LaserSight.transform.up;

    // Set up the line renderer through raycasting
    LaserLineRenderer.SetPosition(0, LaserRay.origin);
    if (Physics.Raycast(LaserRay, out LaserRaycastHit, 200.0f))
      LaserLineRenderer.SetPosition(1, LaserRaycastHit.point);
    else
      LaserLineRenderer.SetPosition(1, LaserRay.origin + LaserRay.direction * 500.0f);
  }

  void Update()
  {
    // Update members
    GunLineTimer -= Time.deltaTime;
    FireRateTimer -= Time.deltaTime;
    MuzzleLightTimer -= Time.deltaTime;

    // Laser-sight
    if (HasLaserSights)
      LaserSightUpdate();
    
    // Ray effect timer
    if (GunLineTimer < 0.0f)
      GunLineRenderer.enabled = false;
    if (MuzzleLightTimer < 0.0f)
      Muzzle.GetComponent<Light>().enabled = false;
  }

  /////////////////////////////////////              /////////////////////////////////////
  /////////////////////////////////// Called Functions ///////////////////////////////////
  /////////////////////////////////////              /////////////////////////////////////
  public void Shoot()
  {
    // Check if we can actually shoot
    if (FireRateTimer > 0.0f)
      return;

    // Update timer
    FireRateTimer = FireDelay;

    // SFX
    AudioSource.PlayClipAtPoint(ShootSound, Muzzle.transform.position);

    // Screenshake
    Camera.GetComponentInParent<Screenshake>().Shake();
    GetComponentInParent<Screenshake>().Shake();

    if (Projectile != null) ShootProjectile();
    if (ShootsRay)          ShootRay();
    if (MuzzleFlashes)      MuzzleFlash();
    if (MuzzleLights)       MuzzleLight();
    if (Casing != null)     SpitCasing();
  }

  ////////////////           ////////////////
  ////////////// VFX functions //////////////
  ////////////////           ////////////////
  void Kickback()
  {
    // ...
  }

  void ShootProjectile()
  {
    // Make projectile
    Quaternion quat = new Quaternion();
    quat.SetLookRotation(Muzzle.transform.forward);
    GameObject projectile = (GameObject) Instantiate(Projectile, Muzzle.transform.position + Muzzle.transform.forward, quat);

    // Set velocity
    projectile.GetComponent<Rigidbody>().velocity = projectile.transform.forward * ProjectileSpeed;

    // Can't shoot self
    Physics.IgnoreCollision(projectile.GetComponent<Collider>(), transform.parent.parent.GetComponent<Collider>());
  }

  void ShootRay()
  {
    // Ray effect timer
    GunLineTimer = GunLineTime;

    // Ray
    ShootyRay.origin = Muzzle.transform.position;
    ShootyRay.direction = Muzzle.transform.forward + new Vector3(Random.Range(-Accuracy, Accuracy), Random.Range(-Accuracy, Accuracy), Random.Range(-Accuracy, Accuracy));

    // Line
    GunLineRenderer.enabled = true;
    GunLineRenderer.SetPosition(0, Muzzle.transform.position);

    // See if we hit something
    if (Physics.Raycast(ShootyRay, out ShootyRaycasHit, Range))
    {
      // Set position of line renderer
      GunLineRenderer.SetPosition(1, ShootyRaycasHit.point);

      // Apply damage
      ShootyRaycasHit.collider.gameObject.SendMessage("OnDamage", 1.0f, SendMessageOptions.DontRequireReceiver);

      // Create explosion at hit point
      if (Explosion != null)
        Instantiate(Explosion, ShootyRaycasHit.point, new Quaternion());

      // Create bullet hole
      if (BulletHoles.Length != 0)
      {
        var orientation = Quaternion.FromToRotation(Vector3.up, ShootyRaycasHit.normal);
        var bulletHole = (GameObject) Instantiate(BulletHoles[Random.Range(0, BulletHoles.Length)], ShootyRaycasHit.point, orientation);
        bulletHole.transform.position += ShootyRaycasHit.normal * 0.01f;
      }
    }
    // If we didn't hit anything
    else
    {
      GunLineRenderer.SetPosition(1, ShootyRay.origin + ShootyRay.direction * Range);
    }
  }

  void MuzzleFlash()
  {
    // Particles
    Muzzle.GetComponent<ParticleSystem>().Play();
  }

  void MuzzleLight()
  {
    // Light
    Muzzle.GetComponent<Light>().enabled = true;
    MuzzleLightTimer = MuzzleLightTime;
  }

  void SpitCasing()
  {
    // Make casing
    Vector3 offset = new Vector3(0.1f, 0.1f, 0.0f);
    GameObject casing = (GameObject) Instantiate(Casing, Muzzle.transform.position + offset, new Quaternion());
    casing.GetComponent<Rigidbody>().velocity = Muzzle.transform.up * 3.0f + Muzzle.transform.right * 2.5f;
  }
}
