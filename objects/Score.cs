using Godot;
using System;

public partial class Score : Label
{
	private static int score;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		score = 0;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var score_label = GetNode<Label>(GetPath());
		score_label.Text = score.ToString();
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
	}
}
