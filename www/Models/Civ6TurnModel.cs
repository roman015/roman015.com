using System;

[Serializable]
public class Civ6TurnModel
{
    public string value1 {get;set;} // Game Name
    public string value2 {get;set;} // Player Name
    public string value3 {get;set;} // Turn #

    public override string ToString()
    {
        return "It's " + value2 + "'s turn in " + value1 + " (Turn " + value3 + ")";
    }

}