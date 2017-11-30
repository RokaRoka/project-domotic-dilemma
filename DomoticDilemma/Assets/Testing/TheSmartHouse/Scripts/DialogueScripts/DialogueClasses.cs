using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class DialogueLine
{
	private string line;
	private int moralityValue;
	public bool isDecision;
	
	public DialogueLine(string _line)
	{
		line = _line;
		isDecision = false;
	}

	public DialogueLine(string _line, int _moralityValue)
	{
		line = _line;
		moralityValue = _moralityValue;
		isDecision = true;
	}

	public string GetText()
	{
		return line;
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
	
}

public class DialogueChunk
{
	//Regex patterns
	private static string dialoguePattern = "\"(.+)\"";
	private static string dialoguePattern2 = "\n"+@"([A-Z][a-z]+):\s"+"\"(.+)\"";
	
	private static string dialoguePattern3 = "\n"+@"([A-Z][a-z]+):\s"+"\"(.+)\"|"
	                                         +@"\t+(>+)\s"+"\"(.+)\""+@"\s\(\w+\s\+\d\)|"
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
		decisionAmount = decisionPointsRegex.Matches(allTextInFile).Count;
		
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
		        decisionForkDepth = Regex.Matches(m.Groups[3].Value, forkDepthPattern).Count;
	        }
	        else if (m.Groups[6].Success)
	        {
		        decisionForkDepth = Regex.Matches(m.Groups[6].Value, forkDepthPattern).Count;
	        }
	        
	        if (decisionForkDepth > lastDecisionForkDepth)
	        {
		        //do decision stuff here
		        decisions[decisionCount] = CreateNewDecision(count, decisionForkDepth);
		        lines[count] = CreateNewDialogueLine(m.Groups[3].Value, m.Groups[4].Value, int.Parse(m.Groups[5].Value));
	        }
	        else if (decisionForkDepth < lastDecisionForkDepth)
	        {
		        //Here, we test to see if the next line is one to skip
		        //If there is a decision waiting to be complete on this depth, than we can skip until
		        //we get another decision depth shorter.
		        //It'll be a good idea to draw this out. 
		        
		        //The dialogue manager also still needs some work
	        }
	        else
	        {
		        //if not change in decision depth, record line
		        if (m.Groups[1].Success)
		        {
			        lines[count] = CreateNewDialogueLine(m.Groups[1].Value, m.Groups[2].Value);
			        count++;
		        } else if (m.Groups[2].Success)
		        {
			        lines[count] = CreateNewDialogueLine(m.Groups[2].Value);
			        count++;
		        }   
	        }
	        
	        lastDecisionForkDepth = decisionForkDepth;
        }
	}

	//Creator functions
    private DialogueLine CreateNewDialogueLine(string theLine)
    {
        return new DialogueLine(theLine);
    }
	
	private DialogueLine CreateNewDialogueLine(string name, string theLine)
	{
		return new DialogueLine(name+theLine);
	}
	
	private DialogueLine CreateNewDialogueLine(string name, string theLine, int moralityValue)
	{
		return new DialogueLine(name+theLine, moralityValue);
	}

	private DecisionPoint CreateNewDecision(int index, int depth)
	{
		return new DecisionPoint(index, depth);
	}

	//Functions for Dialogue manager
	public bool CheckLineForDecision(int lineIndex)
	{
		return lines[lineIndex].isDecision;
	}

	public string GetLineText(int lineIndex) {
		return lines[lineIndex].GetText();
	}

	public string GetDecisionText(int decisionIndex, int firstOrSecond) {
		//this is where we want to querry for the decision

		return "Temp";
	}

	public int GetDecisionValue(int lineIndex, int decisionIndex) {
		return lines[lineIndex].GetDecisionValue();
	}

	private void InitializeDialogueChunkArrays()
	{
		lines = new DialogueLine[lineAmount];
		lineHistory= new string[lineAmount];
		decisions = new DecisionPoint[decisionAmount];
	}

}