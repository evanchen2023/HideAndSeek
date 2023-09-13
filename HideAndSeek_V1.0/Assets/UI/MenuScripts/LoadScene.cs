using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
	private Button btn;
	public String sceneName;
	void Start () {
		Button btn = GetComponent<Button>();
		btn.onClick.AddListener(TaskOnClick);
	}

	void TaskOnClick(){
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
	}
}
