using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    // На этом месте создаём ErrorModel в проекте Models
    public class ErrorModel
    {
        public string Title { get; set; }
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
    // <-- Возвращаемся в контроллер
}
