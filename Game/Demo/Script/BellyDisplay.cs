using Godot;

public partial class BellyDisplay : TextureRect
{
    [Export] public TextureRect FoodTexture;

    public override void _Ready()
    {
        FoodTexture.Visible = false;
    }

    public void Full()
    {
        FoodTexture.Visible = true;
        GD.Print("[BellyDisplay] Set to full");
    }

    public void Empty()
    {
        FoodTexture.Visible = false;
        GD.Print("[BellyDisplay] Set to empty");
    }

}
