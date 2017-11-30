using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class DialogueLine
{
	private string line;
	private int otherDecision;
	private int decisionValue;
	public bool isDecision;
	
	public DialogueLine(string _line)
	{
		line = _line;
		isDecision = false;
	}

	public DialogueLine(string _line, int _decisionValue)
	{
		line = _line;
		decisionValue = _decisionValue;
		isDecision = true;
	}

	public string GetText()
	{
		return line;
	}

	public void SetDecisionPoint()
	{
		
	}
	
	public int GetDecisionValue()
	{
		return decisionValue;
	}
}

public class DecisionPoint
{
	private int decision1;
	private int decision2;
	public bool isCompleted = false;
	
	public DecisionPoint(int _decision1)
	{
		decision1 = _decision1;
	}

	public void SetDecision2Index(int decisionIndex)
	{
		decision2 = decisionIndex;
		isCompleted = true;
	}

}

public class DialogueChunk
{
	//Regex patterns
	//private static string dialoguePattern = "\"(.+)\"";
	private static string dialoguePattern = "\"(.+)\"";
	private static string dialoguePattern2 = "\n"+@"([A-Z][a-z]+):\s"+"\"(.+)\"";
	private static string dialoguePattern3 = "\n"+@"([A-Z][a-z]+):\s"+"\"(.+)\"|"
	                                         +@"(>)\s"+"\".+\""+@"\s\(\w+\s\+\d\)";
	
	private static string decisionForkPattern = @"\t+(>+)\s"+"\"(.+)\""+@"\s\((\w+)\s\+(\d+)\)|"
	                                         +@"\t+(>{2,})\s"+@"([A-Z][a-z]+):\s"+"\"(.+)\"";

	private static string forkDepthPattern = "\t(>)|>(>)";
	
	//finish regex
	static Regex dialogueLinesRegex = new Regex(dialoguePattern3);
	
    public string dialogueName;
	private DialogueLine[,] lines;
	private string[] linesStrings;
	public int lineAmount;
	
	public DialogueChunk() {
		//default class constructor
	}
	
	public DialogueChunk(string allTextInFile) {
       	//Match aMatch = dialogueLinesRegex.Match(allTextInFile);
        MatchCollection allMatches = dialogueLinesRegex.Matches(allTextInFile);
		lineAmount = allMatches.Count;
		InitializeDialogueChunkArrays();
		
		int count = 0;
		int lastDecisionForkDepth = 0;
		int decisionForkDepth = 0;
		
		//for testing purposes
		
        foreach (Match m in allMatches)
        {
	        //test for additional '>'
	        if (m.Groups[3].Success)
	        {
		        //This is still logically broken. Spend at least an hour coming up with a logical solution
		        //lastDecisionForkDepth = decisionForkDepth;
		        //decisionForkDepth = Regex.Match(m.Groups[3].Value, forkDepthPattern).Captures.Count;
		        //if (decisionForkDepth < lastDecisionForkDepth)
		        //lines[count+decisionForkDepth, decisionForkDepth] = LoadNewLine(m.Groups[1].Value+m.Groups[2].Value);
	        }
	        else if (m.Groups[1].Success)
	        {
		        decisionForkDepth = 0;
		        lines[count, decisionForkDepth] = LoadNewLine(m.Groups[1].Value+m.Groups[2].Value);
		        count++;
	        }
	        //add to decision depth
	        //or add to lines at 0 depth
            
        }
	}

    private DialogueLine LoadNewLine(string theLine)
    {
        return new DialogueLine(theLine);
    }

	public bool CheckLineForDecision(int lineIndex) {
		if (lines[lineIndex, 0].isDecision)
			return true;
		else
			return false;
	}

	public string GetLineText(int lineIndex) {
		return lines[lineIndex, 0].GetText();
	}

	public string GetDecisionText(int lineIndex, int decisionIndex) {
		if (decisionIndex == 0)
			return lines[lineIndex, 0].GetText();
		else
			return lines[lineIndex, 1].GetText();
	}

	public int GetDecisionValue(int lineIndex, int decisionIndex) {
		return lines[lineIndex, 0].GetDecisionValue();
	}

	private void InitializeDialogueChunkArrays()
	{
		lines = new DialogueLine[lineAmount, 8];
		linesStrings = new string[lineAmount];
	}

}