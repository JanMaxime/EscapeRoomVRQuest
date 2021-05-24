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

    private bool a_pressed()
    {
        return OVRInput.GetDown(OVRInput.RawButton.A);
    }

    private bool b_pressed()
    {
        return OVRInput.GetDown(OVRInput.RawButton.B);
    }

    private bool x_pressed()
    {
        return OVRInput.GetDown(OVRInput.RawButton.X);
    }

    private bool y_pressed()
    {
        return OVRInput.GetDown(OVRInput.RawButton.Y);
    }

    private void destroyObject(GameObject objectToDestroy){
        DestroyImmediate(objectToDestroy.gameObject);
        sound.Play();
    }
    private void removeConstraints(GameObject objectToRemoveConstraintsOn){
        objectToRemoveConstraintsOn.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        sound.Play();
    }

    // Update is called once per frame
    void Update()
    {
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
        if(buttonPressed == OVRInput.RawButton.None) return;

        openDoor.handleCodeIndex(buttonPressed);
        openSafeDoor.handleCodeIndex(buttonPressed);
        dropLightsaber.handleCodeIndex(buttonPressed);

        if(openDoor.completed()) destroyObject(openDoor.related_object);
        if(openSafeDoor.completed()) destroyObject(openSafeDoor.related_object);
        if(dropLightsaber.completed()) removeConstraints(dropLightsaber.related_object);

    }
}
