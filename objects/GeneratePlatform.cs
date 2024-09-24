using Godot;
using System;

public partial class GeneratePlatform : Node
{

    public const int SEED = 10;
    Random rand = new Random(SEED);
    PackedScene S_PLAT_SC = GD.Load<PackedScene>("res://objects/starting_platform.tscn");
    PackedScene X2_SC = GD.Load<PackedScene>("res://objects/platform2x.tscn");
    PackedScene X3_SC = GD.Load<PackedScene>("res://objects/platform3x.tscn");
    PackedScene X4_SC = GD.Load<PackedScene>("res://objects/platform4x.tscn");
    PackedScene X5_SC = GD.Load<PackedScene>("res://objects/platform5x.tscn");
    Node2D _platforms;
    Timer _timer;
    public const int ST_PLATFORM_X = 0;
    public const int ST_PLATFORM_Y = 0;

    AnimatableBody2D s_plat;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        s_plat = S_PLAT_SC.Instantiate<AnimatableBody2D>();
        s_plat.Set("position",new Vector2(ST_PLATFORM_X,ST_PLATFORM_Y));
        AddChild(s_plat);
        _timer = new Timer();
        AddChild(_timer);
        _timer.Start(10);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        if (_timer.TimeLeft < 0.1)
        {
            GD.Print("Timer");
            if (s_plat != null)
            {
                s_plat.QueueFree();
            }
        }

	}
    public void SpawnPlatform()
    {

    }
}
