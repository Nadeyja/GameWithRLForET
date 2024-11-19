extends AIController2D

@export var ball: Node2D

@onready var player: CharacterBody2D = $".."
#@onready var grid_wall: GridSensor2D = $GridSensor2D
#@onready var grid_wall2: GridSensor2D = $GridSensor2D2
@export var wall: Area2D
#@onready var Score = load("res://objects/Score.cs")
#@onready var score = Score.new()
@onready var GeneratePlatform = load("res://objects/GeneratePlatform.cs")
@onready var platforms = GeneratePlatform.new()
var is_success := false
var best_distance := 0.0
@onready var raycast: RaycastSensor2D = $"RaycastSensor2D"
# Called when the node enters the scene tree for the first time.
func get_obs() -> Dictionary:
	var obs := []
	var velocity = player.get_real_velocity().limit_length(600.0)/600.0
	
	obs.append(player.is_on_floor())
	
	obs.append(velocity.x)
	obs.append(velocity.y)
	
	
	#var current = platforms.GetCurrentPlatform()
	#if(current!=null):
		#var current_platform_position_player: Vector2 = player.to_local(current.global_position)
	#	obs.append(current.position.x)
	#	obs.append(current.position.y-35)
		#if(player.is_on_floor()):
			#print("c, x: ",current_platform_position_player.x)
			#print("c, y: ",current_platform_position_player.y-46)

	#else:
	#	obs.append(0)
	#	obs.append(0)
		
	
	var next = platforms.GetNextPlatform()	
	if(next!=null):
		var next_platform_position_player: Vector2 = player.to_local(next.global_position)
		next_platform_position_player.y = next_platform_position_player.y-46
		next_platform_position_player = next_platform_position_player.limit_length(300.0)/300.0
		obs.append(next_platform_position_player.x)
		obs.append(next_platform_position_player.y)
		
		#print("x: ",next_platform_position_player.x)
		#print("y: ",next_platform_position_player.y)
		
		
		

	else:
		obs.append(0)
		obs.append(0)
		
	var wall_position_player: Vector2 = player.to_local(wall.global_position).limit_length(600.0)/600.0
	if(wall!=null):
		obs.append(wall_position_player.y)
	else:
		obs.append(0)
		
	#print(obs)
	var raycast_obs = raycast.get_observation();
	obs.append_array(raycast_obs);
	#print(raycast_obs)

	#var grid_wall_obs = grid_wall.get_observation();
	#obs.append_array(grid_wall_obs);
	#var grid_wall_obs2 = grid_wall2.get_observation();
	#obs.append_array(grid_wall_obs2);

	return {"obs":obs}

func get_reward() -> float:	
	var next = platforms.GetNextPlatform()
	if(next!=null):
		var next_global_position = next.global_position
		next_global_position.y = next_global_position.y-46
		var distance_to_next = player.global_position.distance_to(next_global_position)
		if distance_to_next < best_distance:
			reward += (best_distance-distance_to_next)/5000
		best_distance=distance_to_next
	return reward;
	
func get_action_space() -> Dictionary:
	return {
		"move" : {
			"size": 1,
			"action_type": "continuous"
		},
		"jump" : {
			"size": 1,
			"action_type": "continuous"
		},
		}
	
func set_action(action) -> void:	
	ball.AIXMovement = Vector2(clamp(action["move"][0],-1,1), 0);
	ball.AIJump = action["jump"][0] > 0;
	

func get_info() -> Dictionary:
	if done: 
		return {"is_success": is_success}
	return {}
	
	
