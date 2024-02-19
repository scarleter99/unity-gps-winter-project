﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TEST CODE
public class Test : MonoBehaviour
{
    private void Start()
    {
        Managers.InputMng.KeyAction -= OnKeyboardClick;
        Managers.InputMng.KeyAction += OnKeyboardClick;
    }

    private void OnKeyboardClick()
    {
        Hero hero = Managers.ObjectMng.Heroes[Managers.ObjectMng.NextHeroId - 1];
        
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BaseItem item = hero.Bag.StoreItem(Define.ITEM_HEALPORTION_ID);
            Debug.Log($"{item.ItemData.Name}: {item.Count}");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            // int itemIdx = 0;
            // Bag.UseItem(itemIdx, this.Id);
            // if (Bag.Items[itemIdx] != null)
            //     Debug.Log($"{Bag.Items[itemIdx].ItemData.Name}: {Bag.Items[itemIdx].Count}");
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Weapon weapon = new SampleSingleSword();
            weapon.SetInfo(Define.WEAPON_SAMPLESINGLESWORD_ID);
            hero.EquipWeapon(weapon);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Weapon weapon = new SampleSwordAndShield();
            weapon.SetInfo(Define.WEAPON_SAMPLESWORDANDSHIELD_ID);
            hero.EquipWeapon(weapon);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            Armor body = new SampleBody1();
            body.SetInfo(Define.ARMOR_SAMPLEBODY1_ID);
            hero.EquipArmor(body);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            Armor body = new SampleBody2();
            body.SetInfo(Define.ARMOR_SAMPLEBODY2_ID);
            hero.EquipArmor(body);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            hero.Move.HandleAction(Managers.BattleMng.HeroGrid[1, 1]);
        }
        else if (Input.GetKeyDown(KeyCode.T))
        {
            // TargetCreature = GameObject.Find("@Monsters").transform.GetChild(0).GetComponent<Creature>();
            // AnimState = Define.AnimState.Attack;
            // CurrentAction = new Strike();
            // CurrentAction.Owner = this;
        }
    }
}