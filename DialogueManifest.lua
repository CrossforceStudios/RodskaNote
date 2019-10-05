local DialogsNodes = {
	Responses = {
		["JoinRequest"] = {
			Speech = "<PlayerFactionNoun> leaders are asking if you would join them.";
					
			Prompts = {
				"QuestStart";	
			};
			Overrides = {
				
			};		
			Condition = function(player, dialogue)
				return true
			end;
			Action = function(player, dialogue, api)
			
			end;											
		};
		["Nvm"] = {
			Speech = "Nevermind.";
			
			Prompts = {};
			Overrides = {
				
			};
			Condition = function(player, dialogue)
				return true
			end;		
		   Action = function(player, dialogue, api)

		   end;						
		};
		["DiscoverQuest"] = {
			Speech = "What's that?";
			
			Prompts = {
				"QuestReveal"
			};
			Overrides = {
				
			};
			Condition = function(player, dialogue)
				return true
			end;		
		    Action = function(player, dialogue, api)

		    end;						
		}		
	};
	Prompts = {
		["InitialHello"] = {
		   Speech = "Hello, there. Can we \"help\" you trespassers?";
		   Responses = {
				"JoinRequest";
				"Nvm";
		   };
		   Overrides = {
			
		   };
		   Condition = function(player, dialogue, api)
			   return true
		   end;		
		   Action = function(player, dialogue, api)

		   end;					
		};
		["ReadyHello"] = {
		   Speech = "Hello, there. Are you willing to help us? Or are you going to give us a reason to not join you?";
		   Responses = {
				"JoinRequest";
				"Nvm";
		   };
		   Overrides = {
			
		   };
		   Condition = function(player, dialogue, api)
			   return true
		   end;		
		   Action = function(player, dialogue, api)

		   end;					
		};
		
		["QuestStart"] = {
			
			Speech = "We will accept your offer...";
			Responses = {
				
			};
  			Overrides = {
			
    		};
			Prompts = {
				"QuestStartChained";
			};
		    Condition = function(player, dialogue)
			    return true
		    end;		
		    Action = function(player, dialogue, api)
 
		    end;					
		};
		["QuestStartChained"] = {
					Speech = "only if you do one thing before that.";
					Responses = {
						"DiscoverQuest";
						"Nvm";
					};
		  			Overrides = {
					
		    		};
					Prompts = {
						
					};
				    Condition = function(player, dialogue)
					    return true
				    end;		
				    Action = function(player, dialogue, api)
		 
				    end;					
		};
		["QuestReveal"] = {
					Speech = "A random quest.";
					Responses = {
						"DiscoverQuest";
						"Nvm";
					};
		  			Overrides = {
					
		    		};
					Prompts = {
						
					};
				    Condition = function(player, dialogue)
					    return true
				    end;		
				    Action = function(player, dialogue, api)
		 
				    end;					
		}			
}
}

return {
	Nodes = DialogsNodes;
	InitialPrompts = {"InitialHello";};
	ConverstationDistance = 10;	
	TriggerDistance = 0;
	Overrides = {
		Title = ("A Tribal Request"):upper();
	};
	TriggerOffset = Vector3.new(0,0,0);
}