/**
WebCamController

Copyright (c) 2020 Yukiya Okuda

This software is released under the MIT License.
http://opensource.org/licenses/mit-license.php
*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebCamController : MonoBehaviour {

	private int width;
	private int height;
	private int fps;

	private bool isPlaying;
	WebCamTexture webcamTexture;

	private Dropdown dropdown;
	private List<string> deviceNames;





	private void Start() {
		width = 1920 / 3;
		height = 1080 / 3;
		fps = 30;

		WebCamDevice[] devices = WebCamTexture.devices;
		deviceNames = new List<string>();
		for (var i = 0; i < devices.Length; i++) {
			deviceNames.Add(devices[i].name);
		}

		dropdown = GameObject.Find("VideoDeviceList").GetComponent<Dropdown>();
		dropdown.ClearOptions();
		dropdown.AddOptions(deviceNames);
		dropdown.onValueChanged.AddListener(DropdownValueChangeHandler);
		dropdown.value = 0;

		isPlaying = false;
		PlayCamera();
	}





	private void PlayCamera() {
		if (isPlaying || deviceNames.Count == 0) return;
		isPlaying = true;

		int deviceIndex = dropdown.value;
		string deviceName = deviceNames[deviceIndex];

		webcamTexture = new WebCamTexture(deviceName, width, height, fps);
		GetComponent<Renderer>().material.mainTexture = webcamTexture;
		webcamTexture.Play();
	}

	private void StopCamera() {
		if (!isPlaying) return;
		isPlaying = false;

		GetComponent<Renderer>().material.mainTexture = null;

		GameObject.Destroy(webcamTexture);
		webcamTexture = null;
	}





	private async void DropdownValueChangeHandler(int value) {
		StopCamera();
		PlayCamera();
	}
}