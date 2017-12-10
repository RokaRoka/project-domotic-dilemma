using System.Collections;
using System.Collections.Generic;
using System;
using System.Security;
using System.Text.RegularExpressions;
using UnityEngine;

public abstract class DialoguePiece
{
	public string type;
	public bool isDecision;
	public abstract string GetText();
	public abstract int GetDepth();
}

public class DialogueLine : DialoguePiece
{
	private string line;
	private int depth;
	private int moralityValue;
	
	public DialogueLine()
	{
		type = "Line";
		
		line = "";
		depth = -1;
		isDecision = false;
	}
	
	public DialogueLine(string _line, int _depth)
	{
		type = "Line";
		
		line = _line;
		depth = _depth;
		isDecision = false;
	}

	public DialogueLine(string _line, int _depth, int _moralityValue)
	{
		type = "Decision";
		
		line = _line;
		depth = _depth;
		moralityValue = _moralityValue;
		isDecision = true;
	}

	public override string GetText()
	{
		return line;
	}

	public override int GetDepth()
	{
		return depth;
	}
	
	public int GetDecisionValue()
	{
		return moralityValue;
	}
}

public class DialogueFunction : DialoguePiece
{
	private string tag;
	private int depth;

	private string functionName;
	private int value;
	
	public DialogueFunction(string _name, string _function, int _depth)
	{
		type = "Function";
		
		tag = _name;
		functionName = _function;
		depth = _depth;

		isDecision = false;
	}
	
	public DialogueFunction(string _name, string _function, int _depth, int _value)
	{
		type = "Function";
		
		tag = _name;
		functionName = _function;
		depth = _depth;
		value = _value;

		isDecision = false;
	}

	public override string GetText()
	{
		return tag;
	}
	
	public override int GetDepth()
	{
		return depth;
	}

	public void CallFunction()
	{
		GameObject target = GameObject.FindGameObjectWithTag(tag);
		if (value >= 0)
			target.SendMessage(functionName, value);
		else
			target.SendMessage(functionName);
	}
}

public class DecisionPoint
{
	private int decision1;
	private int decision2;
    private int depth;
	public bool isCompleted = false;
	public bool decisionFulfilled = false;
	
	public DecisionPoint(int _decision1, int decisionDepth)
	{
		decision1 = _decision1;
        depth = decisionDepth;
	}

	public void SetDecision2Index(int decisionIndex)
	{
		decision2 = decisionIndex;
		isCompleted = true;
	}

	public int GetDecision1Index()
	{
		return decision1;
	}

	public int GetDecision2Index()
	{
		return decision2;
	}

    public int GetDepth()
    {
        return depth;
    }

	public static int QueryIncompleteDecisions(DecisionPoint[] dpArray, int depth)
	{
		//needs some work, but super important to finish.
		//Find decisions that are incomplete and match the same depth
		for (int i = 0; dpArray.Length > i; i++)
		{
            if (dpArray[i] != null)
            {
                if (!dpArray[i].isCompleted && dpArray[i].depth == depth)
                {
                    return i;
                }
            }
		}
		return -1;
	}

	public static DecisionPoint QueryDecisions(DecisionPoint[] dpArray, int lineIndex)
	{
		for (int i = 0; i < dpArray.Length; i++)
		{
			if (dpArray[i].GetDecision1Index() == lineIndex || dpArray[i].GetDecision2Index() == lineIndex)
			{
				return dpArray[i];
			}
		}
		return null;
	}
}

public class DialogueChunk
{
	//Regex patterns
	private static string dialoguePattern = "\"(.+)\"";
	private static string dialoguePattern2 = "\n"+@"([A-Z][a-z]+):\s"+"\"(.+)\"";
																							 // EXAMPLES OF REGEX USUAGE \\
	private static string dialoguePattern3 =  @"([A-Z][a-z]+):\s"+"\"(.+)\"|"                //Aumry: "Blah blah."
	                                         +@"\t+(>+)\s"+"\"(.+)\""+@"\s\(\w+\s\+(\d+)\)|" //> "Blah blah?" (blah +1)
	                                         +@"\t+(>+)\s([A-Z][a-z]+):\s"+"\"(.+)\"|"       //> House: "Blah blah?"
                                             +@"\t+(>+)\s" + "\"(.+)\"|"                     //> "Blah blah..."
	                                         +@"(>*)\s*\(([\w\d]+)\,\s([\w\d]+)\)|"          //(Quinn, WalkForward)
	                                         +@"(>*)\s*\(([\w\d]+)\,\s([\w\d]+)\(\d+\)\)";   //(GameController, Death(4))
	
	private static string decisionForkPattern = @"\t+(>+)\s"+"\"(.+)\""+@"\s\((\w+)\s\+(\d+)\)";

	private static string forkDepthPattern = "(>)+";
	
	//finished regex's
	static Regex dialogueLinesRegex = new Regex(dialoguePattern3);
	static Regex decisionPointsRegex = new Regex(decisionForkPattern);
	
    public string dialogueName;
	private DialoguePiece[] lines;
	private DecisionPoint[] decisions;
	
	//The player's dialogue line history
	//private string[] lineHistory;
	
	public int lineAmount;
	public int decisionAmount;

    public bool isComplete = false;

	public DialogueChunk() {
		//default class constructor
	}
	
	public DialogueChunk(string allTextInFile) {
       	//First, match for all the lines in the text
        MatchCollection allLineMatches = dialogueLinesRegex.Matches(allTextInFile);
		lineAmount = allLineMatches.Count;
		decisionAmount = decisionPointsRegex.Matches(allTextInFile).Count/2;
		
		InitializeDialogueChunkArrays();
		
		int count = 0;
		int decisionCount = 0;
		int lastDecisionForkDepth = 0;
		int decisionForkDepth = 0;
		
		//for testing purposes
		
        foreach (Match m in allLineMatches)
        {
	        //test for additional '>'
	        if (m.Groups[3].Success)
	        {
		        Match _ = Regex.Match(m.Groups[3].Value, forkDepthPattern);
				int decCount = _.Groups[1].Captures.Count;
	        	decisionForkDepth = decCount;
	        }
	        else if (m.Groups[6].Success)
	        {
		        Match _ = Regex.Match(m.Groups[6].Value, forkDepthPattern);
		        int decCount = _.Groups[1].Captures.Count;
		        decisionForkDepth = decCount;
	        }
            else if (m.Groups[9].Success)
            {
                Match _ = Regex.Match(m.Groups[9].Value, forkDepthPattern);
                int decCount = _.Groups[1].Captures.Count;
                decisionForkDepth = decCount;
            }
	        else if (m.Groups[11].Success)
	        {
		        Match _ = Regex.Match(m.Groups[11].Value, forkDepthPattern);
		        int decCount = _.Groups[1].Captures.Count;
		        decisionForkDepth = decCount;
	        }
	        else if (m.Groups[14].Success)
	        {
		        Match _ = Regex.Match(m.Groups[14].Value, forkDepthPattern);
		        int decCount = _.Groups[1].Captures.Count;
		        decisionForkDepth = decCount;
	        }
            else
	        {
		        decisionForkDepth = 0;
	        }
	        
	        //do decision stuff here
	        if (decisionForkDepth > lastDecisionForkDepth)
	        {
                if (m.Groups[5].Success)
                {
                    decisions[decisionCount] = CreateNewDecision(count, decisionForkDepth);
                    Match _ = Regex.Match(m.Value, decisionForkPattern);
                    if (_.Groups[3].Value == "courage")
                    {
                        lines[count] = CreateNewDialogueLine(m.Groups[4].Value, decisionForkDepth, 
	                        int.Parse(m.Groups[5].Value));
                    } else if (_.Groups[3].Value == "fear")
                    {
	                    lines[count] = CreateNewDialogueLine(m.Groups[4].Value, decisionForkDepth,
		                    -1 * int.Parse(m.Groups[5].Value));
                    }
                    else
                    {
	                    lines[count] = CreateNewDialogueLine(m.Groups[4].Value, decisionForkDepth, 0);
                    }
                    count++;
                    decisionCount++;
                }
                else if (m.Groups[6].Success)
                {
                    lines[count] = CreateNewDialogueLine(m.Groups[7].Value, m.Groups[8].Value, decisionForkDepth);
                    count++;
                }
                else if (m.Groups[9].Success)
                {
                    lines[count] = CreateNewDialogueLine("Aumry", m.Groups[10].Value, decisionForkDepth);
                    count++;
                }
                else if (m.Groups[12].Success)
                {
	                lines[count] = CreateNewDialogueFunction(m.Groups[12].Value, m.Groups[13].Value, decisionForkDepth);
	                count++;
                }
                else if (m.Groups[15].Success)
                {
	                lines[count] = CreateNewDialogueFunction(m.Groups[15].Value, m.Groups[16].Value, decisionForkDepth, int.Parse(m.Groups[17].Value));
	                count++;
                }
	        }
	        else if (decisionForkDepth < lastDecisionForkDepth)
	        {
		        int dpIndex = DecisionPoint.QueryIncompleteDecisions(decisions, decisionForkDepth);
		        if (dpIndex >= 0)
		        {
					decisions[dpIndex].SetDecision2Index(count);
			        Match _ = Regex.Match(m.Value, decisionForkPattern);
			        if (_.Groups[3].Value == "courage")
			        {
				        lines[count] = CreateNewDialogueLine(m.Groups[4].Value, decisionForkDepth, int.Parse(m.Groups[5].Value));
			        }
			        else if (_.Groups[3].Value == "fear")
			        {
				        lines[count] = CreateNewDialogueLine(m.Groups[4].Value, decisionForkDepth, -1 * int.Parse(m.Groups[5].Value));
			        }
			        else
			        {
				        lines[count] = CreateNewDialogueLine(m.Groups[4].Value, decisionForkDepth, 0);
			        }
			        count++;
		        }
		        else
		        {
			        if (m.Groups[1].Success)
			        {
				        lines[count] = CreateNewDialogueLine(m.Groups[1].Value, m.Groups[2].Value, decisionForkDepth);
				        count++;
			        } else if (m.Groups[2].Success)
			        {
				        lines[count] = CreateNewDialogueLine(m.Groups[2].Value, decisionForkDepth);
				        count++;
			        }
			        else if (m.Groups[12].Success)
			        {
				        lines[count] = CreateNewDialogueFunction(m.Groups[12].Value, m.Groups[13].Value, decisionForkDepth);
				        count++;
			        }
			        else if (m.Groups[15].Success)
			        {
				        lines[count] = CreateNewDialogueFunction(m.Groups[15].Value, m.Groups[16].Value, decisionForkDepth, int.Parse(m.Groups[17].Value));
				        count++;
			        }
		        }
	
	        }
	        
	        //The normal linear dialogue happens here
	        else
	        {
		        //this is generally a special case, where there is no result of a decision
		        if (count - 1 > 0 && m.Groups[5].Success)
		        {
					if (lines[count - 1].type == "Decision")
					{
						int dpIndex = DecisionPoint.QueryIncompleteDecisions(decisions, decisionForkDepth);
						if (dpIndex >= 0)
						{
							decisions[dpIndex].SetDecision2Index(count);
							Match _ = Regex.Match(m.Value, decisionForkPattern);
							if (_.Groups[3].Value == "courage")
							{
								lines[count] = CreateNewDialogueLine(m.Groups[4].Value, decisionForkDepth, int.Parse(m.Groups[5].Value));
							}
							else if (_.Groups[3].Value == "fear")
							{
								lines[count] = CreateNewDialogueLine(m.Groups[4].Value, decisionForkDepth, -1 * int.Parse(m.Groups[5].Value));
							}
							else
							{
								lines[count] = CreateNewDialogueLine(m.Groups[4].Value, decisionForkDepth, 0);
							}
							count++;
						}
						else
						{
							//Throw error
							Debug.LogError("Error: Open decision not found from depth "+lastDecisionForkDepth+" to "+decisionForkDepth);
						}
					}
			        
		        }
		        //if not change in decision depth, record line
		        else if (m.Groups[1].Success)
		        {
			        lines[count] = CreateNewDialogueLine(m.Groups[1].Value, m.Groups[2].Value, decisionForkDepth);
			        count++;
		        } else if (m.Groups[2].Success)
		        {
			        lines[count] = CreateNewDialogueLine(m.Groups[2].Value, decisionForkDepth);
			        count++;
		        }
		        else if (m.Groups[12].Success)
		        {
			        lines[count] = CreateNewDialogueFunction(m.Groups[12].Value, m.Groups[13].Value, decisionForkDepth);
			        count++;
		        }
		        else if (m.Groups[15].Success)
		        {
			        lines[count] = CreateNewDialogueFunction(m.Groups[15].Value, m.Groups[16].Value, decisionForkDepth, int.Parse(m.Groups[17].Value));
			        count++;
		        }
	        }
	        
	        lastDecisionForkDepth = decisionForkDepth;
        }
	}

	//Creator functions
    private DialogueLine CreateNewDialogueLine(string theLine, int depth)
    {
        return new DialogueLine(theLine, depth);
    }
	
	private DialogueLine CreateNewDialogueLine(string name, string theLine, int depth)
	{
		return new DialogueLine(name+" : "+theLine, depth);
	}
	
	private DialogueLine CreateNewDialogueLine(string theLine, int depth, int moralityValue)
	{
		return new DialogueLine(theLine, depth, moralityValue);
	}

	private DialogueLine CreateNewDialogueLine(string name, string theLine, int depth, int moralityValue)
	{
		return new DialogueLine(name + " : " + theLine, depth, moralityValue);
	}

	private DialogueFunction CreateNewDialogueFunction(string name, string function, int depth)
	{
		return new DialogueFunction(name, function, depth);
	}
	
	private DialogueFunction CreateNewDialogueFunction(string name, string function, int depth, int value)
	{
		return new DialogueFunction(name, function, depth, value);
	}

	private DecisionPoint CreateNewDecision(int index, int depth)
	{
		return new DecisionPoint(index, depth);
	}

	//Functions for retrieving
	public bool CheckLineForDecision(int lineIndex)
	{
		return lines[lineIndex].isDecision;
	}

	public bool CheckLineForFunction(int lineIndex)
	{
		if (lines[lineIndex].type == "Function")
			return true;
		else
			return false;
	}

	public bool CheckIfDecisionMade(int lineIndex)
	{
		return DecisionPoint.QueryDecisions(decisions, lineIndex).decisionFulfilled;
	}

	public void CallLineFunction(int lineIndex)
	{
		if (lines[lineIndex].type == "Function")
			((DialogueFunction)lines[lineIndex]).CallFunction();
	}
	
	public string GetLineText(int lineIndex) {
		return lines[lineIndex].GetText();
	}

	public int GetLineDepth(int lineIndex)
	{
		return lines[lineIndex].GetDepth();
	}
	
	public DecisionPoint GetDecisionPoint(int lineIndex)
	{
		return DecisionPoint.QueryDecisions(decisions, lineIndex);
	}

    public string GetDecisionText(int lineIndex, int firstOrSecond)
    {
        //this is where we want to querry for the decision
        DecisionPoint current = DecisionPoint.QueryDecisions(decisions, lineIndex);
        if (firstOrSecond == 0)
        {
            return lines[current.GetDecision1Index()].GetText();
        }
        else
        {
            return lines[current.GetDecision2Index()].GetText();
        }
    }

	public int GetDecisionValue(int lineIndex)
	{
		if (lines[lineIndex].type == "Decision")
			return ((DialogueLine) lines[lineIndex]).GetDecisionValue();
		else
			return 0;
	}

	public int GetNextLineLowerThanDepth(int startingPointIndex, int targetDepth)
	{
		Debug.Log("Starting point is: "+startingPointIndex);
		for (int i = startingPointIndex; i < lines.Length; i++)
		{
			//Debug.Log("Looking at index "+i+" of depth "+lines[i].GetDepth());
			if (lines[i].GetDepth() <= targetDepth)
			{
				return i;
			}
		}
		return lines.Length + 1;
	}

	private void InitializeDialogueChunkArrays()
	{
		lines = new DialoguePiece[lineAmount];
		//lineHistory= new string[lineAmount];
		decisions = new DecisionPoint[decisionAmount];
	}

}