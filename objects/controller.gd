extends AIController2D


var move = Vector2.ZERO
# Called when the node enters the scene tree for the first time.
func get_obs() -> Dictionary:
	assert(false, "the get_obs method is not implemented when extending from ai_controller") 
	return {"obs":[]}

func get_reward() -> float:	
	assert(false, "the get_reward method is not implemented when extending from ai_controller") 
	return 0.0
	
func get_action_space() -> Dictionary:
	return {
		"example_actions_continous" : {
			"size": 1,
			"action_type": "continuous"
		},
		"example_actions_discrete" : {
			"size": 1,
			"action_type": "discrete"
		},
		}
	
func set_action(action) -> void:	
	move.x = action["move"][0];
	
	
