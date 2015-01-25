using UnityEngine;
using System.Collections;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;

public class CustomGestureManager : MonoBehaviour {
	VisualGestureBuilderDatabase _gestureDatabase;
	VisualGestureBuilderFrameSource _gestureFrameSource;
	VisualGestureBuilderFrameReader _gestureFrameReader;
	KinectSensor _kinect;

	Gesture _handUp;
	Gesture _twirlProgress;

	public GameObject AttachedObject;

	public void SetTrackingId(ulong id)
	{
		_gestureFrameReader.IsPaused = false;
		_gestureFrameSource.TrackingId = id;
		_gestureFrameReader.FrameArrived += _gestureFrameReader_FrameArrived;
	}

	// Use this for initialization
	void Start () {

		_kinect = KinectSensor.GetDefault ();
		_gestureDatabase = VisualGestureBuilderDatabase.Create(Application.streamingAssetsPath + "/Magic101.gbd");
		_gestureFrameSource = VisualGestureBuilderFrameSource.Create (_kinect, 0);

		foreach (var gesture in _gestureDatabase.AvailableGestures)
		{
			_gestureFrameSource.AddGesture(gesture);
			if(gesture.Name.Equals ("handUp"))
			{
				_handUp = gesture;
			}
			if(gesture.Name.Equals("twirlProgress"))
			{
				_twirlProgress = gesture;
			}
		}


		_gestureFrameReader = _gestureFrameSource.OpenReader ();
		_gestureFrameReader.IsPaused = true;

	}

	void _gestureFrameReader_FrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
	{
		VisualGestureBuilderFrameReference frameReference = e.FrameReference;
		using(VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
		{
			if(frame != null && frame.DiscreteGestureResults != null)
			{
				DiscreteGestureResult result = null;

				if(frame.DiscreteGestureResults.Count > 0)
					result = frame.DiscreteGestureResults[_handUp];
				if(result == null)
					return;

				if(result.Detected == true)
				{
					var progressResult = frame.ContinuousGestureResults[_twirlProgress];
					GameObject resulting = (GameObject) Instantiate (AttachedObject);
					var prog = progressResult.Progress;
					if(prog >= 0.5f)
					{
						resulting.transform.position = new Vector3(2, 3, 4);
						resulting.rigidbody.velocity = new Vector3(50, 0, 0);
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
