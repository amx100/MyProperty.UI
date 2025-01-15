namespace Helpers
{
    public class TreeItemData
    {
        public TreeItemData Parent { get; set; } = new();
        public string Text { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public Guid Id { get; set; }

        public bool IsExpanded { get; set; } = false;

        public bool IsChecked { get; set; } = false;

        public bool HasChild => TreeItems != null && TreeItems.Count > 0;

        public HashSet<TreeItemData> TreeItems { get; set; } = [];

        public TreeItemData()
        {
        }

        public TreeItemData(string text, Guid id, string type)
        {
            Text = text;
            Id = id;
            Type = type;
        }

        public void AddChild(string itemName, Guid id, string type)
        {
            TreeItemData item = new TreeItemData(itemName, id, type);
            item.Parent = this;
            this.TreeItems.Add(item);
        }

        public bool HasPartialChildSelection()
        {
            int iChildrenCheckedCount = (from c in TreeItems where c.IsChecked select c).Count();
            return HasChild && iChildrenCheckedCount > 0 && iChildrenCheckedCount < TreeItems.Count();
        }
    }
}