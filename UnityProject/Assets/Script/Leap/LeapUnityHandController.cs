/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2013.                                   *
* Leap Motion proprietary and  confidential.  Not for distribution.            *
* Use subject to the terms of the Leap Motion SDK Agreement available at       *
* https://developer.leapmotion.com/sdk_agreement, or another agreement between *
* Leap Motion and you, your company or other organization.                     *
\******************************************************************************/
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Leap;

/// <summary>
/// This class manipulates the hand representation in the unity scene based on the
/// input from the leap device. Fingers and Palm objects are moved around between
/// higher level 'hand' objects that mainly serve to organize.  Be aware that when
/// fingers are lost, unity does not dispatch OnTriggerExit events.
/// </summary>
public class LeapUnityHandController : MonoBehaviour 
{	
	public HandObjectController[]	m_HandControllerScript = new HandObjectController[2];
	public GameObject[]				m_palms		= null;
	public GameObject[]				m_fingers	= null;
	public GameObject[]				m_hands 	= null;
	public Material[]				m_materials = null;
	public bool						m_DisplayHands = true;
	
	//These arrays allow us to use our game object arrays much like pools.
	//When a new hand/finger is found, we mark a game object by active
	//by storing it's id, and when it goes out of scope we make the
	//corresponding gameobject invisible & set the id to -1.
	private int[]					m_fingerIDs = null;
	private int[]					m_handIDs	= null;
	
	void SetCollidable( GameObject obj, bool collidable )
	{
		foreach( Collider component in obj.GetComponents<Collider>() )
			component.enabled = collidable;
	
		foreach( Collider child in obj.GetComponentsInChildren<Collider>() )
			child.enabled = collidable;
	}
	
	void SetVisible( GameObject obj, bool visible )
	{
		foreach( Renderer component in obj.GetComponents<Renderer>() )
			component.enabled = visible && m_DisplayHands;
		
		foreach( Renderer child in obj.GetComponentsInChildren<Renderer>() )
			child.enabled = visible && m_DisplayHands;
	}
	
	void Start()
	{
		m_fingerIDs = new int[10];
		for( int i = 0; i < m_fingerIDs.Length; i++ )
		{
			m_fingerIDs[i] = -1;	
		}
		
		m_handIDs = new int[2];
		for( int i = 0; i < m_handIDs.Length; i++ )
		{
			m_handIDs[i] = -1;	
		}
		
		LeapInput.HandFound += new LeapInput.HandFoundHandler(OnHandFound);
		LeapInput.HandLost += new LeapInput.ObjectLostHandler(OnHandLost);
		LeapInput.HandUpdated += new LeapInput.HandUpdatedHandler(OnHandUpdated);
		LeapInput.PointableFound += new LeapInput.PointableFoundHandler(OnPointableFound);
		LeapInput.PointableLost += new LeapInput.ObjectLostHandler(OnPointableLost);
		LeapInput.PointableUpdated += new LeapInput.PointableUpdatedHandler(OnPointableUpdated);
		
		//assign default materials
		//foreach( Renderer r in m_hands[2].GetComponentsInChildren<Renderer>() )
		{
			//r.material = m_materials[2];	
		}
		//do a pass to hide the objects.
		/*foreach( GameObject palm in m_palms )
		{
			updatePalm(Leap.Hand.Invalid, palm);
		}*/
		foreach( GameObject finger in m_fingers)
		{
			updatePointable(Leap.Pointable.Invalid, finger);
		}
	}
	
	//When an object is found, we find our first inactive game object, activate it, and assign it to the found id
	//When lost, we deactivate the object & set it's id to -1
	//When updated, load the new data
	void OnPointableUpdated( Pointable p )
	{
		int index = Array.FindIndex(m_fingerIDs, id => id == p.Id);
		if( index != -1 )
		{
			updatePointable( p, m_fingers[index] );	
		}
	}
	void OnPointableFound( Pointable p )
	{
		int index = Array.FindIndex(m_fingerIDs, id => id == -1);
		if( index != -1 )
		{
			m_fingerIDs[index] = p.Id;
			updatePointable( p, m_fingers[index] );
		}
	}
	void OnPointableLost( int lostID )
	{
		int index = Array.FindIndex(m_fingerIDs, id => id == lostID);
		if( index != -1 )
		{
			updatePointable( Pointable.Invalid, m_fingers[index] );
			m_fingerIDs[index] = -1;
		}
	}

	void OnHandFound( Hand h )
	{
		int index = Array.FindIndex(m_handIDs, id => id == -1);
		if( index != -1 )
		{
			if( m_HandControllerScript[1-index].gameObject.activeSelf == false ){
				//m_HandControllerScript[1-index].gameObject.GetComponent<LeapOrigamiCollider>().enabled 	= false;
				//m_HandControllerScript[index].gameObject.GetComponent<LeapOrigamiCollider>().enabled 	= true;
			}
			else{
				//m_HandControllerScript[1-index].gameObject.GetComponent<LeapOrigamiCollider>().enabled 	= false;
				//m_HandControllerScript[index].gameObject.GetComponent<LeapOrigamiCollider>().enabled 	= false;
			}
			m_handIDs[index] = h.Id;
			updatePalm(h, m_HandControllerScript[index]);
		}
	}
	void OnHandUpdated( Hand h )
	{
		int index = Array.FindIndex(m_handIDs, id => id == h.Id);
		if( index != -1 )
		{
			updatePalm(	h, m_HandControllerScript[index] );
		}
	}
	void OnHandLost(int lostID)
	{
		int index = Array.FindIndex(m_handIDs, id => id == lostID);
		if( index != -1 )
		{
			if( m_HandControllerScript[1-index].gameObject.activeSelf == true ){
				//m_HandControllerScript[1-index].gameObject.GetComponent<LeapOrigamiCollider>().enabled = true;
			}
			m_HandControllerScript[index].SetOutOfScreen( true );
			updatePalm(Hand.Invalid, m_HandControllerScript[index]);
			m_handIDs[index] = -1;
		}
	}
	
	void updatePointable( Leap.Pointable pointable, GameObject fingerObject )
	{
		updateParent( fingerObject, pointable.Hand.Id );
		
		//SetVisible(fingerObject, pointable.IsValid);
		SetCollidable(fingerObject, pointable.IsValid);
		
		if ( pointable.IsValid )
		{
			Vector3 vFingerDir = pointable.Direction.ToUnity();
			Vector3 vFingerPos = pointable.TipPosition.ToUnityTranslated();
			
			vFingerPos.z = 4.6f;
			
			fingerObject.transform.localPosition = vFingerPos;
			fingerObject.transform.localRotation = Quaternion.FromToRotation( Vector3.forward, vFingerDir );
		}
	}

	void updatePalm( Leap.Hand leapHand, HandObjectController palmObject )
	{
		//updateParent( palmObject, leapHand.Id);
		
		//SetVisible(palmObject.gameObject, leapHand.IsValid);
		SetCollidable(palmObject.gameObject, leapHand.IsValid);
		
		if( leapHand.IsValid )
		{
			Vector3 Pos = leapHand.PalmPosition.ToUnityTranslated();
			Pos.z = 4.4f;
			/*
			Pos = Camera.main.WorldToScreenPoint( Pos );
			Pos.z = 10.5f;
			Pos = Camera.main.ScreenToWorldPoint( Pos );
			*/
			palmObject.SetPos( Pos );
			palmObject.SetOutOfScreen( false );
		}
	}
	
	void updateParent( GameObject child, int handId )
	{
		//check the hand & update the parent
		int handIndex = Array.FindIndex(m_handIDs, id => id == handId);
		if( handIndex == -1 || handId == -1 )
			handIndex = 2;
		
		GameObject parent = m_hands[handIndex];
		if( child.transform.parent != parent.transform )
		{
			child.transform.parent = parent.transform;
			/*
			foreach( Renderer r in child.GetComponents<Renderer>() )
				r.material = m_materials[handIndex];;	
			foreach( Renderer r in child.GetComponentsInChildren<Renderer>() )
				r.material = m_materials[handIndex];;
				*/
		}
	}
}
