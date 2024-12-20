using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

public class Player : MonoBehaviour
{

    private GameObject LeftHand;
    private GameObject RightHand;
    private Rigidbody rb;

    public void Add(GameObject obj, string hand)
    {
        if(hand == "left")
        {
            LeftHand = obj;
        }
        else if(hand == "right") 
        {
            RightHand = obj;
        }
    }

    public void Remove( string hand) 
    {
        print("enlever");
        if (hand == "left")
        {
            LeftHand = null;
        }
        else
        {
            RightHand = null;
        }
    }

    public bool Verrify(string hand)
    {
        if (hand == "left" && LeftHand != null)
        {
            return true;
        }
        else if (hand == "right" && RightHand != null)
        {
            return true;
        }
        return false;
    }

    public void AddAsChild(GameObject obj, Vector3 position)
    {

        obj.transform.SetParent(transform);

        obj.transform.localPosition = position;


        Debug.Log($"{obj.name} a été ajouté en tant qu'enfant du joueur.");
    }

    public void Switch(GameObject obj, GameObject handGO, Vector3 position, string hand)
    {
        GameObject tmp = handGO;
        Drop(tmp);
        Remove(hand);
        AddAsChild(obj, position);
        Add(obj, hand);
    }

    public void Drop(GameObject obj)
    {
        print("coucou");
        obj.transform.parent = null;
        obj.GetComponent<Collider>().enabled = true;
        rb = obj.GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }

    public GameObject ItemHand(string hand)
    {
        if(Verrify(hand))
        {
            if( hand == "left" )
            {
                return LeftHand;
            }
            else if( hand == "right" )
            {
                return RightHand;
            }
        }
        return null;
    }

    public void AddHand(GameObject obj, string hand, Vector3 position)
    {
        rb = obj.GetComponent<Rigidbody>();
        rb.isKinematic = true;
        if (Verrify(hand))
        {
            print("ok");
            Switch(obj, ItemHand(hand), position, hand);
        }
        else
        {
            Add(obj, hand);
            AddAsChild(obj, position);
            obj.GetComponent<Collider>().enabled = false;
        }
        
    }

    public void DropHande(GameObject obj, string hand, Vector3 position)
    {
        if (Verrify(hand))
        {
            GameObject tmp = ItemHand(hand);
            if (tmp != null)
            {
                Debug.Log($"Temp object to drop: {tmp.name}");

                Drop(tmp);
                Debug.Log($"Dropped object: {tmp.name}");

                tmp.transform.SetParent(obj.transform);
                Debug.Log($"Set parent to: {obj.name}");

                tmp.transform.localPosition = position;
                Debug.Log($"Local position set to: {position}");

                Remove(hand);
            }
            else
            {
                Debug.LogError("Le jeu d'objets temporaire est nul.");
            }
        }
        else
        {
            Debug.LogWarning($"You have nothing in your {hand} hand.");
        }
    }



}
