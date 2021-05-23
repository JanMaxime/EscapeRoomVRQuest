using UnityEngine;

public class HittableBox : ContainerBeHacked
{

    [Header("Sound effects")]
    public AudioSource hit_sound;
    public AudioSource destroy_sound;

    [Header("Life points")]
    public float total_life = 200f;
    private float current_life;


    [Header("Material")]
    public Material material;
    private Material material_instantiated;


    void Start()
    {
        current_life = total_life;
        material_instantiated = Instantiate(material);
        //Give the cube a rotation to make it look "magic" and floating
        this.GetComponent<Rigidbody>().angularVelocity = new Vector3(0.5f, 0.5f, 0.5f);
    }

    /// <summary>
    /// Handles the behaviour when the cube is hit by the lightsaber
    /// The cube takes damages proportional to the force of the hit and the color get darker and darker as
    /// the life points go down
    /// </summary>
    /// <param name="velocity_magnitude">The force of the hit</param>
    /// <param name="right">true when the object is hit with the right hand, false when it is hit with the left one</param>
    public void hit(float velocity_magnitude, bool right)
    {
        //If the cube is already broken, nothing needs to be done
        if (current_life <= 0) {
            return;
        }
        //Modify the colour of the cube by an amount proportional to the hit force
        material_instantiated.SetColor("_Color", material_instantiated.GetColor("_Color") - new Color(1f / total_life * velocity_magnitude, 1f / total_life * velocity_magnitude, 1f / total_life * velocity_magnitude, 0));
        foreach (Renderer renderer in this.GetComponentsInChildren<Renderer>())
        {
            renderer.material.SetColor("_Color", material_instantiated.GetColor("_Color"));
        }
        //Take damage proportional to the hit strength
        current_life -= velocity_magnitude;

        //If the hit breaks the cube
        if (current_life <= 0f)
        {
            //Play the destroy sound and start a vibration proportional to the hit strength
            destroy_sound.Play();
            StartCoroutine(VibrationManager.vibrate(right, 0.3f, 1, Mathf.Min(1f, velocity_magnitude / 40f)));

            //Put rigidbodies to all sides of the cube so that they fall to the ground
            GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            foreach (Transform child in transform)
            {
                child.gameObject.AddComponent<Rigidbody>();
                child.GetComponent<Rigidbody>().AddExplosionForce(50, child.position, 2);
            }

        }
        //If the damages did not break the cube
        else
        {
            //Play the hit sound and start a vibration proportional to the hit strength
            hit_sound.Play();
            StartCoroutine(VibrationManager.vibrate(right, 0.1f, 1, Mathf.Min(1f, velocity_magnitude / 40f)));
 
        }
    }

    /// <summary>
    /// Checks wheter the cube is hacked (broken)
    /// </summary>
    /// <returns>true when the cube is hacked (broken), false otherwise</returns>
    public override bool isHacked()
    {
        return current_life <= 0.0f;
    }
}


