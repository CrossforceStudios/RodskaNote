return {
	Name = "PickupItem";
	Context = "Interaction";
	Description = "Interactions under this type allow players to pickup dropped items.";
	KeyContext = "Regular";
	ActionType = "Hold";
	ActionBuilder = function(object)
		return ("%s <Color=Blue>%s<Color=/>"):format("PICKUP",object.Name:upper())
	end;
	CreationCondition = function(object)
		return object.Parent == (workspace.LootIgnore)
	end;
	ActivationDistance = 5;
	ClientInt = function(object)
		_G.TweenToUnequip()
	end;
	Location = workspace.LootIgnore;
}