using Godot;
using System;


public partial class BasicMovement : CharacterBody2D
{
	//Speed of left right movement.
	public const float Speed = 300.0f;
	//Velocity of jump.
	public const float JumpVelocity = -600.0f;
	public const float GravityValue = 980.0f;
	public const int JumpBufferFrames = 10;
	public const int CoyoteTimeFrames = 5;
	public int actual_coyote_time_frames = 0;
	public int actual_jump_buffer_frames = 0;
	public bool can_double_jump = false;
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		velocity = Gravity(velocity,delta);

		// Handle jump.
		velocity = JumpMovement(velocity);

		//Handle double jump.
        velocity = DoubleJump(velocity);

        //Handle left right movement.
        velocity = Movement(velocity);

		Velocity = velocity;
		MoveAndSlide();
	}
	public Vector2 Movement(Vector2 velocity)
	{
		//Take direction from input map.
		Vector2 direction = Input.GetVector("player_move_left", "player_move_right", "ui_up", "ui_down");
		if (direction != Vector2.Zero)
		{
			//If there is direction make velocity.
			velocity.X = direction.X * Speed;
		}
		else
		{
			//If any direction button isn't pressed then decelerate.
			velocity.X = Mathf.MoveToward(velocity.X, 0, Speed/10);
		}
		return velocity;
	}
	public Vector2 Gravity(Vector2 velocity, double delta)
	{
        if (!IsOnFloor())
            velocity.Y += GravityValue * (float)delta;

        return velocity;
	}
	public Vector2 JumpMovement(Vector2 velocity)
	{
		//On floor, start coyote time timer.
		if (IsOnFloor())
		{
			actual_coyote_time_frames = CoyoteTimeFrames;
		}
		//Check if jump just been pressed.
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
				actual_jump_buffer_frames = JumpBufferFrames;
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
		return velocity;
	}
	public Vector2 DoubleJump(Vector2 velocity)
	{
		if(IsOnFloor() && can_double_jump==false)
		{
			can_double_jump = true;
		}
		if (Input.IsActionJustPressed("player_jump") && can_double_jump==true && !IsOnFloor())
		{
			velocity = Jump(velocity);
			can_double_jump = false;
		}
		return velocity;
	}
	public Vector2 Jump(Vector2 velocity)
	{
        velocity.Y = JumpVelocity;
        actual_jump_buffer_frames = 0;
        actual_coyote_time_frames = 0;
        return velocity;
	}
	
}
