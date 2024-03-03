using Godot;
using System;

public partial class BasicMovement : CharacterBody2D
{
	public const float Speed = 300.0f;
	public const float JumpVelocity = -600.0f;
	public const float Gravity = 980.0f;
	public int jump_buffer_frames = 0;
	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y += Gravity * (float)delta;

		// Handle Jump.
		velocity = Jump(velocity);

		// Get the input direction and handle the movement/deceleration.
		// As good practice, you should replace UI actions with custom gameplay actions.
		Vector2 direction = Input.GetVector("player_move_left","player_move_right","ui_up","ui_down");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, (Speed/10));
		}

		Velocity = velocity;
		MoveAndSlide();
	}
	public Vector2 Jump(Vector2 velocity)
	{
		//Check if jump just been pressed.
		if (Input.IsActionJustPressed("player_jump"))
		{

			if(IsOnFloor())
			{
				//If on floor, jump by adding velocity.
				velocity.Y = JumpVelocity;
				jump_buffer_frames = 0;
			}
			else
			{
				//Not on the floor, start jump buffer timer.
				jump_buffer_frames = 10;
			}
		}
		else
		{
			//Jump automatically if on floor and jump buffer timer didn't run out.
			if(IsOnFloor() && jump_buffer_frames>0)	
			{
				velocity.Y = JumpVelocity;
				jump_buffer_frames = 0;
			}
			//Decrement jump_buffer_frames if timer didn't run out.
			else if(jump_buffer_frames>0) {
				jump_buffer_frames--;
			}
		}
		return velocity;
	}
}
