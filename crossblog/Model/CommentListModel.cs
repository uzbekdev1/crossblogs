using System.Collections.Generic;

namespace crossblog.Model
{
    public class CommentListModel
    {
        public IEnumerable<CommentModel> Comments { get; set; }

        public CommentListModel() : this(new CommentModel[0])
        {
        }

        public CommentListModel(IEnumerable<CommentModel> comments)
        {
            Comments = comments;
        }
    }
}