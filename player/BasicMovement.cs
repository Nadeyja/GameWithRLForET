using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

//Class for basic movement of the player. Include: right-left movement, jump, gravity, jump buffer and coyote time.

public partial class BasicMovement : CharacterBody2D
{
	[Export]
	public bool AIJump = false;
	[Export]
	public Vector2 AIXMovement { get; set; }
	[Export]
	public Node2D AIController2D;
	//Speed of left right movement.
	public const float SPEED = 550.0f;
	//Velocity of jump.
	public const float JUMPVELOCITY = -600.0f;
	//Velocity of gravity.
	public const float GRAVITYVALUE = 980.0f;
	//Frames for jump buffer.
	public const int JUMPBUFFERFRAMES = 10;
	//Frames for coyote time.
	public const int COYOTETIMEFRAMES = 5;
	//Time for jump buffer.
	public int actual_jump_buffer_frames = 0;
	//Timer for coyote time.
	public int actual_coyote_time_frames = 0;
	KinematicCollision2D last_collision;
	private int first_collision = 0;
	List<KinematicCollision2D> collision_list = new List<KinematicCollision2D>();
	public double increment_reward = 0;
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		velocity = Gravity(velocity,delta);

		// Handle jump.
		velocity = JumpMovement(velocity);

		//Handle left right movement.
		velocity = Movement(velocity);

		Velocity = velocity;
		
		MoveAndSlide();
		Scoring();
		
	}
	public Vector2 Movement(Vector2 velocity)
	{
		Vector2 direction;
        //Take direction from input map.
        if ((string)AIController2D.Get("heuristic") == "human") 
		{ 
			direction = Input.GetVector("player_move_left", "player_move_right", "ui_up", "ui_down");
		}
		else
		{
			direction = AIXMovement;
		}
		if (direction != Vector2.Zero)
		{
			//If there is direction make velocity.
			velocity.X = direction.X * SPEED;
		}
		else
		{
			//If any direction button isn't pressed then decelerate.
			velocity.X = Mathf.MoveToward(velocity.X, 0, SPEED/10);
		}
		return velocity;
	}
	public Vector2 Gravity(Vector2 velocity, double delta)
	{
		//Move with gravity velocity if not on the floor.
        if (!IsOnFloor())
            velocity.Y += GRAVITYVALUE * (float)delta;

        return velocity;
	}
	public Vector2 JumpMovement(Vector2 velocity)
	{
		//On floor, start coyote time timer.
		if (IsOnFloor())
		{
			actual_coyote_time_frames = COYOTETIMEFRAMES;
		}
        //Check if jump just been pressed.
        if ((string)AIController2D.Get("heuristic") == "human") { 
			if (Input.IsActionJustPressed("player_jump"))
			{

				if(IsOnFloor())
				{
					//If on floor, jump.
					velocity = Jump(velocity);
				}
				else if(actual_coyote_time_frames > 0)
				{
					//Not on the floor and still in coyote time frames, jump.
					velocity = Jump(velocity);
				}
				else
				{
					//Not on the floor, start jump buffer timer.
					actual_jump_buffer_frames = JUMPBUFFERFRAMES;
				}
			}
			else
			{
				//Jump automatically if on floor and jump buffer timer didn't run out.
				if(IsOnFloor() && actual_jump_buffer_frames>0)	
				{
					velocity = Jump(velocity);
				}
				//Decrement actual_jump_buffer_frames if timer didn't run out.
				else if(actual_jump_buffer_frames > 0) {
					actual_jump_buffer_frames--;
				}
				//Decrement actual_coyote_time_frames if timer didn't run out.
				else if (actual_coyote_time_frames > 0)
				{
					actual_coyote_time_frames--;
				}
			}
		}
		else
		{
            if (AIJump)
            {

                if (IsOnFloor())
                {
                    //If on floor, jump.
                    velocity = Jump(velocity);
                }
                else if (actual_coyote_time_frames > 0)
                {
                    //Not on the floor and still in coyote time frames, jump.
                    velocity = Jump(velocity);
                }
                else
                {
                    //Not on the floor, start jump buffer timer.
                    actual_jump_buffer_frames = JUMPBUFFERFRAMES;
                }
                
            }
            else
            {
                //Jump automatically if on floor and jump buffer timer didn't run out.
                if (IsOnFloor() && actual_jump_buffer_frames > 0)
                {
                    velocity = Jump(velocity);
                }
                //Decrement actual_jump_buffer_frames if timer didn't run out.
                else if (actual_jump_buffer_frames > 0)
                {
                    actual_jump_buffer_frames--;
                }
                //Decrement actual_coyote_time_frames if timer didn't run out.
                else if (actual_coyote_time_frames > 0)
                {
                    actual_coyote_time_frames--;
                }
            }
            


        }
		return velocity;
	}
	public Vector2 Jump(Vector2 velocity)
	{
        //Add jump velocity to Y-axis velocity and reset jump buffer and coyote time timers.
        velocity.Y = JUMPVELOCITY;
        actual_jump_buffer_frames = 0;
        actual_coyote_time_frames = 0;
        return velocity;
	}
	public void Scoring()
	{


        if (IsOnFloor()) {
			int collision_check = 0;
			if (first_collision == 0)
			{
				first_collision = 1;
				collision_list.Add(GetLastSlideCollision());
			}
			var collision = GetLastSlideCollision();
		
			for(int i = 0;  i < collision_list.Count; i++) 
			{
				if (collision != null) { 
					if (collision_list.ElementAt(i).GetColliderId() != collision.GetColliderId()) 
					{
						collision_check++;
					}
				}
			}
			if (collision_check == collision_list.Count) 
			{
				GeneratePlatform generatePlatform = new GeneratePlatform();
				generatePlatform.SetCurrentPlatform((AnimatableBody2D)collision.GetCollider());
				Score sc = new Score();
				sc.append_score();
				double reward = (double)AIController2D.Get("reward");
				AIController2D.Set("reward", reward+0.2+increment_reward);
				increment_reward = increment_reward + 0.002;
				collision_list.Add(collision);
				//GD.Print("After collision: "+ sc.get_score());
                
            }

		}
		/*else
		{
			if(IsOnCeiling()) { 
				Score sc = new Score();
				sc.touched_celling();
                double reward = (double)AIController2D.Get("reward");
                AIController2D.Set("reward", reward-0.002);
                GD.Print("After collision: " + sc.get_score());

            }
            if (IsOnWall())
            {
                Score sc = new Score();
				sc.append_score_by(-0.02);
                double reward = (double)AIController2D.Get("reward");
                AIController2D.Set("reward", reward-0.001);
                GD.Print("After collision: " + sc.get_score());

            }
        }*/

    }
	public void ResetPlayerPosition()
	{
		Score sc2 = new Score();
		double score2 = sc2.get_score2();
		if (score2 > 50) {
			AIController2D.Set("is_success", true);
		}
		else
		{
			AIController2D.Set("is_success", false);
		}		
		increment_reward = 0;
		Score sc = new Score();
		sc.lose_score();
        double reward = (double)AIController2D.Get("reward");
        AIController2D.Set("reward", reward-1);
		AIController2D.Set("done", true);
		sc.reset_score(); 
		Velocity = Vector2.Zero;	
		first_collision = 0;
		collision_list.Clear();
		this.Set("position", new Vector2(0,-50));
	

	}
	
}
