using UnityEngine;
using TMPro;

public class NumberPad : MonoBehaviour
{
    public TextMeshProUGUI displayText;
    public string correctCode = "0";

    private string input = null;

    public DoorOpenByE door; // كود الباب اللي سويتَه

    void Start()
    {
      //  door = GameObject.Find("GlowingChestDoor").GetComponent<DoorOpenByE>();
       // displayText.text = "";
    }

    public void AddNumber(string number)
    {
        if (input.Length >= 4) return;

        input += number;
        displayText.text = input;
    }

    public void Clear()
    {
        input = "";
        displayText.text = "";
    }

    public void Enter()
    {
       // door.PlayReverseAnimation();
        if (input == "123")
        {
           door.PlayReverseAnimation();
           // displayText.text = "UNLOCKED";
        }
        else
        {
          //  displayText.text = "ERROR";
            input = "";
        }
    }
    public void one()

    {
        //print(input);
        input += "1";
        print(input);
    }
     public void two()

    {
        //print(input);
        input += "2";
        print(input);
    }
     public void three()

    {
        //print(input);
        input += "3";
        print(input);
    }
     public void four()

    {
        //print(input);
        input += "4";
        print(input);
    }
     public void five()

    {
        //print(input);
        input += "5";
        print(input);
    }
     public void six()

    {
        //print(input);
        input += "6";
        print(input);
    }
     public void seven()

    {
       // print(input);
        input += "7";
        print(input);
    }
     public void eight()

    {
        //print(input);
        input += "8";
        print(input);
    }
     public void nine()

    {
        //print(input);
        input += "9";
        print(input);
    }
     public void zero()

    {
        input += "0";
        print(input);
    }
}
