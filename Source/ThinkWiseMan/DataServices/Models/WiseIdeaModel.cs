using Prism.Mvvm;

namespace DataServices.Models
{
    public class WiseIdeaModel : BindableBase
    {
        public string Author { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public string Content { get; set; }
        bool _Selected;
        public bool Selected { get { return _Selected; } set { base.SetProperty(ref _Selected, value); } }
    }
}
