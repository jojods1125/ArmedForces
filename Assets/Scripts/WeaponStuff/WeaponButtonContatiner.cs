using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WeaponButtonContatiner : MonoBehaviour, ISelectHandler
{
    [Header("Weapon")]
    [Tooltip("Weapon tied to the button")]
    public Weapon weapon;
    [Tooltip("Weapon Damage")]
    public float damage;
    [Tooltip("Weapon Recoil")]
    public float recoil;
    [Tooltip("Weapon Pushback")]
    public float pushback;
    [Tooltip("Variable Attribute")]
    public float variable;
    [Tooltip("Ammo Capacity")]
    public float ammoCapacity;
    [Tooltip("Reload Speed")]
    public float reloadSpeed;

    public void OnSelect(BaseEventData eventData)
    {
        GameObject descriptionBox = transform.parent.parent.parent.parent.parent.parent.Find("Description Box").gameObject;
        WeaponButtonContatiner wbc = GetComponent<WeaponButtonContatiner>();
        Weapon weapon = wbc.weapon;
        descriptionBox.gameObject.SetActive(true);
        Transform nameAndDesc = descriptionBox.transform.Find("Name and Desc");
        nameAndDesc.Find("Name").GetComponent<Text>().text = weapon.weaponName;
        nameAndDesc.Find("Description").GetComponent<Text>().text = weapon.description;

        // Get attribute holder
        Transform attributes = descriptionBox.transform.Find("Attributes");
        Transform dmg = attributes.Find("Damage");
        Transform recoil = attributes.Find("Recoil");
        Transform pushBack = attributes.Find("PushBack");
        Transform variable = attributes.Find("VariableAttribute");
        Transform ammoCap = attributes.Find("AmmoCap");
        Transform reloadSpeed = attributes.Find("ReloadSpeed");

        // Zero vector
        Vector3 zeroed = new Vector3(0f, 0f, 0f);

        if (weapon is W_AutoGun)
        {
            // Set damage bar
            if (wbc.damage > 0)
            {
                dmg.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.damage / MenuManager.Instance().maxAutoDamage, 1f, 1f);
                dmg.Find("BarBack").Find("Negative").localScale = zeroed;
            }
            else if (wbc.damage < 0)
            {
                dmg.Find("BarBack").Find("Negative").localScale = new Vector3(-1 * wbc.damage / MenuManager.Instance().maxAutoDamage, 1f, 1f);
                dmg.Find("BarBack").Find("Positive").localScale = zeroed;
            }

            // Set recoil bar
            if (wbc.recoil > 0)
            {
                recoil.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.recoil / MenuManager.Instance().maxAutoRecoil, 1f, 1f);
                recoil.Find("BarBack").Find("Negative").localScale = zeroed;
            }
            else if (wbc.recoil < 0)
            {
                recoil.Find("BarBack").Find("Negative").localScale = new Vector3(-1 * wbc.recoil / MenuManager.Instance().maxAutoRecoil, 1f, 1f); ;
                recoil.Find("BarBack").Find("Positive").localScale = zeroed;
            }

            // Set pushback bar
            if (wbc.pushback > 0)
            {
                pushBack.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.pushback / MenuManager.Instance().maxAutoPushback, 1f, 1f);
                pushBack.Find("BarBack").Find("Negative").localScale = zeroed;
            }
            else if (wbc.pushback < 0)
            {
                pushBack.Find("BarBack").Find("Negative").localScale = new Vector3(-1 * wbc.pushback / MenuManager.Instance().maxAutoPushback, 1f, 1f);
                pushBack.Find("BarBack").Find("Positive").localScale = zeroed;
            }

            // Set Variable bar
            variable.Find("Text").GetComponent<Text>().text = "Fire Rate";
            variable.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.variable / MenuManager.Instance().maxFireRate, 1f, 1f);

            // Set ammo capacity bar
            ammoCap.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.ammoCapacity / MenuManager.Instance().maxAutoAmmo, 1f, 1f);

            // Set reload speed bar
            reloadSpeed.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.reloadSpeed / MenuManager.Instance().maxAutoReloadSpeed, 1f, 1f);
        }
        else if (weapon is W_SemiGun)
        {
            // Set damage bar
            if (wbc.damage > 0)
            {
                dmg.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.damage / MenuManager.Instance().maxSemiDamage, 1f, 1f);
                dmg.Find("BarBack").Find("Negative").localScale = zeroed;
            }
            else if (wbc.damage < 0)
            {
                dmg.Find("BarBack").Find("Negative").localScale = new Vector3(-1 * wbc.damage / MenuManager.Instance().maxSemiDamage, 1f, 1f);
                dmg.Find("BarBack").Find("Positive").localScale = zeroed;
            }

            // Set recoil bar
            if (wbc.recoil > 0)
            {
                recoil.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.recoil / MenuManager.Instance().maxSemiRecoil, 1f, 1f);
                recoil.Find("BarBack").Find("Negative").localScale = zeroed;
            }
            else if (wbc.recoil < 0)
            {
                recoil.Find("BarBack").Find("Negative").localScale = new Vector3(-1 * wbc.recoil / MenuManager.Instance().maxSemiRecoil, 1f, 1f); ;
                recoil.Find("BarBack").Find("Positive").localScale = zeroed;
            }

            // Set pushback bar
            if (wbc.pushback > 0)
            {
                pushBack.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.pushback / MenuManager.Instance().maxSemiPushback, 1f, 1f);
                pushBack.Find("BarBack").Find("Negative").localScale = zeroed;
            }
            else if (wbc.pushback < 0)
            {
                pushBack.Find("BarBack").Find("Negative").localScale = new Vector3(-1 * wbc.pushback / MenuManager.Instance().maxSemiPushback, 1f, 1f);
                pushBack.Find("BarBack").Find("Positive").localScale = zeroed;
            }

            // Set Variable bar
            variable.Find("Text").GetComponent<Text>().text = "Scatter Count";
            variable.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.variable / MenuManager.Instance().maxScatterCount, 1f, 1f);

            // Set ammo capacity bar
            ammoCap.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.ammoCapacity / MenuManager.Instance().maxSemiAmmo, 1f, 1f);

            // Set reload speed bar
            reloadSpeed.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.reloadSpeed / MenuManager.Instance().maxSemiReloadSpeed, 1f, 1f);
        }
        else if (weapon is W_Launcher)
        {
            // Set damage bar
            if (wbc.damage > 0)
            {
                dmg.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.damage / MenuManager.Instance().maxLauncherDamage, 1f, 1f);
                dmg.Find("BarBack").Find("Negative").localScale = zeroed;
            }
            else if (wbc.damage < 0)
            {
                dmg.Find("BarBack").Find("Negative").localScale = new Vector3(-1 * wbc.damage / MenuManager.Instance().maxLauncherDamage, 1f, 1f);
                dmg.Find("BarBack").Find("Positive").localScale = zeroed;
            }

            // Set recoil bar
            if (wbc.recoil > 0)
            {
                recoil.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.recoil / MenuManager.Instance().maxLauncherRecoil, 1f, 1f);
                recoil.Find("BarBack").Find("Negative").localScale = zeroed;
            }
            else if (wbc.recoil < 0)
            {
                recoil.Find("BarBack").Find("Negative").localScale = new Vector3(-1 * wbc.recoil / MenuManager.Instance().maxLauncherRecoil, 1f, 1f); ;
                recoil.Find("BarBack").Find("Positive").localScale = zeroed;
            }

            // Set pushback bar
            if (wbc.pushback > 0)
            {
                pushBack.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.pushback / MenuManager.Instance().maxLauncherPushback, 1f, 1f);
                pushBack.Find("BarBack").Find("Negative").localScale = zeroed;
            }
            else if (wbc.pushback < 0)
            {
                pushBack.Find("BarBack").Find("Negative").localScale = new Vector3(-1 * wbc.pushback / MenuManager.Instance().maxLauncherPushback, 1f, 1f);
                pushBack.Find("BarBack").Find("Positive").localScale = zeroed;
            }

            // Set Variable bar
            variable.Find("Text").GetComponent<Text>().text = "Projectile Power";
            variable.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.variable / MenuManager.Instance().maxProjectilePower, 1f, 1f);

            // Set ammo capacity bar
            ammoCap.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.ammoCapacity / MenuManager.Instance().maxLauncherAmmo, 1f, 1f);

            // Set reload speed bar
            reloadSpeed.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.reloadSpeed / MenuManager.Instance().maxLauncherReloadSpeed, 1f, 1f);
        }
        else if (weapon is W_Sprayer)
        {
            // Set damage bar
            if (wbc.damage > 0)
            {
                dmg.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.damage / MenuManager.Instance().maxSprayerDamage, 1f, 1f);
                dmg.Find("BarBack").Find("Negative").localScale = zeroed;
            }
            else if (wbc.damage < 0)
            {
                dmg.Find("BarBack").Find("Negative").localScale = new Vector3(-1 * wbc.damage / MenuManager.Instance().maxSprayerDamage, 1f, 1f);
                dmg.Find("BarBack").Find("Positive").localScale = zeroed;
            }

            // Set recoil bar
            if (wbc.recoil > 0)
            {
                recoil.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.recoil / MenuManager.Instance().maxSprayerRecoil, 1f, 1f);
                recoil.Find("BarBack").Find("Negative").localScale = zeroed;
            }
            else if (wbc.recoil < 0)
            {
                recoil.Find("BarBack").Find("Negative").localScale = new Vector3(-1 * wbc.recoil / MenuManager.Instance().maxSprayerRecoil, 1f, 1f); ;
                recoil.Find("BarBack").Find("Positive").localScale = zeroed;
            }

            // Set pushback bar
            if (wbc.pushback > 0)
            {
                pushBack.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.pushback / MenuManager.Instance().maxSprayerPushback, 1f, 1f);
                pushBack.Find("BarBack").Find("Negative").localScale = zeroed;
            }
            else if (wbc.pushback < 0)
            {
                pushBack.Find("BarBack").Find("Negative").localScale = new Vector3(-1 * wbc.pushback / MenuManager.Instance().maxSprayerPushback, 1f, 1f);
                pushBack.Find("BarBack").Find("Positive").localScale = zeroed;
            }

            // Set Variable bar
            variable.Find("Text").GetComponent<Text>().text = "Sprayer Distance";
            variable.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.variable / MenuManager.Instance().maxSprayDistance, 1f, 1f);

            // Set ammo capacity bar
            ammoCap.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.ammoCapacity / MenuManager.Instance().maxSprayerAmmo, 1f, 1f);

            // Set reload speed bar
            reloadSpeed.Find("BarBack").Find("Positive").localScale = new Vector3(wbc.reloadSpeed / MenuManager.Instance().maxSprayerReloadSpeed, 1f, 1f);
        }
    }
}
