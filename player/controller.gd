extends AIController2D

@export var ball: Node2D
@export var Score: Node2D
@onready var player: CharacterBody2D = $".."
@onready var grid: GridSensor2D = $GridSensor2D
#@onready var raycast: RaycastSensor2D = $"RaycastSensor2D"
# Called when the node enters the scene tree for the first time.
func get_obs() -> Dictionary:
	#var obs := []
	#obs.append(player.position.x);
	#obs.append(player.position.y);
	#var raycast_obs = raycast.get_observation();
	#obs.append_array(raycast_obs);
	var obs = grid.get_observation();
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
	
	
