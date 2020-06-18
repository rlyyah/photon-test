using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerNameInput : MonoBehaviour
{
    [SerializeField] private TMP_InputField nameInputField = null;
    [SerializeField] private Button continueButton = null;

    private const string PlayerPrefsNamesKey = "PlayerName";

    private void Start() => SetUpInputField();

    private void SetUpInputField()
    {
        if(!PlayerPrefs.HasKey(PlayerPrefsNamesKey)) { return; }

        string defaultName = PlayerPrefs.GetString(PlayerPrefsNamesKey);

        nameInputField.text = defaultName;

        SetPlayerName(defaultName);
    }

    public void SetPlayerName(string name)
    {

        //continueButton.interactable = !string.IsNullOrEmpty(name);
        continueButton.interactable = !string.IsNullOrEmpty(nameInputField.text);
    }
     
    public void SavePlayerName()
    {
        string playerName = nameInputField.text;

        PhotonNetwork.NickName = playerName;

        PlayerPrefs.SetString(PlayerPrefsNamesKey, playerName);
    }
}
