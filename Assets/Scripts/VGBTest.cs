using UnityEngine;
using System.Collections;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using System.IO;

[RequireComponent (typeof (Renderer))]
public class VGBTest : MonoBehaviour {

	public Material standardMaterial;
	public Material offensiveMaterial;
	public Material healMaterial;
	public Material protectionMaterial;
	
	Gesture offensiveGesture;
	Gesture healGesture;
	Gesture protectionGesture;

	string offensiveGestureName = "Offensive";
	string healGestureName = "Heal";
	string protectionGestureName = "Protection";

	VisualGestureBuilderFrameSource vgbFrameSource = null;
	VisualGestureBuilderFrameReader vgbFrameReader = null;
	KinectSensor sensor = null;

	Renderer renderer;

	// Use this for initialization
	void Start () {
		renderer = GetComponent<Renderer>();

		sensor = KinectSensor.GetDefault();
		if (sensor != null)
		{
			if (!sensor.IsOpen)
			{
				sensor.Open();
			}
			
			vgbFrameSource = VisualGestureBuilderFrameSource.Create(sensor, 0);
			vgbFrameReader = vgbFrameSource.OpenReader();
			
			if (vgbFrameReader != null)
			{
				vgbFrameReader.IsPaused = true;
				Debug.Log("vgbFrameReader is paused");
			}
			
			var databasePath = Path.Combine(Application.streamingAssetsPath, "GoinGoblins - Mage.gbd");
			using (VisualGestureBuilderDatabase database = VisualGestureBuilderDatabase.Create(databasePath))
			{
				foreach (Gesture gesture in database.AvailableGestures)
				{
					if(gesture.Name.Equals(offensiveGestureName))
					{
						this.vgbFrameSource.AddGesture(gesture);
						offensiveGesture = gesture;
					}
					if(gesture.Name.Equals(healGestureName))
					{
						this.vgbFrameSource.AddGesture(gesture);
						healGesture = gesture;
					}
					if(gesture.Name.Equals(protectionGestureName))
					{
						this.vgbFrameSource.AddGesture(gesture);
						protectionGesture = gesture;
					}
				}

				vgbFrameReader = vgbFrameSource.OpenReader();
				vgbFrameReader.IsPaused = true;
			}
		}	
	}

	public void SetTrackingId(ulong id)
	{
		vgbFrameReader.IsPaused = false;
		vgbFrameSource.TrackingId = id;
		vgbFrameReader.FrameArrived += vgbFrameArrived;
	}
	
	void vgbFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs args) {
		VisualGestureBuilderFrameReference frameReference = args.FrameReference;
		using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
		{
			if (frame != null && frame.DiscreteGestureResults != null)
			{
				foreach (Gesture gesture in vgbFrameSource.Gestures) {
					DiscreteGestureResult offensiveResult = null;
					DiscreteGestureResult healResult = null;
					DiscreteGestureResult protectionResult = null;
					
					if (frame.DiscreteGestureResults.Count > 0) {
						offensiveResult = frame.DiscreteGestureResults[offensiveGesture];
						healResult = frame.DiscreteGestureResults[healGesture];
						protectionResult = frame.DiscreteGestureResults[protectionGesture];
					}
					
					if (offensiveResult.Detected) {
						renderer.material = offensiveMaterial;
					}
					else if (healResult.Detected) {
						renderer.material = healMaterial;
					}
					else if (protectionResult.Detected) {
						renderer.material = protectionMaterial;
					}
					else {
						renderer.material = standardMaterial;
					}
				}
			}
		}
	}
}
