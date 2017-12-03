using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEngine;

public class DialogueLine
{
	private string line;
	private int depth;
	private int moralityValue;
	public bool isDecision;
	
	public DialogueLine(string _line, int _depth)
	{
		line = _line;
		depth = _depth;
		isDecision = false;
	}

	public DialogueLine(string _line, int _depth, int _moralityValue)
	{
		line = _line;
		depth = _depth;
		moralityValue = _moralityValue;
		isDecision = true;
	}

	public string GetText()
	{
		return line;
	}

	public int GetDepth()
	{
		return depth;
	}
	
	public int GetDecisionValue()
	{
		return moralityValue;
	}
}

public class DecisionPoint
{
	private int decision1;
	private int decision2;
	public bool isCompleted = false;
	public bool decisionFulfilled = false;
	
	public DecisionPoint(int _decision1, int decisionDepth)
	{
		decision1 = _decision1;
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

	public void DecisionMade()
	{
		decisionFulfilled = true;
	}

	public static int QueryIncompleteDecisions(DecisionPoint[] dpArray, int depth)
	{
		//needs some work, but super important to finish.
		//Find decisions that are incomplete and match the same depth
		for (int i = 0; dpArray.Length > i; i++)
		{
			if (!dpArray[i].isCompleted)
			{
				return i;
			}
		}
		return -1;
	}

	public static DecisionPoint QueryDecisions(DecisionPoint[] dpArray, int lineIndex)
	{
		for (int i = 0; dpArray.Length > i; i++)
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
	
	private static string dialoguePattern3 = "\n"+@"([A-Z][a-z]+):\s"+"\"(.+)\"|"
	                                         +@"\t+(>+)\s"+"\"(.+)\""+@"\s\(\w+\s\+(\d+)\)|"
	                                         +@"\t+(>+)\s([A-Z][a-z]+):\s"+"\"(.+)\"";
	
	private static string decisionForkPattern = @"\t+(>+)\s"+"\"(.+)\""+@"\s\((\w+)\s\+(\d+)\)";

	private static string forkDepthPattern = "\t(>)|>(>)";
	
	//finished regex's
	static Regex dialogueLinesRegex = new Regex(dialoguePattern3);
	static Regex decisionPointsRegex = new Regex(decisionForkPattern);
	
    public string dialogueName;
	private DialogueLine[] lines;
	private DecisionPoint[] decisions;
	
	//The player's dialogue line history
	private string[] lineHistory;
	
	public int lineAmount;
	public int decisionAmount;
	
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
				int decCount = _.Groups.Count;
	        	decisionForkDepth = decCount;
	        }
	        else if (m.Groups[6].Success)
	        {
		        decisionForkDepth = Regex.Matches(m.Groups[6].Value, forkDepthPattern).Count;
	        }
	        
	        //do decision stuff here
	        if (decisionForkDepth > lastDecisionForkDepth)
	        {
		        decisions[decisionCount] = CreateNewDecision(count, decisionForkDepth);
		        lines[count] = CreateNewDialogueLine(m.Groups[4].Value, decisionForkDepth,int.Parse(m.Groups[5].Value));
		        count++;
		        decisionCount++;
	        }
	        else if (decisionForkDepth < lastDecisionForkDepth)
	        {
		        int dpIndex = DecisionPoint.QueryIncompleteDecisions(decisions, decisionForkDepth);
		        if (dpIndex >= 0)
		        {
					decisions[dpIndex].SetDecision2Index(count);
			        lines[count] = CreateNewDialogueLine(m.Groups[4].Value, decisionForkDepth, int.Parse(m.Groups[5].Value));
			        count++;
		        }
		        else
		        {
			        //Throw error
			        Debug.LogError("Error: Incomplete decision not found from depth"+lastDecisionForkDepth+" to "+decisionForkDepth);
		        }
	
	        }
	        
	        //The normal linear dialogue happens here
	        else
	        {
		        //this is generally a special case, where there is no result of a decision
		        if (count - 1 > 0 && m.Groups[5].Success)
		        {
			        if (lines[count - 1].isDecision)
			        {
				        int dpIndex = DecisionPoint.QueryIncompleteDecisions(decisions, decisionForkDepth);
				        if (dpIndex >= 0)
				        {
					        decisions[dpIndex].SetDecision2Index(count);
					        lines[count] = CreateNewDialogueLine(m.Groups[4].Value, decisionForkDepth, int.Parse(m.Groups[5].Value));
					        count++;
				        }
				        else
				        {
					        //Throw error
					        Debug.LogError("Error: Incomplete decision not found from depth"+lastDecisionForkDepth+" to "+decisionForkDepth);
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

	private DecisionPoint CreateNewDecision(int index, int depth)
	{
		return new DecisionPoint(index, depth);
	}

	//Functions for retrieving
	public bool CheckLineForDecision(int lineIndex)
	{
		return lines[lineIndex].isDecision;
	}

	public string GetLineText(int lineIndex) {
		return lines[lineIndex].GetText();
	}

	public int GetLineDepth(int lineIndex)
	{
		return lines[lineIndex].GetDepth();
	}

	public bool CheckIfDecisionMade(int lineIndex)
	{
		return DecisionPoint.QueryDecisions(decisions, lineIndex).decisionFulfilled;
	}
	
	public DecisionPoint GetDecisionPoint(int lineIndex)
	{
		return DecisionPoint.QueryDecisions(decisions, lineIndex);
	}
	
	public string GetDecisionText(int lineIndex, int firstOrSecond) {
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

	public int GetDecisionValue(int lineIndex, int decisionIndex) {
		return lines[lineIndex].GetDecisionValue();
	}

	public int GetNextLineLowerThanDepth(int startingPointIndex, int targetDepth)
	{
		for (int i = 0; i < lines.Length - startingPointIndex; i++)
		{
			if (lines[i].GetDepth() <= targetDepth)
			{
				return i;
			}
		}
		return -1;
	}

	private void InitializeDialogueChunkArrays()
	{
		lines = new DialogueLine[lineAmount];
		lineHistory= new string[lineAmount];
		decisions = new DecisionPoint[decisionAmount];
	}

}