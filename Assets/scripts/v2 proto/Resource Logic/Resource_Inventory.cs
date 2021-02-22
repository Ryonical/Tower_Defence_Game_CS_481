﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//added
using System.Linq;

//SINGLETON! Don't have more than 1!


public class Resource_Inventory : MonoBehaviour
{
    

    #region MEMBERS
    public static List<Resource_Attributes> resource_inventory;
    #endregion
    #region EVENTS

    #endregion
    #region EVENT SUBSCRIPTIONS
    private void OnEnable()
    {
        Resource_V2.AddResourceEvent += AddResourcesToInventory;
    }
    private void OnDisable()
    {
        Resource_V2.AddResourceEvent -= AddResourcesToInventory;
    }
    #endregion
    #region EVENT HANDLERS
    void AddResourcesToInventory(object caller, AddResourceArgs args)
    {
        Resource_Attributes temp = FindResourceAttribute(args.resource_type);

        if (temp != null)
            temp.resource_amount += args.resource_amount;

        PrintResourceInventory();
    }
    #endregion
    #region INIT
    private void Start()
    {
        if(resource_inventory == null)
        {
            resource_inventory = new List<Resource_Attributes>();
            foreach (Resource_V2.ResourceType rsc_t in System.Enum.GetValues(typeof(Resource_V2.ResourceType)))
            {
                Resource_Attributes temp = gameObject.AddComponent<Resource_Attributes>();
                temp.resource_type = rsc_t;
                temp.resource_amount = 0;
                resource_inventory.Add(temp);

                //Debug logging stuff
                //Resource_Attributes temp = resource_inventory.Find(x => x.resource_type == rsc_t);
                //Debug.Log("Init Resource_Attributes to Inv: " + temp.resource_type.ToString() + " " + temp.resource_amount.ToString());
            }
            Debug.Log("Resource Inventory init successfully!");
        }
    }
    #endregion
    
    //will setting this as static cause issues??
    private static Resource_Attributes FindResourceAttribute(Resource_V2.ResourceType type)
    {
        Resource_Attributes temp = null;
        foreach (Resource_Attributes ra in resource_inventory)
        {
            if (ra.resource_type == type)
            {
                temp = ra;
                break;
            }
        }
        return temp;
    }

    public int GetResourceAmount(Resource_V2.ResourceType type)
    {
        Resource_Attributes temp = FindResourceAttribute(type);
        return temp.resource_amount;
    }
    //sets resource_amount of type argument to amount argument
    public void SetResourceAmount(Resource_V2.ResourceType type, int amount)
    {
        Resource_Attributes temp = FindResourceAttribute(type);
        temp.resource_amount = amount;
    }
    //will add argument amount to the resource_amount of type argument
    public void AppendResourceAmount(Resource_V2.ResourceType type, int amount)
    {
        Resource_Attributes temp = FindResourceAttribute(type);
        temp.resource_amount += amount;
    }
    public void PrintResourceInventory()
    {
        foreach(Resource_Attributes ra in resource_inventory)
        {
            //Debug.Log("rsc: " + ra.resource_type.ToString() + ": " + ra.resource_amount.ToString());
        }
    }

    //TODO: Towers cost a LIST of resources. This ain't gonna work. Refactor to accept param Resource_Attributes instead of this garbage!
    //Tries to take the amount of resources asked out of the inventory. Return bool indicates success/failure
    public static bool TryTakeResources(Resource_V2.ResourceType type, int amount)
    {
        Resource_Attributes temp = FindResourceAttribute(type);
        if(temp.resource_amount >= amount)
        {
            //no need to clamp
            temp.resource_amount -= amount;
            return true;
        }
        else return false;
    }
    //take 2
    public static bool TryTakeResources(Resource_Attributes attributes, int amount)
    {
        Resource_Attributes temp = FindResourceAttribute(attributes.resource_type);
        if (temp.resource_amount >= amount)
        {
            //no need to clamp
            temp.resource_amount -= amount;
            return true;
        }
        else return false;
    }
}
