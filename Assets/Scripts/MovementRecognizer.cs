using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using System.Linq;
using TMPro;
    
public class MovementRecognizer : MonoBehaviour
{
    [SerializeField] public XRNode xrNode = XRNode.LeftHand;
    private InputDevice device;
    private List<InputDevice> devices = new List<InputDevice>();

    private List<Vector3> positionList = new List<Vector3>();
    public Transform movementSource;

    bool triggerButtonAction = false;
    static bool trackingOn = false;

    public float inputThreshold = 0.1f;
    public float newPositionThresholdDistance = 0.002f; //0,002f 2mm

    public TextMeshProUGUI simpleUIText; 

    void GetDevice()
    {
        InputDevices.GetDevicesAtXRNode(xrNode, devices);
        device = devices.FirstOrDefault();
    }

    void OnEnable() 
    {

        if(!device.isValid)
        {
            GetDevice();
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        positionList.Clear();
        positionList.Add(movementSource.position);
        StartCoroutine(displayPosition());
    }

    // Update is called once per frame
    void Update()
    {
        //InputHelpers.IsPressed(InputDevices.GetDeviceAtXRNode(inputSource), inputButton, out bool isPressed, inputThreshold);

        //Start breath tracking
        if(!device.isValid)
        {
            GetDevice();
        }


        if(device.TryGetFeatureValue(CommonUsages.triggerButton, out triggerButtonAction) && triggerButtonAction)
        {
            trackingOn = true;
            //simpleUIText.text += "Trigger pressed" + "\n";
             
        }

        if(true)
        {
            Vector3 previousPosition = positionList[positionList.Count - 1];
            if(Vector3.Distance(movementSource.position, previousPosition) > newPositionThresholdDistance)
            {
                positionList.Add(movementSource.position);
            }
        }

    }


    IEnumerator displayPosition() 
    {  
        simpleUIText.text = positionList[positionList.Count - 1].ToString("F3") + "\n";    
        yield return new WaitForSeconds(0.5f);
    }

}
