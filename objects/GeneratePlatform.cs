using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

public partial class GeneratePlatform : Node
{
    public Node2D AIController2D;
    public Node2D ParrentNode;
    public CharacterBody2D Player;
    public Timer _timer;
    static public Area2D wall;
    Score sc2 = new Score();

    //Randomizing
    public const int SEED = 10;
    public const int SEED2 = 20;
    
    BasicMovement BasicMovement = new BasicMovement();

    Random rand = new Random();
    Random rand2 = new Random();
    //PackedScenes
    static PackedScene S_PLAT_SC = GD.Load<PackedScene>("res://objects/starting_platform.tscn");
    static PackedScene X2_SC = GD.Load<PackedScene>("res://objects/platform2x.tscn");
    static PackedScene X3_SC = GD.Load<PackedScene>("res://objects/platform3x.tscn");
    static PackedScene X4_SC = GD.Load<PackedScene>("res://objects/platform4x.tscn");
    static PackedScene X5_SC = GD.Load<PackedScene>("res://objects/platform5x.tscn");
    PackedScene[] scene_list = {X2_SC, X3_SC, X4_SC, X5_SC};
    
    Node2D _platforms;
    
    

    //public static readonly int[] SPAWN_SPEED = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
    public const int ST_PLATFORM_X = 0;
    public const int ST_PLATFORM_Y = 0;
    public const int OFFSET_Y = 150;
    
    int current_y = ST_PLATFORM_Y;
    int current_x = ST_PLATFORM_X;
    int last_x = 0;
    int removed;
    public Vector2 STARTING_VECTOR = new Vector2(ST_PLATFORM_X, ST_PLATFORM_Y);
    
    public AnimatableBody2D first_platform;
    public AnimatableBody2D second_platform;
    public AnimatableBody2D third_platform;
    public AnimatableBody2D fourth_platform;
    public AnimatableBody2D fifth_platform;
    public AnimatableBody2D sixth_platform;
    public AnimatableBody2D last_platform;
    static public AnimatableBody2D current_platform;
    static public AnimatableBody2D next_platform;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
	{
        removed = 0;
        AIController2D = GetNode<Node2D>("/root/MainNode/Player/AIController2D");
        ParrentNode = GetNode<Node2D>("..");
        Player = GetNode<CharacterBody2D>("/root/MainNode/Player");
        _timer = GetNode<Timer>("/root/MainNode/Timer");
        wall = GetNode<Area2D>("/root/MainNode/Wall");
        
       

        first_platform = S_PLAT_SC.Instantiate<AnimatableBody2D>();
        first_platform.Set("position",STARTING_VECTOR);
        wall.Set("position", new Vector2(ST_PLATFORM_X, ST_PLATFORM_Y + 40));
        second_platform = SpawnPlatform(scene_list[rand2.Next(0, 4)]);
        third_platform = SpawnPlatform(scene_list[rand2.Next(0, 4)]);
        fourth_platform = SpawnPlatform(scene_list[rand2.Next(0, 4)]);
        fifth_platform = SpawnPlatform(scene_list[rand2.Next(0, 4)]);
        sixth_platform = SpawnPlatform(scene_list[rand2.Next(0, 4)]);
        last_platform = SpawnPlatform(scene_list[rand2.Next(0, 4)]);
        current_platform = first_platform;
        next_platform = second_platform;

        AddChild(first_platform);
        AddChild(second_platform);
        AddChild(third_platform);
        AddChild(fourth_platform);
        AddChild(fifth_platform);
        AddChild(sixth_platform);
        AddChild(last_platform);

        //GD.Print("dead1");

        _timer.Start(1.5);
       

	}

    //First free, First = Second,..., Sixth = Last, Last = spawn new.
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta){        
        SetNextPlatform();
        if (_timer.TimeLeft < 0.1){
            MovePlatforms();
            _timer.Start(1.5);
        }
        if (current_platform == third_platform){
            MovePlatforms();
            _timer.Start(1.5);
        }      
        if (wall.HasOverlappingBodies()){   
            wall.Set("position", new Vector2(ST_PLATFORM_X, ST_PLATFORM_Y + 40));
            Player.Call("ResetPlayerPosition");
            Player.Call("RewardAfterDeath");
            ParrentNode.Call("reset_platforms");
            AIController2D.Call("reset");          
        }
        double score2 = sc2.get_score2();
        if (score2 >= 200){
            wall.Set("position", new Vector2(ST_PLATFORM_X, ST_PLATFORM_Y + 40));
            Player.Call("ResetPlayerPosition");
            Player.Call("RewardAfterWin");
            ParrentNode.Call("reset_platforms");
            AIController2D.Call("reset");
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
        if (current_platform == first_platform) {
            current_platform = second_platform;           
        }
        DeletePlatform(first_platform);
        first_platform = second_platform;
        second_platform = third_platform;
        third_platform = fourth_platform;
        fourth_platform = fifth_platform;
        fifth_platform = sixth_platform;
        sixth_platform = last_platform;
        last_platform = SpawnPlatform(scene_list[rand2.Next(0, 4)]);
        AddChild(last_platform);
        wall.Set("position", new Vector2(ST_PLATFORM_X, current_y + 7 * OFFSET_Y));


    }
    public void CheckPlatform()
    {
        if (Math.Abs(current_x-last_x)>400 || Math.Abs(current_x - last_x) < 200){
            current_x = rand.Next(-300, 300);
            CheckPlatform();
        }
    }
    public void SetCurrentPlatform(AnimatableBody2D platform)
    {
        current_platform = platform;
    }
    public void SetNextPlatform()
    {
        if (current_platform == first_platform)
        {
            next_platform = second_platform;
        }
        else if (current_platform == second_platform)
        {
            next_platform = third_platform;
        }
        else if (current_platform == third_platform)
        {
            next_platform = fourth_platform;
        }
        else if (current_platform == fourth_platform)
        {
            next_platform = fifth_platform;
        }
        else if (current_platform == fifth_platform)
        {
            next_platform = sixth_platform;
        }
        else if (current_platform == sixth_platform)
        {
            next_platform = last_platform;
        }
    }
    public AnimatableBody2D GetCurrentPlatform()
    {
        return current_platform;
    }
    public AnimatableBody2D GetNextPlatform()
    {
        return next_platform;
    }
}
