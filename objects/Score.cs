using Godot;
using System;

public partial class Score : Label
{
	private static double score;
    private static double score2;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
		score = 0;
		score2 = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var score_label = GetNode<Label>(GetPath());
		score_label.Text = score2.ToString();
	}
	public void set_score(double sc)
	{
		score = sc;
	}
	public double get_score()
	{
		return score;
	}
    public double get_score2()
    {
        return score2;
    }
    public void append_score() { 
		score = score+5;
		score2++;
	}
	public void lose_score() {
		score = score-20;

    }
	public void reset_score() { 
		score = 0;
		score2 = 0;
	}
	public void touched_celling() { 
		score = score-1;
	}
	public void append_score_by(double sc)
	{
		score += sc;
	}
  
}
