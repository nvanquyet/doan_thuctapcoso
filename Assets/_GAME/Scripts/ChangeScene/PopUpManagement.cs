using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PopUpManagement : MonoBehaviour
{
    public GameObject popUpPrefab;
    public GameObject canvasObject;

    /*public void Start()
    {
        CreatePopUp("Test name");
    }*/
    public void CreatePopUp(string name)
    {
        GameObject createdPopUpObject = Instantiate(popUpPrefab, canvasObject.transform); 
        createdPopUpObject.GetComponent<PopUp>().SetPopUpName(name);
        MovePopUp(createdPopUpObject);
    }
    public void MovePopUp(GameObject createdPopUpObject)
    {
        createdPopUpObject.GetComponent<RectTransform>().DOAnchorPosY(-50, 1f).OnComplete(() => Destroy(createdPopUpObject));
    }
    public void OnLoginButtonClicked()
    {
        CreatePopUp("Successful!");
    }
}
