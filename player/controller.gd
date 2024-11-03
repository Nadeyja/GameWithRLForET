extends AIController2D

@export var ball: Node2D
@export var Score: Node2D
@onready var player: CharacterBody2D = $".."
@onready var grid_wall: GridSensor2D = $GridSensor2D
@onready var grid_wall2: GridSensor2D = $GridSensor2D2
@onready var wall = load("res://objects/wall.tscn")
@onready var GeneratePlatform = load("res://objects/GeneratePlatform.cs")
@onready var platforms = GeneratePlatform.new()
var is_success := false
#@onready var raycast: RaycastSensor2D = $"RaycastSensor2D"
# Called when the node enters the scene tree for the first time.
func get_obs() -> Dictionary:
	var obs := []
	obs.append(player.position.x);
	obs.append(player.position.y);
	
	var current = platforms.GetCurrentPlatform()
	if(current!=null):
		obs.append(current.position.x)
		obs.append(current.position.y-35)
		print("CurentPlat pos: ", current.position)
	else:
		obs.append(0)
		obs.append(0)
	
	var next = platforms.GetNextPlatform()	
	if(next!=null):
		obs.append(next.position.x)
		obs.append(next.position.y-35)
		print("NextPlat pos: ", next.position)
	else:
		obs.append(0)
		obs.append(0)
		
	var wall = platforms.GetWall()
	if(wall!=null):
		obs.append(wall.position.y)
		print("GetWall pos: ", wall.position)
	else:
		obs.append(0)
		
	#var raycast_obs = raycast.get_observation();
	#obs.append_array(raycast_obs);
	var grid_wall_obs = grid_wall.get_observation();
	obs.append_array(grid_wall_obs);
	var grid_wall_obs2 = grid_wall2.get_observation();
	obs.append_array(grid_wall_obs2);

	return {"obs":obs}

func get_reward() -> float:	
	
	return reward;
	
func get_action_space() -> Dictionary:
	return {
		"move" : {
			"size": 1,
			"action_type": "continuous"
		},
		"jump" : {
			"size": 2,
			"action_type": "discrete"
		},
		}
	
func set_action(action) -> void:	
	ball.AIXMovement = Vector2(clamp(action["move"][0],-1,1), 0);
	ball.AIJump = action["jump"] == 1;
	

func get_info() -> Dictionary:
	if done: 
		return {"is_success": is_success}
	return {}
	
	
