//------------------------------------------------------------------------------
// <copyright file="GestureDetector.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.IO;
using Windows.Kinect;
using Microsoft.Kinect.VisualGestureBuilder;
using UnityEngine;

public class GestureEventArgs : EventArgs
{
    public bool IsBodyTrackingIdValid { get; private set; }

    public bool IsGestureDetected { get; private set; }

    public float DetectionConfidence { get; private set; }

    public GestureEventArgs(bool isBodyTrackingIdValid, bool isGestureDetected, float detectionConfidence)
    {
        this.IsBodyTrackingIdValid = isBodyTrackingIdValid;
        this.IsGestureDetected = isGestureDetected;
        this.DetectionConfidence = detectionConfidence;
    }
}

/// <summary>
/// Gesture Detector class which listens for VisualGestureBuilderFrame events from the service
/// and calls the OnGestureDetected event handler when a gesture is detected.
/// </summary>
public class GestureDetector : IDisposable
{
    /// <summary> Path to the gesture database that was trained with VGB </summary>
    private readonly string gestureDatabase = "left and right.gbd";

    /// <summary> Name of the discrete gesture in the database that we want to track </summary>
    //private readonly string seatedGestureName = "handUp";
	//private readonly string notseatedGestureName = "twirlProgress";
	private readonly string meteor = "Spell2";
	private readonly string holy = "liftarmhigh";
	private readonly string hurricane = "liftarmcast";

    /// <summary> Gesture frame source which should be tied to a body tracking ID </summary>
    private VisualGestureBuilderFrameSource vgbFrameSource = null;

    /// <summary> Gesture frame reader which will handle gesture events coming from the sensor </summary>
    private VisualGestureBuilderFrameReader vgbFrameReader = null;

    public event EventHandler<GestureEventArgs> OnGestureDetected;

	private KinectManager kinectManager;


    /// <summary>
    /// Initializes a new instance of the GestureDetector class along with the gesture frame source and reader
    /// </summary>
    /// <param name="kinectSensor">Active sensor to initialize the VisualGestureBuilderFrameSource object with</param>
    public GestureDetector(KinectSensor kinectSensor, KinectManager kinectManager)
    {
		this.kinectManager = kinectManager;

        if (kinectSensor == null)
        {
            throw new ArgumentNullException("kinectSensor");
        }

        // create the vgb source. The associated body tracking ID will be set when a valid body frame arrives from the sensor.
        this.vgbFrameSource = VisualGestureBuilderFrameSource.Create(kinectSensor, 0);
        this.vgbFrameSource.TrackingIdLost += this.Source_TrackingIdLost;

        // open the reader for the vgb frames
        this.vgbFrameReader = this.vgbFrameSource.OpenReader();
        if (this.vgbFrameReader != null)
        {
            this.vgbFrameReader.IsPaused = true;
            this.vgbFrameReader.FrameArrived += this.Reader_GestureFrameArrived;
        }

        // load the 'Magic101' gesture from the gesture database
        var databasePath = Path.Combine(Application.streamingAssetsPath, this.gestureDatabase);
        using (VisualGestureBuilderDatabase database = VisualGestureBuilderDatabase.Create(databasePath))
        {
            // we could load all available gestures in the database with a call to vgbFrameSource.AddGestures(database.AvailableGestures), 
            // but for this program, we only want to track one discrete gesture from the database, so we'll load it by name
            foreach (Gesture gesture in database.AvailableGestures)
            {
//                if (gesture.Name.Equals(this.seatedGestureName))
//                {
//                    this.vgbFrameSource.AddGesture(gesture);
//					Debug.Log(gesture.Name);
//                }
//				if (gesture.Name.Equals(this.notseatedGestureName))
//				{
//					this.vgbFrameSource.AddGesture(gesture);
//					Debug.Log(gesture.Name);
//				}
				if (gesture.Name.Equals (this.meteor))
				{
					this.vgbFrameSource.AddGesture(gesture);
					Debug.Log(gesture.Name);
				}
				if (gesture.Name.Equals (this.holy))
				{
					this.vgbFrameSource.AddGesture(gesture);
					Debug.Log(gesture.Name);
				}
				if (gesture.Name.Equals (this.hurricane))
				{
					this.vgbFrameSource.AddGesture(gesture);
					Debug.Log(gesture.Name);
				}
            }
        }
    }

    /// <summary>
    /// Gets or sets the body tracking ID associated with the current detector
    /// The tracking ID can change whenever a body comes in/out of scope
    /// </summary>
    public ulong TrackingId
    {
        get
        {
            return this.vgbFrameSource.TrackingId;
        }

        set
        {
            if (this.vgbFrameSource.TrackingId != value)
            {
                this.vgbFrameSource.TrackingId = value;
            }
        }
    }

    /// <summary>
    /// Gets or sets a value indicating whether or not the detector is currently paused
    /// If the body tracking ID associated with the detector is not valid, then the detector should be paused
    /// </summary>
    public bool IsPaused
    {
        get
        {
            return this.vgbFrameReader.IsPaused;
        }

        set
        {
            if (this.vgbFrameReader.IsPaused != value)
            {
                this.vgbFrameReader.IsPaused = value;
            }
        }
    }

    /// <summary>
    /// Disposes all unmanaged resources for the class
    /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Disposes the VisualGestureBuilderFrameSource and VisualGestureBuilderFrameReader objects
    /// </summary>
    /// <param name="disposing">True if Dispose was called directly, false if the GC handles the disposing</param>
    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (this.vgbFrameReader != null)
            {
                this.vgbFrameReader.FrameArrived -= this.Reader_GestureFrameArrived;
                this.vgbFrameReader.Dispose();
                this.vgbFrameReader = null;
            }

            if (this.vgbFrameSource != null)
            {
                this.vgbFrameSource.TrackingIdLost -= this.Source_TrackingIdLost;
                this.vgbFrameSource.Dispose();
                this.vgbFrameSource = null;
            }
        }
    }

    /// <summary>
    /// Handles gesture detection results arriving from the sensor for the associated body tracking Id
    /// </summary>
    /// <param name="sender">object sending the event</param>
    /// <param name="e">event arguments</param>
    private void Reader_GestureFrameArrived(object sender, VisualGestureBuilderFrameArrivedEventArgs e)
    {
        VisualGestureBuilderFrameReference frameReference = e.FrameReference;
        using (VisualGestureBuilderFrame frame = frameReference.AcquireFrame())
        {
            if (frame != null)
            {
                // get the discrete gesture results which arrived with the latest frame
                var discreteResults = frame.DiscreteGestureResults;

                if (discreteResults != null)
                {
                    // we only have one gesture in this source object, but you can get multiple gestures
                    foreach (Gesture gesture in this.vgbFrameSource.Gestures)
                    {
//                        if (gesture.Name.Equals(this.seatedGestureName) && gesture.GestureType == GestureType.Discrete)
//                        {
//                            DiscreteGestureResult result = null;
//                            discreteResults.TryGetValue(gesture, out result);
//
//                            if (result != null)
//                            {
//                                if (this.OnGestureDetected != null)
//                                {
//                                    //this.OnGestureDetected(this, new GestureEventArgs(true, result.Detected, result.Confidence));
//									Debug.Log ("seated");
//                                }
//                            }
//                        }
						if (gesture.Name.Equals(this.holy) && gesture.GestureType == GestureType.Discrete)
						{
							DiscreteGestureResult result = null;
							discreteResults.TryGetValue(gesture, out result);

							if (result != null)
							{
								if(this.OnGestureDetected != null && result.Confidence >= .7f)
								{
									Debug.Log ("holy fired");
									kinectManager.ActivateSpell(3);
								}
							}
						}
						if (gesture.Name.Equals(this.hurricane) && gesture.GestureType == GestureType.Discrete)
						{
							DiscreteGestureResult result = null;
							discreteResults.TryGetValue(gesture, out result);
							
							if (result != null)
							{
								if(this.OnGestureDetected != null && result.Confidence >= .7f)
								{
									Debug.Log ("hurricane fired");
									kinectManager.ActivateSpell(4);
								}
							}
						}
						if (gesture.Name.Equals(this.meteor) && gesture.GestureType == GestureType.Discrete)
						{
							DiscreteGestureResult result = null;
							discreteResults.TryGetValue(gesture, out result);
							
							if (result != null)
							{
								if(this.OnGestureDetected != null && result.Confidence >= .7f)
								{
									Debug.Log ("meteor fired");
									kinectManager.ActivateSpell(2);
								}
							}
						}

                    }
                }

				//get the continuous results which arrived with the latest frame
				var continuousResults = frame.ContinuousGestureResults;

				if (continuousResults != null)
				{
					foreach (Gesture gesture in this.vgbFrameSource.Gestures)
					{
						Debug.Log ("searching for Conti motion");
//						if(gesture.Name.Equals (this.notseatedGestureName) && gesture.GestureType == GestureType.Continuous)
//						{
//							Debug.Log ("found Conti!");
//							ContinuousGestureResult resultCon = null;
//							continuousResults.TryGetValue(gesture, out resultCon);
//
////							DiscreteGestureResult resultDis = null;
////							discreteResults.TryGetValue(gesture, out resultDis);
//
//							if(resultCon.Progress >= .6f) //&& resultDis != null)
//							{
//								if (this.OnGestureDetected != null)
//								{
//									kinectManager.ActivateSpell(1);
//									Debug.Log ("twirled");
//								}
//							}
//						}
//						if(gesture.Name.Equals (this.meteor) && gesture.GestureType == GestureType.Continuous)
//						{
//							Debug.Log ("meteor!!");
//							ContinuousGestureResult resultCon = null;
//							continuousResults.TryGetValue(gesture, out resultCon);
//
//							if(resultCon.Progress >= .7f)
//							{
//								if(this.OnGestureDetected != null)
//								{
//									kinectManager.ActivateSpell(2);
//									Debug.Log ("meteor is down!");
//								}
//							}
//						}
					}
				}
            }
        }
    }

    /// <summary>
    /// Handles the TrackingIdLost event for the VisualGestureBuilderSource object
    /// </summary>
    /// <param name="sender">object sending the event</param>
    /// <param name="e">event arguments</param>
    private void Source_TrackingIdLost(object sender, TrackingIdLostEventArgs e)
    {
        if (this.OnGestureDetected != null)
        {
            this.OnGestureDetected(this, new GestureEventArgs(false, false, 0.0f));
        }
    }
}
