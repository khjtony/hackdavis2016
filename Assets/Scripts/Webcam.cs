using UnityEngine;

public class Webcam : MonoBehaviour
{
	
	public MeshRenderer[] UseWebcamTexture;
    public int deviceNum=1;
	private WebCamTexture webcamTexture;
	
	void Start()
	{
        WebCamDevice[] devices = WebCamTexture.devices;
        for (int i = 0; i < devices.Length; i++)
        {
            print("The camera "+i+" is" + devices[i].name);
        }
        webcamTexture = new WebCamTexture(devices[deviceNum].name, 1280, 720, 30);

        webcamTexture.deviceName = devices[deviceNum].name;
		
        foreach(MeshRenderer r in UseWebcamTexture)
		{
			r.material.mainTexture = webcamTexture;
		}
        
		GetComponent<Renderer>().material.mainTexture = webcamTexture;
		webcamTexture.Play();
	}
	
	void OnGUI()
	{
		
	}
}