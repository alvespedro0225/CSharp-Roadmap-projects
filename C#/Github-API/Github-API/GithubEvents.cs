using static System.String;

namespace Github_API;
public class GithubEvents
{
    public string? Id { get; set; }
    public string? Type { get; set; }
    public Dictionary<string, dynamic>? Actor { get; set; }
    public Dictionary<string, dynamic>? Repo { get; set; }
    //public Dictionary<string, dynamic>? Payload { get; set; }
    public Dictionary<string, dynamic>? Commits { get; set; }
    public bool? Public { get; set; }
    public string? CreatedAt { get; set; }

    public GithubEvents(){}

    public override string ToString()
    {
        var actor = Actor == null ? Empty : DictToString(Actor);
        var repo = Repo == null ? Empty : DictToString(Repo);
        //var payload = Payload == null ? Empty : DictToString(Payload);
        var commits = Commits == null ? Empty : DictToString(Commits);
        var rep = Empty;
        
        if (Id != null) rep += $"Id: {Id}\n";
        if (Type != null) rep += $"Type: {Type}\n";
        if (Actor != null) rep += $"Actor: {actor}\n";
        if (Repo != null) rep += $"Repo: {repo}\n";
        //if (Payload != null) rep += $"Payload: {payload}\n";
        if (Commits != null) rep += $"Commits: {commits}\n";
        if (Public != null) rep += $"Public: {Public}\n";
        if (CreatedAt != null) rep += $"CreatedAt: {CreatedAt}\n";
        return rep;

    }

    private static string DictToString(Dictionary<string, dynamic> dict)
    {
        var stringDict = Empty;
        if (dict.Count == 0) return stringDict;
        stringDict += "\n{\n";
        stringDict = dict.Aggregate(stringDict, (current, pair) => current + $"\t\t{pair.Key}: {pair.Value}\n");
        stringDict += "}";
        return stringDict;
    }
}