using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public Image weaponA_L_image;
    public Image weaponB_L_image;
    public Image weaponC_L_image;
    public Image weaponD_L_image;

    public Image weaponA_R_image;
    public Image weaponB_R_image;
    public Image weaponC_R_image;
    public Image weaponD_R_image;

    public TMP_Text weaponA_L_ammo;
    public TMP_Text weaponB_L_ammo;
    public TMP_Text weaponC_L_ammo;
    public TMP_Text weaponD_L_ammo;

    public TMP_Text weaponA_R_ammo;
    public TMP_Text weaponB_R_ammo;
    public TMP_Text weaponC_R_ammo;
    public TMP_Text weaponD_R_ammo;


    private Vector2Int weaponA_L_ammoNum;
    private Vector2Int weaponB_L_ammoNum;
    private Vector2Int weaponC_L_ammoNum;
    private Vector2Int weaponD_L_ammoNum;

    private Vector2Int weaponA_R_ammoNum;
    private Vector2Int weaponB_R_ammoNum;
    private Vector2Int weaponC_R_ammoNum;
    private Vector2Int weaponD_R_ammoNum;


    private Arm[] arms;

    // Start is called before the first frame update
    void Start()
    {
        arms = GameManager.Instance().mainPlayer.GetArms();

        Image[] weapons_L_images = new Image[] { weaponA_L_image, weaponB_L_image, weaponC_L_image, weaponD_L_image };
        Image[] weapons_R_images = new Image[] { weaponA_R_image, weaponB_R_image, weaponC_R_image, weaponD_R_image };

        for (int i = 0; i < GameManager.Instance().mainPlayer.backArmWeapons.Length; i++)
        {
            weapons_L_images[i].sprite = GameManager.Instance().mainPlayer.backArmWeapons[i].icon;
        }

        for (int i = 0; i < GameManager.Instance().mainPlayer.frontArmWeapons.Length; i++)
        {
            weapons_R_images[i].sprite = GameManager.Instance().mainPlayer.frontArmWeapons[i].icon;
        }

    }


    public void UpdateUI()
    {
        if (arms[0].armType == ArmType.Back)
        {
            Dictionary<Weapon, int> ammo_L = arms[0].GetAmmoRemaining();
            Debug.Log(ammo_L);
        }
        else
        {
            Dictionary<Weapon, int> ammo_L = arms[1].GetAmmoRemaining();
            for (int i = 0; i < ammo_L.Keys.Count; i++)
            {
                // TODO: CREATE ARRAY IN ARM THAT WILL FORMAT AMMO REMAINING AS NEEDED
            }
        }


    }

}
