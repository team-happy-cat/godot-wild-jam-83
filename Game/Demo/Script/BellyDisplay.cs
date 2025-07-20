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
	
	[Export] public GpuParticles2D JuiceParticles;

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
		
		Animate(true);

		GD.Print("[BellyDisplay] Set to full");
	}

	public void Empty()
	{
		Texture = EmptyTexture;
		ChickenTexture.Visible = false;
		MosquitoTexture.Visible = false;
		Animate(false);
		GD.Print("[BellyDisplay] Set to empty");
	}

	private void Animate(bool full)
	{
		PivotOffset = Size / 2f;
		Scale = Vector2.One * 0.9f;
		
		Tween tween = CreateTween();
		tween.TweenProperty(this, "scale", Vector2.One, 0.1);
		tween.Play();
		
		if (full) JuiceParticles.Restart();
	}
}
