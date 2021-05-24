using UnityEngine;

public class CheatCodesHandler : MonoBehaviour
{

    [Header("Destroy Door cheat code")]
    public CheatCode openDoor;

    [Header("Destroy second safe door cheat code")]
    public CheatCode openSafeDoor;

    [Header("Drop lightsaber cheat code")]
    public CheatCode dropLightsaber;

    [Header("Audio")]
    public AudioSource sound;


    /// <summary>
    /// Checks whether the button A is pressed during this frame
    /// </summary>
    private bool a_pressed()
    {
        return OVRInput.GetDown(OVRInput.RawButton.A);
    }
    /// <summary>
    /// Checks whether the button B is pressed during this frame
    /// </summary>
    private bool b_pressed()
    {
        return OVRInput.GetDown(OVRInput.RawButton.B);
    }
    /// <summary>
    /// Checks whether the button X is pressed during this frame
    /// </summary>
    private bool x_pressed()
    {
        return OVRInput.GetDown(OVRInput.RawButton.X);
    }
    /// <summary>
    /// Checks whether the button Y is pressed during this frame
    /// </summary>
    private bool y_pressed()
    {
        return OVRInput.GetDown(OVRInput.RawButton.Y);
    }

    /// <summary>
    /// Destroy the object passed as parameter
    /// </summary>
    private void destroyObject(GameObject objectToDestroy)
    {
        DestroyImmediate(objectToDestroy.gameObject);
        sound.Play();
    }

    /// <summary>
    /// Removes any physical constraint on the object passed as parameter
    /// </summary>
    private void removeConstraints(GameObject objectToRemoveConstraintsOn)
    {
        objectToRemoveConstraintsOn.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        sound.Play();
    }

    // Update is called once per frame
    void Update()
    {

        //Check for user input
        OVRInput.RawButton buttonPressed = OVRInput.RawButton.None;
        if (a_pressed())
        {
            buttonPressed = OVRInput.RawButton.A;
        }
        else if (b_pressed())
        {
            buttonPressed = OVRInput.RawButton.B;
        }
        else if (x_pressed())
        {
            buttonPressed = OVRInput.RawButton.X;
        }
        else if (y_pressed())
        {
            buttonPressed = OVRInput.RawButton.Y;
        }

        //If the user did not press any button during that frame, do nothing
        if (buttonPressed == OVRInput.RawButton.None) return;

        //Handle all codes development
        openDoor.handleCodeIndex(buttonPressed);
        openSafeDoor.handleCodeIndex(buttonPressed);
        dropLightsaber.handleCodeIndex(buttonPressed);

        //Check for codes completness and do the cheat if it is complete
        if (openDoor.completed()) destroyObject(openDoor.related_object);
        if (openSafeDoor.completed()) destroyObject(openSafeDoor.related_object);
        if (dropLightsaber.completed()) removeConstraints(dropLightsaber.related_object);

    }
}
