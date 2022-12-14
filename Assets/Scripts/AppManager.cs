using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AppManager : MonoBehaviour
{
	public GameObject LostTab;
	public GameObject FoundTab;
	public GameObject AddDogButton;
	public GameObject InputDog;
	
	public GameObject dogProfilePrefab;
	public GameObject lostList;
	public GameObject foundList;
	
	public SaveDogProfiles profiles;
	
	[Header("Add New Dog")]
	public TMP_InputField nameInputField;
	public TMP_InputField typeInputField;
	public TMP_InputField dateInputField;
	public TMP_InputField descriptionInputField;
	public TMP_Dropdown lostOrFoundDropdown;
	public Image dogImageField;
	
	byte[] dogImageData;
	
	void Start()
	{
		LoadData();
		UpdateData();
	}
	
	public void OpenLostDogTab()
	{
		LostTab.SetActive(true);
		AddDogButton.SetActive(true);
		
		FoundTab.SetActive(false);
		InputDog.SetActive(false);
	}
	
	public void OpenFoundDogTab()
	{
		FoundTab.SetActive(true);
		AddDogButton.SetActive(true);
		
		LostTab.SetActive(false);
		InputDog.SetActive(false);
	}
	
	public void AddDogScreen()
	{
		LostTab.SetActive(false);
		FoundTab.SetActive(false);
		AddDogButton.SetActive(false);
		InputDog.SetActive(true);
	}
	
	public void CloseAddDogScreen()
	{
		LostTab.SetActive(true);
		FoundTab.SetActive(false);
		AddDogButton.SetActive(true);
		InputDog.SetActive(false);
	}
	
    public void AddDog()
	{
		SaveDogProfile newDogProfile = new SaveDogProfile();
		
		newDogProfile.dogName = nameInputField.text;
		nameInputField.text = "";
		newDogProfile.dogType = typeInputField.text;
		typeInputField.text = "";
		newDogProfile.dogDate = dateInputField.text;
		dateInputField.text = "";
		newDogProfile.dogDescription = descriptionInputField.text;
		descriptionInputField.text = "";
		
		// var texture = new Texture2D(1, 1);
		// texture = dogImageField.sprite.texture;
		newDogProfile.dogImage = dogImageData;
		dogImageField.sprite = null;
		
		if(lostOrFoundDropdown.captionText.text == "Lost")
			newDogProfile.isLost = true;
		else
			newDogProfile.isLost = false;

		if(newDogProfile.isLost)
		{
			profiles.lostDogs.Add(newDogProfile);
			OpenLostDogTab();
		}
		else
		{
			profiles.foundDogs.Add(newDogProfile);
			OpenFoundDogTab();
		}
		
		//Save Data
		SaveData();
		//Update Lists
		UpdateData();
	}
	
	public void UpdateData()
	{
		foreach(GameObject profile in lostList.transform)
		{
			Destroy(profile);
		}
		foreach(GameObject profile in foundList.transform)
		{
			Destroy(profile);
		}
		
		foreach(SaveDogProfile profile in profiles.lostDogs)
		{
			GameObject dogProfileInstance = Instantiate(dogProfilePrefab, lostList.transform);
			dogProfileInstance.GetComponent<DogProfile>().CreateNewDogProfile(profile);
		}
		foreach(SaveDogProfile profile in profiles.foundDogs)
		{
			GameObject dogProfileInstance = Instantiate(dogProfilePrefab, foundList.transform);
			dogProfileInstance.GetComponent<DogProfile>().CreateNewDogProfile(profile);
		}
	}
	
	public void TakePicture(int maxSize)
	{
		if(NativeCamera.CheckPermission() != NativeCamera.Permission.Granted)
		{
			NativeCamera.RequestPermission();
		}
		
		NativeCamera.Permission permission = NativeCamera.TakePicture((path) =>
		{
			print("Image path: " + path);
			if(path != null)
			{
				Texture2D texture = NativeCamera.LoadImageAtPath(path, maxSize, false, false);
				if(texture == null)
				{
					print("Couldn't load texture from " + path);
					return;
				}
				// Assign texture to a temporary quad and destroy it after 5 seconds
				GameObject quad = GameObject.CreatePrimitive( PrimitiveType.Quad );
				quad.transform.position = Camera.main.transform.position + Camera.main.transform.forward * 2.5f;
				quad.transform.forward = Camera.main.transform.forward;
				quad.transform.localScale = new Vector3( 1f, texture.height / (float) texture.width, 1f );

				Material material = quad.GetComponent<Renderer>().material;
				if( !material.shader.isSupported ) // happens when Standard shader is not included in the build
					material.shader = Shader.Find( "Legacy Shaders/Diffuse" );

				material.mainTexture = texture;

				Destroy( quad, 5f );

				Sprite dogImageSprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
				dogImageField.sprite = dogImageSprite;
				dogImageField.color = new Color(1, 1, 1, 1);
				
				//texture.ReadPixels(new Rect(0, 0, texture.width, texture.height), 0, 0);
				//texture.Apply();
				dogImageData = texture.EncodeToPNG();
				
				Debug.Log(dogImageData);

				// If a procedural texture is not destroyed manually, 
				// it will only be freed after a scene change
				//Destroy( texture, 5f );
			}
		}, maxSize );

		Debug.Log( "Permission result: " + permission );
	}
	
	public void SaveData()
	{
		string json = JsonUtility.ToJson(profiles);
		File.WriteAllText(Application.persistentDataPath + "/save.txt", json);
	}
	
	public void LoadData()
	{
		if(File.Exists(Application.persistentDataPath + "/save.txt"))
		{
			string saveString = File.ReadAllText(Application.persistentDataPath + "/save.txt");
			
			SaveDogProfiles loadedProfiles = JsonUtility.FromJson<SaveDogProfiles>(saveString);
			
			profiles.lostDogs = loadedProfiles.lostDogs;
			profiles.foundDogs = loadedProfiles.foundDogs;
		}
		UpdateData();
	}

}

[System.Serializable]
public class SaveDogProfiles
{
	public List<SaveDogProfile> lostDogs = new List<SaveDogProfile>();
	public List<SaveDogProfile> foundDogs = new List<SaveDogProfile>();
}

[System.Serializable]
public class SaveDogProfile
{
	public string dogName;
	public string dogType;
	public string dogDate;
	public string dogDescription;
	public byte[] dogImage;
	public bool isLost;
}