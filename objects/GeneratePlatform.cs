using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public partial class GeneratePlatform : Node
{

    public const int SEED = 10;
    public const int SEED2 = 20;
    public static readonly int[] SPAWN_SPEED = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
    Random rand = new Random(SEED);
    Random rand2 = new Random(SEED2);
    static PackedScene S_PLAT_SC = GD.Load<PackedScene>("res://objects/starting_platform.tscn");
    static PackedScene X2_SC = GD.Load<PackedScene>("res://objects/platform2x.tscn");
    static PackedScene X3_SC = GD.Load<PackedScene>("res://objects/platform3x.tscn");
    static PackedScene X4_SC = GD.Load<PackedScene>("res://objects/platform4x.tscn");
    static PackedScene X5_SC = GD.Load<PackedScene>("res://objects/platform5x.tscn");
    static PackedScene WALL = GD.Load<PackedScene>("res://objects/wall.tscn");
    PackedScene[] scene_list = {X2_SC, X3_SC, X4_SC, X5_SC};
    Node2D _platforms;
    Timer _timer;
    public const int ST_PLATFORM_X = 0;
    public const int ST_PLATFORM_Y = 0;
    public const int OFFSET_Y = 150;
    int current_y = ST_PLATFORM_Y;
    int current_x = ST_PLATFORM_X;
    int last_x = 0;
    public Vector2 STARTING_VECTOR = new Vector2(ST_PLATFORM_X, ST_PLATFORM_Y);
    public AnimatableBody2D c_plat;
    public Area2D wall;
    public AnimatableBody2D n_plat;
    public AnimatableBody2D a_plat;
    public AnimatableBody2D b_plat;
    public AnimatableBody2D d_plat;
    public AnimatableBody2D e_plat;
    public AnimatableBody2D l_plat;
    static public AnimatableBody2D current_platform;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
       
        c_plat = S_PLAT_SC.Instantiate<AnimatableBody2D>();
        c_plat.Set("position",STARTING_VECTOR);
        wall = WALL.Instantiate<Area2D>();
        wall.Set("position", new Vector2(ST_PLATFORM_X, ST_PLATFORM_Y + 40));
        n_plat = SpawnPlatform(scene_list[rand2.Next(0,3)]);
        a_plat = SpawnPlatform(scene_list[rand2.Next(0, 3)]);
        b_plat = SpawnPlatform(scene_list[rand2.Next(0, 3)]);
        d_plat = SpawnPlatform(scene_list[rand2.Next(0, 3)]);
        e_plat = SpawnPlatform(scene_list[rand2.Next(0, 3)]);
        l_plat = SpawnPlatform(scene_list[rand2.Next(0, 3)]);
        AddChild(c_plat);
        AddChild(n_plat);
        AddChild(a_plat);
        AddChild(b_plat);
        AddChild(d_plat);
        AddChild(e_plat);
        AddChild(l_plat);
        AddChild(wall);
        _timer = new Timer();
        AddChild(_timer);
        _timer.Start(5);

	}

    //Current->Next->Last, CurrnetFree, Current = Next, Next = Last, Last = spawn new.
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        //GD.Print(_timer.TimeLeft);
        if (_timer.TimeLeft < 0.01)
        {
            //GD.Print("Timer:"+_timer.TimeLeft);
            MovePlatforms();
            _timer.Start(1);
        }
        if (current_platform == a_plat)
        {
            MovePlatforms();
            _timer.Start(1);
        }
        if (wall.HasOverlappingBodies())
        {
            GetTree().ReloadCurrentScene();
        }
        

	}
    public AnimatableBody2D SpawnPlatform(PackedScene packedScene)
    {
        AnimatableBody2D plat;
        current_y = current_y - OFFSET_Y;
        plat = packedScene.Instantiate<AnimatableBody2D>(); 
        current_x = rand.Next(-300,300);
        CheckPlatform();
        plat.Set("position",new Vector2(current_x,current_y));
        last_x = current_x;
        return plat;
    }
    public void DeletePlatform(AnimatableBody2D plat)
    {
        plat.QueueFree();
    }
    public void MovePlatforms()
    {
        //4-1-2
        
        DeletePlatform(c_plat);
        wall.QueueFree();
        c_plat = n_plat;
        n_plat = a_plat;
        a_plat = b_plat;
        b_plat = d_plat;
        d_plat = e_plat;
        e_plat = l_plat;
        l_plat = SpawnPlatform(scene_list[rand2.Next(0, 3)]);
        AddChild(l_plat);
        wall = WALL.Instantiate<Area2D>();
        wall.Set("position", new Vector2(ST_PLATFORM_X, current_y + 7 * OFFSET_Y));
        AddChild(wall);

    }
    public void CheckPlatform()
    {
        if (Math.Abs(current_x-last_x)>400 || Math.Abs(current_x - last_x) < 100){
            current_x = rand.Next(-300, 300);
            CheckPlatform();
        }
    }
    public void set_current_platform(AnimatableBody2D platform)
    {
        current_platform = platform;
    }
}
