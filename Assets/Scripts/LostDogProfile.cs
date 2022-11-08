using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LostDogProfile", menuName = "ScriptableObject/LostDogProfile")]
public class LostDogProfile : ScriptableObject
{
	public string name;
	public string type;
	public string dateLost;
	public Sprite dogImage;
}
