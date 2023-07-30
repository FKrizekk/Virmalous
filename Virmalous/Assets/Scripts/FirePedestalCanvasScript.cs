using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirePedestalCanvasScript : MonoBehaviour
{
	public Sprite[] sprites;
	
	public Image img;
	
	GameObject player;
	
	void Start()
	{
		player = GameObject.Find("Camera");
	}
	
	public void SetSprite(int index)
	{
		img.sprite = sprites[index];
	}
	
	void Update()
	{
		transform.rotation = Quaternion.LookRotation(new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z) - transform.position, Vector3.up);
	}
}
