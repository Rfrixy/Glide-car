using System.Collections;
using System.Collections.Generic;
using UnityEngine;

	public class BrakeLightDiv : MonoBehaviour {
		private Renderer m_Renderer;
		public plainHover car; // reference to the car controller, must be dragged in inspector

		// Use this for initialization
		void Awake () {
			m_Renderer = GetComponent<Renderer>();

		}
		
		// Update is called once per frame
		void Update () {
			if(car.grounded)
			m_Renderer.enabled = car.powerInput < 0f;

		}
	}

