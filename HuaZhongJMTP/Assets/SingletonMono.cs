using UnityEngine;
using System.Collections;
using System;
public class SingletonMono<T> : MonoBehaviour where T:MonoBehaviour
{

	protected static T instance = null;

	public static T getInstance
	{
		get
		{ 
			return instance;
		}
	}

	protected  SingletonMono()
	{

    }

	protected virtual void Awake()
	{
		instance = GetComponent<T> ();
	}

}
