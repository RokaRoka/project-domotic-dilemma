using System.Collections;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;

public class DialogueLine
{
	private string line;
	private string alternate;

	public bool isDecision;
	private int[] decisionValues = new int[2];

	public DialogueLine(string _line)
	{
		line = _line;
		isDecision = false;
	}

	public DialogueLine(string _line, string _alternate, int decision1Value, int decision2Value)
	{
		line = _line;
		alternate = _alternate;
		isDecision = true;
		decisionValues[0] = decision1Value;
		decisionValues[1] = decision2Value;
	}

	public string GetText() {
		return line;
	}

	public string GetAlternateText() {
		return alternate;
	}

	public int GetDecisionValue(int decisionIndex) {
		return decisionValues[decisionIndex];
	}
}

public class DialogueChunk
{
	//finish regex
	static Regex dialogueLinesRegex = new Regex("\n(\".+\")");
    static Regex dialogueLinesRegex2 = new Regex("\n"+@"([A-Z][a-z]+):\s"+"\"(.+)\"");

    static Regex decisionLinesRegex = new Regex(">"+@"\s"+ "\"(.+)\""+@"\((\d)\)");
    // Use this Regex -> \((fear)\s\+(\d+)\)|\((courage)\s\+(\d+)\)

    public string dialogueName;
	private DialogueLine[] lines;
	public int lineAmount;
	
	public DialogueChunk(string allTextInFile) {
        //load file stuff here
        //regex for dialogue lines, decisions, and decision values

        MatchCollection allMatches = dialogueLinesRegex2.Matches(allTextInFile);

        foreach (Match m in allMatches)
        {
            lineAmount++;
            lines[lineAmount] = LoadNewLine(m.Groups[0].Value + m.Groups[1].Value);
        }
	}

    private DialogueLine LoadNewLine(string theLine)
    {
        return new DialogueLine(theLine);
    }

	public bool CheckLineForDecision(int lineIndex) {
		if (lines[lineIndex].isDecision)
			return true;
		else
			return false;
	}

	public string GetLineText(int lineIndex) {
		return lines[lineIndex].GetText();
	}

	public string GetDecisionText(int lineIndex, int decisionIndex) {
		if (decisionIndex == 0)
			return lines[lineIndex].GetText();
		else
			return lines[lineIndex].GetAlternateText();
	}

	public int GetDecisionValue(int lineIndex, int decisionIndex) {
		return lines[lineIndex].GetDecisionValue(decisionIndex);
	}

}