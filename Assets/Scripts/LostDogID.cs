using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LostDogID : MonoBehaviour
{
	
	public LostDogProfile profile;
	
	[SerializeField] Image dogImage;
	[SerializeField] TMP_Text dogName;
	[SerializeField] TMP_Text dogType;
	[SerializeField] TMP_Text dateLost;
	
    // Start is called before the first frame update
    void Start()
    {
        dogImage.sprite = profile.dogImage;
		dogName.text += profile.name;
		dogType.text += profile.type;
		dateLost.text += profile.dateLost;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
