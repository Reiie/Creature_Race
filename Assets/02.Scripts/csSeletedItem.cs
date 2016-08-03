using UnityEngine;
using System.Collections;

public class csSeletedItem : MonoBehaviour {


    public UILabel itemAmount;

    public UILabel description;

    public UILabel itemName;

    public UISprite sprite;

    public UISprite rankSprite;

    public UIAtlas defaultAtlas;


    public string defaultSpriteName;

    // Use this for initialization
    void Start () {
    
    }
	
	// Update is called once per frame
	void Update () {
	
	}


    public void ChangeSprite(string name,string text, Color color,string Itemname)
    {
        if (name.Length < 1)
        {
            sprite.spriteName = "";
            description.text = "";
            itemName.text = "";
        }
        else
        {
            string removeName = name.Remove(name.Length - 1, 1);
            string insertName = removeName.Insert(removeName.Length, "1");

            sprite.spriteName = insertName;
            itemName.text = Itemname;
            description.text = text;
        }

        rankSprite.color = color;
    }
}
