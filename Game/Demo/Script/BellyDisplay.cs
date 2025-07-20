using Godot;

public enum FoodType
{
	Chicken,
	Mosquito,
	Goose
}

public partial class BellyDisplay : TextureRect
{
	[Export] public Texture2D EmptyTexture;
	[Export] public Texture2D FullTexture;
	
	[Export] public TextureRect ChickenTexture;
	[Export] public TextureRect MosquitoTexture;

	public override void _Ready()
	{
		Empty();
		// ChickenTexture.Visible = false;
		// MosquitoTexture.Visible = false;
	}

	public void Full(FoodType foodType)
	{
		Texture = FullTexture;
		ChickenTexture.Visible = false;
		MosquitoTexture.Visible = false;

		switch (foodType)
		{
			case FoodType.Chicken:
				ChickenTexture.Visible = true;
				break;
			case FoodType.Goose:
				ChickenTexture.Visible = true;
				break;
			case FoodType.Mosquito:
				MosquitoTexture.Visible = true;
				break;
		}

		GD.Print("[BellyDisplay] Set to full");
	}

	public void Empty()
	{
		Texture = EmptyTexture;
		ChickenTexture.Visible = false;
		MosquitoTexture.Visible = false;
		GD.Print("[BellyDisplay] Set to empty");
	}

}
