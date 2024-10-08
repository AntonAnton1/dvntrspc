using Content.Client.Stylesheets;
using Content.Client.UserInterface.Systems.Ghost.Controls;
using Content.Corvax.Interfaces.Shared;
using Robust.Client.AutoGenerated;
using Robust.Client.UserInterface.Controls;
using Robust.Client.UserInterface.XAML;

namespace Content.Client.UserInterface.Systems.Ghost.Widgets;

[GenerateTypedNameReferences]
public sealed partial class GhostGui : UIWidget
{
    public GhostTargetWindow TargetWindow { get; }

    public event Action? RequestWarpsPressed;
    public event Action? ReturnToBodyPressed;
    public event Action? GhostRolesPressed;
    public event Action? RespawnPressed;

    public GhostGui()
    {
        RobustXamlLoader.Load(this);

        TargetWindow = new GhostTargetWindow();

        MouseFilter = MouseFilterMode.Ignore;

        GhostWarpButton.OnPressed += _ => RequestWarpsPressed?.Invoke();
        ReturnToBodyButton.OnPressed += _ => ReturnToBodyPressed?.Invoke();
        GhostRolesButton.OnPressed += _ => GhostRolesPressed?.Invoke();
        RespawnButton.OnPressed += _ => RespawnPressed?.Invoke();
    }

    public void Hide()
    {
        TargetWindow.Close();
        Visible = false;
    }

    public void Update(int? roles, bool? canReturnToBody)
    {
        ReturnToBodyButton.Disabled = !canReturnToBody ?? true;

        if (roles != null)
        {
            GhostRolesButton.Text = Loc.GetString("ghost-gui-ghost-roles-button", ("count", roles));
            if (roles > 0)
            {
                GhostRolesButton.StyleClasses.Add(StyleBase.ButtonCaution);
            }
            else
            {
                GhostRolesButton.StyleClasses.Remove(StyleBase.ButtonCaution);
            }
        }

        // Alteros-Sponsors-start
        var sponsors = IoCManager.Resolve<ISharedSponsorsManager>(); // Alteros-Sponsors
        if (sponsors.GetClientAllowedRespawn())
        {
            RespawnButton.Disabled = false;
            RespawnButton.Text = Loc.GetString("new-life-gui-button");
        }
        else
        {
            RespawnButton.Disabled = true;
            RespawnButton.Text = Loc.GetString("new-life-gui-button-disable");
        }
        // Alteros-Sponsors-end

        TargetWindow.Populate();
    }

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            TargetWindow.Dispose();
        }
    }
}
