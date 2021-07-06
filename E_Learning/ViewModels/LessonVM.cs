using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Learning.ViewModels
{
    public class LessonVM
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string VideoLink { get; set; }
        public int? Order { get; set; }
        public string Image { get; set; }
        public string CourseName { get; set; }

    }
}
