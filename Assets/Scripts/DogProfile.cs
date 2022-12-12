using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DogProfile : MonoBehaviour
{
	public TMP_Text dogName;
	public TMP_Text type;
	public TMP_Text date;
	public TMP_Text description;
	public Image dogImage;
	
	public SaveDogProfile connectedProfile;
	
	public void CreateNewDogProfile(SaveDogProfile newProfile)
	{
		connectedProfile = newProfile;
		dogName.text = "Name: " + newProfile.dogName;
		type.text = "Type: " + newProfile.dogType;
		//description.text = newProfile.dogDescription;
		dogImage.sprite = newProfile.dogImage;
		if(newProfile.isLost)
		{
			date.text = "Date Lost: " + newProfile.dogDate;
		}
		else
		{
			date.text = "Date Found: " + newProfile.dogDate;
		}
	}
}