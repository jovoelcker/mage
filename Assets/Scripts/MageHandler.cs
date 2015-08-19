using UnityEngine;
using System.Collections.Generic;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using System.IO;

public class MageHandler : MonoBehaviour {

	// The magical effects for the different quests
	public List<MagicEffects> tutorialEffectedObjects;
	public List<MagicEffects> fenceEffectedObjects;
	public List<MagicEffects> spiderEffectedObjects;
	public List<MagicEffects> goblinsEffectedObjects;
	public List<MagicEffects> bossEffectedObjects;

	private Dictionary<Gamestate.QuestStates, List<MagicEffects>> effectedObjects;

	// Specifies the used gestures in the loaded database
	Gesture attackGesture;
	string attackGestureName = "Attack";
	Gesture defendGesture;
	string defendGestureName = "Defend";
	
	VisualGestureBuilderFrameSource vgbFrameSource = null;
	VisualGestureBuilderFrameReader vgbFrameReader = null;
	
	KinectSensor sensor = null;

	// Use this for initialization
	void Start () {
		// The effected Objects depend on the queststate
		effectedObjects = new Dictionary<Gamestate.QuestStates, List<MagicEffects>>();
		effectedObjects.Item[Gamestate.QuestStates.Q_INIT] = tutorialEffectedObjects;
		effectedObjects.Item[Gamestate.QuestStates.Q_BlOCKADE_BEGIN] = fenceEffectedObjects;
		effectedObjects.Item[Gamestate.QuestStates.Q_SPIDER_BEGIN] = spiderEffectedObjects;
		effectedObjects.Item[Gamestate.QuestStates.Q_GOBBLINS_BEGIN] = goblinsEffectedObjects;
		effectedObjects.Item[Gamestate.QuestStates.Q_BOSS_INTERIM] = bossEffectedObjects;

		sensor = KinectSensor.GetDefault();
		if (sensor != null)
		{
			// The database has to be located in the StreamingAssets-folder
			var databasePath = Path.Combine(Application.streamingAssetsPath, "Mage.gbd");

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

			using (VisualGestureBuilderDatabase database = VisualGestureBuilderDatabase.Create(databasePath))
			{
				foreach (Gesture gesture in database.AvailableGestures)
				{
					if(gesture.Name.Equals(attackGestureName))
					{
						this.vgbFrameSource.AddGesture(gesture);
						attackGesture = gesture;
					}
					if(gesture.Name.Equals(defendGestureName))
					{
						this.vgbFrameSource.AddGesture(gesture);
						defendGesture = gesture;
					}
				}
				
				vgbFrameReader = vgbFrameSource.OpenReader();
				vgbFrameReader.IsPaused = true;
			}
		}
	}
	
	// Update is called once per frame
	void Update() {
		float forwardAxis = Input.GetAxisRaw("Vertical");
		if (forwardAxis > 0)
			Attack();
		else if (forwardAxis < 0)
			Defend();
	}
	
	public void SetTrackingId(ulong id)
	{
		vgbFrameReader.IsPaused = false;
		vgbFrameSource.TrackingId = id;
		vgbFrameReader.FrameArrived += vgbFrameArrived;
	}

	// Analyses the incoming frames
	void vgbFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs args) {
		VisualGestureBuilderFrameReference frameReference = args.FrameReference;
		using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
		{
			if (frame != null && frame.DiscreteGestureResults != null)
			{
				foreach (Gesture gesture in vgbFrameSource.Gestures)
				{
					DiscreteGestureResult attackResult = null;
					DiscreteGestureResult defendResult = null;
					
					if (frame.DiscreteGestureResults.Count > 0) {
						attackResult = frame.DiscreteGestureResults[attackGesture];
						defendResult = frame.DiscreteGestureResults[defendGesture];
					}

					// When a gesture is detected, the correct method has to be opened
					if (attackResult.Detected) {
						Attack();
					}
					else if (defendResult.Detected) {
						Defend();
					}
				}
			}
		}
	}

	void Attack() {
		if (effectedObjects.Item[Gamestate.getInstance().questState] != null) {
			// All effected objects can react on the gestures
			foreach (MagicEffects effectedObject in effectedObjects.Item[Gamestate.getInstance().questState]) {
				effectedObject.Attack();
			}
		}
	}

	void Defend() {
		if (effectedObjects.Item[Gamestate.getInstance().questState] != null) {
			// All effected objects can react on the gestures
			foreach (MagicEffects effectedObject in effectedObjects.Item[Gamestate.getInstance().questState]) {
				effectedObject.Defend();
			}
		}
	}

}
