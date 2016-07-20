using Prism.Mvvm;

namespace DataServices.Models
{
    public class WiseIdeaModel : BindableBase
    {
        public string Id { get; set; }
        public string Author { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public string Content { get; set; }
        bool _selected;
        public bool Selected { get { return _selected; } set { base.SetProperty(ref _selected, value); } }
        bool _isFavorite;
        public bool IsFavorite { get { return _isFavorite; } set { base.SetProperty(ref _isFavorite, value); } }

        public string DateText { get { return Day + " " + System.Globalization.DateTimeFormatInfo.CurrentInfo.MonthGenitiveNames.GetValue(Month-1); } }
    }
}
