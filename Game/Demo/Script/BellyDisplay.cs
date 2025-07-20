using Godot;

public enum FoodType
{
    Chicken,
    Mosquito
}

public partial class BellyDisplay : TextureRect
{
    [Export] public TextureRect ChickenTexture;
    [Export] public TextureRect MosquitoTexture;

    public override void _Ready()
    {
        ChickenTexture.Visible = false;
        MosquitoTexture.Visible = false;
    }

    public void Full(FoodType foodType)
    {
        ChickenTexture.Visible = false;
        MosquitoTexture.Visible = false;

        switch (foodType)
        {
            case FoodType.Chicken:
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
        ChickenTexture.Visible = false;
        MosquitoTexture.Visible = false;
        GD.Print("[BellyDisplay] Set to empty");
    }

}

