using Godot;
using System;

public partial class Score : Label
{
	private static int score;
	private static int score2;
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
		score_label.Text = score.ToString()+'|'+score2.ToString();
	}
	public void set_score(int sc)
	{
		score = sc;
	}
	public int get_score()
	{
		return score;
	}
	public void append_score() { 
		score++;
		score2++;
	}
	public void lose_score() {
		score = score-1;

    }
	public void reset_score()
	{
		score2 = 0;
	}
}
