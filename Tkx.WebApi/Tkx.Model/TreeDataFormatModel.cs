using System;

namespace Tkx.Model
{
    /// <summary>树的数据格式
    /// 
    /// </summary>
    public class TreeDataFormatModel
    {
        /// <summary>节点的 id，它对于加载远程数据很重要。
        /// 
        /// </summary>
        public int id
        {
            get; set;
        }
        /// <summary>要显示的节点文本。
        /// 
        /// </summary>
        public string text { get; set; }

        /// <summary>节点状态，'open' 或 'closed'，默认是 'open'。当设置为 'closed' 时，该节点有子节点，并且将从远程站点加载它们。 
        /// </summary>
        public string state { get; set; }
        /// <summary>给一个节点添加的自定义属性。 
        /// </summary>
        public string attributes { get; set; }

        /// <summary>指示节点是否被选中。 
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>定义了一些子节点的节点数组
        /// 
        /// </summary>
        public System.Collections.Generic.List<TreeDataFormatModel> children { get; set; }
    }
    /// <summary>节点展开开关
    /// 
    /// </summary>
    public enum TreeState { open, closed }
}
