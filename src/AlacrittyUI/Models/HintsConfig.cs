namespace AlacrittyUI.Models;

public class HintRule
{
    public string Regex { get; set; } = string.Empty;
    public bool Hyperlinks { get; set; }
    public bool PostProcessing { get; set; }
    public bool Persist { get; set; }
    public string Action { get; set; } = string.Empty;
    public string Command { get; set; } = string.Empty;
    public string BindingKey { get; set; } = string.Empty;
    public string BindingMods { get; set; } = string.Empty;
    public bool MouseEnabled { get; set; } = true;
    public string MouseMods { get; set; } = string.Empty;

    public static readonly string[] ActionOptions = ["Copy", "Paste", "Select", "MoveViModeCursor"];
}

public class HintsConfig
{
    public string Alphabet { get; set; } = "jfkdls;ahgurieowpq";
    public List<HintRule> Enabled { get; set; } = [];
}
