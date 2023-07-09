using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    //Menus
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject informationMenu;
    //Page for Legend
    [SerializeField] GameObject page1;
    [SerializeField] GameObject page2;
    [SerializeField] TMP_Text page;
    public void OnPageButton()
    {
        if (page1.activeSelf)
        {
            page.text = "Page: 1";
            page2.SetActive(true);
            page1.SetActive(false);
        }
        else
        {
            page.text = "Page: 2";
            page1.SetActive(true);
            page2.SetActive(false);
        }
    }
    public void OnButtonLegend()
    {
        mainMenu.SetActive(false);
        informationMenu.SetActive(true);
        page.text = "Page: 2";
        page1.SetActive(true);
        page2.SetActive(false);
    }
    public void OnButtonBack()
    {
        mainMenu.SetActive(true);
        informationMenu.SetActive(false);
    }
    public void OnButtonPlayGame()
    {
        SceneManager.LoadScene(1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
