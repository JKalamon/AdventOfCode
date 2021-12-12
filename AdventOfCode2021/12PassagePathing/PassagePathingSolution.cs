namespace AdventOfCode2021
{
  internal class PassagePathingSolution : IChallenge
  {
    public string Title => "--- Day 12: Passage Pathing ---";

    public DateTime DateTime => new(2021, 12, 12);

    record Cave(string Name)
    {
      public bool IsBig => this.Name == this.Name.ToUpper();

      public bool IsSmall => !this.IsBig;

      public bool IsStart => this.Name == "start";
    }

    record Path(Cave A, Cave B);

    class TreeNode
    {
      public TreeNode(Cave cave, TreeNode? parent = null)
      {
        this.Cave = cave;
        if (parent != null)
        {
          this.Parents.Add(parent);
          this.Parents.AddRange(parent.Parents);
        }
      }

      public Cave Cave { get; set; }

      public List<TreeNode> Children = new List<TreeNode>();

      public List<TreeNode> Parents = new List<TreeNode>();
    }

    private List<TreeNode> flatTree = new List<TreeNode>();

    private IEnumerable<Path> inputFile = File.ReadAllLines("12PassagePathing/input.txt").Select(x => new Path(new(x.Split('-')[0]), new(x.Split('-')[1])));

    public object SolvePart1()
    {
      var startNode = new TreeNode(new("start"));
      flatTree.Add(startNode);
      CreateTree(startNode);      
      return this.flatTree.Count(x => x.Cave.Name == "end");
    }

    void CreateTree(TreeNode node)
    {
      if (node.Cave.Name == "end")
        return;

      var possiblePaths = this.inputFile
        .Where(x => x.A.Name == node.Cave.Name).Select(x => x.B)
        .Concat(this.inputFile.Where(x => x.B.Name == node.Cave.Name).Select(x => x.A));

      foreach (var possiblePath in possiblePaths.Where(x => x.IsBig || !node.Parents.Any(y => y.Cave == x)))
      {
        var pathNode = new TreeNode(possiblePath, node);
        this.flatTree.Add(pathNode);
        node.Children.Add(pathNode);
        CreateTree(pathNode);        
      }
    }

    void CreateTreePart2(TreeNode node)
    {
      if (node.Cave.Name == "end")
        return;

      var possiblePaths = this.inputFile
        .Where(x => x.A == node.Cave).Select(x => x.B)
        .Concat(this.inputFile.Where(x => x.B == node.Cave).Select(x => x.A));

      foreach (var possiblePath in possiblePaths
        .Where(x => !x.IsStart && (x.IsBig || !node.Parents.Any(y => y.Cave == x) || node.Parents.Where(y => y.Cave.IsSmall).Select(x => x.Cave).Append(node.Cave).GroupBy(y => y).All(x => x.Count() < 2))))
      {
        var pathNode = new TreeNode(possiblePath, node);
        this.flatTree.Add(pathNode);
        node.Children.Add(pathNode);
        CreateTreePart2(pathNode);
      }
    }

    public object? SolvePart2()
    {
      var startNode = new TreeNode(new("start"));
      flatTree = new List<TreeNode>();
      flatTree.Add(startNode);
      CreateTreePart2(startNode);
      return this.flatTree.Count(x => x.Cave.Name == "end");
    }
  }
}

